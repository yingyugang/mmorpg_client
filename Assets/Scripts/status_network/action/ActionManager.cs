using System.Collections;
using System.Collections.Generic;
using TPS.CameraControl;
using UnityEngine;


namespace MMO
{
    public class ActionManager : SingleMonoBehaviour<ActionManager>
    {
        //1:unit skill,2:start auto,3:end auto(must),4,change status(actionId:1=idle,2=move,3=run,4=death,演出するしかない)。
        public void DoAction(StatusInfo action)
        {
            MMOUnit unit = MMOController.Instance.GetUnitByUnitId(action.casterId);
            if (unit == null)
            {
                Debug.Log("unit is null.");
                return;
            }
            switch (action.status)
            {
                case BattleConst.UnitMachineStatus.STANDBY:
                    //TODO 共通化が必要だ。
                    if (MMOController.Instance.playType == PlayType.RPG || !unit.unitInfo.isPlayer)
                    {
                        unit.unitAnimator.Play(AnimationConstant.UNIT_ANIMATION_CLIP_IDEL);
                    }
                    unit.unitAnimator.SetSpeed(1);
                    unit.unitAnimator.SetMoveSpeed(0);
                    break;
                case BattleConst.UnitMachineStatus.MOVE:
                    unit.unitAnimator.SetTrigger(AnimationConstant.UNIT_ANIMATION_CLIP_RUN);
                    unit.unitAnimator.SetMoveSpeed(3.0f);
                    break;
                case BattleConst.UnitMachineStatus.CAST:
                    //当たり前なプレーヤーのアクションは動かない。
                    if (!MMOController.Instance.IsPlayer(action.casterId))
                    {
                        DoSkill(action);
                    }
                    break;
                case BattleConst.UnitMachineStatus.DEATH:
                    unit.Death();
                    //TODO 共通化が必要だ。
                    if (!MMOController.Instance.IsPlayer(unit.unitInfo.attribute.unitId) && (MMOController.Instance.playType == PlayType.RPG || !unit.unitInfo.isPlayer))
                    {
                        unit.unitAnimator.Play(AnimationConstant.UNIT_ANIMATION_CLIP_DEAD);
                    }
                    else
                    {
                        unit.unitAnimator.SetTrigger(AnimationConstant.UNIT_ANIMATION_PARAMETER_DEAD);
                    }
                    unit.unitAnimator.SetSpeed(1);
                    break;
                case BattleConst.UnitMachineStatus.RESPAWN:
                    PerformManager.Instance.ShowRespawnEffect(unit.transform.position);
                    break;
                case BattleConst.UnitMachineStatus.FIRE:
                    unit.unitAnimator.StartFire();
                    break;
                case BattleConst.UnitMachineStatus.UNFIRE:
                    unit.unitAnimator.StopFire();
                    break;
                case BattleConst.UnitMachineStatus.JUMP:
                    unit.unitAnimator.Jump();
                    break;
                case BattleConst.UnitMachineStatus.LYING:
                    unit.unitAnimator.Lying();
                    break;
                case BattleConst.UnitMachineStatus.UNLYING:
                    unit.unitAnimator.Lying();
                    break;
                case BattleConst.UnitMachineStatus.RELOAD:
                    unit.unitAnimator.Reload();
                    break;
                case BattleConst.UnitMachineStatus.SQUAT:
                    unit.unitAnimator.Squat();
                    break;
                case BattleConst.UnitMachineStatus.UNSQUAT:
                    unit.unitAnimator.Squat();
                    break;
                default:
                    unit.unitAnimator.Play(AnimationConstant.UNIT_ANIMATION_CLIP_IDEL);
                    unit.unitAnimator.SetSpeed(1);
                    break;
            }
        }
        //Do Skill.
        public void DoSkill(StatusInfo action)
        {
            //now the action.actionId means the skill id.
            StartCoroutine(_Cast(action));
        }

        //Do Shoot
        public void DoShoot(ShootInfo shootInfo)
        {
            MMOUnit caster = MMOController.Instance.GetUnitByUnitId(shootInfo.casterId);
            if (shootInfo.unitSkillId == BattleConst.DEFAULT_GUN_SHOOT_ID)
            {
                RaycastHit hit;
                caster.UnCollider();
                if (caster.characterEffectUtility)
                {
                    caster.characterEffectUtility.ShowSlash();
                }
                if (!MMOController.Instance.IsPlayer(shootInfo.casterId))
                {
                    SoundManager.Instance.PlayShoot(caster.playerAudioSource);
                }
                if (Physics.Raycast(IntVector3.ToVector3(shootInfo.position), IntVector3.ToVector3(shootInfo.forward), out hit, Mathf.Infinity))
                {
                    PerformManager.Instance.ShowBulletHit(hit.point, hit.normal, hit.collider.gameObject.layer);
                }
                caster.EnCollider();
            }
            else
            {
                MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic[shootInfo.unitSkillId];
                MSkill mSkill = CSVManager.Instance.skillDic[mUnitSkill.skill_id];
                MMOUnit target = null;
                if (shootInfo.targetId >= 0)
                {
                    target = MMOController.Instance.GetUnitByUnitId(shootInfo.targetId);
                }
                GameObject effect = ResourcesManager.Instance.GetEffect(mUnitSkill.skillShoot.shoot_object_id);
                Shoot(effect, mSkill, caster, target);
            }
        }

