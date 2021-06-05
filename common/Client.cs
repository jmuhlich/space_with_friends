namespace client {
	using Ceras;
	using Ceras.Helpers; // This is where the ceras.WriteToStream extensions are in

	using System;
	using System.Collections.Generic;
	using System.Net.Sockets;
	using System.Threading.Tasks;

	public class Client {
		TcpClient _client;
		NetworkStream _netStream;
		CerasSerializer _sendCeras;
		CerasSerializer _receiveCeras;

		public void Start() {
			// Create network connection	
			log.info( "Create TCP connection" );
			_client = new TcpClient();

			string server = "localhost";
			int port = 7887;

			log.info( $"Connect to {server}:{port}" );
			_client.Connect( "localhost", 7887 );
			// And use a network stream, much more comfortable to use

			log.logProps( _client, "_client Properties" );

			_netStream = _client.GetStream();


			// Now we need our serializer
			// !! Important:
			// !! The settings of the serializers for client and server must be the same!
			var configSend = new SerializerConfig();
			log.logProps( configSend, "configSend Properties" );
			configSend.Advanced.PersistTypeCache = true;

			_sendCeras = new CerasSerializer( configSend );

			var configRecv = new SerializerConfig();
			log.logProps( configRecv, "configRecv Properties" );
			configRecv.Advanced.PersistTypeCache = true;

			_receiveCeras = new CerasSerializer( configRecv );
			// Start a thread that receives and reacts to messages from the server

			log.info( $"Start receiving packets" );
			StartReceiving();
		}

		void SendToAll<T>( T msg ) {
			SendRaw( new msg.SendToAll { Message = msg } );
		}

		void SendToTarget<T>( string target, T msg ) {
			SendRaw( new msg.SendToTarget { Target = target, Message = msg } );
		}


		void StartReceiving() {
			Task.Run( async () => {
				try {
					while (true) {
						// Read until we received the next message from the server
						var msg = await _receiveCeras.ReadFromStream( _netStream );
						HandleMessage( msg );
					}
				}
				catch (Exception e) {
					log.error( "Client error while receiving: " + e );
				}
			} );
		}

		void HandleMessage( object msg ) {
		}

		// A little helper function that sends any object to the server
		void SendRaw( object msg ) => _sendCeras.WriteToStream( _netStream, msg );








		// D E B U G  T E S T I N G
		public void SendExampleObjects() {
			// First thing is sending our login message, and then some random stuff
			SendRaw( new msg.ClientLogin { Name = "riki", Password = "123" } );

			// Since every type of object is supported we could also send strings directly 
			SendRaw( "Hello I'm the client, sending a string directly! :)" );
			// And we can even send numbers and stuff...
			SendRaw( 500.678 );
			SendRaw( DateTime.Now );
			SendRaw( (short)435 );

			// Now lets try a more complicated object
			var bob = new msg.Person { Name = "Bob", Age = 20 };
			var alice = new msg.Person { Name = "Alice", Age = 21 };
			bob.Friends.Add( alice );
			alice.Friends.Add( bob );

			SendRaw( bob );

			// And finally some example for polymorphic types (inheritance)
			List<msg.ISpell> spells = new List<msg.ISpell>();
			spells.Add( new msg.Lightning() );
			spells.Add( new msg.Fireball() );

			SendRaw( spells );
		}









	}
}
