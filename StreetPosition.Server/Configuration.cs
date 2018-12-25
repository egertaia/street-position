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
		public bool ShowDirection { get; set; } = true;
		public string Format { get; set; } = "<div id=\"left-section\"><span id=\"direction\">{direction}</span></div><div id=\"right-section\"><div id=\"top-row\"><span id=\"street\">{street}</span> in <span id=\"area\">{area}</span></div><div id=\"bottom-row\">Crossing <span id=\"crossing\">{crossing}</span></div></div>";
	}
}
