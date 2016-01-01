using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ServiceProcess;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Linq;
using System.ComponentModel;

namespace ServMon {
	public partial class SettingsForm : Form {
		private static Settings AppSettings = new Settings();
		private static List<ServiceController> services;
		private static string lServ, rServ, lAuto, rAuto;
		private static bool singleService, leftSide;
		private static char side;
		public static NotifyIcon NIcon = new NotifyIcon();

		private static Dictionary<char, string> serviceNames = new Dictionary<char, string>();
		private static Dictionary<char, ServiceManager> serviceMgrs = new Dictionary<char, ServiceManager>();

		public SettingsForm() {
			InitializeComponent();

			var CtxMenu = new ContextMenu();
			CtxMenu.MenuItems.Add("ServMon");
			CtxMenu.MenuItems[0].Enabled = false;
			CtxMenu.MenuItems.Add("-");
			CtxMenu.MenuItems.Add("Show settings", new EventHandler(NIcon_MouseClick));
			CtxMenu.MenuItems[2].DefaultItem = true;
			CtxMenu.MenuItems.Add("Exit ServMon", new EventHandler(Exit_Click));

			NIcon.ContextMenu = CtxMenu;
			NIcon.Icon = Properties.Resources.logo;
			NIcon.Visible = true;
			NIcon.MouseClick += NIcon_MouseClick;
			NIcon.MouseDoubleClick += NIcon_MouseClick;

			this.Icon = NIcon.Icon;

			GetServiceList();
			SetServices();

			lAuto = AppSettings.Get("leftServiceAutostart");
			rAuto = AppSettings.Get("rightServiceAutostart");

			if (lAuto == "true") {
				LAutoStart.Checked = true;
				StartService('l');
			}
			if (rAuto == "true") {
				RAutoStart.Checked = true;
				StartService('r');
			}
		}
		private void GetServiceList() {
			lServ = AppSettings.Get("leftServiceName");
			rServ = AppSettings.Get("rightServiceName");

			ServiceController[] servicesArray = ServiceController.GetServices();
			services = new List<ServiceController>();
			foreach (ServiceController s in servicesArray) {
				var serviceName = s.ServiceName;
				if (!Regex.IsMatch(serviceName, @"(apache|postgres|mysql)", RegexOptions.IgnoreCase))
					continue;
				LServiceSel.Items.Add(serviceName);
				RServiceSel.Items.Add(serviceName);
				services.Add(s);
			}

			if (services.Count < 1) {
				MessageBox.Show("No supported service","No supported service found. Please install Apache/PostgreSQL/MySQL and run ServMon again.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			if (lServ.Length > 0) {
				var leftService = services.FirstOrDefault(x => x.ServiceName == lServ);
				if (leftService != null)
					LServiceSel.Text = leftService.ServiceName;
				else AppSettings.Set("leftServiceName", "");
			}
			if (rServ.Length > 0) {
				var rightService = services.FirstOrDefault(x => x.ServiceName == rServ);
				if (rightService != null)
					RServiceSel.Text = rightService.ServiceName;
				else AppSettings.Set("rightServiceName", "");
			}
			if (lServ.Length + rServ.Length == 0) {
				var leftService = services[0];
				lServ = leftService.ServiceName;
				LServiceSel.Text = lServ;
				AppSettings.Set("leftServiceName", lServ);

				if (services.Count > 1) {
					var rightService = services[1];
					rServ = rightService.ServiceName;
					RServiceSel.Text = rServ;
					AppSettings.Set("rightServiceName", rServ);
				}

				AppSettings.Save();
			}
		}
		private void SetServices() {
			singleService = lServ.Length == 0 || rServ.Length == 0;
			if (singleService) {
				leftSide = lServ.Length != 0;
				side = leftSide ? 'l' : 'r';
			}

			if (singleService) {
				serviceNames[side] = leftSide ? lServ : rServ;
				serviceMgrs[side] = new ServiceManager(serviceNames[side], this);
			}
			else {
				serviceNames['l'] = lServ;
				serviceNames['r'] = rServ;

				serviceMgrs['l'] = new ServiceManager(serviceNames['l'], this);
				serviceMgrs['r'] = new ServiceManager(serviceNames['r'], this);
			}
			UpdateEverything();
		}
		public void UpdateEverything() {
			UpdateIcon();
			UpdateButtons();
		}
		private void UpdateIcon() {
			if (singleService) {
				var color = serviceMgrs[side].StatusColor();
				NIcon.Icon = (Icon) Properties.Resources.ResourceManager.GetObject(color + color);
			}
			else {
				string[] name = new string[] { serviceMgrs['l'].StatusColor(), serviceMgrs['r'].StatusColor() };
				NIcon.Icon = (Icon) Properties.Resources.ResourceManager.GetObject(String.Join("",name));
			}
		}
		private void UpdateButtons() {
			foreach (var updateSide in new string[]{"l","r"}){
				if (!serviceNames.ContainsKey(updateSide[0]))
					continue;

				var updateSideUpper = updateSide.ToUpper();
				Button BtnStart = (Button) this.Controls.Find(updateSideUpper + "BtnStart", true).First();
				Button BtnStop = (Button) this.Controls.Find(updateSideUpper + "BtnStop", true).First();
				Button BtnRestart = (Button) this.Controls.Find(updateSideUpper + "BtnRestart", true).First();

				string value = serviceMgrs[updateSide[0]].StatusColor();
				switch (value) {
					case StatusColors.Waiting:
						BtnRestart.Enabled =
						BtnStart.Enabled =
						BtnStop.Enabled = false;
						break;
					case StatusColors.Running:
						BtnStart.Text = "Running";
						BtnStart.Enabled = !(BtnRestart.Enabled = BtnStop.Enabled = true);
						break;
					case StatusColors.Stopped:
						BtnStart.Text = "Start";
						BtnStart.Enabled = !(BtnRestart.Enabled = BtnStop.Enabled = false);
						break;
				}
			}
		}
		private static void ShowForm() {
			if (!Program.FormInstance.Visible)
				Program.FormInstance.Show();
			Program.FormInstance.Activate();
		}
		private static void HideForm() {
			Program.FormInstance.Hide();
		}
		private void StartService(char side) {
			if (serviceMgrs[side].StatusColor() != StatusColors.Stopped)
				return;
			serviceMgrs[side].Start();
		}
		private void StopService(char side) {
			if (serviceMgrs[side].StatusColor() != StatusColors.Running)
				return;
			serviceMgrs[side].Stop();
		}
		private void RestartService(char side) {
			if (serviceMgrs[side].StatusColor() != StatusColors.Running)
				return;
			serviceMgrs[side].Restart();
		}

		private void BtnSave_Click(object sender, EventArgs e) {
			AppSettings.Save();
		}
		private static void NIcon_MouseClick(object sender, EventArgs e) {
			ShowForm();
		}
		private static void NIcon_MouseClick(object sender, MouseEventArgs e) {
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
			lAuto = LAutoStart.Checked ? "true" : "";
			AppSettings.Set("leftServiceAutostart", lAuto);
		}
		private void RAutoStart_CheckedChanged(object sender, EventArgs e) {
			rAuto = RAutoStart.Checked ? "true" : "";
			AppSettings.Set("rightServiceAutostart", rAuto);
		}
		private void Exit_Click(object sender, EventArgs e) {
			NIcon.Visible = false;
			Application.Exit();
		}
	}
	struct StatusColors {
		public const string
			Stopped = "r",
			Running = "g",
			Waiting = "y";
		private static Dictionary<ServiceControllerStatus, string> StatusMeaning = new Dictionary<ServiceControllerStatus, string>() {
			{ ServiceControllerStatus.ContinuePending, StatusColors.Waiting },
			{ ServiceControllerStatus.PausePending, StatusColors.Waiting },
			{ ServiceControllerStatus.StartPending, StatusColors.Waiting },
			{ ServiceControllerStatus.StopPending, StatusColors.Waiting },
			{  ServiceControllerStatus.Paused, StatusColors.Stopped },
			{ ServiceControllerStatus.Stopped, StatusColors.Stopped },
			{ ServiceControllerStatus.Running, StatusColors.Running },
		};
		public static string ForStatus(ServiceControllerStatus s) {
			return StatusMeaning[s];
		}
	}
	class ServiceManager {
		ServiceController service;
		SettingsForm form;
		BackgroundWorker starter, stopper, restarter;
		int timeoutSeconds = 7;
		bool busy = false;

