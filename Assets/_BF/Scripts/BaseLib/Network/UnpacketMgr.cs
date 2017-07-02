using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class UnPacketMgr : Singleton<UnPacketMgr>
	{
        Dictionary<int, IUnPacketFactory> _unpacketList = new Dictionary<int, IUnPacketFactory>();
        Dictionary<string, ushort> _packetList = new Dictionary<string, ushort>();

        private UnPacketMgr()
        {
        }

        protected override void init()
        {
            _unpacketList.Clear();
            _packetList.Clear();
        }

        public void addUnPacket<T>(int id) where T : IUnPacket, new()
        {
            _unpacketList[id] = new UnPacketFactory<T>();

            Type type = typeof(T);
            _packetList[type.FullName] = (ushort)id;
        }

        public IUnPacket getUnPacket(int id)
        {
            IUnPacketFactory factory;
            if (_unpacketList.TryGetValue(id, out factory))
            {
                if (factory != null)
                {
                    IUnPacket unpack = factory.create();
                    unpack.msgid = id;
                    return unpack;
                }
            }
            return null;
        }

        public ushort getPacketId(IUnPacket obj)
        {
            if (obj == null)
                return 0;
            ushort id;
            if (_packetList.TryGetValue(obj.GetType().FullName, out id))
                return id;
            return 0;
        }
	}
}
