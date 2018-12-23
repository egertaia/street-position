using JetBrains.Annotations;

namespace StreetPosition.Shared
{
	[PublicAPI]
	public interface IConfiguration
	{
		bool DisplayOnFoot { get; set; }

		bool DisplayInVehicle { get; set; }

		bool ShowStreet { get; set; }

		bool ShowCrossing { get; set; }

		bool ShowArea { get; set; }

		bool ShowDirection { get; set; }

		string Format { get; set; }
	}
}
