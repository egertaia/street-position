using StreetPosition.Shared;
using JetBrains.Annotations;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rpc;
using NFive.SDK.Core.Diagnostics;

namespace StreetPosition.Server
{
	[PublicAPI]
	public class StreetPositionController : ConfigurableController<Configuration>
	{
		public StreetPositionController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event(StreetPositionEvents.GetConfig).On(e => e.Reply((IConfiguration)this.Configuration));
		}
	}
}
