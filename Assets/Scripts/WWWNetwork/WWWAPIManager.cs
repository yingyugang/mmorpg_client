using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WWWNetwork
{
	public class WWWAPIManager : SingleMonoBehaviour<WWWAPIManager>
	{

		protected override void Awake ()
		{
			base.Awake ();
		}

		// 发送移动建筑消息(web服务器)
		public void SendMoveBuilding (uint idBuilding, float x, float y)
		{
			ChangeBuildingPosAPI api = WWWNetworkManager.Instance.gameObject.GetOrAddComponent<ChangeBuildingPosAPI> ();
			api.data = new ChangeBuildingModel ();
			api.data.id = (int)idBuilding;
			api.data.pos = (int)x;
			api.Send ((WWW www) => {
				Debug.Log (www.text);
			});
		}

		// 发送创建建筑消息(web服务器)
		public void SendCreateBuilding (uint buildingType, float x, float y)
		{
			CreateBuildingAPI api = WWWNetworkManager.Instance.gameObject.GetOrAddComponent<CreateBuildingAPI> ();
			api.data = new ChangeBuildingModel ();
			api.data.type = (int)buildingType;
			api.data.pos = (int)x;
			api.Send ((WWW www) => {
				Debug.Log (www.text);
			});
		}

		// 发送升级建筑消息(web服务器)
		public void SendUpgradeBuilding(uint idBuilding){
			UpgradeBuildingAPI api = WWWNetworkManager.Instance.gameObject.GetOrAddComponent<UpgradeBuildingAPI> ();
			api.data = new ChangeBuildingModel ();
			api.data.id = (int)idBuilding;
			api.Send ((WWW www) => {
				Debug.Log (www.text);
			});
		}




	}

}
