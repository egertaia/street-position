using StreetPosition.Shared;
using NFive.SDK.Core.Controllers;

namespace StreetPosition.Server
{
	public class Configuration : ControllerConfiguration, IConfiguration
	{
		public bool DisplayOnFoot { get; set; } = true;
		public bool DisplayInVehicle { get; set; } = true;
		public bool ShowStreet { get; set; } = true;
		public bool ShowCrossing { get; set; } = true;
		public bool ShowArea { get; set; } = true;
		public bool ShowDirection { get; set; } = false;
		public string Format { get; set; } = "{heading} | {street} in {area}\n Crossing {area}";
	}
}
