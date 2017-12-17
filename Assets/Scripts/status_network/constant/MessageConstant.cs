using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public static class MessageConstant
	{
		public const short SERVER_TO_CLIENT_MSG = 8001;
		public const short SERVER_TO_CLIENT_PLAYER_INFO = 8002;
		public const short SERVER_TO_CLIENT_MONSTER_INFO = 8003;

		public const short CLIENT_TO_SERVER_MSG = 9001;
		public const short CLIENT_TO_SERVER_PLAYER_INFO = 9002;

	}
}
