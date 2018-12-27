using JetBrains.Annotations;
using NFive.SDK.Core.Controllers;

namespace StreetPosition.Shared
{
	/// <inheritdoc />
	[PublicAPI]
	public class Configuration : ControllerConfiguration
	{
		public WhenConfiguration When { get; set; } = new WhenConfiguration();

		public DisplayConfiguration Display { get; set; } = new DisplayConfiguration();

		public uint UpdateInterval { get; set; } = 500;

		public string ActivationEvent { get; set; } = string.Empty;
		
		public string Template { get; set; } = @"<div id=""left-section"">
	<span id=""direction"">{ Direction }</span>
</div>

<div id=""right-section"">
	<div id=""top-row"">
		<span id=""street"">{ Street }</span> in <span id=""area"">{ Area }</span>
	</div>
	<div id=""bottom-row"">
		Crossing <span id=""crossing"">{ Crossing }</span>
	</div>
</div>".Replace("\r\n", "\n").Replace("\n\n", "\n"); // Remove double new lines, YAML issue

		[PublicAPI]
		public class WhenConfiguration
		{
			public bool OnFoot { get; set; } = true;
			public bool InVehicle { get; set; } = true;
		}

		[PublicAPI]
		public class DisplayConfiguration
		{
			public bool Street { get; set; } = true;
			public bool Crossing { get; set; } = true;
			public bool Area { get; set; } = true;
			public bool Direction { get; set; } = true;
		}
	}
}
