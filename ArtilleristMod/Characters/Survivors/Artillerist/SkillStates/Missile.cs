using EntityStates;
using ArtilleristMod.Survivors.Artillerist;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using RoR2.Skills;
using UnityEngine.Networking;
using EntityStates.Mage.Weapon;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{
    public class Missile : GenericProjectileBaseState, SteppedSkillDef.IStepSetter
    {
        public static float BaseDuration = 0.3f;
        //delays for projectiles feel absolute ass so only do this if you know what you're doing, otherwise it's best to keep it at 0
        public static float BaseDelayDuration = 0.0f;

        public static float DamageCoefficient = ArtilleristStaticValues.missileDamageCoefficient;

        int swingIndex;
 
        public void SetStep(int i)
        {
            swingIndex = i;
        }

        public override void OnEnter()
        {

            projectilePrefab = ArtilleristAssets.missileProjectilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            targetMuzzle = "muzzleThrow";
            
            attackSoundString = "Play_mage_m1_shoot";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 0f;

           
            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
            //Ray aimRay = base.GetAimRay();
            //TrajectoryAimAssist.ApplyTrajectoryAimAssist(ref aimRay, this.projectilePrefab, base.gameObject, 1f);
        }

        public override void ModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectileInfo(ref fireProjectileInfo);
            fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericPrimary;
            fireProjectileInfo.speedOverride = 100f;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        //public override InterruptPriority GetMinimumInterruptPriority()
        //{
        //    return InterruptPriority.PrioritySkill;
        //}

        public override void PlayAnimation(float duration)
        {

            if (GetModelAnimator())
            {
                base.PlayAnimation("Gesture Right, Additive", FireFireBolt.FireGauntletRightStateHash, FireFireBolt.FireGauntletParamHash, this.duration);
                base.PlayAnimation("Gesture, Additive", FireFireBolt.HoldGauntletsUpStateHash, FireFireBolt.FireGauntletParamHash, this.duration);
                //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", duration * 2f, 0.2f * duration);
                //PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
            }
        }
    }
}