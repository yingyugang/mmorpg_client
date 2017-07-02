using System;
using System.Collections.Generic;

namespace DataCenter
{
	public class BattleResultInfo
    {
		public bool isWin;
        public string BattleName;
        public string FieldName;
        public int gold;
        public int soul;
        public int exp;
		public int remainHp;


        public List<BattleMaterial> materialList = new List<BattleMaterial>();
        public List<int> partnerList = new List<int>();

	}

    public class BattleMaterial
    {
        public int id;
        public int count;
    }
}
