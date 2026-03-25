using EntityStates;
using ArtilleristMod.Survivors.Artillerist;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Mage.Weapon;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{
    public class ThrowBomb : GenericProjectileBaseState
    {
        public static float BaseDuration = 0.65f;
        //delays for projectiles feel absolute ass so only do this if you know what you're doing, otherwise it's best to keep it at 0
        public static float BaseDelayDuration = 0.0f;

        public static float DamageCoefficient = 16f;

        public override void OnEnter()
        {
            projectilePrefab = ArtilleristAssets.bombProjectilePrefab;
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            attackSoundString = "Play_commando_M2_grenade_throw";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;

            base.OnEnter();
        }

        public override void ModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectileInfo(ref fireProjectileInfo);
            fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericSpecial;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {

            if (GetModelAnimator())
            {
                base.PlayAnimation("Gesture Left, Additive", FireFireBolt.FireGauntletLeftStateHash, FireFireBolt.FireGauntletParamHash, this.duration);
                base.PlayAnimation("Gesture, Additive", FireFireBolt.HoldGauntletsUpStateHash, FireFireBolt.FireGauntletParamHash, this.duration);
            }
        }
    }
}