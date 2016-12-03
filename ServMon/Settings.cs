using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ServMon {
	internal class Settings {
		public string FileName = "ServMon.config.json", SavePath;
		private Dictionary<string, string> _config;
		private SettingsForm _form;
		private Button _btnSave;

		public Settings(SettingsForm form){
			SavePath = "./" + FileName;
			string configFile = "{}";
			try {
				var sr = new StreamReader(SavePath, Encoding.UTF8);
				configFile = sr.ReadToEnd();
				sr.Close();
			}
			catch (Exception) {
				// ignored
			}

			try {
				_config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configFile);
			}
			catch (JsonReaderException) {
				File.Delete(SavePath);
				_config = new Dictionary<string, string>();
			}
			_form = form;
			_btnSave = (Button) _form.Controls.Find("BtnSave", true).First();
		}

		public string Get(string option){
			return _config.ContainsKey(option) ? _config[option] : "";
		}

		public void Set(string option, string value){
			_config[option] = value;
		}

		public void Save(){
			_btnSave.Enabled = false;
			var fs = new FileStream(SavePath, FileMode.Create);
			var sw = new StreamWriter(fs, Encoding.UTF8);
			sw.Flush();
			var json = JsonConvert.SerializeObject(_config, Formatting.Indented);
			sw.Write(json);
			sw.Close();
			fs.Close();

			_btnSave.Enabled = true;
			_form.Log.Log("Settings have been saved", Logger.Severity.Success);
		}
	}
}