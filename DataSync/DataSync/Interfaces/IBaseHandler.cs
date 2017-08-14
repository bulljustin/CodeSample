using Core.Dto;
using System.Collections.Generic;
using DataSync.Helpers;

namespace DataSync.Interfaces
{
	public interface IBaseHandler : IInputHandler, IOutputHandler
	{
		ProcessorSettings Settings { get; set; }
		IEnumerable<DeviceSyncDto> Devices { get; set; }
	}
}