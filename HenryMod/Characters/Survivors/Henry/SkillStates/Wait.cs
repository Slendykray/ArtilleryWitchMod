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
using EntityStates.Railgunner.Backpack;
using EntityStates.Railgunner.Weapon;

namespace HenryMod.Survivors.Henry.SkillStates
{
    public class Wait : BaseSkillState
    {
        public static float duration = HenryStaticValues.nukeWaitTime;
        public override void OnEnter()
        {      
            skillLocator.primary.SetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.utility.SetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.secondary.SetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.special.SetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);

            EntityStateMachine entityStateMachine2 = EntityStateMachine.FindByCustomName(base.gameObject, "Backpack");
            Offline of = new Offline();
            of.baseDuration = duration;
            EntityState entityState = of;

            if (entityStateMachine2 && entityState != null)
            {
                entityStateMachine2.SetNextState(entityState);
            }

            base.OnEnter();         
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.skillLocator.primary.UnsetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.utility.UnsetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.UnsetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.UnsetSkillOverride(base.characterBody, CharacterBody.CommonAssets.disabledSkill, GenericSkill.SkillOverridePriority.Contextual);

            base.OnExit();        
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }


    }
}
