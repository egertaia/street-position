using NFive.SDK.Client.Interface;

namespace StreetPosition.Client.Overlays
{
	public class StreetPositionOverlay : Overlay
	{
		public StreetPositionOverlay(OverlayManager manager) : base("StreetPositionOverlay.html", manager) { }

		public void Set(string streetName, string crossing, string areaName, string direction)
		{
			Send("set", new {streetName, crossing, areaName, direction });
		}
	}
}
