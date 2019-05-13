using Sannel.House.Sensor;
using Sannel.House.SensorCaptureSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	internal class ThermostController : IDisposable
	{
		private readonly ITHPSensor thpSensor;
		private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
		private System.Collections.Concurrent.ConcurrentBag<ISensorEntry> entries = new System.Collections.Concurrent.ConcurrentBag<ISensorEntry>();


		internal float Temperature { get; private set; }
		internal float Pressure { get; private set; }
		internal float RelativeHumidity { get; private set; }
		public Guid SystemId { get; }

		internal ThermostController(ITHPSensor thpSensor)
		{
			this.thpSensor = thpSensor;
			var di = new EasClientDeviceInformation();
			SystemId = di.Id;
		}

		internal async void Start()
		{
			var controller = await GpioController.GetDefaultAsync();
			thpSensor.Begin();

			controller.TryOpenPin()


			while(true)
			{
				readSensor();
				logValues();
				checkAndUpdateStatus();
				await Task.Delay(60000);
			}
		}

		private void readSensor()
		{
			Temperature = thpSensor.GetTemperatureCelsius();
			Pressure = thpSensor.GetPressure();
			RelativeHumidity = thpSensor.GetRelativeHumidity();
		}

		private async void logValues()
		{
			var se = await Task.Run(() => SensorHelper.THP.Create(Temperature, RelativeHumidity, Pressure, SystemId));
			this.entries.Add(new ThermostatSensorEntry(se));
			try
			{
				await semaphoreSlim.WaitAsync();


				if (entries.Count > 25)
				{
					using (var connection = new SensorCaptureConnection())
					{
						if(await connection.ConnectAsync() == Windows.ApplicationModel.AppService.AppServiceConnectionStatus.Success)
						{
							await connection.SendEntriesAsync(this.entries);
							entries.Clear();
						}
					}
				}
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}

		private void checkAndUpdateStatus()
		{
		}

		public void Dispose()
		{
			semaphoreSlim?.Dispose();
		}
	}
}
