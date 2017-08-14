using Core.Dto;
using DataSync.Helpers;
using DataSync.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataSync.Processors
{
	public abstract class ConversionProcessor : IProcessor
	{
		protected IBaseHandler InternalHandler;
		protected IBaseHandler ExternalHandler;
		protected DateTime LastRunDate;
		protected DateTime CurrentRunDate = DateTime.Now;
		protected bool Success;
		protected ProcessorSettings Settings { get; set; }
		protected bool HasUpdates { get; set; }

		protected ConversionProcessor(ProcessorSettings settings)
		{
			Settings = settings;
			InternalHandler = settings.InternalHandler;
			ExternalHandler = settings.ExternalHandler;
			Initialize();
		}

		protected void Initialize()
		{
			if (Settings.Prefetch)
			{
				bool countInternal = false, countExternal = false;
				Parallel.Invoke(
					() => { countInternal = InternalHandler.GetAll(); },
					() => { countExternal = ExternalHandler.GetAll(); }
				);
				HasUpdates = countInternal || countExternal;
			}
			else
			{
				HasUpdates = true;
			}
		}

		public virtual bool ProcessAll()
		{
			if (!HasUpdates)
			{
				Success = true;
			}
			else
			{
				//if the config says to not process it, then let the variables say it has already been processed
				bool devicesProcessed = false;

				try
				{
					Parallel.Invoke(() => devicesProcessed = ProcessDevices());
				}
				catch (Exception ex)
				{
					//handle error
				}

				Success = devicesProcessed;
			}
			return Success;
		}

		public virtual bool ProcessDevices()
		{
			if (!Settings.ProcessDevices)
			{
				return true;
			}
			// get the national accounts from the two groups
			var internalEditedDevices = InternalHandler.GetDevices().ToList();
			var externalEditedDevices = ExternalHandler.GetDevices().ToList();

			var missingExternal = internalEditedDevices.Where(w => !string.IsNullOrEmpty(w.ExternalId) && !externalEditedDevices.Select(s => s.ExternalId).Contains(w.ExternalId)).ToList();
			if (missingExternal.Any())
			{
				externalEditedDevices.AddRange(ExternalHandler.GetDevices(missingExternal));
			}

			var missingInternal = externalEditedDevices.Where(w => w.Id > 0 && !internalEditedDevices.Select(s => s.Id).Contains(w.Id)).ToList();
			if (missingInternal.Any())
			{
				internalEditedDevices.AddRange(InternalHandler.GetDevices(missingInternal));
			}

			var combinedDevices = TransferDtoHelper<DeviceSyncDto>.CombineTransferDtos(internalEditedDevices, externalEditedDevices);
			if (combinedDevices.Combined.Count == 0)
			{
				return true;
			}

			//save them all
			bool down = false, up = false;
			var tasks = new[]
			{
				Task.Run(() => { down = ExternalHandler.SaveDevices(combinedDevices.ExternalProcessed); }),
				Task.Run(() => { up = InternalHandler.SaveDevices(combinedDevices.InternalProcessed); })
			};
			Task.WaitAll(tasks);

			return down && up;
		}

		public virtual void Dispose()
		{
			InternalHandler.Dispose();
			ExternalHandler.Dispose();
		}
	}
}