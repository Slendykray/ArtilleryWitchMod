using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ArtilleristMod.Survivors.Artillerist.Components
{
    internal class ArtilleristWeaponComponent : MonoBehaviour
    {
        private void Awake()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.name.Contains("ArtilleristMissileProjectile") || other.transform.name.Contains("Nuke"))
            {
                ProjectileController pc = other.GetComponent<ProjectileController>();
                FuckingExplode(pc.owner, transform.position);

                other.GetComponent<ProjectileImpactExplosion>().lifetime = 0f;
                //Destroy(other.gameObject);
            }
        }

        public void FuckingExplode(GameObject attacker, Vector3 pos)
        {
            CharacterBody body = attacker.GetComponent<CharacterBody>();

            float num3 = ArtilleristStaticValues.gasExplosionDamageCoefficient;
            float baseDamage = body.damage * num3;

            RoR2.Projectile.ProjectileManager.instance.FireProjectile(
                     ArtilleristAssets.fireEffect,
                     pos,
                     Quaternion.identity,
                     attacker,
                     baseDamage,
                     0f,
                     Util.CheckRoll(body.crit, body.master),
                     damageType: DamageSource.Primary
                     );

            Destroy(gameObject);
        }
    }
}