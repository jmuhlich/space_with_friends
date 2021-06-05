namespace space_with_friends {

	[KSPAddon( KSPAddon.Startup.Flight, once: false )]
	public class RPMBridge {
		public void Start() {
			JSI.Core.Events.onEvent += on_event;
		}

		public void OnDestroy() {
			JSI.Core.Events.onEvent -= on_event;
		}

		public static void on_event( JSI.Core.Event _event ) {
			if (!(_event.name.StartsWith( "click:" ) || _event.name.StartsWith( "release:" ))) {
				return;
			}

			utils.Log( "sending: " + _event.name );
		}

		void on_network_message( byte[] data ) {
			string message;
			try {
				message = System.Text.Encoding.ASCII.GetString( data );
			}
			catch (System.Exception ex) {
				UnityEngine.Debug.LogError( ex.StackTrace );
				return;
			}

			if (!(message.StartsWith( "click:" ) || message.StartsWith( "release:" ))) {
				return;
			}

			utils.Log( "replaying: " + message );

			JSI.SmarterButton.Replay( message );
		}
	}
}

