using System;
//using System.Collections.Immutable; // doesn't exist for me? - andy

namespace space_with_friends {
	using System.Collections.Generic;
	using Ceras;
	using Ceras.Helpers;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;
	using System.Threading.Tasks;

	class ServerClient {
		readonly TcpClient _tcpClient;
		readonly NetworkStream _netStream;
		readonly CerasSerializer _sendCeras;
		readonly CerasSerializer _receiveCeras;

		string _clientName;

		public ServerClient( TcpClient tcpClient ) {
			_tcpClient = tcpClient;

			var lo = new LingerOption( false, 0 );
			_tcpClient.LingerState = lo;

			_tcpClient.NoDelay = true;


			_netStream = tcpClient.GetStream();

			// We want to keep "learned" types
			// That means when the other side sends us a type (using the full name) we never want to transmit that again,
			// the type should (from then on) be known as a some ID.
			var configSend = new SerializerConfig();
			configSend.Advanced.PersistTypeCache = true;

			_sendCeras = new CerasSerializer( configSend );

			var configRecv = new SerializerConfig();
			configRecv.Advanced.PersistTypeCache = true;

			_receiveCeras = new CerasSerializer( configRecv );

			StartReceivingMessages();
		}

		void StartReceivingMessages() {
			Task.Run( async () => {
				try {
					// Keep receiving packets from the client and respond to them
					// Eventually when the client disconnects we'll just get an exception and end the thread...
					while (true) {
						var obj = await _receiveCeras.ReadFromStream( _netStream );
						HandleMessage( obj );
					}
				}
				catch (Exception e) {
					log.info( $"Error while handling client '{_tcpClient.Client.RemoteEndPoint}': {e}" );
				}
			} );
		}

		void HandleMessage( object msg ) {
			log.debug( $"Got: {msg.GetType()}" );

			if (msg is msg.SendToAll sendToALl) {
				log.trace( $"Bouncing: {sendToALl.Message.GetType()}" );
				return;
			}

			if (msg is msg.SendToTarget sendToTarget) {
				log.trace( $"Bouncing: {sendToTarget.Message.GetType()}" );
				return;
			}

			if (msg is msg.login login) {
				log.info( $"login: {login.player_id}" );
				return;
			}

			if (msg is msg.logout logout) {
				log.info( $"logout: {logout.player_id}" );

				// if (msg is msg.SendToAll sendToALl) {
				// }

				// if (msg is msg.SendToTarget sendToTarget) {
				// }
			}

			// If we have no clue how to handle something, we
			// just print it out to the console
			log.warn( $"RECEIVED UNHANDLED: '{msg.GetType().Name}': {msg}" );

		}

		void Send( object obj ) => _sendCeras.WriteToStream( _netStream, obj );
	}

	static class Server {
		public static int port = 7887;

		//public static ImmutableList<ServerClient> s_client = ImmutableList<ServerClient>.Empty;

		public static void Start() {
			log.info( "Starting thread." );
			new Thread( AcceptClients ).Start();
		}

		static void AcceptClients() {
			log.info( $"Starting listener on {port}" );
			var listener = new TcpListener( IPAddress.Any, port );
			listener.Start();

			log.info( $"Looping waiting for clients" );
			while (true) {
				var tcpClient = listener.AcceptTcpClient();
				log.info( $"Got a client!" );
				log.logProps( tcpClient, "   " );

				var serverClientHandler = new ServerClient( tcpClient );


			}
		}

	}

}

