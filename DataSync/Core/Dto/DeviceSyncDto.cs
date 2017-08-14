using Core.Interfaces;
using System;

namespace Core.Dto
{
	public class DeviceSyncDto : ITransferDto<DeviceSyncDto>
	{
		public long Id { get; set; }
		public string ExternalId { get; set; }
		public string Description { get; set; }
		public DateTime? LastModified { get; set; }
		public DateTime? LastSynced { get; set; }

		public DeviceSyncDto Clone()
		{
			return this.Clone();
		}

		#region Equality members

		public bool Equals(DeviceSyncDto other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Id == other.Id &&
				string.Equals(ExternalId, other.ExternalId) &&
				string.Equals(Description, other.Description) &&
				LastModified.Equals(other.LastModified) &&
				LastSynced.Equals(other.LastSynced);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((DeviceSyncDto)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = (hashCode * 397) ^ (ExternalId != null ? ExternalId.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ LastModified.GetHashCode();
				hashCode = (hashCode * 397) ^ LastSynced.GetHashCode();
				return hashCode;
			}
		}

		#endregion Equality members
	}
}