using EntityStates;
using EntityStates.Merc;
using HenryMod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace HenryMod.Survivors.Henry.SkillStates
{

    public class Dash : BaseMeleeAttack
    {

        protected Vector3 dashVector;

        private int originalLayer;

        protected float dashSpeed = 12f;

        public override void OnEnter()
        {
            hitboxGroupName = "SwordGroup";

            damageType = DamageType.Stun1s;
            damageCoefficient = HenryStaticValues.dashDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 0.25f;

            attackStartPercentTime = 0f;
            attackEndPercentTime = 1f;
            earlyExitPercentTime = 1f;

            hitStopDuration = 0f;
            attackRecoil = 0f;
            hitHopVelocity = 0f;

            hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion();

            base.OnEnter();

            dashVector = inputBank.aimDirection;
            this.originalLayer = base.gameObject.layer;


            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Cloak, duration);
            }

            Util.PlaySound("Play_bandit2_shift_enter", gameObject);
            EffectManager.SimpleEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2SmokeBomb.prefab").WaitForCompletion(), characterBody.footPosition, transform.rotation, false);
             
            Util.PlaySound("Stop_bandit2_shift_loop", gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterMotor.velocity = Vector3.zero;

            characterDirection.forward = dashVector;

            base.characterMotor.rootMotion += dashVector * (this.moveSpeedStat * dashSpeed * base.GetDeltaTime());
        
            base.gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(base.teamComponent.teamIndex).intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();           
        }

        public override void OnExit()
        {
         

            base.characterMotor.velocity *= 0.1f;
            SmallHop(characterMotor, 2f);

            base.gameObject.layer = this.originalLayer;
            base.characterMotor.Motor.RebuildCollidableLayers();
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        protected override void PlayAttackAnimation()
        {
        }

        protected override void PlaySwingEffect()
        {
        }

    }


}
