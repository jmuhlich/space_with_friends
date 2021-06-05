using System;
using System.Threading;

namespace client {
	class Program {
		static void Main( string[] args ) {
			log.create( "logs/headless.log" );

			log.info( "Create a new client" );
			var c = new Client();
			c.Start();

			while (true) {
				c.SendExampleObjects();
				Thread.Sleep(250);
			}
		}
	}
}
