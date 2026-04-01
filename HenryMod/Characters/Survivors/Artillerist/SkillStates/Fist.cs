using ArtilleristMod.Modules.BaseStates;
using ArtilleristMod.Survivors.Artillerist;
using EntityStates;
using EntityStates.Loader;
using EntityStates.Mage.Weapon;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{
    public class Fist : BaseTimedSkillState
    {
        private bool _heldTooLongYaDoofus;
        private bool _inputDown;

        public override float TimedBaseDuration => 2f;
        public override float TimedBaseCastStartPercentTime => 0.8f;

        private Transform slamIndicatorInstance;

        private float slamForce = 3000f;
        private float slamRadius = 25f;

        public override void OnEnter()
        {
            base.OnEnter();

            //skillLocator.special.SetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            float dur = TimedBaseDuration * TimedBaseCastStartPercentTime;
            //base.PlayAnimation("Gesture, Additive", BaseChargeBombState.ChargeNovaBombStateHash, BaseChargeBombState.ChargeNovaBombParamHash, dur);
            base.PlayAnimation("Gesture, Additive", PrepWall.PrepWallStateHash, PrepWall.PrepWallParamHash, dur);

            if (isAuthority && base.inputBank.skill4.down)
            {
                _heldTooLongYaDoofus = true;
            }
            else
            {
                _inputDown = true;
            }
        }

        protected override void OnCastEnter()
        {
            //if (isAuthority)
            //{
            //    AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
            //}
            //Util.PlaySound("", gameObject);

            EffectManager.SimpleEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/OmniExplosionVFXCommandoGrenade.prefab").WaitForCompletion(), transform.position, Quaternion.identity, false);

            float dur = TimedBaseDuration * (1 - TimedBaseCastStartPercentTime);


            //base.PlayAnimation("Gesture, Additive", BaseThrowBombState.FireNovaBombStateHash, BaseThrowBombState.FireNovaBombParamHash, dur);

            this.PlayAnimation("Gesture, Additive", PrepWall.FireWallStateHash);

            if (!this.slamIndicatorInstance) this.CreateIndicator();

            Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justReleased)
            {
                _heldTooLongYaDoofus = false;
            }
            if (!_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justPressed)
            {
                _inputDown = true;
            }

            //if (isAuthority && inputBank.skill4.justReleased && _inputDown)
            //{
            //    GetModelAnimator().Rebind();

            //    base.skillLocator.special.UnsetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);

            //    this.outer.SetNextStateToMain();
            //}


        }
        private void Fire()
        {
            List<HurtBox> HurtBoxes = new List<HurtBox>();
            HurtBoxes = new SphereSearch
            {
                radius = slamRadius,
                mask = LayerIndex.entityPrecise.mask,
                origin = transform.position
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes().ToList();

            foreach (HurtBox hurtbox in HurtBoxes)
            {
                //float _level = Mathf.Floor(base.characterBody.level / 4f);
                //float bonus = HayMaker.hayMakerGritBonus + (_level * HayMaker.hayMakerGritBonusPer4);
                //Vector3 direction = (hurtbox.gameObject.transform.position - base.characterBody.corePosition).normalized;


                Vector3 force = Vector3.zero;

                HealthComponent healthComponent = hurtbox.healthComponent;
                if (healthComponent)
                {
                    Vector3 dir = Vector3.up * slamForce;

                    CharacterMotor motor = healthComponent.body.characterMotor;
                    if (motor)
                    {
                        float massFactor = motor.mass / 100f;
                        force = Vector3.zero + dir * massFactor;
                        //motor.ApplyForce(Vector3.zero + dir * massFactor);
                    }
                }

                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = this.damageStat * ArtilleristStaticValues.fistDamageCoefficient;
                damageInfo.attacker = base.gameObject;
                damageInfo.inflictor = base.gameObject;
                damageInfo.force = force;
                damageInfo.crit = base.RollCrit();
                damageInfo.procCoefficient = 1f;
                damageInfo.position = hurtbox.gameObject.transform.position;
                damageInfo.damageType = DamageType.Stun1s;
                //DamageAPI.AddModdedDamageType(damageInfo, SettPlugin.settDamage);

                hurtbox.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, hurtbox.healthComponent.gameObject);
                GlobalEventManager.instance.OnHitAll(damageInfo, hurtbox.healthComponent.gameObject);
                //GameObject hitEffectPrefab = Modules.Assets.swordHitImpactEffect;
                //if (hitEffectPrefab)
                //{
                //    EffectManager.SpawnEffect(hitEffectPrefab, new EffectData
                //    {
                //        origin = hurtbox.healthComponent.gameObject.transform.position,
                //        rotation = Quaternion.identity,
                //        networkSoundEventIndex = Modules.Assets.swordHitSoundEvent.index
                //    }, true);
                //}



               
            }
        }

       
        private void UpdateSlamIndicator()
        {
            if (this.slamIndicatorInstance)
            {
                float maxDistance = 250f;

                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                RaycastHit raycastHit;
                if (Physics.Raycast(this.downRay, out raycastHit, maxDistance, LayerIndex.world.mask))
                {
                    this.slamIndicatorInstance.transform.position = raycastHit.point;
                    this.slamIndicatorInstance.transform.up = raycastHit.normal;
                }
            }
        }
        private Ray downRay;

        private void CreateIndicator()
        {
            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.downRay = new Ray
                {
                    direction = Vector3.down,
                    origin = base.transform.position
                };

                this.slamIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab).transform;
                this.slamIndicatorInstance.localScale = Vector3.one * slamRadius;
            }
        }
        public override void Update()
        {
            base.Update();

            if (this.slamIndicatorInstance) this.UpdateSlamIndicator();
        }

        public override void OnExit()
        {
            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);
            base.OnExit();
        }

        //protected override void SetNextState()
        //{     
        //    outer.SetNextState(new Fist());
        //}


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }


}
