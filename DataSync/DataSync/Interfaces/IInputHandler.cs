using Core.Dto;
using System;
using System.Collections.Generic;

namespace DataSync.Interfaces
{
	public interface IInputHandler : IDisposable
	{
		bool GetAll();
		ICollection<DeviceSyncDto> GetDevices();
		ICollection<DeviceSyncDto> GetDevices(IEnumerable<DeviceSyncDto> devices);
	}
}