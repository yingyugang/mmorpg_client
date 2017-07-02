using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{

//////////////////////////////////////////////////////////////////////////
// MSG_ITEM_LIST_EVENT    
    public struct ITEM_INFO
    {
        public uint      idItem;
        public uint      idItemType;
        public uint      idHeroEquip;
        public uint      unAmount;
    }

    public class MSG_ITEM_LIST_EVENT:UnPacketBase
    {
        public ushort usCnt;
        public ITEM_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new ITEM_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idItem = br.ReadUInt32();
                lst[i].idItemType = br.ReadUInt32();
                lst[i].idHeroEquip = br.ReadUInt32();
                lst[i].unAmount = br.ReadUInt32();
            }
            return true;
        }
    } 

//////////////////////////////////////////////////////////////////////////
// MSG_ITEM_UPDATE_EVENT    
    public class MSG_ITEM_UPDATE_EVENT: UnPacketBase
    {
        public uint   idItem;
        public uint   idItemType;
        public uint   idHeroEquip;
        public uint   unAmount;

        protected override bool unpackBody()
        {
            idItem = br.ReadUInt32();
            idItemType = br.ReadUInt32();
            idHeroEquip = br.ReadUInt32();
            unAmount = br.ReadUInt32();
            return true;
        }
    } 

//////////////////////////////////////////////////////////////////////////
// MSG_ITEM_DELETE_EVENT    
    public class MSG_ITEM_DELETE_EVENT : UnPacketBase
    {
        public uint   idItem;

        protected override bool unpackBody()
        {
            idItem = br.ReadUInt32();
            return true;
        }
    }

//////////////////////////////////////////////////////////////////////////
// MSG_ITEM_SELL_REQUEST    
    public class MSG_ITEM_SELL_REQUEST : PacketBase
    {
        public uint   idItem;
        public uint   unAmount;

        protected override bool packetBody()
        {
            bw.Write(idItem);
            bw.Write(unAmount);
            return true;
        }
    }

//////////////////////////////////////////////////////////////////////////
// MSG_ITEM_SELL_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_ITEM_SELL_RESPONSE : UnPacketBase
    {
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

//////////////////////////////////////////////////////////////////////////
// MSG_SET_BATTLE_IEM_REQUEST    

    public struct BATTLE_ITEM_REQ_INFO
    {
        public uint      idItem;
        public uint      unAmount;
        public uint      unIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_SET_BATTLE_IEM_REQUEST : PacketBase
    {
        public ushort usCnt;
        public BATTLE_ITEM_REQ_INFO[] lst;
        
        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idItem);
                bw.Write(lst[i].unAmount);
                bw.Write(lst[i].unIndex);
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_SET_BATTLE_IEM_RESPONSE    
    public struct BATTLE_ITEM_RES_INFO
    {
        public uint      wErrCode;
    }

    public class MSG_SET_BATTLE_IEM_RESPONSE : UnPacketBase
    {
        public ushort usCnt;
        public BATTLE_ITEM_RES_INFO[] lst;
        
        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new BATTLE_ITEM_RES_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].wErrCode = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUY_ITEM_SIZE_REQUEST    
    public class MSG_BUY_ITEM_SIZE_REQUEST : PacketBase
    {
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUY_ITEM_SIZE_RESPONSE    

    public class MSG_BUY_ITEM_SIZE_RESPONSE : UnPacketBase
    {
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLEINT_EQUIP_ITEM_REQUEST    
    public class MSG_CLEINT_EQUIP_ITEM_REQUEST : PacketBase
    {
        public uint   idItem;
        public uint   idHero;

        protected override bool packetBody()
        {
            bw.Write(idItem);
            bw.Write(idHero);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLEINT_EQUIP_ITEM_RESPONSE    
    public class MSG_CLEINT_EQUIP_ITEM_RESPONSE : UnPacketBase
    {
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLEINT_UNEQUIP_ITEM_REQUEST    
    public class MSG_CLEINT_UNEQUIP_ITEM_REQUEST : PacketBase
    {
        public uint   idHero;

        protected override bool packetBody()
        {
            bw.Write(idHero);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLEINT_UNEQUIP_ITEM_RESPONSE    
    public class MSG_CLEINT_UNEQUIP_ITEM_RESPONSE : UnPacketBase
    {
        public uint   wErrCode;

        protected override bool unpackBody()
        {
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_MAKE_ITEM_BY_FORMULA_REQUEST    
    public class MSG_MAKE_ITEM_BY_FORMULA_REQUEST : PacketBase
    {
        public uint   idItemType;
        public uint   unAmount;
        
        protected override bool packetBody() 
        {
            bw.Write(idItemType);
            bw.Write(unAmount);

            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_MAKE_ITEM_BY_FORMULA_RESPONSE    

    public class MSG_MAKE_ITEM_BY_FORMULA_RESPONSE : UnPacketBase
    {
        public uint   wErrCode;
        protected override bool unpackBody()
        {
            wErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_USE_BATTLE_IEM_REQUEST    
    public struct USE_BATTLE_IEM_REQ_INFO
    {
        public uint      idItemType;
        public uint      unAmount;
    }

    public class MSG_USE_BATTLE_IEM_REQUEST : PacketBase
    {
        public ushort usCnt;
        public USE_BATTLE_IEM_REQ_INFO[] lst;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idItemType);
                bw.Write(lst[i].unAmount);
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_USE_BATTLE_IEM_RESPONSE    
    public struct USE_BATTLE_IEM_RES_INFO
    {
        public uint      wErrCode;
    }

    public class MSG_USE_BATTLE_IEM_RESPONSE : UnPacketBase
    {
        public ushort usCnt;
        public USE_BATTLE_IEM_RES_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new USE_BATTLE_IEM_RES_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].wErrCode = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct
}

