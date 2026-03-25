using RoR2;
using UnityEngine;


namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{

    public class NapalmDash : BaseDash
    {
        public override void OnEnter()
        {

         
            damageCoefficient = 0f;
                    
            base.OnEnter(); 
                 
            dashVector = -dashVector;
            dashSpeed = 9f;   
             
            float num3 = ArtilleristStaticValues.napalmDamageCoefficient;
            float baseDamage = characterBody.damage * num3;

            RoR2.Projectile.ProjectileManager.instance.FireProjectile(
                     ArtilleristAssets.napalm,
                     transform.position,
                     Quaternion.identity,
                     gameObject,
                     baseDamage,
                     0f,
                     Util.CheckRoll(characterBody.crit, characterBody.master),
                     damageType: DamageSource.Utility
                     );
        }

    }


}
