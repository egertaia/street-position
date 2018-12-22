using JetBrains.Annotations;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using System.Threading.Tasks;
using StreetPosition.Client.Overlays;
using StreetPosition.Shared;

namespace StreetPosition.Client
{
	[PublicAPI]
	public class StreetPositionService : Service
	{
		private StreetPositionOverlay overlay;

		public bool DisplayInVehicle;
		public bool DisplayOnFoot;
		public bool ShowHeading;
		public bool ShowStreet;
		public bool ShowCrossing;
		public bool ShowArea;

		private string lastStreet;
		private string lastArea;
		private string lastCrossing;
		private string lastHeading;

		public StreetPositionService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, overlay, user)
		{
		}

		public override async Task Started()
		{
			var config = await this.Rpc.Event(StreetPositionEvents.GetConfig).Request<Configuration>();
			this.DisplayInVehicle = config.DisplayInVehicle;
			this.DisplayOnFoot = config.DisplayOnFoot;
			this.ShowArea = config.ShowArea;
			this.ShowCrossing = config.ShowCrossing;
			this.ShowHeading = config.ShowHeading;
			this.ShowStreet = config.ShowStreet;

			this.overlay = new StreetPositionOverlay(this.OverlayManager);

			this.Ticks.Attach(OnTick);
		}

		//<div id ="heading">{heading}</div>
		//<div id="street-container">
		//	<div>
		//		<span id = "street" >{street}</span><span id ="crossing">- {crossing}</span>
		//	</div>
		//	<div id ="area">{area}</div>
		//</div>

		private async Task OnTick()
		{
			// Hide GTA V steet & area name
			Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
			Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);

			// Save position
			var position = Game.Player.Character.Position;

			// Get new Street & Area name
			var streetName = World.GetStreetName(position);
			var areaName = World.GetZoneLocalizedName(position);
		
			if (this.lastStreet == streetName && this.lastArea == areaName) return;
			this.lastStreet = streetName;
			this.lastArea = areaName;

			this.overlay.Set(streetName, areaName);
		}

	}
}
