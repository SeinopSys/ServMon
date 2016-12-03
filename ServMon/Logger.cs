using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ServMon {
	public class Logger {
		private RichTextBox _logOutput;

		public struct Severity {
			public const int
				Info = 0,
				Error = 1,
				Warn = 2,
				Success = 3,
				Attention = 4;
		}

		public Dictionary<int, Color> SeverityColors = new Dictionary<int, Color> {
			{0, Color.DeepSkyBlue},
			{1, Color.Red},
			{2, Color.Orange},
			{3, Color.Lime},
			{4, Color.Yellow},
		};

		public Logger(SettingsForm form){
			_logOutput = (RichTextBox) form.Controls.Find("LogOutput", true).First();
		}

		private void WriteInColor(string text, Color color){
			_logOutput.SelectionStart = _logOutput.TextLength;
			_logOutput.SelectionLength = 0;

			_logOutput.SelectionColor = color;
			_logOutput.AppendText(text);
			_logOutput.SelectionColor = _logOutput.ForeColor;
		}

		public void Log(string message, int severity = Severity.Info){
			WriteInColor(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ' ', Color.White);
			WriteInColor(message + '\n', SeverityColors[severity]);
			_logOutput.SelectionStart = _logOutput.Text.Length;
			_logOutput.ScrollToCaret();
		}
	}
}