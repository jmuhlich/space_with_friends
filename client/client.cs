
namespace space_with_friends {

	using Ceras;
	using Ceras.Helpers; // This is where the ceras.WriteToStream extensions are in
	using System;
	using System.Collections.Generic;
	using System.Net.Sockets;
	using System.Threading.Tasks;

	public class Client : swf_common.ClientBase {

		/*
		public override void HandleMessage( object obj ) {
			Console.WriteLine( $"[Client] Received a '{obj.GetType().Name}': {obj}" );
		}
		*/
	}
}