using Core.Interfaces;
using System;

namespace Core.Model
{
	public class Device : IDevice
	{
		public long Id { get; set; }
		public string Description { get; set; }
		public DateTime LastModified { get; set; }
		public DateTime LastSynced { get; set; }
	}
}