// CameraMovement.cs
using UnityEngine;
using System.Collections;

public class FixedCamera : MonoBehaviour
{
		public float smooth = 1.5f;         // 摄像机跟踪速度
		
		
	public Transform player;           // 引用角色位置
//		private Transform boss;
		private Vector3 relCameraPos;       // 摄像机与角色的相对位置
		private float relCameraPosMag;      // 摄像机到角色的距离向量长度
		private Vector3 newPos;             // 摄像机的新位置
	
	
		void Start ()
		{
				// 引用角色位置
//				player = GameObject.FindGameObjectWithTag ("Player").transform;
				
//				boss = GameObject.FindGameObjectWithTag ("Enemy").transform;
				//获取摄像机与角色的相对位置
				relCameraPos = transform.position - player.position; // 相对位置 = 摄像机位置 - 角色位置
				relCameraPosMag = relCameraPos.magnitude - 10.5f; // 相对位置向量的长度 = 相对位置的长度 - 0.5f  防止光线投射碰撞地面
		}


	Vector3 standardPos;
	Vector3 abovePos;
	Vector3[] checkPoints;
		void FixedUpdate ()
		{
				// 摄像机初始位置 = 角色位置 + 角色与摄像机的相对位置
				 standardPos = player.position + relCameraPos;
		
				// 俯视位置 = 角色位置 + 角色正上方 * 相对位置向量的长度
				 abovePos = player.position + Vector3.up * relCameraPosMag;
		
				// 创建长度为5的数组 储存5个摄像机位置
				//

				 checkPoints = new Vector3[5];
		
				// 第一个检测 摄像机标准位置
				checkPoints [0] = standardPos;
		
				// 这三个检测位置为 标准位置到俯视位置之间的三个位置 插值分别为25% 50% 75%
				checkPoints [1] = Vector3.Lerp (standardPos, abovePos, 0.25f);
				checkPoints [2] = Vector3.Lerp (standardPos, abovePos, 0.5f);
				checkPoints [3] = Vector3.Lerp (standardPos, abovePos, 0.75f);
		
				// 最后检测位置为 摄像机俯视位置
				checkPoints [4] = abovePos;
		
				// 通过循环检测每个位置是否可以看到角色
				for (int i = 0; i < checkPoints.Length; i++) {
						// 如果可以看到角色
						if (ViewingPosCheck (checkPoints [i]))
				// 跳出循环
								break;
				}
		
				// 让摄像机位置 从当前位置 平滑转至 新位置
				transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
		
				// 确保摄像机朝向角色方向
				SmoothLookAt ();
		}


		bool ViewingPosCheck (Vector3 checkPos)
		{
				RaycastHit hit;
				
				// 如果光线投射碰撞到某个对象
				if (Physics.Raycast (checkPos, player.position - checkPos, out hit,Mathf.Max(1,relCameraPosMag)))
			// 如果光线投射碰撞点不是角色位置
				if (hit.transform != player)
				// 当前检测位置不合适 返回false
						return false;
		
				// If we haven't hit anything or we've hit the player, this is an appropriate position.如果光线投射没有碰撞到任何东西 或者碰撞点为角色位置时 更新当前检测位置为摄像机的新位置
				newPos = checkPos;
				return true;
		}
	
		void SmoothLookAt ()
		{
				// 创建从摄像机到角色的向量
				Vector3 relPlayerPosition = player.position - transform.position;
		
				// 根据摄像机到角色的向量 创建旋转角度 
				Quaternion lookAtRotation = Quaternion.LookRotation (relPlayerPosition, Vector3.up);
		
				// 让摄像机从 当前角度 平划转至创建的旋转角度
				transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, smooth * Time.deltaTime);
		}
}