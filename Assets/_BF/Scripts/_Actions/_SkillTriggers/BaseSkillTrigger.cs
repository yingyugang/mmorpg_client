using System.Collections;
using StatusMachines;
using System.Collections.Generic;

public class BaseSkillTrigger: StatusAction {

	public BaseSkill baseSkill;
	//在回合游戏中，把对方所有单位都认为是在攻击搜索范围以内的。
	public virtual List<Unit> GetTargets ()
	{
		List<Unit> units = ScanUtility.GetTargets (baseSkill);
		return units;
	}

}