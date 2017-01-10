using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Drawing;
using System.Linq;
using ServMon.Properties;

namespace ServMon {
	public partial class SettingsForm : Form {
		private Settings _appSettings;
		public Logger Log;
		public bool StartMinimized, Started = false;
		public int TimeoutSeconds;
		private static List<ServiceController> _services;
		private static string _lServ, _rServ, _lAuto, _rAuto;
		private static bool _singleService, _singleServiceOnLeftSide;
		private static char _singleServiceSide;
		public static NotifyIcon NIcon = new NotifyIcon();

		private static Dictionary<char, string> _serviceNames = new Dictionary<char, string>();
		private static Dictionary<char, ServiceManager> _serviceMgrs = new Dictionary<char, ServiceManager>();

		public SettingsForm() {
			InitializeComponent();

			_appSettings = new Settings(this);
			Log = new Logger(this);

			var ctxMenu = new ContextMenu();
			ctxMenu.MenuItems.Add("ServMon");
			ctxMenu.MenuItems[0].Enabled = false;
			ctxMenu.MenuItems.Add("-");
			ctxMenu.MenuItems.Add("Show settings", NIcon_MouseClick);
			ctxMenu.MenuItems[2].DefaultItem = true;
			ctxMenu.MenuItems.Add("Exit ServMon", Exit_Click);

			NIcon.ContextMenu = ctxMenu;
			NIcon.Icon = Resources.logo;
			NIcon.Visible = true;
			NIcon.MouseClick += NIcon_MouseClick;
			NIcon.MouseDoubleClick += NIcon_MouseClick;

			Icon = NIcon.Icon;

			GetServiceList();
			SetServices();

			_lAuto = _appSettings.Get("leftServiceAutostart");
			_rAuto = _appSettings.Get("rightServiceAutostart");

			Log.Log("Processing autostart entries...");
			if (_lAuto == "true") {
				LAutoStart.Checked = true;
				if (_serviceMgrs['l'].StatusColor() == StatusColors.Running)
					Log.Log(_serviceNames['l'] + " is already running.", Logger.Severity.Success);
				else {
					StartService('l', true);
					Log.Log("Auto-starting " + _serviceNames['l'] + " service...");
				}
			}
			if (_rAuto == "true") {
				RAutoStart.Checked = true;
				if (_serviceMgrs['r'].StatusColor() == StatusColors.Running)
					Log.Log(_serviceNames['r'] + " is already running.", Logger.Severity.Success);
				else {
					StartService('r', true);
					Log.Log("Auto-starting " + _serviceNames['r'] + " service...");
				}
			}

			StartMinimized = _appSettings.Get("startMinimized") != "false";
			MinimizedCheckbox.Checked = StartMinimized;

			TimeoutSeconds = _appSettings.Get("timeoutSeconds").Length > 0
				? int.Parse(_appSettings.Get("timeoutSeconds"))
				: 15;
			TimeoutInput.Value = TimeoutSeconds;
		}
		private void GetServiceList() {
			_lServ = _appSettings.Get("leftServiceName");
			_rServ = _appSettings.Get("rightServiceName");

			ServiceController[] servicesArray = ServiceController.GetServices();
			_services = new List<ServiceController>();
			LServiceSel.Items.Add("");
			RServiceSel.Items.Add("");
			foreach (ServiceController s in servicesArray) {
				var serviceName = s.ServiceName;
				if (!Regex.IsMatch(serviceName, @"(apache|postgres|mysql|elasticsearch|redis|memcached|nginx)", RegexOptions.IgnoreCase))
					continue;
				LServiceSel.Items.Add(serviceName);
				RServiceSel.Items.Add(serviceName);
				_services.Add(s);
			}

			if (_services.Count < 1) {
				MessageBox.Show("No supported service found. Please install Apache/PostgreSQL/MySQL/ElasticSearch and run ServMon again.", "No supported service", MessageBoxButtons.OK, MessageBoxIcon.Error);
				NIcon.Visible = false;
				Environment.Exit(0);
			}

			if (_lServ.Length > 0) {
				var leftService = _services.FirstOrDefault(x => x.ServiceName == _lServ);
				if (leftService != null)
					LServiceSel.Text = leftService.ServiceName;
				else _appSettings.Set("leftServiceName", "");
			}
			if (_rServ.Length > 0) {
				var rightService = _services.FirstOrDefault(x => x.ServiceName == _rServ);
				if (rightService != null)
					RServiceSel.Text = rightService.ServiceName;
				else _appSettings.Set("rightServiceName", "");
			}
			if (_lServ.Length + _rServ.Length == 0) {
				var leftService = _services[0];
				_lServ = leftService.ServiceName;
				LServiceSel.Text = _lServ;
				_appSettings.Set("leftServiceName", _lServ);

				if (_services.Count > 1) {
					var rightService = _services[1];
					_rServ = rightService.ServiceName;
					RServiceSel.Text = _rServ;
					_appSettings.Set("rightServiceName", _rServ);
				}
			}
		}
		private void SetServices(){
			bool wasSingleService = _singleService;
			_singleService = _lServ.Length == 0 || _rServ.Length == 0;
			if (_singleService) {
				_singleServiceOnLeftSide = _lServ.Length != 0;
				string
					singleServiceSideStr = _singleServiceOnLeftSide ? "left" : "right",
					oppositeServiceSideStr = _singleServiceOnLeftSide ? "right" : "left";
				_singleServiceSide = singleServiceSideStr[0];

				_serviceNames[_singleServiceSide] = _singleServiceOnLeftSide ? _lServ : _rServ;
				_serviceMgrs[_singleServiceSide] = new ServiceManager(_serviceNames[_singleServiceSide], this);

				_appSettings.Set(singleServiceSideStr + "ServiceName", _serviceNames[_singleServiceSide]);
				_appSettings.Set(oppositeServiceSideStr + "ServiceName", "");

				if (!wasSingleService && Started)
					Log.Log("Switched to single service mode");
			}
			else {
				_serviceNames['l'] = _lServ;
				_serviceNames['r'] = _rServ;

				_serviceMgrs['l'] = new ServiceManager(_serviceNames['l'], this);
				_serviceMgrs['r'] = new ServiceManager(_serviceNames['r'], this);

				_appSettings.Set("leftServiceName", _serviceNames['l']);
				_appSettings.Set("rightServiceName", _serviceNames['r']);

				if (wasSingleService && Started)
					Log.Log("Switched to dual service mode");
			}
			UpdateEverything();
		}
		public void UpdateEverything() {
			UpdateIcon();
			UpdateButtons();
		}
		private void UpdateIcon() {
			if (_singleService) {
				var color = _serviceMgrs[_singleServiceSide].StatusColor();
				NIcon.Icon = (Icon) Resources.ResourceManager.GetObject(color + color);
			}
			else NIcon.Icon = (Icon) Resources.ResourceManager.GetObject(_serviceMgrs['l'].StatusColor() + _serviceMgrs['r'].StatusColor());

			Icon = NIcon.Icon;
		}
		private void UpdateButtons(){
			foreach (var updateSide in new[] { 'l', 'r' }) {
				var updateSideUpper = updateSide.ToString().ToUpper();
				Button btnStart = (Button) Controls.Find(updateSideUpper + "BtnStart", true).First();
				Button btnStop = (Button) Controls.Find(updateSideUpper + "BtnStop", true).First();
				Button btnRestart = (Button) Controls.Find(updateSideUpper + "BtnRestart", true).First();

				bool foceDisable = _singleService && ((_singleServiceOnLeftSide && updateSide != 'l') || (!_singleServiceOnLeftSide && updateSide == 'l'));

				string value = _serviceNames.ContainsKey(updateSide) && !foceDisable
					? _serviceMgrs[updateSide].StatusColor()
					: StatusColors.Waiting;

				switch (value) {
					case StatusColors.Waiting:
						btnRestart.Enabled =
						btnStart.Enabled =
						btnStop.Enabled = false;
					break;
					case StatusColors.Running:
						btnStart.Text = "Running";
						btnRestart.Enabled = true;
						btnStop.Enabled = true;
						btnStart.Enabled = false;
					break;
					case StatusColors.Stopped:
						btnStart.Text = "Start";
						btnRestart.Enabled = false;
						btnStop.Enabled = false;
						btnStart.Enabled = true;
					break;
				}
			}
		}
		public void ShowForm() {
			if (!Program.FormInstance.Visible)
				Program.FormInstance.Show();
			Program.FormInstance.Activate();
		}
		private static void HideForm() {
			Program.FormInstance.Hide();
		}
		private void StartService(char side, bool autoStart = false) {
			if (_serviceMgrs[side].StatusColor() != StatusColors.Stopped)
				return;
			_serviceMgrs[side].Start(autoStart);
		}
		private void StopService(char side) {
			if (_serviceMgrs[side].StatusColor() != StatusColors.Running)
				return;
			_serviceMgrs[side].Stop();
		}
		private void RestartService(char side) {
			if (_serviceMgrs[side].StatusColor() != StatusColors.Running)
				return;
			_serviceMgrs[side].Restart();
		}

