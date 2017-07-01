using UnityEngine;
using System.Collections;

namespace DataCenter
{
    public class DataModule
    { 
        public DataModule()
        {
        }

        public virtual bool init()
        {
            return true;
        }

        public virtual void release()
        {

        }
    }
}