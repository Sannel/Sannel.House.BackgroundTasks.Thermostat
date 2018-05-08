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
			var b = new byte[length];
			var t = device.ReadPartial(b).BytesTransferred;
			Array.Copy(b, read, length);
			return b[0];
		}

		public byte ReadByte()
		{
			var b = new byte[1];
			device.Read(b);
			return b[0];
		}

		public void Write(byte b) 
			=> device.Write(new byte[] { b });

		public void Write(byte b1, byte b2) 
			=> device.Write(new byte[] { b1, b2 });

		public void Write(byte b1, byte b2, byte b3) 
			=> device.Write(new byte[] { b1, b2, b3 });

		public void Write(ref byte[] bytes, int length)
		{
			var b = new byte[length];
			Array.Copy(bytes, b, length);
			device.Write(b);
		}

		public byte WriteRead(byte write)
		{
			var b = new byte[1];
			device.WriteRead(new byte[] { write }, b);
			return b[0];
		}

		public void WriteRead(byte write, ref byte[] read, int length)
		{
			var b = new byte[length];
			device.WriteRead(new byte[] { write }, b);
			Array.Copy(b, read, length);
		}

		public void Dispose() 
			=> device.Dispose();
	}
}
