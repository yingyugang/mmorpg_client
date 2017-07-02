using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public class DataPost : DataModule
    {
        public DataPost()
        {
        }

        public override void release()
        {
        }

        public override bool init()
        {
            
            return true;
        }


        //UI界面
        public PanelPost panelPost;

        public void ShowPlacard()
        {
            panelPost.ShowPageByIndex(0);
        }

        public void ShowPost()
        {
            panelPost.ShowPageByIndex(1);
            PostList pl = panelPost.PostPages[1].GetComponent<PostList>();
            pl.ShowPost();
        }
    }
}