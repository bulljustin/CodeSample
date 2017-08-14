using Core.Dto;
using Core.Filters;
using Core.Interfaces;
using DataSync.Helpers;
using DataSync.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSync.Handlers
{
	public class InternalHandler : IBaseHandler
	{
		public IDataService<DeviceSyncDto> ServiceHandler { get; set; }
		public DateTime LastRunDate { get; set; }
		public ProcessorSettings Settings { get; set; }
		public IEnumerable<DeviceSyncDto> Devices { get; set; }

		public void Dispose()
		{
			ServiceHandler.Dispose();
		}

		public bool GetAll()
		{
			Parallel.Invoke(
				() => GetDevices()
			);
			return Devices.Any();
		}

		public ICollection<DeviceSyncDto> GetDevices()
		{
			var filter = new DeviceFilter();
			return ServiceHandler.Get(filter).ToList();
		}

		public ICollection<DeviceSyncDto> GetDevices(IEnumerable<DeviceSyncDto> devices)
		{
			var filter = new DeviceFilter
			{
				Ids = devices.Select(s => s.Id)
			};
			return ServiceHandler.Get(filter).ToList();
		}

		public bool SaveAll()
		{
			Parallel.Invoke(
				() => SaveDevices()
			);
			return true;
		}

		public bool SaveDevices()
		{
			return ServiceHandler.Save(Devices);
		}

		public bool SaveDevices(IEnumerable<DeviceSyncDto> devices)
		{
			Devices = devices;
			return ServiceHandler.Save(Devices);
		}

		public void SetDevices(IEnumerable<DeviceSyncDto> devices)
		{
			Devices = devices;
		}
	}
}