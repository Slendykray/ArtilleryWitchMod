using HenryMod.Modules;
using HenryMod.Survivors.Henry.Components;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry
{
    public static class HenryAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        private static AssetBundle _assetBundle;

        public static GameObject fireEffect;
        public static GameObject missileProjectilePrefab;
        public static GameObject chargeNuke;
        public static GameObject nuke;
        public static GameObject fireDot;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            Content.AddProjectilePrefab(bombProjectilePrefab);
        }

        private static void CreateBombProjectile()
        {
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>();       
            bombImpactExplosion.blastDamageCoefficient = 0f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            bombImpactExplosion.fireChildren = true;  
            bombImpactExplosion.childrenCount = 1;
            bombImpactExplosion.childrenProjectilePrefab = _assetBundle.LoadAsset<GameObject>("gas");

            //ProjectileDamage bombDamage = bombProjectilePrefab.GetComponent<ProjectileDamage>();
            //bombDamage.damageType = DamageType.Stun1s;


            GameObject gas = _assetBundle.LoadAsset<GameObject>("gas");

            gas.AddComponent<HenryWeaponComponent>();

            gas.GetComponent<ProjectileController>().flightSoundLoop = 
                Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MiniMushroom/SporeGrenadeProjectileDotZone.prefab").WaitForCompletion().GetComponent<ProjectileController>().flightSoundLoop;

            gas.transform.Find("Smoke").GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/MiniMushroom/matSporeGrenadeGasCloud.mat").WaitForCompletion();

          


            fireEffect = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryFBombProjectile");
            ProjectileImpactExplosion fireExplosion = fireEffect.GetComponent<ProjectileImpactExplosion>();
            fireExplosion.lifetime = 0f;
            fireExplosion.dotIndex = DotController.DotIndex.Burn;
            fireExplosion.dotDuration = 5f;
            fireExplosion.applyDot = true;
            fireExplosion.blastRadius = 16f;
            fireExplosion.blastImpactEffect = GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab;
            fireExplosion.bonusBlastForce = Vector3.zero;


            missileProjectilePrefab = Asset.CloneProjectilePrefab("MageFireboltBasic", "WitchMissileProjectile");
            ProjectileImpactExplosion missileImpactExplosion = missileProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            missileImpactExplosion.blastRadius = 8f;
            missileImpactExplosion.bonusBlastForce = Vector3.zero;
            ProjectileDamage damage = missileProjectilePrefab.GetComponent<ProjectileDamage>();
            damage.damageType = DamageType.Generic;

             

            GameObject missileGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/MissileGhost.prefab").WaitForCompletion();

            GameObject missileModel = R2API.PrefabAPI.InstantiateClone(missileGhost.transform.Find("missile VFX").gameObject, "missileModel");
            missileModel.transform.localScale *= 10f;

            chargeNuke = _assetBundle.LoadAsset<GameObject>("ChargeNuke");
            GameObject chargeNukeGhost = R2API.PrefabAPI.InstantiateClone(missileModel, "chargeNukeGhost");
            //chargeNukeGhost.transform.SetParent(chargeNuke.transform, false);
             

          
            GameObject nukeGhost = R2API.PrefabAPI.InstantiateClone(missileGhost, "nukeGhost");
            nukeGhost.transform.localScale *= 10f;

            nuke = _assetBundle.LoadAsset<GameObject>("Nuke");

            //nuke = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "nuke");

            ProjectileController nukeController = nuke.GetComponent<ProjectileController>();
            //nukeController.ghostPrefab = nukeGhost;

            ProjectileImpactExplosion nukeImpactExplosion = nuke.GetComponent<ProjectileImpactExplosion>();
            nukeImpactExplosion.explosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion(); 
            //nukeImpactExplosion.explosionEffect = bombExplosionEffect;
            //nukeImpactExplosion.explosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();
            //GameObject nukeImpact = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion(), "nukeImpact");
            //nukeImpact.GetComponent<EffectComponent>().soundName = "Play_mage_m2_iceSpear_impact";
            //nukeImpactExplosion.explosionEffect = nukeImpact;
            //nukeImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");

            fireDot = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Molotov/MolotovProjectileDotZone.prefab").WaitForCompletion(), "fireNuke");
            fireDot.transform.localScale *= 3f;
            //nukeImpactExplosion.childrenProjectilePrefab = fireDot;



            //nukeImpactExplosion.destroyOnEnemy = true;
            //nukeImpactExplosion.lifetimeAfterImpact = 0f;
            //nukeImpactExplosion.lifetime = 0f; 
            //nukeImpactExplosion.dotIndex = DotController.DotIndex.Burn;
            //nukeImpactExplosion.dotDuration = 5f;
            //nukeImpactExplosion.applyDot = true;
            //nukeImpactExplosion.blastRadius = 16f;
            //nukeImpactExplosion.blastImpactEffect = GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab;
            //nukeImpactExplosion.bonusBlastForce = Vector3.zero;


            //bombImpactExplosion.fireChildren = true;
            //bombImpactExplosion.childrenCount = 1;
            //bombImpactExplosion.childrenProjectilePrefab = _assetBundle.LoadAsset<GameObject>("gas");



            //nukeGhost.transform.SetParent(nuke.transform, false);

            //nuke.GetComponent<>
            //nuke = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion(), "nuke");

            //GameObject nukeGhost = R2API.PrefabAPI.InstantiateClone(nukeGhost, "nukeGhost");







        }
        #endregion projectiles
    }
}
