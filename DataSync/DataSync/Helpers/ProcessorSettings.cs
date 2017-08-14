using DataSync.Interfaces;

namespace DataSync.Helpers
{
	public class ProcessorSettings
	{
		public IBaseHandler InternalHandler { get; set; }
		public IBaseHandler ExternalHandler { get; set; }
		public bool ProcessDevices { get; set; } = true;
		public bool Prefetch { get; set; } = true;
	}
}