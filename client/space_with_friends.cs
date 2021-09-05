using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Timers;

namespace space_with_friends {

	[KSPAddon( KSPAddon.Startup.FlightEditorAndKSC, once: true )]
	public class Core : MonoBehaviour {

		public static space_with_friends.Client client;
		public static string player_id;

		public void Start() {
			utils.Log( "starting" );

			if ( space_with_friends_settings.instance.host != "" ) {
				if ( client != null ) {
					return;
				}

				client = new space_with_friends.Client();

				utils.Log( "connecting" );
				utils.Log( "host: " + space_with_friends_settings.instance.host + " port: " + space_with_friends_settings.instance.port );
				client.connect( space_with_friends_settings.instance.host, space_with_friends_settings.instance.port );

				// default to the device id
				player_id = SystemInfo.deviceUniqueIdentifier;

				string player_id_file = Path.GetFullPath( Path.Combine( KSPUtil.ApplicationRootPath, "space_with_friends_player_id.txt" ) );
				if ( File.Exists( player_id_file ) ) {
					List<string> lines = (List<string>)File.ReadLines( player_id_file );
					if ( lines.Count > 0 ) {
						player_id = lines[ 0 ].TrimStart().TrimEnd();
					}
				}

				client.broadcast( new msg.login { player_id = player_id } );
			}
		}
		public void OnDestroy() {
			utils.Log( "stopping" );

			if ( client != null ) {
				utils.Log( "disconnecting" );
				client.broadcast( new msg.logout { player_id = player_id } );
				client.disconnect();
				client = null;
				utils.Log( "disconnected" );
			}
		}
	}

	// [KSPAddon( KSPAddon.Startup.Flight, once: false )]
	// public class PositionTest : MonoBehaviour {

	// 	private float secondsSinceUpdate;

	// 	private void Log(string msg) {
	// 		utils.Log($"PR: {msg}");
	// 	}

	// 	void Start() {
	// 		Log("Start");
	// 		// switch (vessel.vesselType) {
	// 		// 	case VesselType.Debris:
	// 		// 	case VesselType.SpaceObject:
	// 		// 	// TODO shut down instance for this vessel (?);
	// 		// }
	// 	}

	// 	public void Update() {

	// 		secondsSinceUpdate += Time.deltaTime;
	// 		if (secondsSinceUpdate < 20.0f) {
	// 			return;
	// 		}
	// 		secondsSinceUpdate = 0.0f;

	// 		if ( FlightGlobals.fetch == null ) {
	// 			Log( "FlightGlobals not ready" );
	// 		}
	// 		if ( FlightGlobals.ActiveVessel == null ) {
	// 			Log( "no active vessel, skipping" );
	// 			return;
	// 		}

	// 		Log( "moving vessel to random orbit" );
	// 		var vessel = FlightGlobals.ActiveVessel;
	// 		var o = Orbit.CreateRandomOrbitAround(vessel.mainBody);
	// 		Log($"New orbit\n=============\n  INC: {o.inclination}\n  ECC: {o.eccentricity}\n  SMA: {o.semiMajorAxis}\n  LAN: {o.LAN}\n  APE: {o.argumentOfPeriapsis}\n  MEP: {o.epoch}\n  EPO: {Planetarium.GetUniversalTime()}\n  BOD: {vessel.mainBody}\n=============\n");
	// 		FlightGlobals.fetch.SetShipOrbit(o.referenceBody.flightGlobalsIndex, o.eccentricity, o.semiMajorAxis, o.inclination, o.LAN, o.meanAnomalyAtEpoch, o.argumentOfPeriapsis, Planetarium.GetUniversalTime());
	// 		// // Prepare for setting orbit.
	// 		// vessel.Landed = false;
	// 		// vessel.Splashed = false;
	// 		// vessel.SetLandedAt(string.Empty, null, string.Empty);
	// 		// vessel.KillPermanentGroundContact();
	// 		// vessel.ResetGroundContact();
	// 		// // Set new orbit.
	// 		// utils.Log("PT: Setting orbit");
	// 		// vessel.orbit.SetOrbit(o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, Planetarium.GetUniversalTime(), vessel.orbit.referenceBody);
	// 		// // Update things after setting orbit.
	// 		// utils.Log("PT: Performing updates");
	// 		// vessel.orbitDriver.updateFromParameters();
	// 		// CollisionEnhancer.bypass = true;
	// 		// FloatingOrigin.SetOffset(vessel.transform.position);
	// 		// OrbitPhysicsManager.CheckReferenceFrame();
	// 		// OrbitPhysicsManager.HoldVesselUnpack(10);
	// 		// vessel.IgnoreGForces(20);
	// 		// vessel.IgnoreSpeed(20);

