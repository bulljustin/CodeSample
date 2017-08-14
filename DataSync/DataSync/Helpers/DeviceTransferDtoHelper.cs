using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSync.Helpers
{
	public class DeviceTransferDtoHelper : TransferDtoHelper<DeviceSyncDto>
	{
		public new static DeviceSyncDto FindExisting(IEnumerable<DeviceSyncDto> searchList, DeviceSyncDto finder)
		{
			var found = searchList.FirstOrDefault(w => w.Id == finder.Id);
			return found?.Clone();
		}

		public new static DeviceSyncDto Update(IEnumerable<DeviceSyncDto> searchList, DeviceSyncDto updater)
		{
			var existing = FindExisting(searchList, updater);

			if (existing != null)
			{
				if (!existing.LastModified.HasValue || existing.LastModified < (updater.LastModified ?? DateTime.Now))
				{
					if (existing.Id == 0)
					{
						existing.Id = updater.Id;
					}
					if (!string.IsNullOrEmpty(updater.ExternalId))
					{
						existing.ExternalId = updater.ExternalId;
					}
					existing.Description = updater.Description;
					existing.LastModified = updater.LastModified;
					existing.LastSynced = updater.LastSynced;
				}
			}
			return existing;
		}
	}
}