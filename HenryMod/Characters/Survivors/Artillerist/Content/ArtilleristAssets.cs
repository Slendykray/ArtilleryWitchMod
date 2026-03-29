using ArtilleristMod.Modules;
using ArtilleristMod.Survivors.Artillerist.Components;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace ArtilleristMod.Survivors.Artillerist
{
    public static class ArtilleristAssets
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
        public static GameObject cluster;
        public static GameObject napalm;

      

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

            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "ArtilleristGasGrenade");

            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>();       
            bombImpactExplosion.blastDamageCoefficient = 0f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            bombImpactExplosion.fireChildren = true;  
            bombImpactExplosion.childrenCount = 1;
            bombImpactExplosion.childrenProjectilePrefab = _assetBundle.LoadAsset<GameObject>("gas");

            GameObject gas = _assetBundle.LoadAsset<GameObject>("gas");

            gas.AddComponent<ArtilleristWeaponComponent>();

            gas.GetComponent<ProjectileController>().flightSoundLoop = 
                Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MiniMushroom/SporeGrenadeProjectileDotZone.prefab").WaitForCompletion().GetComponent<ProjectileController>().flightSoundLoop;

            gas.transform.Find("Smoke").GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/MiniMushroom/matSporeGrenadeGasCloud.mat").WaitForCompletion();

      
            fireEffect = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "ArtilleristGasExplosion");
            ProjectileImpactExplosion fireExplosion = fireEffect.GetComponent<ProjectileImpactExplosion>();
            fireExplosion.lifetime = 0f; 
            fireExplosion.dotIndex = DotController.DotIndex.Burn;
            fireExplosion.dotDuration = 5f;
            fireExplosion.applyDot = true;
            fireExplosion.blastRadius = 16f;
            fireExplosion.blastImpactEffect = GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab;
            fireExplosion.bonusBlastForce = Vector3.zero;
            fireExplosion.falloffModel = BlastAttack.FalloffModel.None;


            //missile
            missileProjectilePrefab = Asset.CloneProjectilePrefab("MageFireboltBasic", "ArtilleristMissileProjectile");
            ProjectileImpactExplosion missileImpactExplosion = missileProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            missileImpactExplosion.blastRadius = 8f;
            missileImpactExplosion.bonusBlastForce = Vector3.zero;
            ProjectileDamage damage = missileProjectilePrefab.GetComponent<ProjectileDamage>();
            damage.damageType = DamageType.Generic;




            //nuke
            chargeNuke = _assetBundle.LoadAsset<GameObject>("ChargeNuke");
            
            nuke = _assetBundle.LoadAsset<GameObject>("Nuke");

            ProjectileImpactExplosion nukeImpactExplosion = nuke.GetComponent<ProjectileImpactExplosion>();
            nukeImpactExplosion.explosionEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion(); 

            fireDot = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Molotov/MolotovProjectileDotZone.prefab").WaitForCompletion(), "ArtilleristFireNuke");
            fireDot.transform.localScale *= 3f;


            //cluster
            GameObject clusterBomblet = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/CryoCanisterBombletsProjectile.prefab").WaitForCompletion(), "ArtilleristClusterBomblet");
            clusterBomblet.GetComponent<ProjectileSimple>().desiredForwardSpeed = 15f;
            clusterBomblet.GetComponent<ProjectileImpactExplosion>().blastRadius = 9f;

            cluster = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/CryoCanisterProjectile.prefab").WaitForCompletion(), "ArtilleristCluster");
            ProjectileImpactExplosion clusterExplosion = cluster.GetComponent<ProjectileImpactExplosion>();
            clusterExplosion.childrenDamageCoefficient = 1f;
            clusterExplosion.childrenCount = ArtilleristStaticValues.clusterBomblets;
            clusterExplosion.destroyOnWorld = false;
            clusterExplosion.timerAfterImpact = true;
            clusterExplosion.lifetimeAfterImpact = 0.1f;
      

            clusterExplosion.childrenProjectilePrefab = clusterBomblet;
            clusterExplosion.rangeRollDegrees = 360f;

            ProjectileController clusterController = cluster.GetComponent<ProjectileController>();
            clusterController.ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoGrenadeGhost.prefab").WaitForCompletion();


            //napalm

            napalm = R2API.PrefabAPI.InstantiateClone(fireEffect, "ArtilleristNapalm");
            ProjectileImpactExplosion napalmExplosion = napalm.GetComponent<ProjectileImpactExplosion>();        
            napalmExplosion.fireChildren = true;
            napalmExplosion.childrenCount = 1;  
            GameObject napalmDot = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Molotov/MolotovProjectileDotZone.prefab").WaitForCompletion(), "ArtilleristNapalmDot");
            napalmDot.transform.localScale *= 2f;
            napalmExplosion.childrenProjectilePrefab = napalmDot;
            napalmExplosion.transformSpace = ProjectileImpactExplosion.TransformSpace.Normal;
            napalmExplosion.falloffModel = BlastAttack.FalloffModel.None;
        }
        #endregion projectiles
    }
}
