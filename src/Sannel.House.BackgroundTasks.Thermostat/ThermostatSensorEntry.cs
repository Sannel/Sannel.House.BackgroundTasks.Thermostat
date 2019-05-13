using Newtonsoft.Json;
using Sannel.House.Sensor;
using Sannel.House.SensorCaptureSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	public class ThermostatSensorEntry : ISensorEntry
	{
		private SensorEntry entry;
		internal ThermostatSensorEntry(SensorEntry entry) 
			=> this.entry = entry ?? throw new ArgumentNullException(nameof(entry));

		public Guid Id 
			=> entry.Id;

		public string SensorType 
			=> entry.SensorType;

		public int DeviceId 
			=> entry.DeviceId;

		public long? DeviceMacAddress 
			=> entry.DeviceMacAddress;

		public Guid? DeviceUuid 
			=> entry.DeviceUuid;

		public DateTimeOffset DateCreatedOffset 
			=> DateTime.SpecifyKind(entry.DateCreated.ToUniversalTime(), DateTimeKind.Utc);

		public IDictionary<string, float> Values 
			=> entry.Values;
	}
}
