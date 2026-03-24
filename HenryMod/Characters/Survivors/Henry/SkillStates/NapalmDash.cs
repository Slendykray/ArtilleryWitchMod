using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.SkillStates
{

    public class NapalmDash : BaseDash
    {
        public override void OnEnter()
        {

         
            damageCoefficient = 0f;
                    
            base.OnEnter(); 
                 
            dashVector = -dashVector;
            dashSpeed = 9f;   
             
            float num3 = HenryStaticValues.napalmDamageCoefficient;
            float baseDamage = characterBody.damage * num3;

            RoR2.Projectile.ProjectileManager.instance.FireProjectile(
                     HenryAssets.napalm,
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
