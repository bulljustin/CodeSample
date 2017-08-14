using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
	public interface IFilter
	{
		IEnumerable<long> Ids { get; set; }
		string Description { get; set; }
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
	}
}