using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtility {

    //Create class name from a path.
    public static string GetClassName(string apiName,string suffix){
        while(apiName.IndexOf("/",System.StringComparison.CurrentCulture)!=-1){
            int index = apiName.IndexOf ("/", System.StringComparison.CurrentCulture);
            apiName = apiName.Remove (index,1);
            if(index < apiName.Length -1){
                apiName = OneCharToUpper (apiName,index);
            }
        }
        apiName = System.Text.RegularExpressions.Regex.Replace(apiName, "[^0-9a-zA-Z]+", "");
        apiName = OneCharToUpper (apiName,0) + suffix;
        return apiName;
    }

    //Change one char to upper mode.
    public static string OneCharToUpper(string str,int index){
        string sp = str.Substring (index,1);
        sp = sp.ToUpper ();
        str = str.Remove (index,1);
        str = str.Insert (index,sp);
        return str;
    }

    public static string NameSpaceToPathFormat(string nameSpace)
    {
        return nameSpace.Replace(".", "/");
    }

}
