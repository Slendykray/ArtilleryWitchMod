using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace ArtilleristMod.Survivors.Artillerist.Components
{
    internal class NukeDynamicScaler : MonoBehaviour
    {
        private ProjectileDamage projectileDamage;
        private ProjectileImpactExplosion impactExplosion;
        private ProjectileController controller;
        void Awake()
        {
            projectileDamage = GetComponent<ProjectileDamage>();
            impactExplosion = GetComponent<ProjectileImpactExplosion>();
            controller = GetComponent<ProjectileController>();
        }

        private void Start()
        {      
            CharacterBody ownerBody = controller.owner.GetComponent<CharacterBody>();

            float totalDamage = projectileDamage.damage;
            float baseDamage = ownerBody.damage;
            float currentCoefficient = totalDamage / baseDamage;

            // Map the coefficient back to a 0.0 - 1.0 charge value
            float minCoeff = ArtilleristStaticValues.nukeMinDamageCoefficient;
            float maxCoeff = ArtilleristStaticValues.nukeMaxDamageCoefficient;
            float charge = Mathf.InverseLerp(minCoeff, maxCoeff, currentCoefficient);



            impactExplosion.blastRadius = ArtilleristStaticValues.nukeMaxRadius * charge;

            if (charge >= 1f)
            {
                impactExplosion.childrenProjectilePrefab = ArtilleristAssets.fireDot;
                impactExplosion.fireChildren = true;
            }
            else
            {
                impactExplosion.childrenProjectilePrefab = null;
                impactExplosion.fireChildren = false;
            }
        }

       
    }
}