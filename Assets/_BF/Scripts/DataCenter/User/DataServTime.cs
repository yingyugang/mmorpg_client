using System;
using System.Collections.Generic;
using BaseLib;
using System.Globalization;
using UnityEngine;

namespace DataCenter
{
    class DataServTime : DataModule
    {
        UInt64 _unStartServTime = 0;
        DateTime _dTStartServerTime;
        DateTime _dTStartClientTime;

        DateTime _baseDt = DateTime.Parse("01/01/1970 00:00:0");
        

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SERVER_TIME_EVENT, onServTime, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ACTIVE_RESPONSE, onActive, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        public override void release()
        {

        }

        void onActive(int nEvent, System.Object param)
        {
            MSG_CLIENT_ACTIVE_RESPONSE msg = (MSG_CLIENT_ACTIVE_RESPONSE)param;
            return;
            //更新时间
            if (msg != null)
            {
                _unStartServTime = msg.u64ServerTime;
                _dTStartServerTime = getServTime(_unStartServTime);
                _dTStartClientTime = DateTime.Now;
            }
        }

        void onServTime(int nEvent, System.Object param)
        {
            MSG_CLIENT_SERVER_TIME_EVENT msg = (MSG_CLIENT_SERVER_TIME_EVENT)param;
            if (msg != null)
            {
                _unStartServTime = msg.serverTime;
                _dTStartServerTime = getServTime(_unStartServTime);
                _dTStartClientTime = DateTime.Now;
            }
        }

        UInt64 realTime
        {
            get { return (UInt64)(0); }
        }

        //获得客户端当前时间换算为服务端时间
        public UInt64 getCurSvrtime()
        {
            Int64 time = (DateTime.Now.Ticks - _dTStartClientTime.Ticks) / 10000000;
            return _unStartServTime + (UInt64)time;
        }

        public DateTime getServTime(UInt64 time)
        {
            return _baseDt.AddSeconds(time).ToLocalTime();
        }

        /*
格式模式 说明 
d 月中的某一天。一位数的日期没有前导零。 
dd 月中的某一天。一位数的日期有一个前导零。 
ddd 周中某天的缩写名称，在 AbbreviatedDayNames 中定义。 
dddd 周中某天的完整名称，在 DayNames 中定义。 
M 月份数字。一位数的月份没有前导零。 
MM 月份数字。一位数的月份有一个前导零。 
MMM 月份的缩写名称，在 AbbreviatedMonthNames 中定义。 
         * 
MMMM 月份的完整名称，在 MonthNames 中定义。 
y 不包含纪元的年份。如果不包含纪元的年份小于 10，则显示不具有前导零的年份。 
yy 不包含纪元的年份。如果不包含纪元的年份小于 10，则显示具有前导零的年份。 
yyyy 包括纪元的四位数的年份。 
gg 时期或纪元。如果要设置格式的日期不具有关联的时期或纪元字符串，则忽略该模式。 
h 12 小时制的小时。一位数的小时数没有前导零。 
hh 12 小时制的小时。一位数的小时数有前导零。 
H 24 小时制的小时。一位数的小时数没有前导零。 
HH 24 小时制的小时。一位数的小时数有前导零。 
m 分钟。一位数的分钟数没有前导零。 
mm 分钟。一位数的分钟数有一个前导零。 
s 秒。一位数的秒数没有前导零。 
ss 秒。一位数的秒数有一个前导零。 
         * 
         * "yyyy-MM-dd HH:mm:ss"
         */
        public string getServTime(UInt64 time, string format)
        {
            return getServTime(time).ToString(format, DateTimeFormatInfo.InvariantInfo);
        }

//         public string GetTimeString(long time)
//         {
//             string strTemp = "";
// 
//             long ltime = DateTime.Now.Ticks + ((time - (long)_servStartTime) * 10000000);
// 
//             strTemp = new DateTime(ltime).ToString("yyyy-MM-dd HH:mm");
// 
//             return strTemp;
//         }
	}

}
