using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public interface IPacket
    {
        bool pack();
        byte[] data { get; }
        UInt32 length { get; }
        void addSequence();
        void release();
        int msgid { get; set; }
    }

    public interface IUnPacket
    {
        bool unpack(byte[] msg, int len);
        UInt32 sequence { get; }
        int msgid { get; set; }
        void release();
    }
}
