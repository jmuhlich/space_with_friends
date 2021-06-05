using UnityEngine;

namespace space_with_friends {

	[KSPAddon( KSPAddon.Startup.AllGameScenes, once: true )]
	public class space_with_friends : MonoBehaviour {

		public void Start() {
			utils.Log( "starting..." );

			utils.Log( "host: " + space_with_friends_settings.instance.host + " port: " + space_with_friends_settings.instance.port );
		}

		public void OnDestroy() {
		}
	}
}
