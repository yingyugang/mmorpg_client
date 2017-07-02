using System;
using System.Collections.Generic;

namespace BaseLib
{
    public delegate void onEventFunc(int nEvent, System.Object param);
	public class EventGroup
	{
        Dictionary<int, onEventFunc> _funcList = new Dictionary<int, onEventFunc>();
        public EventGroup()
        {
        }

        onEventFunc getEventFunc(int nEvent)
        {
            onEventFunc func;
            if (!_funcList.TryGetValue(nEvent,out func))
                return null;
            return func;
        }

        public void register(int nEvent,onEventFunc onfunc)
        {
            onEventFunc func = getEventFunc(nEvent);
            if (func == null)
                this._funcList[nEvent] = onfunc;
            else
                this._funcList[nEvent] += onfunc;
        }

        public void unRegister(int nEvent, onEventFunc onfunc)
        {
            onEventFunc func = getEventFunc(nEvent);
            if (func != null)
                this._funcList[nEvent] -= onfunc;
        }

        public void release()
        {
            this._funcList.Clear();
        }

        public void sendEvent(int nEvent, Object param)
        {
            onEventFunc func = getEventFunc(nEvent);
            if (func != null)
                func(nEvent,param);
        }
	}
}
