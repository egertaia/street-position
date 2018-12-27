using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using JetBrains.Annotations;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using StreetPosition.Client.Extensions;
using StreetPosition.Client.Overlays;
using StreetPosition.Shared;

namespace StreetPosition.Client
{
	/// <inheritdoc />
	[PublicAPI]
	public class StreetPositionService : Service
	{
		private Configuration config;
		private StreetPositionOverlay overlay;
		private int lastUpdate;
		private Shared.StreetPosition last;

		/// <inheritdoc />
		public StreetPositionService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, overlay, user) { }

		/// <inheritdoc />
		public override async Task Started()
		{
			// Request server config
			this.config = await this.Rpc.Event(StreetPositionEvents.GetConfig).Request<Configuration>();

			// Update local config on server config change
			this.Rpc.Event(StreetPositionEvents.GetConfig).On<Configuration>((e, c) => this.config = c);

			// Create overlay
			this.overlay = new StreetPositionOverlay(this.OverlayManager);

			if (!string.IsNullOrWhiteSpace(this.config.ActivationEvent))
			{
				this.Logger.Debug($"Attaching to event: {this.config.ActivationEvent}");
				this.Rpc.Event(this.config.ActivationEvent.Trim()).On(e =>
				{
					this.Logger.Debug($"Attaching Tick");
					this.Ticks.Attach(OnTick);
				});
			}
			else
			{
				this.Logger.Debug($"Attaching Tick");
				// Run every frame
				this.Ticks.Attach(OnTick);
			}
		}

		private async Task OnTick()
		{
			// Hide stock street & area name
			Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
			Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);

			// Update twice a second
			if (Game.GameTime < this.lastUpdate + 500) return;

			this.lastUpdate = Game.GameTime;

			if (Game.Player == null) return;

			// Cache character
			var character = Game.Player.Character;
			if (character == null) return;

			// Should display overlay
			DisplayOverlay(character.IsInVehicle());

			if (!this.overlay.Visible) return;

			// Cache position
			var position = character.Position;

			// Get new values
			var current = new Shared.StreetPosition
			{
				Street = this.config.Display.Street ? World.GetStreetName(position) : string.Empty,
				Crossing = this.config.Display.Crossing ? GetIntersectingStreetName(position) : string.Empty,
				Area = this.config.Display.Area ? World.GetZoneLocalizedName(position) : string.Empty,
				Direction = this.config.Display.Direction ? GetDirection(character.Heading) : string.Empty
			};

			// Check for changed values
			if (this.last == null || !current.Equals(this.last))
			{
				this.last = current;

				// Update overlay
				this.overlay.Set(this.config.Template.FormatWith(new
				{
					current.Street,
					current.Crossing,
					current.Area,
					current.Direction
				}));
			}
		}

		private void DisplayOverlay(bool inVehicle)
		{
			if (this.overlay.Visible)
			{
				if (!this.config.When.InVehicle && !this.config.When.OnFoot)
				{
					this.overlay.Hide();
					return;
				}

				if (inVehicle && !this.config.When.InVehicle)
				{
					this.overlay.Hide();
					return;
				}

				if (!inVehicle && !this.config.When.OnFoot)
				{
					this.overlay.Hide();
				}

				return;
			}

			if (this.config.When.InVehicle && this.config.When.OnFoot)
			{
				this.overlay.Show();
				return;
			}

			if (inVehicle && this.config.When.InVehicle)
			{
				this.overlay.Show();
				return;
			}

			if (!inVehicle && this.config.When.OnFoot)
			{
				this.overlay.Show();
			}
		}

		private static string GetIntersectingStreetName(Vector3 position)
		{
			var areaHash = new OutputArgument();
			Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, new OutputArgument(), areaHash);
			return API.GetStreetNameFromHashKey(areaHash.GetResult<uint>());
		}

		private static string GetDirection(float heading)
		{
			var deg = 360 - heading;

			if (deg >= 22.5 && deg < 67.5) return "NE";
			if (deg >= 67.5 && deg < 112.5) return "E";
			if (deg >= 112.5 && deg < 157.5) return "SE";
			if (deg >= 157.5 && deg < 202.5) return "S";
			if (deg >= 202.5 && deg < 247.5) return "SW";
			if (deg >= 247.5 && deg < 292.5) return "W";
			if (deg >= 292.5 && deg < 337.5) return "NW";
			return "N";
		}
	}
}
