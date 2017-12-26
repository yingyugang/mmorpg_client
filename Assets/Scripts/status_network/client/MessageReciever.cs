using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;

//recieve the machine controlling messages.
using UnityEngine.Events;


public class MessageReciever : SingleMonoBehaviour<MessageReciever> {
	//メセージを監視用UDPクライアント。
	public static UdpClient udp;
	//メセージを監視用スレッド
	public Thread thread;

	public static Dictionary<string,float> ips;
	static float time;

	protected override void Awake ()
	{
		//監視しているポート
		int LOCAL_PORT = 50001;
		ips = new Dictionary<string, float> ();
		udp = new UdpClient(LOCAL_PORT);
		thread = new Thread(new ThreadStart(ThreadMethod));
		thread.IsBackground = true;
		thread.Start();
	}

	void Update(){
		time = Time.time;	
	}

	public void StopReceive(){
		thread.Abort();
	}

	public static bool isRunning = true;
	private static void ThreadMethod()
	{
		while(isRunning)
		{
			//メセージを受け取っていない時、読み取ない。
			if (udp.Available == 0) {
				Thread.Sleep (100);
				continue;
			}
			IPEndPoint remoteEP = null;
			//TODO Should receive data sub scene ip&&port from main server.
			//今はこのデータがない。
			byte[] data = udp.Receive(ref remoteEP);
//			Debug.Log (remoteEP.Address.ToString ());
			if (!ips.ContainsKey (remoteEP.Address.ToString ()))
				ips.Add (remoteEP.Address.ToString (),time);
			else
				ips[remoteEP.Address.ToString ()] =time;
		}
		Debug.Log ("Thread Done!");
	} 

	void OnApplicationQuit(){
		thread.Abort();
	}
}