using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Sannel.House.Sensor.Temperature;
using System.Diagnostics;
using System.Threading.Tasks;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sannel.House.BackgroundTasks.Thermostat
{
	public sealed class StartupTask : IBackgroundTask
	{
		private BackgroundTaskDeferral deferral;

		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			// 
			// TODO: Insert code to perform background work
			//
			// If you start any asynchronous methods here, prevent the task
			// from closing prematurely by using BackgroundTaskDeferral as
			// described in http://aka.ms/backgroundtaskdeferral
			//
			deferral = taskInstance.GetDeferral();

			var wire = new UWire();

			await wire.InitAsync();

			using (var sensor = new BME280(await wire.GetDeviceByIdAsync(0x77)))
			{
				try
				{
					sensor.RunMode = 3;
					sensor.TemperatureOverSample = 5;
					sensor.PressureOverSample = 5;
					sensor.HumidityOverSample = 5;
					sensor.Begin();
					while (true)
					{
						var temp = sensor.GetTemperatureCelsius();
						var press = sensor.GetPressure();
						var rh = sensor.GetRelativeHumidity();
						Debug.WriteLine($"temp {temp}");
						Debug.WriteLine($"press {press}");
						Debug.WriteLine($"rh {rh}");
						await Task.Delay(500);
					}
				}
				catch(Exception ex)
				{
					Debugger.Break();
				}
			}
		}
	}
}
