using JetBrains.Annotations;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Threading.Tasks;
using StreetPosition.Client.Overlays;

namespace StreetPosition.Client
{
	[PublicAPI]
	public class StreetPositionService : Service
	{
		private StreetPositionOverlay overlay;
		private string lastStreet;
		private string lastArea;

		public StreetPositionService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, overlay, user)
		{
			this.overlay = new StreetPositionOverlay(this.OverlayManager);

			this.Ticks.Attach(OnTick);
		}

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
