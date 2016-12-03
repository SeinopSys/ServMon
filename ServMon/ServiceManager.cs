using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;

namespace ServMon {
	internal struct StatusColors {
		public const string
			Stopped = "r",
			Running = "g",
			Waiting = "y";

		private static Dictionary<ServiceControllerStatus, string> _statusMeaning =
			new Dictionary<ServiceControllerStatus, string>() {
				{ServiceControllerStatus.ContinuePending, Waiting},
				{ServiceControllerStatus.PausePending, Waiting},
				{ServiceControllerStatus.StartPending, Waiting},
				{ServiceControllerStatus.StopPending, Waiting},
				{ServiceControllerStatus.Paused, Stopped},
				{ServiceControllerStatus.Stopped, Stopped},
				{ServiceControllerStatus.Running, Running},
			};

		public static string ForStatus(ServiceControllerStatus s){
			return _statusMeaning[s];
		}
	}

	internal class ServiceManager {
		private ServiceController _service;
		private SettingsForm _form;
		private BackgroundWorker _starter, _stopper, _restarter;
		private bool _busy;

		public ServiceManager(string serviceName, SettingsForm targetform){
			_service = new ServiceController(serviceName);
			_form = targetform;

			_starter = new BackgroundWorker {
				WorkerSupportsCancellation = false,
				WorkerReportsProgress = false
			};
			_starter.DoWork += StartBeginHandler;
			_starter.RunWorkerCompleted += StartEndHandler;

			_stopper = new BackgroundWorker {
				WorkerSupportsCancellation = false,
				WorkerReportsProgress = false
			};
			_stopper.DoWork += StopBeginHandler;
			_stopper.RunWorkerCompleted += StopEndHandler;

			_restarter = new BackgroundWorker {
				WorkerSupportsCancellation = false,
				WorkerReportsProgress = false
			};
			_restarter.DoWork += RestartBeginHandler;
			_restarter.RunWorkerCompleted += RestartEndHandler;
		}

		public string StatusColor(){
			return _busy ? StatusColors.Waiting : StatusColors.ForStatus(_service.Status);
		}

		public void BeBusy(){
			_busy = true;
			_form.UpdateEverything();
		}

		public void Start(bool autoStart = false){
			if (!autoStart)
				_form.Log.Log("Starting service " + _service.ServiceName + "...", Logger.Severity.Warn);
			BeBusy();
			_starter.RunWorkerAsync();
		}

		private void StartBeginHandler(object sender, DoWorkEventArgs e){
			_service.Start();
			var timeout = TimeSpan.FromSeconds(_form.TimeoutSeconds);
			_service.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}

		private void StartEndHandler(object sender, RunWorkerCompletedEventArgs e){
			if (_service.Status != ServiceControllerStatus.Running)
				_form.Log.Log("Starting service " + _service.ServiceName + " failed.", Logger.Severity.Error);
			else
				_form.Log.Log("Service " + _service.ServiceName + " started.", Logger.Severity.Success);

			WorkerEndHandler(sender, e);
		}

		public void Stop(){
			_form.Log.Log("Stopping service " + _service.ServiceName + "...", Logger.Severity.Warn);
			BeBusy();
			_stopper.RunWorkerAsync();
		}

		private void StopBeginHandler(object sender, DoWorkEventArgs e){
			_service.Stop();
			var timeout = TimeSpan.FromSeconds(_form.TimeoutSeconds);
			_service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
		}

		private void StopEndHandler(object sender, RunWorkerCompletedEventArgs e){
			if (_service.Status != ServiceControllerStatus.Stopped)
				_form.Log.Log("Stopping service " + _service.ServiceName + " failed.", Logger.Severity.Error);
			else
				_form.Log.Log("Service " + _service.ServiceName + " stopped.", Logger.Severity.Success);

			WorkerEndHandler(sender, e);
		}

		public void Restart(){
			_form.Log.Log("Restarting service " + _service.ServiceName + "...", Logger.Severity.Warn);
			BeBusy();
			_restarter.RunWorkerAsync();
		}

		private void RestartBeginHandler(object sender, DoWorkEventArgs e){
			StopBeginHandler(sender, e);

			StartBeginHandler(sender, e);
		}

		private void RestartEndHandler(object sender, RunWorkerCompletedEventArgs e){
			if (_service.Status != ServiceControllerStatus.Running)
				_form.Log.Log("Restarting service " + _service.ServiceName + " failed.", Logger.Severity.Error);
			else
				_form.Log.Log("Service " + _service.ServiceName + " restarted.", Logger.Severity.Success);

			WorkerEndHandler(sender, e);
		}

		private void WorkerEndHandler(object sender, RunWorkerCompletedEventArgs e){
			_busy = false;
			_form.UpdateEverything();
		}
	}
}