	// 	}

	// }

	[KSPAddon( KSPAddon.Startup.EveryScene, once: false )]
	public class PositionReporter : MonoBehaviour {

		private float secondsSinceUpdate;

		private void Log(string msg) {
			utils.Log($"PosRep: {msg}");
		}

		void LateUpdate()
		{
			secondsSinceUpdate += Time.deltaTime;
			if (secondsSinceUpdate > 1.0f)
			{
				secondsSinceUpdate = 0.0f;
				Log($"Scene: {HighLogic.LoadedScene}");
				if (FlightGlobals.fetch != null) {
					Log("=== Vessel list ===");
					foreach (Vessel vessel in FlightGlobals.Vessels) {
						switch (vessel.vesselType) {
							case VesselType.Debris:
							case VesselType.SpaceObject:
							continue;
						}
						var name = vessel.GetDisplayName();
						var body = vessel.orbit.referenceBody.bodyName;
						var type = vessel.vesselType;
						var situation = vessel.SituationString;
						var state = vessel.state;
						var loaded = vessel.loaded;
						var packed = vessel.packed;
						var controlLevel = vessel.CurrentControlLevel;
						var o = vessel.orbit;
						//var activeMsg = vessel == FlightGlobals.ActiveVessel ? " (active)" : "";
						//Log($"{name} -- BOD:{body} TYP:{type} SIT:{situation} ST:{state} CL:{controlLevel} {activeMsg}");
						Log($"{name} -- TYP:{type} SIT:{situation} ST:{state} LO:{loaded} PA:{packed} CL:{controlLevel}");
						//Log($"    INC:{o.inclination:G3} ECC:{o.eccentricity:G3} SMA:{o.semiMajorAxis:G3} LAN:{o.LAN:G3} APE:{o.argumentOfPeriapsis:G3} MEP:{o.epoch:G3}");
					}
					Log("");
				}
			}
		}

		// void Update() {
		// 	Log( "Update" );
		// }

		void Start() {
			Log( "Start" );
		}

		void Awake() {
			Log( "Awake" );
		}

		void OnDestroy() {
			Log( "OnDestroy" );
		}

		void OnEnable() {
			Log( "OnEnable" );
		}

		void OnDisable() {
			Log( "OnDisable" );
		}

	}


	[KSPAddon( KSPAddon.Startup.EveryScene, once: false )]
	public class RandomShipSpawner : MonoBehaviour {

		private float secondsSinceUpdate;

		private void Log(string msg) {
			utils.Log($"RSS: {msg}");
		}

		void Update()
		{
			secondsSinceUpdate += Time.deltaTime;
			if (secondsSinceUpdate > 20.0f && HighLogic.CurrentGame != null)
			{
				secondsSinceUpdate = 0.0f;
				Log("Spawning pod in random orbit around Kerbin");
				var o = Orbit.CreateRandomOrbitAround(FlightGlobals.GetBodyByName("Kerbin"));
				Log($"    INC:{o.inclination:G3} ECC:{o.eccentricity:G3} SMA:{o.semiMajorAxis:G3} LAN:{o.LAN:G3} APE:{o.argumentOfPeriapsis:G3} MEP:{o.meanAnomalyAtEpoch:G3}");
				var partId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
				var partNode = ProtoVessel.CreatePartNode("mk1pod.v2", partId, new ProtoCrewMember[]{});
				var num = new KSPRandom().Next(1, 1000);
				var protoVesselNode = ProtoVessel.CreateVesselNode($"{num} pod", VesselType.Ship, o, 0, new ConfigNode[]{ partNode }, new ConfigNode[]{});
				protoVesselNode.AddValue("prst", true);
				HighLogic.CurrentGame.AddVessel(protoVesselNode);
			}
		}

	}
}
