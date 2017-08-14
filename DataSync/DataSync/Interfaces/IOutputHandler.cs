using System;
using System.Collections.Generic;
using Core.Dto;

namespace DataSync.Interfaces
{
	public interface IOutputHandler : IDisposable
	{
		bool SaveAll();
		bool SaveDevices();
		bool SaveDevices(IEnumerable<DeviceSyncDto> devices);
	}
}