using UnityEngine;

public class NetworkCheck : MonoBehaviour
{
	private const bool allowCarrierDataNetwork = false;
	private const string pingAddress = "192.168.1.152"; // Google Public DNS server
	private const float waitingTime = 2.0f;
	
//	private Ping ping;
//	private float pingStartTime;
//
//	public bool internetPossiblyAvailable;
//	public int currentNetworkEv;
//
//	public void Start()
//	{
//		switch (Application.internetReachability)
//		{
//			case NetworkReachability.ReachableViaLocalAreaNetwork:
//				internetPossiblyAvailable = true;
//				currentNetworkEv = 0;
//				break;
//			case NetworkReachability.ReachableViaCarrierDataNetwork:
//				internetPossiblyAvailable = allowCarrierDataNetwork;
//				currentNetworkEv = 1;
//				break;
//			default:
//				internetPossiblyAvailable = false;
//				break;
//		}
//		if (!internetPossiblyAvailable)
//		{
//			InternetIsNotAvailable();
//			return;
//		}
//		ping = new Ping(pingAddress);
//		pingStartTime = Time.time;
//	}
//	
//	public void Update()
//	{
//		if (ping != null)
//		{
//			bool stopCheck = true;
//			if (ping.isDone)
//				InternetAvailable();
//			else if (Time.time - pingStartTime < waitingTime)
//				stopCheck = false;
//			else
//				InternetIsNotAvailable();
//			if (stopCheck)
//				ping = null;
//		}
//	}
	
	private void InternetIsNotAvailable()
	{
		Debug.Log("No Internet :(");
	}
	
	private void InternetAvailable()
	{
		Debug.Log("Internet is available! ");
	}
}