        //TODO use trigger to controll the cast clip;
        //TODO the end time point need to same as the animclip end time point.
        //TODO need to get the real skill information from csv.
        IEnumerator _Cast(StatusInfo action)
        {
            MUnitSkill mUnitSkill = CSVManager.Instance.unitSkillDic[action.actionId];
            MMOUnit caster = MMOController.Instance.GetUnitByUnitId(action.casterId);
            //TODO need know the cast anim name from csv or from other area.
            caster.unitAnimator.SetTrigger(mUnitSkill.anim_name);
            yield return null;
        }
        //Shoot Action.
        public void Shoot(GameObject shootPrefab, MSkill mSkill, MMOUnit caster, MMOUnit target)
        {
            Vector3 spawnPos;
            UnitPerform unitPerform = caster.GetComponent<UnitPerform>();
            if (unitPerform != null && unitPerform.shootPoint != null)
            {
                spawnPos = caster.GetComponent<UnitPerform>().shootPoint.position;
            }
            else
            {
                spawnPos = caster.GetBodyPos();
            }
            GameObject shootGo = Instantiater.Spawn(false, shootPrefab, spawnPos, caster.transform.rotation * Quaternion.Euler(60, 0, 0));
            ShootObject shootObj = null;
            Vector3 targetPos = MMOController.Instance.GetTerrainPos(caster.transform.position + caster.transform.forward * mSkill.range);
            //TODO need to check logic.
            switch (mSkill.skillShoot.shoot_type)
            {
                case BattleConst.BattleShoot.PROJECTILE:
                    shootObj = shootGo.GetOrAddComponent<ShootProjectileObject>();
                    shootObj.maxShootRange = 3;
                    shootObj.speed = mSkill.skillShoot.shoot_move_speed;
                    ShootProjectileObject shootProjectileObject = (ShootProjectileObject)shootObj;
                    if (target == null)
                    {
                        if (MMOController.Instance.IsPlayer(caster.unitInfo.attribute.unitId))
                        {
                            TPSCameraController tps = (TPSCameraController)MMOController.Instance.cameraController;
                            shootProjectileObject.maxShootAngle = 30 - tps.angle;
                            targetPos = MMOController.Instance.GetTerrainPos(caster.transform.position + caster.transform.forward * mSkill.range * (1 - tps.angle / 100f));
                        }
                    }
                    break;
                default:
                    shootObj = shootGo.GetOrAddComponent<ShootLineObject>();
                    shootObj.speed = mSkill.skillShoot.shoot_move_speed;
                    break;
            }
            if (target != null)
            {
                shootObj.Shoot(caster, target, new Vector3(0, target.GetBodyHeight() / 2f, 0));
            }
            else
            {
                shootObj.Shoot(caster, targetPos, Vector3.zero);
            }
        }
        //Do Respawn.
        public void DoRespawn(int unitId)
        {
            if (MMOController.Instance.IsPlayer(unitId))
            {
                PanelManager.Instance.HideCommonDialog();
                PerformManager.Instance.HideCurrentPlayerDeathEffect();
                MMOUnit playerUnit = MMOController.Instance.player.GetComponent<MMOUnit>();
                if (playerUnit.GetComponent<BasePlayerController>() != null)
                    playerUnit.GetComponent<BasePlayerController>().enabled = true;
                if (playerUnit.GetComponent<CharacterController>() != null)
                {
                    playerUnit.GetComponent<CharacterController>().enabled = true;
                }
                switch (MMOController.Instance.playType)
                {
                    case PlayType.RPG:
                        break;
                    case PlayType.TPS:
                        PanelManager.Instance.mainInterfacePanel.ShowAims();
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //TODO Do other monster respawn;
                MMOUnit playerUnit = MMOController.Instance.GetUnitByUnitId(unitId);
                if (playerUnit != null && playerUnit.headUIBase != null)
                {
                    playerUnit.headUIBase.ShowHealthBar();
                }
                if (playerUnit.GetComponent<Collider>() != null)
                {
                    playerUnit.GetComponent<Collider>().enabled = true;
                }
            }
            MMOUnit mmoUnit = MMOController.Instance.GetUnitByUnitId(unitId);
            mmoUnit.unitAnimator.ResetTriggers();
            mmoUnit.isDead = false;
            PerformManager.Instance.ShowRespawnEffect(mmoUnit.transform.position);
        }


    }
}
