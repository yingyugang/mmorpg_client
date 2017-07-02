using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;
using LitJson;
using BaseLib;
//-
namespace DataCenter
{
    public delegate void IggLoginFun(IggLogin obj);
	public class IggLogin
	{
		const string c_strPubKey = "85e927bd9203be51dfb40e6f9d245252";
		const string c_strUrl = "http://cgi.igg.com:9000/public/guest_user_login_igg?";
		const string c_strGuest = "m_guest";
		const string c_strKey = "m_key";
		const string c_strData = "m_data";
		const string c_strKeeptime = "keep_time";
		const int 	 c_nKeeptime = 259200;
		const int 	 c_nTimeOut = 4;

		const string c_strErrmsg = "errStr";
		const string c_strErrcode = "errCode";
		const string c_strIggid = "iggid";
		const string c_strAccessKey = "access_key";
		const string c_strResult = "result";

		string m_strGuest = "123456";
		string m_strKey = "";
		string m_strData = "";
		string m_strErrmsg = "";
		public string m_strIggId = "12345";
        public string m_strToken = "12345";
		int    m_nErrcode;

		public IggLogin()
		{
		}

		public string iggId{ get; set; }
		public string token{ get; set; }
		public string errMsg{ get; set; }
        public IggLoginFun succFunc{ get; set; }
        public IggLoginFun failedFunc { get; set; }

		string getUrl()
		{
			string strUrl = c_strUrl;

			strUrl += c_strGuest;
			strUrl += "=";
			strUrl += m_strGuest;

			strUrl += "&";
			strUrl += c_strKey;
			strUrl += "=";
			strUrl += m_strKey;

			strUrl += "&";
			strUrl += c_strData;
			strUrl += "=";
			strUrl += m_strData;

			strUrl += "&";
			strUrl += c_strKeeptime;
			strUrl += "=";
			strUrl += c_nKeeptime.ToString();

            Debug.Log(strUrl);
            return strUrl;
		}

		public bool login()
		{
			//设备机器码
            m_strGuest = UnityEngine.SystemInfo.deviceUniqueIdentifier;
			m_strKey = DateTime.Now.Ticks.ToString();
			m_strData = String2MD5(m_strGuest,m_strKey);

			HttpWebRequest request = WebRequest.Create(this.getUrl()) as HttpWebRequest;
			request.ContentType = "Accept-Encoding";
			request.Method = "GET";
			request.Timeout = c_nTimeOut;
			request.BeginGetResponse(new AsyncCallback(onRead),request);
			return true;
		}

		string String2MD5(string strGuest, string strKey)
		{
			MD5 md5Hash = MD5.Create();		
			string str = strGuest + c_strPubKey + strKey;
			
			byte[] BytesIn = Encoding.ASCII.GetBytes(str);
			byte[] BytesOut = md5Hash.ComputeHash(BytesIn);
			
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < BytesOut.Length; j++)
			{
				stringBuilder.Append(BytesOut[j].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		void onRead(IAsyncResult result)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)result.AsyncState;
				HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream);
				string strResult = reader.ReadToEnd();

				parseResult(strResult);
			}
			catch(Exception e)
			{
				this.m_strErrmsg = e.Message;
                onFailed();
			}
            onSucc();
        }

        void onFailed()
        {
            if (this.failedFunc != null)
                this.failedFunc(this);
        }

        void onSucc()
        {
            if (this.succFunc != null)
                this.succFunc(this);
        }

		bool parseResult(string strResult)
		{
			Debug.Log(strResult);
			int nEnd = strResult.LastIndexOf("}");
			string strJson = strResult.Substring(0,nEnd+1);
			//json
			try
			{
				JsonData json = JsonMapper.ToObject(strJson);
				if(json!=null)
				{
					this.m_nErrcode = (int)json[c_strErrcode];
					this.m_strErrmsg = (string)json[c_strErrmsg];
					if(this.m_nErrcode!=0)
						return false;
					JsonData item = json[c_strResult]["0"];
					this.m_strIggId = (string)item[c_strIggid];
					this.m_strToken = (string)item[c_strAccessKey];
					return true;
				}
				this.m_strErrmsg = "Json parse failed!";
                onFailed();
				return false;
			}
			catch(Exception e)
			{
				Debug.Log(e.Message);
				this.m_strErrmsg = "Json parse failed!";
                onFailed();
				return false;
			}
		}
	}
}