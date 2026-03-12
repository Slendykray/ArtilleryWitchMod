using HenryMod.Modules;
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
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>();
            
            //bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 0f;
            //bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            //bombImpactExplosion.destroyOnEnemy = true;
            //bombImpactExplosion.lifetime = 12f;
            //bombImpactExplosion.impactEffect = bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            //bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            bombImpactExplosion.fireChildren = true;
            bombImpactExplosion.childrenCount = 1;
            bombImpactExplosion.childrenProjectilePrefab = _assetBundle.LoadAsset<GameObject>("gas");        

            GameObject g = _assetBundle.LoadAsset<GameObject>("gas");
            
            g.GetComponent<ProjectileController>().flightSoundLoop = 
                Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MiniMushroom/SporeGrenadeProjectileDotZone.prefab").WaitForCompletion().GetComponent<ProjectileController>().flightSoundLoop;

            g.transform.Find("Smoke").GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material =
                Addressables.LoadAssetAsync<Material>("RoR2/Base/MiniMushroom/matSporeGrenadeGasCloud.mat").WaitForCompletion();




           
           

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "Play_commando_M2_grenade_throw";
        }
        #endregion projectiles
    }
}
