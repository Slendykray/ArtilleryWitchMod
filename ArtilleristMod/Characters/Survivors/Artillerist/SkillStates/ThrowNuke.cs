using EntityStates.Mage.Weapon;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.Railgunner.Reload;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{
    public class ThrowNuke : BaseThrowBombState
    {

        public override void OnEnter()
        {
            projectilePrefab = ArtilleristAssets.nuke;
                
            minDamageCoefficient = ArtilleristStaticValues.nukeMinDamageCoefficient;
            maxDamageCoefficient = ArtilleristStaticValues.nukeMaxDamageCoefficient;
            //projectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIceBombProjectile.prefab").WaitForCompletion();
            baseDuration = 0.4f;
   
            base.OnEnter();

            Util.PlaySound("Play_mage_m2_iceSpear_shoot", gameObject);

            if (charge >= 1f)
            {
                this.outer.SetNextState(new Wait());
            }
        }
        public override void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
            base.ModifyProjectile(ref projectileInfo);
            projectileInfo.damageTypeOverride = new DamageTypeCombo?(new DamageTypeCombo(DamageType.Generic, DamageTypeExtended.Generic, DamageSource.Special));
        }

        //public override void PlayThrowAnimation() 
        //{
        //    PlayCrossfade("Gesture, Override", "Slash" + 1, "Slash.playbackRate", duration * 2f, 0.2f * duration);
        //    //base.PlayAnimation("Gesture, Additive", BaseThrowBombState.FireNovaBombStateHash, BaseThrowBombState.FireNovaBombParamHash, this.duration);
        //}

        public override void OnExit()
        {
            base.OnExit();         
        }


    }
}
