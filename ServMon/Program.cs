using System;
using System.Windows.Forms;

namespace ServMon {
	static class Program {
		public static SettingsForm FormInstance;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			FormInstance = new SettingsForm();
			if (FormInstance.StartMinimized == false)
				FormInstance.ShowForm();
			FormInstance.Log.Log("Application started", Logger.Severity.Attention);
			FormInstance.Started = true;
			Application.Run();
		}
	}
}
