namespace StreetPosition.Shared
{
	public class Configuration : IConfiguration
	{
		public bool DisplayOnFoot { get; set; }
		public bool DisplayInVehicle { get; set; }
		public bool ShowStreet { get; set; }
		public bool ShowCrossing { get; set; }
		public bool ShowArea { get; set; }
		public bool ShowDirection { get; set; }
		public string Format { get; set; }
	}
}
