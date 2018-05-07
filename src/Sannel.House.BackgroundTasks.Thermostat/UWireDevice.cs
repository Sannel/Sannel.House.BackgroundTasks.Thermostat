using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	internal class UWireDevice : IWireDevice
	{
		private I2cDevice device;
		internal UWireDevice(I2cDevice device)
		{
			this.device = device ?? throw new ArgumentNullException(nameof(device));
		}

		public uint Read(ref byte[] read, int length)
		{
			throw new NotImplementedException();
		}

		public byte ReadByte()
		{
			throw new NotImplementedException();
		}

		public void Write(byte b)
		{
			throw new NotImplementedException();
		}

		public void Write(byte b1, byte b2)
		{
			throw new NotImplementedException();
		}

		public void Write(byte b1, byte b2, byte b3)
		{
			throw new NotImplementedException();
		}

		public void Write(ref byte[] bytes, int length)
		{
			throw new NotImplementedException();
		}

		public byte WriteRead(byte write)
		{
			throw new NotImplementedException();
		}

		public void WriteRead(byte write, ref byte[] read, int length)
		{
			throw new NotImplementedException();
		}
		public void Dispose()
		{
		}
	}
}
