using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace WWWNetwork
{
	public class WWWNetworkManager : SingleMonoBehaviour<WWWNetworkManager>
	{
		public Model model;

		public  Dictionary<string,string> cookies;

		public UnityAction<WWW> failCallBack;


		protected override void Awake ()
		{
			base.Awake ();
			model = new Model ();
		}

		public void CommonCallBack (WWW www)
		{
			Debug.Log ("Error: " + www.error);
			Debug.Log ("Text: " + www.text);
		}

		IEnumerator WaitWWW (WWW www, UnityAction<WWW> callBack)
		{
			yield return www;
			if (www.error != null) {
				Debug.LogError (www.error);
				if (failCallBack != null)
					failCallBack (www);
			} else {
				Debug.Log (www.text);
				if (cookies == null)
					cookies = www.ParseCookies ();//获取服务端Session
				JsonUtility.FromJsonOverwrite(www.text,model);
				if (callBack != null)
					callBack (www);
			}
		}

		public void Send (string apiPath, byte[] data, UnityAction<WWW> complete)
		{
			WWW www;
			Debug.Log ("apiPath:" + PathConstant.SERVER_PATH + apiPath);
			Debug.Log (SystemInfo.deviceUniqueIdentifier);
			if(cookies==null)
				www = new WWW (PathConstant.SERVER_PATH + apiPath, data,new Dictionary<string,string>{ { "DeviceID",SystemInfo.deviceUniqueIdentifier} });
			else
				www = new WWW (PathConstant.SERVER_PATH + apiPath, data, UnityCookies.GetCookieRequestHeader(cookies));
			StartCoroutine (WaitWWW (www, complete));
		}

		//TODO 读取cookie的部分还要修改，也许可以不用cookie
		public void Send(string apiPath, byte[] data, UnityAction complete){
			UnityWebRequest request = new UnityWebRequest (PathConstant.SERVER_PATH + apiPath, UnityWebRequest.kHttpVerbPOST);//  CreateUnityWebRequest (PathConstant.SERVER_PATH + apiPath, wwwForm);
			if (data == null || data.Length == 0)
				data = new byte[2]{0,1};
			UploadHandlerRaw uH = new UploadHandlerRaw (data);
			uH.contentType = "application/json"; 
			request.uploadHandler = uH;
			DownloadHandlerBuffer dH = new DownloadHandlerBuffer ();
			request.downloadHandler = dH;
			if (cookies == null) {
				request.SetRequestHeader ( "DeviceID", SystemInfo.deviceUniqueIdentifier);
			} else {
				Dictionary<string,string> keys = UnityCookies.GetCookieRequestHeader (cookies);
				foreach (KeyValuePair<string, string> keyValuePair in keys)
				{
					request.SetRequestHeader (keyValuePair.Key, keyValuePair.Value);
				}
			}
			StartCoroutine (WaitWWWRequest(request,complete));
		}

		IEnumerator WaitWWWRequest (UnityWebRequest request, UnityAction callBack)
		{
			yield return request.Send ();

			if (request.isDone && string.IsNullOrEmpty (request.error)) {
				if (request.responseCode.Equals (200)) {
					Debug.Log ("failCallBack");
				}
			} else {
//				if (cookies == null) {
//					cookies = www.ParseCookies ();//获取服务端Session
//				}
				JsonUtility.FromJsonOverwrite(request.downloadHandler.text,model);
				if (callBack != null)
					callBack ();
			}
		}



	}
}