namespace space_with_friends {
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

		public void connect( string host, UInt16 port ) {
			// Create network connection	
			_client = new TcpClient();
			_client.Connect( host, port );

			// And use a network stream, much more comfortable to use
			_netStream = _client.GetStream();

			// Now we need our serializer
			// !! Important:
			// !! The settings of the serializers for client and server must be the same!
			var configSend = new SerializerConfig();
			configSend.Advanced.PersistTypeCache = true;

			_sendCeras = new CerasSerializer( configSend );

			var configRecv = new SerializerConfig();
			configRecv.Advanced.PersistTypeCache = true;

			_receiveCeras = new CerasSerializer( configRecv );

			// Start a thread that receives and reacts to messages from the server
			StartReceiving();
		}

		public void disconnect() {

			_netStream.Flush();
			_netStream.Close();
			_netStream.Dispose();
			_netStream = null;

			_client.Close();
			_client = null;
		}

		public void send( object obj ) => _sendCeras.WriteToStream( _netStream, obj );

		void StartReceiving() {
			Task.Run( async () => {
				try {
					while (true) {
						// Read until we received the next message from the server
						var obj = await _receiveCeras.ReadFromStream( _netStream );
						HandleMessage( obj );
					}
				}
				catch (Exception e) {
					Console.WriteLine( "Client error while receiving: " + e );
				}
			} );
		}

		void HandleMessage( object obj ) {
			Console.WriteLine( $"[Client] Received a '{obj.GetType().Name}': {obj}" );
		}

		// A little helper function that sends any object to the server
	}
}