		public ServiceManager(string serviceName, SettingsForm targetform) {
			service = new ServiceController(serviceName);
			form = targetform;

			starter = new BackgroundWorker();
			starter.WorkerSupportsCancellation = false;
			starter.WorkerReportsProgress = false;
			starter.DoWork += new DoWorkEventHandler(StartBeginHandler);
			starter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerEndHandler);

			stopper = new BackgroundWorker();
			stopper.WorkerSupportsCancellation = false;
			stopper.WorkerReportsProgress = false;
			stopper.DoWork += new DoWorkEventHandler(StopBeginHandler);
			stopper.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerEndHandler);

			restarter = new BackgroundWorker();
			restarter.WorkerSupportsCancellation = false;
			restarter.WorkerReportsProgress = false;
			restarter.DoWork += new DoWorkEventHandler(RestartBeginHandler);
			restarter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerEndHandler);
		}

		public string StatusColor() {
			return busy ? StatusColors.Waiting : StatusColors.ForStatus(service.Status);
		}
		public void BeBusy() {
			busy = true;
			form.UpdateEverything();
		}

		public void Start() {
			BeBusy();
			starter.RunWorkerAsync();
		}
		private void StartBeginHandler(object sender, DoWorkEventArgs e) {
			service.Start();
			var timeout = TimeSpan.FromSeconds(timeoutSeconds);
			service.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}

		public void Stop() {
			BeBusy();
			stopper.RunWorkerAsync();
		}
		private void StopBeginHandler(object sender, DoWorkEventArgs e) {
			service.Stop();
			var timeout = TimeSpan.FromSeconds(timeoutSeconds);
			service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
		}

		public void Restart() {
			BeBusy();
			restarter.RunWorkerAsync();
		}
		private void RestartBeginHandler(object sender, DoWorkEventArgs e) {
			service.Stop();
			TimeSpan timeout = TimeSpan.FromSeconds(timeoutSeconds);
			service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

			service.Start();
			timeout = TimeSpan.FromSeconds(timeoutSeconds);
			service.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}

		private void WorkerEndHandler(object sender, RunWorkerCompletedEventArgs e) {
			busy = false;
			form.UpdateEverything();
		}
	}
	class Settings {
		public string fileName = "ServMon.config.json", savePath;
		public Dictionary<string, string> config;

		public Settings() {
			this.savePath = "./" + fileName;
			string configFile = "{}";
			try {
				var sr = new StreamReader(savePath, Encoding.UTF8);
				configFile = sr.ReadToEnd();
				sr.Close();
			}
			catch (Exception) { };
			config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configFile);
		}

		public string Get(string option) {
			return config.ContainsKey(option) ? config[option] : "";
		}

		public void Set(string option, string value) {
			config[option] = value;
		}

		public void Save() {
			var fs = new FileStream(savePath, FileMode.OpenOrCreate);
			var sw = new StreamWriter(fs, Encoding.UTF8);
			sw.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
			sw.Close();
			fs.Close();
		}
	}
}
