using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StatusMachines
{
	public class StatusMachine : MonoBehaviour
	{
		string mPreStatus;
		string mCurrentStatus;

		public List<StatusAction> currentStatusAction;
		Dictionary<string,List<StatusAction>> statusDic;

		Dictionary<string,List<StatusAction>> StatusDic{
			get{ 
				if (statusDic == null)
					statusDic = new Dictionary<string, List<StatusAction>> ();
				return statusDic;
			}
		}

		public void AddStatus (string statusName)
		{
			if (string.IsNullOrEmpty (statusName) || StatusDic.ContainsKey (statusName))
				return;
			StatusDic.Add (statusName, new List<StatusAction> ());
		}

		public void AddAction (string statusName, StatusAction action)
		{
			if (string.IsNullOrEmpty (statusName) || action == null)
				return;
			AddStatus (statusName);
			StatusDic [statusName].Add (action);
			action.GO = gameObject;
			action.statusMachine = this;
			action.OnAwake ();
		}

		public void AddActions (string statusName, List<StatusAction> actions)
		{
			if (string.IsNullOrEmpty (statusName) || actions == null)
				return;
			AddStatus (statusName);
			for (int i = 0; i < actions.Count; i++) {
				AddAction (statusName, actions [i]);
			}
		}

		string mPreStatusForToggle;
		string mCurrentStatusForToggle;
		public void ChangeStatus (string status)
		{
			if (string.IsNullOrEmpty (status))
				return;
			if (StatusDic.ContainsKey (status)) {
				mCurrentStatus = status;
				currentStatusAction = StatusDic [mCurrentStatus];

				if (mCurrentStatus != mPreStatus) {
					mPreStatus = mCurrentStatus;
					mPreStatusForToggle = mPreStatus;
					mCurrentStatusForToggle = mCurrentStatus;
					//OnExit
					if (!string.IsNullOrEmpty(mPreStatusForToggle) && StatusDic.ContainsKey (mPreStatusForToggle)) {
						List<StatusAction> exitActions = StatusDic [mPreStatusForToggle];
						for (int i = 0; i < exitActions.Count; i++) {
							exitActions [i].OnExit ();
						}
					}
					//OnEnter
					if (!string.IsNullOrEmpty(mCurrentStatusForToggle) && StatusDic.ContainsKey (mCurrentStatusForToggle)) {
						List<StatusAction> enterActions = StatusDic [mCurrentStatusForToggle];
						for (int i = 0; i < enterActions.Count; i++) {
							enterActions [i].OnEnter ();
						}
					}

				}
			}
		}

		public string PreStatus ()
		{
			return mPreStatus;
		}

		public string CurrentStatus ()
		{
			return mCurrentStatus;
		}

		void Update ()
		{
			if (currentStatusAction != null) {
				for (int i = 0; i < currentStatusAction.Count; i++) {
					currentStatusAction [i].OnUpdate ();
				}
			}
		}
	}
}