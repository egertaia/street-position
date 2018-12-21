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
		private string lastValue;

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

			// Get new Street & Area name
			var streetName = World.GetStreetName(Game.Player.Character.Position);
			var areaName = GetAreaName(Game.Player.Character.Position);
			if (!string.IsNullOrWhiteSpace(areaName)) streetName += $" & {areaName}";

			if (this.lastValue == streetName) return;

			this.lastValue = streetName;

			this.overlay.Set(streetName);
		}

		private static string GetAreaName(Vector3 position)
		{
			OutputArgument areaHash = new OutputArgument();
			Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, new OutputArgument(), areaHash);
			return API.GetStreetNameFromHashKey(areaHash.GetResult<uint>());
		}
	}
}