		private void BtnSave_Click(object sender, EventArgs e) {
			_appSettings.Save();
		}
		private void NIcon_MouseClick(object sender, EventArgs e) {
			ShowForm();
		}
		private void LBtnStart_Click(object sender, EventArgs e) {
			StartService('l');
		}
		private void LBtnStop_Click(object sender, EventArgs e) {
			StopService('l');
		}
		private void LBtnRestart_Click(object sender, EventArgs e) {
			RestartService('l');
		}
		private void RBtnStart_Click(object sender, EventArgs e) {
			StartService('r');
		}
		private void RBtnStop_Click(object sender, EventArgs e) {
			StopService('r');
		}
		private void RBtnRestart_Click(object sender, EventArgs e) {
			RestartService('r');
		}
		private void LAutoStart_CheckedChanged(object sender, EventArgs e) {
			_lAuto = LAutoStart.Checked ? "true" : "false";
			_appSettings.Set("leftServiceAutostart", _lAuto);
		}
		private void RAutoStart_CheckedChanged(object sender, EventArgs e) {
			_rAuto = RAutoStart.Checked ? "true" : "false";
			_appSettings.Set("rightServiceAutostart", _rAuto);
		}
		private void LServiceSel_SelectedIndexChanged(object sender, EventArgs e){
			_lServ = LServiceSel.Text;
			SetServices();
		}
		private void RServiceSel_SelectedIndexChanged(object sender, EventArgs e) {
			_rServ = RServiceSel.Text;
			SetServices();
		}

		private void SettingsForm_Load(object sender, EventArgs e){
			var labelTip = new ToolTip();
			string text = "How long service tasks are allowed to run (in seconds) before being aborted";
			labelTip.SetToolTip(TimeoutLabel, text);
			labelTip.SetToolTip(TimeoutInput, text);
		}

		private void TimeoutInput_ValueChanged(object sender, EventArgs e){
			TimeoutSeconds = Convert.ToInt32(TimeoutInput.Value);
			_appSettings.Set("timeoutSeconds", TimeoutSeconds.ToString());
		}
		private void MinimizedCheckbox_CheckedChanged(object sender, EventArgs e) {
			_appSettings.Set("startMinimized", ((CheckBox) sender).Checked ? "true" : "false");
		}
		private bool _cancelClose = true;
		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (_cancelClose)
				e.Cancel = true;
			HideForm();
		}
		private void Exit_Click(object sender, EventArgs e) {
			NIcon.Visible = false;
			_cancelClose = false;
			Application.Exit();
		}
	}
}
