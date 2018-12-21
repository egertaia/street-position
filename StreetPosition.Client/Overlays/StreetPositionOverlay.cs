using NFive.SDK.Client.Interface;

namespace StreetPosition.Client.Overlays
{
	public class StreetPositionOverlay : Overlay
	{
		public StreetPositionOverlay(OverlayManager manager) : base("StreetPositionOverlay.html", manager) { }

		public void Set(string streetName, string areaName)
		{
			Send("set", new {streetName, areaName });
		}
	}
}
