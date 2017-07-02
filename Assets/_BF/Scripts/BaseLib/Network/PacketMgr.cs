using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class PacketMgr : Singleton<PacketMgr>
	{
        Dictionary<string, ushort> _packetList = new Dictionary<string, ushort>();
        uint _packetSequence = 1;

        private PacketMgr()
        {
        }

        protected override void init()
        {
            clear();
        }

        public void clear()
        {
            _packetSequence = 1;
        }

        public uint getSequence()
        {
            uint sequence = this._packetSequence++;
            UnityEngine.Debug.Log("sequence="+sequence);
            return sequence;
        }

        public void addPacket<T>(int id) where T : IPacket, new()
        {
            Type type = typeof(T);
            _packetList[type.FullName] = (ushort)id;
        }

        public ushort getPacketId(IPacket obj)
        {
            if(obj==null)
                return 0;
            ushort id;
            if(_packetList.TryGetValue(obj.GetType().FullName,out id))
                return id;
            return 0;
        }
	}
}
