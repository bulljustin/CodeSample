using System;

namespace Core.Interfaces
{
	public interface ITransferDto<T> : IEquatable<T>
	{
		long Id { get; set; }
		string ExternalId { get; set; }
		string Description { get; set; }
		DateTime? LastModified { get; set; }
		DateTime? LastSynced { get; set; }

		T Clone();

		int GetHashCode();
	}
}