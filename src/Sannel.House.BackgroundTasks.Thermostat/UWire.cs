using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	internal class UWire : IWire
	{
		private string i2cDevice;
		
		internal UWire()
		{
		}

		public async Task InitAsync()
		{
			var i2cDeviceSelector = I2cDevice.GetDeviceSelector();
			IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
			if(devices.Count > 0)
			{
				i2cDevice = devices[0].Id;
			}
		}

		public async Task<IWireDevice> GetDeviceByIdAsync(byte deviceId)
		{
			var connectionString = new I2cConnectionSettings(deviceId);

			return new UWireDevice(await I2cDevice.FromIdAsync(i2cDevice, connectionString));
		}

		public void Dispose()
		{
		}
	}
}
