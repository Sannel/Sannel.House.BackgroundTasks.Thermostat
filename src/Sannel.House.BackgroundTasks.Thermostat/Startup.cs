using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.DependencyInjection;
using Sannel.House.Configuration;
using Sannel.House.Sensor;
using Sannel.House.Sensor.Temperature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	public class Startup
	{
		private static Startup current = null;
		private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

		internal ServiceProvider Provider
		{
			get;
			private set;
		}

		internal Config Config
		{
			get;
		} = new Config();

		internal static async Task<Startup> GetStartupAsync()
		{
			await semaphoreSlim.WaitAsync();
			try
			{
				if (current != null)
				{
					return current;
				}

				current = new Startup();
				await current.InitAsync();
				return current;
			}
			catch(Exception ex)
			{
				throw;
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}

		private Startup()
		{

		}

		internal async Task InitAsync()
		{
			var serviceCollection = new ServiceCollection();

			serviceCollection.AddLogging();
			var w = new UWire();
			await w.InitAsync();
			serviceCollection.AddSingleton<IWire>(w);

			var d = await w.GetDeviceByIdAsync(0x77);
			var bme = new BME280(d)
			{
				Filter = BME280.Filters.Coefficients8,
				TemperatureOverSample = BME280.OverSamples.SixTeenTime,
				PressureOverSample = BME280.OverSamples.SixTeenTime,
				HumidityOverSample = BME280.OverSamples.SixTeenTime
			};

			serviceCollection.AddSingleton<ITHPSensor>(bme);
			serviceCollection.AddSingleton(i => new ThermostController(i.GetService<ITHPSensor>()));

			Provider = serviceCollection.BuildServiceProvider();

			using (var con = new ConfigurationConnection())
			{
				if (await con.ConnectAsync() == Windows.ApplicationModel.AppService.AppServiceConnectionStatus.Success)
				{

					var result = await con.GetConfiguration("ThermostatAppSecret",
															"ServerApiUrl",
															"ServerUsername",
															"ServerPassword");
					Uri.TryCreate(result["ServerApiUrl"] as string, UriKind.Absolute, out var tmp);
					Config.ServiceApiUrl = tmp;
					Config.AppSecret = result["ThermostatAppSecret"] as string;
					Config.Username = result["ServerUsername"] as string;
					var ss = new SecureString();
					foreach(var c in result["ServerPassword"] as string ?? "")
					{
						ss.AppendChar(c);
					}
					Config.Password = ss;
				}
			}
			AppCenter.Start(Config.AppSecret, typeof(Analytics));
			Analytics.TrackEvent("Background Task Started");

		}

	}
}
