using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    interface IUnPacketFactory
    {
        IUnPacket create();
    }

    class UnPacketFactory<T> : IUnPacketFactory where T : IUnPacket, new() 
    {
        public IUnPacket create()
        {
            return new T();
        }
    }
}
