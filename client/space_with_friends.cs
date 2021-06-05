using UnityEngine;

namespace space_with_friends {

	[KSPAddon( KSPAddon.Startup.Flight, once: false )]
	public class space_with_friends : MonoBehaviour {

		public void Start() {
			utils.Log( "starting..." );

			if (FlightGlobals.ActiveVessel == null) {
				utils.Log( "  no active vessel, skipping" );
				return;
			}
		}

		public void OnDestroy() {
		}
	}
}
