using System;

namespace Core.Interfaces
{
	public interface IDevice
	{
		long Id { get; set; }
		string Description { get; set; }
		DateTime LastModified { get; set; }
		DateTime LastSynced { get; set; }
	}
}