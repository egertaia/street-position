using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;
using StreetPosition.Shared;

namespace StreetPosition.Server
{
	/// <inheritdoc />
	[PublicAPI]
	public class StreetPositionController : ConfigurableController<Configuration>
	{
		/// <inheritdoc />
		public StreetPositionController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration)
		{
			this.Rpc.Event(StreetPositionEvents.GetConfig).On(e => e.Reply(this.Configuration));
		}

		/// <inheritdoc />
		public override void Reload(Configuration configuration)
		{
			this.Rpc.Event(StreetPositionEvents.GetConfig).Trigger(configuration);
		}
	}
}
