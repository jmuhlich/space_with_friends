using UnityEngine;

namespace space_with_friends {

	[KSPAddon( KSPAddon.Startup.Flight, once: false )]
	public class space_with_friends : MonoBehaviour {

		public void Start() {
			utils.Log( "starting..." );

			G
			if (FlightGlobals.ActiveVessel == null) {
				utils.Log( "  no active vessel, skipping" );
				return;
			}

			/*
			var controls = FindObjectsOfType<UI_Control>();
			foreach (var module in FlightGlobals.ActiveVessel.vesselModules )
			{
				Log("    module: " + module.name);

				foreach(var field in module.Fields)
				{
					Log("     field: " + field.name);
					field.OnValueModified += onValueModified;
					field.uiControlFlight.onFieldChanged = onFieldChanged;
				}
			}
			*/

		}

		public void OnDestroy() {
		}

		/*
		public void onFieldChanged(BaseField field, object value)
		{
			Log("onFieldChanged: " + field.name + ": " + value);
		}

		public void onValueModified(object value)
		{
			Log("onValueModified: " + value.ToString());
		}
		*/

	}
}
