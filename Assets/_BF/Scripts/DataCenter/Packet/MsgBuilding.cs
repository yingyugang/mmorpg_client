using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_BUILD_REQUEST    
    public class MSG_BUILDING_BUILD_REQUEST : PacketBase
    {
        public uint   idBuildingType;

        protected override bool packetBody()
        {
            bw.Write(idBuildingType);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_BUILD_RESPONSE    
    public class MSG_BUILDING_BUILD_RESPONSE : UnPacketBase
    {
        public uint   idBuilding;
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_REQUEST    
    public class MSG_BUILDING_UPLEV_REQUEST : PacketBase
    {
        public uint   idBuilding;
        public uint   unCostSoul;

        protected override bool packetBody()
        {
            bw.Write(idBuilding);
            bw.Write(unCostSoul);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_RESPONSE 
    public class MSG_BUILDING_UPLEV_RESPONSE : UnPacketBase
    {
        public uint   idBuilding;
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LIST_EVENT    
    public struct BUILDING_INFO
    {
        public uint      idBuilding;
        public uint      idBuildingType;
        public byte      cbLev;
        public uint      unSoul;
        public uint      u32LastLevy;
        public uint      u32LevyTimes;
    }

    public class MSG_BUILDING_LIST_EVENT : UnPacketBase
    {
        public ushort usCnt;
        public BUILDING_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new BUILDING_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idBuilding = br.ReadUInt32();
                lst[i].idBuildingType = br.ReadUInt32();
                lst[i].cbLev = br.ReadByte();
                lst[i].unSoul = br.ReadUInt32();
                lst[i].u32LastLevy = br.ReadUInt32();
                lst[i].u32LevyTimes = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPDATE_EVENT    

    public class MSG_BUILDING_UPDATE_EVENT : UnPacketBase
    {
        public uint   idBuilding;
        public uint   idBuildingType;
        public byte   cbLev;
        public uint   unSoul;
        public uint   u32LastLevy;
        public uint   u32LevyTimes;

        protected override bool unpackBody()
        {
            idBuilding = br.ReadUInt32();
            idBuildingType = br.ReadUInt32();
            cbLev = br.ReadByte();
            unSoul = br.ReadUInt32();
            u32LastLevy = br.ReadUInt32();
            u32LevyTimes = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LEVY_REQUEST    

    public class MSG_BUILDING_LEVY_REQUEST : PacketBase
    {
        public uint idBuilding;
        public byte count;

        protected override bool packetBody()
        {
            bw.Write(idBuilding);
            bw.Write(count);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LEVY_RESPONSE    

    public class MSG_BUILDING_LEVY_RESPONSE : UnPacketBase
    {
        public uint   idBuilding;
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct
}

