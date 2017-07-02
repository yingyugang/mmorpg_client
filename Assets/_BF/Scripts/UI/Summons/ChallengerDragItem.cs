using UnityEngine;
using DataCenter;
using BaseLib;
using UI;
/// <summary>
/// UIDragDropItem is a base script for your own Drag & Drop operations.
/// </summary>

//[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class ChallengerDragItem : MonoBehaviour
{
    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold,
    }

    public int heroId;
    public GameObject chanllengeTeam;

    /// <summary>
    /// What kind of restriction is applied to the drag & drop logic before dragging is made possible.
    /// </summary>

    public Restriction restriction = Restriction.None;

    /// <summary>
    /// Whether a copy of the item will be dragged instead of the item itself.
    /// </summary>

    public bool cloneOnDrag = false;

    #region Common functionality

    protected Transform mTrans;
    protected Transform mParent;
    protected Collider mCollider;
    protected UIRoot mRoot;
    protected UIGrid mGrid;
    protected UITable mTable;
    protected int mTouchID = int.MinValue;
    protected float mPressTime = 0f;
    protected UIDragScrollView mDragScrollView = null;

    /// <summary>
    /// Cache the transform.
    /// </summary>

    protected virtual void Start()
    {
        mTrans = transform;
        mCollider = GetComponent<Collider>();
        mDragScrollView = GetComponent<UIDragScrollView>();
    }

    /// <summary>
    /// Record the time the item was pressed on.
    /// </summary>

    void OnPress(bool isPressed) 
    { 
        if (isPressed) mPressTime = RealTime.time;
    }

    void OnClick()
    {
    }

    /// <summary>
    /// Start the dragging operation.
    /// </summary>

    void OnDragStart()
    {
        //隐藏属性信息记录位置
        ChanllengeTeam ct = chanllengeTeam.GetComponent<ChanllengeTeam>();
        if (ct.nState != 2)
        { 
            return;
        }

        DragDropGround ddg = gameObject.transform.parent.GetComponent<DragDropGround>();
        ct.nDragGround = ddg.idGround;
        ct.HideInfo();
        
        if (!enabled || mTouchID != int.MinValue) return;

        // If we have a restriction, check to see if its condition has been met first
        if (restriction != Restriction.None)
        {
            if (restriction == Restriction.Horizontal)
            {
                Vector2 delta = UICamera.currentTouch.totalDelta;
                if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.Vertical)
            {
                Vector2 delta = UICamera.currentTouch.totalDelta;
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.PressAndHold)
            {
                if (mPressTime + 1f > RealTime.time) return;
            }
        }

        if (cloneOnDrag)
        {
            GameObject clone = NGUITools.AddChild(transform.parent.gameObject, gameObject);
            clone.transform.localPosition = transform.localPosition;
            clone.transform.localRotation = transform.localRotation;
            clone.transform.localScale = transform.localScale;

            UIButtonColor bc = clone.GetComponent<UIButtonColor>();
            if (bc != null) bc.defaultColor = GetComponent<UIButtonColor>().defaultColor;

            UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);

            UICamera.currentTouch.pressed = clone;
            UICamera.currentTouch.dragged = clone;

            ChallengerDragItem item = clone.GetComponent<ChallengerDragItem>();
            item.Start();
            item.OnDragDropStart();
            ClearInfo();
        }
        else OnDragDropStart();

    }

    /// <summary>
    /// Perform the dragging.
    /// </summary>

    void OnDrag(Vector2 delta)
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropMove((Vector3)delta * mRoot.pixelSizeAdjustment);
    }

    /// <summary>
    /// Notification sent when the drag event has ended.
    /// </summary>

    void OnDragEnd()
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropRelease(UICamera.hoveredObject);

        //显示属性信息
        ChanllengeTeam ct = chanllengeTeam.GetComponent<ChanllengeTeam>();
        ct.ShowInfo();
    }

    #endregion

    /// <summary>
    /// Perform any logic related to starting the drag & drop operation.
    /// </summary>

    protected virtual void OnDragDropStart()
    {
        // Automatically disable the scroll view
        if (mDragScrollView != null) mDragScrollView.enabled = false;

        // Disable the collider so that it doesn't intercept events
        if (mCollider != null) mCollider.enabled = false;

        mTouchID = UICamera.currentTouchID;
        mParent = mTrans.parent;
        mRoot = NGUITools.FindInParents<UIRoot>(mParent);
        mGrid = NGUITools.FindInParents<UIGrid>(mParent);
        mTable = NGUITools.FindInParents<UITable>(mParent);

        // Re-parent the item
        if (UIDragDropRoot.root != null)
            mTrans.parent = UIDragDropRoot.root;

        Vector3 pos = mTrans.localPosition;
        pos.z = 0f;
        mTrans.localPosition = pos;

        // Notify the widgets that the parent has changed
        NGUITools.MarkParentAsChanged(gameObject);

        if (mTable != null) mTable.repositionNow = true;
        if (mGrid != null) mGrid.repositionNow = true;
    }

    /// <summary>
    /// Adjust the dragged object's position.
    /// </summary>

    protected virtual void OnDragDropMove(Vector3 delta)
    {
        mTrans.localPosition += delta;
    }

    /// <summary>
    /// Drop the item onto the specified object.
    /// </summary>

    
    protected virtual void OnDragDropRelease(GameObject surface)
    {
        if (surface != null)
        {
            DragDropGround ddg = surface.GetComponent<DragDropGround>();
            
            if (surface.name == "Item")
            {
                ddg = surface.transform.parent.GetComponent<DragDropGround>();
            }
            else
            {
                surface = PanelTools.findChild(surface, "Item");
            }
            
            //ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            if (ddg != null)
            {
                
                //id
                UILabel idHeroLable = PanelTools.findChild<UILabel>(surface, "idLabel");
                UILabel idLable = PanelTools.findChild<UILabel>(gameObject, "idLabel");

                if (idLable != null && idHeroLable != null)
                {
                    //int id = int.Parse(idLable.text);
//                     if (id == 0)
//                     {
//                         NGUITools.Destroy(gameObject);
//                         return;
//                     }
                    
                    int nID = int.Parse(idHeroLable.text);
                    int idHero = int.Parse(idLable.text);
                    ChanllengeTeam ct = chanllengeTeam.GetComponent<ChanllengeTeam>();
					if (surface.activeSelf)
                    {
                        //交换
                        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ChangeTeamerById(ddg.idGround, idHero, ct.nCurTeamId);
                        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ChangeTeamerById(ct.nDragGround, nID, ct.nCurTeamId);

                        if (DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos == ddg.idGround + 1)
                        {
                            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos = ct.nDragGround + 1;
                        }
                        else if (DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos == ct.nDragGround + 1)
                        {
                            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos = ddg.idGround + 1;
                        }

                    }
                    else
                    {
                        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ChangeTeamerById(ddg.idGround, idHero, ct.nCurTeamId);
                        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).DelTeamerById(ct.nDragGround, idHero, ct.nCurTeamId);

                        if (DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos == ct.nDragGround + 1)
                        {
                            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[ct.nCurTeamId + 1].leaderPos = ddg.idGround + 1;
                        }

                        surface.SetActive(true);
                    }

                    idHeroLable.text = idLable.text;

                    ct.ShowInfo();
                }

                NGUITools.Destroy(gameObject);
                return;
            }
            else
            {
                NGUITools.Destroy(gameObject);
                return;
            }
        }

        NGUITools.Destroy(gameObject);
    }

    public void ClearInfo()
    {
        
    }

    public void UpdateInfo()
    {

    }
}
