using DataSync.Handlers;
using DataSync.Helpers;

namespace DataSync.Processors
{
	internal class OtherOptionProcessor : ConversionProcessor
	{
		private InternalHandler InternalHandlerInstance { get { return (InternalHandler)InternalHandler; } }
		private OtherOptionHandler ExternalHandlerInstance { get { return (OtherOptionHandler)ExternalHandler; } }

		public OtherOptionProcessor(ProcessorSettings settings)
			: base(settings)
		{
			InternalHandler = settings.InternalHandler as InternalHandler;
			ExternalHandler = settings.InternalHandler as OtherOptionHandler;
		}

		public override void Dispose()
		{
			// any special dispose functionality
			base.Dispose();
		}
	}
}