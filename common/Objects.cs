using System.Collections.Generic;
using System;

namespace space_with_friends.msg
{
	public class login
	{
		public string player_id;
	}
	
	public class logout
	{
		public string player_id;
	}

	public class SendToAll<T> {
		public T Message;
	}

	public class SendToTarget<T> {
		public string Target;
		public T Message;
	}
}
