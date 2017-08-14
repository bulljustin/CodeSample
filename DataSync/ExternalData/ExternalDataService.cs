using Core.Dto;
using Core.Interfaces;
using System.Collections.Generic;

namespace ExternalData
{
	public class ExternalDataService : IDataService<DeviceSyncDto>
	{
		/// <summary>
		/// get a list of devices based on the filter
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<DeviceSyncDto> Get(IFilter filter)
		{
			return new List<DeviceSyncDto>();
		}

		/// <summary>
		/// Get a specific device based on the ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DeviceSyncDto Get(long id)
		{
			return new DeviceSyncDto();
		}

		/// <summary>
		/// Create or update a device's information
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Save(DeviceSyncDto item)
		{
			return true;
		}

		public bool Save(IEnumerable<DeviceSyncDto> item)
		{
			return true;
		}

		/// <summary>
		/// Deletes a device from the data store
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(long id)
		{
			return true;
		}

		public void Dispose()
		{
			//nothing to dispose yet
		}
	}
}