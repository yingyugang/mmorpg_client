using UnityEngine;
using System.Collections;
using Spine.Unity;

public enum _UnitArtActionType{
	//待機
	cmn_0001,
	//行使
	cmn_0002,
	//前ステップ
	cmn_0003,
	//後ステップ
	cmn_0004,
	//被弾
	cmn_0006,
	//通常攻撃
	atk_0001,
	//スキル1 
	atk_0101,
	//スキル2
	atk_0102,
	//スキル3
	atk_0103,
	//High level スキル1 
	atk_0201,
	//High level スキル2
	atk_0202,
	//High level スキル3
	atk_0203
};

public class UnitRes : MonoBehaviour {

	SkeletonAnimation mSkeletonAnimation;
	MeshRenderer mMeshRenderer;

	void Awake(){
		mSkeletonAnimation = GetComponent<SkeletonAnimation> ();
		mMeshRenderer = GetComponent<MeshRenderer> ();
		SetLayer ("UnitLayer0");
	}

	void Update(){
		SetLayerOrder (-(int)(transform.position.y * 10));
	}

	public Vector2 GetHitPos(){
		return GetCenterPos() + new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
	}

	public Vector2 GetCenterPos(){
		if (mSkeletonAnimation != null) {
			return new Vector2 (transform.position.x, transform.position.y + mSkeletonAnimation.Skeleton.Data.Height / 200);
		} else {
			return (Vector2)transform.position;
		}
	}

	public Vector2 GetWidthOffset(){
		return new Vector2 (mSkeletonAnimation.Skeleton.Data.Width / 200, 0) *  transform.right.x ;
	}

	public void SetLayer(string layer){
		mMeshRenderer.sortingLayerName = layer;
	}

	public void SetLayerOrder(int layerOrder){
		mMeshRenderer.sortingOrder = layerOrder;
	}


}
