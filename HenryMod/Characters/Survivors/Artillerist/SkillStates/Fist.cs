using EntityStates.Loader;
using EntityStates.Mage.Weapon;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{

    public class Fist : GroundSlam
    {
		//private float upForce = 12f;
		//private float forceDur = 0.25f;
		public override void OnEnter()
        {
            blastRadius = 20f;
            //blastBonusForce = Vector3.zero;
			//blastBonusForce *= 2f;
			blastDamageCoefficient = ArtilleristStaticValues.fistDamageCoefficient;
           //blastEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion();

			base.OnEnter();
                 
			base.PlayAnimation("Gesture, Additive", BaseChargeBombState.ChargeNovaBombStateHash, BaseChargeBombState.ChargeNovaBombParamHash, 1f);

            //gameObject.AddComponent<IgnoreFallDamage>();
            //DetonateAuthority(
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
        }



  //      public override void FixedUpdate()
		//{
		//	base.FixedUpdate();
		//	if (base.isAuthority && base.characterMotor)
		//	{
		//		base.characterMotor.moveDirection = base.inputBank.moveVector;
		//		base.characterDirection.moveVector = base.characterMotor.moveDirection;
		//		CharacterMotor characterMotor = base.characterMotor;
		//		characterMotor.velocity.y = characterMotor.velocity.y + GroundSlam.verticalAcceleration * base.GetDeltaTime();
		//		if (base.fixedAge >= GroundSlam.minimumDuration && (this.detonateNextFrame || base.characterMotor.Motor.GroundingStatus.IsStableOnGround))
		//		{
		//			BlastAttack.HitPoint[] points =  this.DetonateAuthority().hitPoints;

		//			float stopwatch = 0f;
		//			stopwatch += GetDeltaTime();

		//			for (int i = 0; i < points.Length; i++)
		//			{					
		//				HealthComponent healthComponent = points[i].hurtBox.healthComponent;
		//				if (healthComponent)
		//				{
		//					Vector3 force = Vector3.up * upForce;
		//					CharacterMotor motor = healthComponent.body.characterMotor;
		//					if (motor)
		//					{
		//						motor.rootMotion.y += upForce;
		//					}



		//				}
		//			}
		//			if (stopwatch >= forceDur)
		//				this.outer.SetNextStateToMain();
		//		}
		//	}
		//}

	


		public override void OnExit()
        {

            //Destroy(gameObject.GetComponent<IgnoreFallDamage>());
            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            base.PlayAnimation("Gesture, Additive", BaseThrowBombState.FireNovaBombStateHash, BaseThrowBombState.FireNovaBombParamHash, 1f);

			base.OnExit();
        }
    }


}
