using NFive.SDK.Client.Interface;

namespace StreetPosition.Client.Overlays
{
	public class StreetPositionOverlay : Overlay
	{
		public StreetPositionOverlay(string streetName, OverlayManager manager) : base("StreetPositionOverlay.html", manager)
		{
			Attach("show-street-name", (_, callback) => Send(streetName));
		}
	}
}
