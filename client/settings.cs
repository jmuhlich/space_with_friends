using System.Collections;
using System.Reflection;

namespace space_with_friends {
	public class Settings : GameParameters.CustomParameterNode {
		public override string Title { get { return "General"; } }
		public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
		public override string Section { get { return "space_with_friends"; } }
		public override string DisplaySection { get { return "Space With Friends"; } }
		public override int SectionOrder { get { return 1; } }
		public override bool HasPresets { get { return true; } }


		[GameParameters.CustomStringParameterUI( "host", toolTip = "The IP/name of the server" )]
		public string host = "localhost";

		[GameParameters.CustomStringParameterUI( "port", toolTip = "The server port" )]
		public string port = "7887";

		public override void SetDifficultyPreset( GameParameters.Preset preset ) { }
		public override bool Enabled( MemberInfo member, GameParameters parameters ) { return true; }
		public override bool Interactible( MemberInfo member, GameParameters parameters ) { return true; }
		public override IList ValidValues( MemberInfo member ) { return null; }
	}
}