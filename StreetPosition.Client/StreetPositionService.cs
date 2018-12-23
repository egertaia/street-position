using JetBrains.Annotations;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
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
		public bool ShowDirection;
		public bool ShowStreet;
		public bool ShowCrossing;
		public bool ShowArea;

		public string Format;

		private string lastStreet;
		private string lastArea;
		private string lastCrossing;
		private string lastDirection;

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
			this.ShowDirection = config.ShowDirection;
			this.ShowStreet = config.ShowStreet;
			this.Format = config.Format;

			this.overlay = new StreetPositionOverlay(this.OverlayManager);
			this.overlay.Hide();

			this.Ticks.Attach(OnTick);
		}

		private async Task OnTick()
		{
			// Hide GTA V steet & area name
			Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
			Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);

			//Should overlay show?
			HideOrShowOverlay();
			if (!this.overlay.Visible) return;

			// Save position
			var position = Game.Player.Character.Position;

			// Get new names
			var streetName = World.GetStreetName(position);
			var areaName = World.GetZoneLocalizedName(position);
			var crossing = GetIntersectingStreetName(position);
			var direction = GetDirection(Game.Player.Character);

			// Should I update?
			if (ShouldUpdateLocation(streetName, crossing, areaName, direction))
			{
				UpdateLastValues(streetName, crossing, areaName, direction);
				this.overlay.Set(GetProperHtmlForPosition(streetName, crossing, areaName, direction));
			}

		}

		private static string GetIntersectingStreetName(Vector3 position)
		{
			OutputArgument areaHash = new OutputArgument();
			Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, new OutputArgument(), areaHash);
			return API.GetStreetNameFromHashKey(areaHash.GetResult<uint>());
		}

		private static string GetDirection(Ped ped)
		{
			//TODO return proper direction
			return "N";
		}

		private static string GetProperHtmlForPosition(string streetName, string crossing, string areaName, string direction)
		{
			//TODO: Make tis dependent on the format and different sections that are hidden.
			return string.Format("<div id=\"left-section\"><span id=\"direction\">{0}</span></div><div id=\"right-section\"><div id=\"top-row\"><span id=\"street\">{1}</span> in <span id=\"area\">{2}</span</div><div id=\"bottom-row\">Crossing <span id=\"crossing\">{3}</span></div></div>",
				direction, streetName, areaName, crossing);
		}

		private bool ShouldUpdateLocation(string streetName, string crossingName, string areaName, string direction)
		{
			if (this.ShowStreet && this.lastStreet != streetName) return true;
			if (this.ShowCrossing && this.lastCrossing != crossingName) return true;
			if (this.ShowArea && this.lastArea != areaName) return true;
			if (this.ShowDirection && this.lastDirection != direction) return true;

			return false;
		}

		private void HideOrShowOverlay()
		{
			bool isPlayerInVehicle = Game.Player.Character.IsInVehicle();
			if (this.overlay.Visible && this.DisplayInVehicle && !isPlayerInVehicle) this.overlay.Hide();
			if (this.overlay.Visible && this.DisplayOnFoot && !isPlayerInVehicle) this.overlay.Hide();
			if (!this.overlay.Visible && this.DisplayInVehicle && isPlayerInVehicle) this.overlay.Show();
			if (!this.overlay.Visible && this.DisplayOnFoot && !isPlayerInVehicle) this.overlay.Show();
		}
		
		private void UpdateLastValues(string streetName, string crossing, string areaName, string direction)
		{
			this.lastStreet = streetName;
			this.lastCrossing = crossing;
			this.lastArea = areaName;
			this.lastDirection = direction;
		}
	}
}
