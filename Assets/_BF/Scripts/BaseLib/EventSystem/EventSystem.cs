using System;
using System.Collections.Generic;

namespace BaseLib
{
	public class EventSystem:Singleton<EventSystem>
	{
        Dictionary<int, EventGroup> _groupList = new Dictionary<int, EventGroup>();
        
        private EventSystem()
        {
        }

        static public EventGroup getGroupDict(int nGroup=0)
        {
            EventGroup group = null;
            if (!EventSystem.me._groupList.TryGetValue(nGroup, out group))
            {
                group = new EventGroup();
                EventSystem.me._groupList.Add(nGroup, group);
            }
            return group;
        }

        static public void register(int nEvent, onEventFunc onFunc, int groupId = 0)
        {
            EventGroup group = getGroupDict(groupId);
            if (group != null)
                group.register(nEvent, onFunc);
        }

        static public void unregEvent(int nEvent, onEventFunc onFunc,int groupId = 0)
        {
            EventGroup group = getGroupDict(groupId);
            if (group != null)
                group.unRegister(nEvent, onFunc);
        }

        static public void releaseGroup(int groupId = 0)
        {
            EventGroup group = getGroupDict(groupId);
            if (group != null)
                group.release();
        }

        static public void release()
        {
            EventSystem.me._groupList.Clear();
        }

        public static void sendEvent(int nEvent, Object param=null,int groupId = 0)
        {
            EventGroup group = getGroupDict(groupId);
            if (group != null)
                group.sendEvent(nEvent, param);
        }
    }
}
