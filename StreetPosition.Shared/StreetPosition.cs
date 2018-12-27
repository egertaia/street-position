using System;
using JetBrains.Annotations;

namespace StreetPosition.Shared
{
	/// <inheritdoc />
	/// <summary>
	/// Represents a street position on the game map.
	/// </summary>
	public class StreetPosition : IEquatable<StreetPosition>
	{
		/// <summary>
		/// Gets or sets the street name.
		/// </summary>
		/// <value>
		/// The street.
		/// </value>
		[CanBeNull]
		public string Street { get; set; }

		/// <summary>
		/// Gets or sets the street crossing name.
		/// </summary>
		/// <value>
		/// The street crossing.
		/// </value>
		[CanBeNull]
		public string Crossing { get; set; }

		/// <summary>
		/// Gets or sets the area name.
		/// </summary>
		/// <value>
		/// The area.
		/// </value>
		[CanBeNull]
		public string Area { get; set; }

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>
		/// The direction.
		/// </value>
		[CanBeNull]
		public string Direction { get; set; }

		/// <inheritdoc />
		public bool Equals(StreetPosition other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(this.Street, other.Street) && string.Equals(this.Crossing, other.Crossing) && string.Equals(this.Area, other.Area) && string.Equals(this.Direction, other.Direction);
		}
	}
}
