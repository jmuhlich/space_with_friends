namespace server
{
	class Program
	{
		static void Main(string[] args)
		{
			log.create("logs/server.log");

			msg.ClientLogin loginDummy;

			Server.Start();
		}
	}
}
