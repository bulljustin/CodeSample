using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.Filters
{
	public class DeviceFilter : IFilter
	{
		public IEnumerable<long> Ids { get; set; }
		public string Description { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public DeviceFilter()
		{
			Ids = new HashSet<long>();
		}
	}
}