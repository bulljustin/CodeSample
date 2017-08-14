using DataSync.Handlers;
using DataSync.Helpers;

namespace DataSync.Processors
{
	public class InternalExternalProcessor : ConversionProcessor
	{
		private InternalHandler InternalHandlerInstance { get { return (InternalHandler)InternalHandler; } }
		private ExternalHandler ExternalHandlerInstance { get { return (ExternalHandler)ExternalHandler; } }

		public InternalExternalProcessor(ProcessorSettings settings)
			: base(settings)
		{
			InternalHandler = settings.InternalHandler as InternalHandler;
			ExternalHandler = settings.InternalHandler as ExternalHandler;
		}

		public override void Dispose()
		{
			// any special dispose functionality
			base.Dispose();
		}
	}
}