using System;

namespace DataSync.Interfaces
{
	public interface IProcessor : IDisposable
	{
		bool ProcessAll();
		bool ProcessDevices();
	}
}