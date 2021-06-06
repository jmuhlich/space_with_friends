
namespace headless {

	using Ceras;
	using Ceras.Helpers; // This is where the ceras.WriteToStream extensions are in
	using System;
	using System.Collections.Generic;
	using System.Net.Sockets;
	using System.Threading.Tasks;
	using space_with_friends.msg;

	public class Client : swf_common.ClientBase {

		public override void HandleMessage( object obj ) {
			log.info( $"Received a '{obj.GetType().Name}': {obj}" );
		}

		// D E B U G  T E S T I N G
		public void sendExampleObjects() {
			//*
			// First thing is sending our login message, and then some random stuff
			sendRaw( new ClientLogin { Name = "riki", Password = "123" } );

			// Since every type of object is supported we could also send strings directly 
			sendRaw( "Hello I'm the client, sending a string directly! :)" );
			// And we can even send numbers and stuff...
			sendRaw( 500.678 );
			sendRaw( DateTime.Now );
			sendRaw( (short)435 );

			// Now lets try a more complicated object
			var bob = new Person { Name = "Bob", Age = 20 };
			var alice = new Person { Name = "Alice", Age = 21 };
			bob.Friends.Add( alice );
			alice.Friends.Add( bob );

			sendRaw( bob );

			// And finally some example for polymorphic types (inheritance)
			List<ISpell> spells = new List<ISpell>();
			spells.Add( new Lightning() );
			spells.Add( new Fireball() );

			sendRaw( spells );
			//*/
		}

	}
}