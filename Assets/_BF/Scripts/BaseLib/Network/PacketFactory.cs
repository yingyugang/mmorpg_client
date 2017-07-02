using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public interface IPacketFactory
    {
        IPacket create();
    }

    public class PacketFactory<T> : IPacketFactory where T : IPacket, new() 
    {
        public IPacket create()
        {
            return new T();
        }
    }
}
