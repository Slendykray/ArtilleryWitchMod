using RoR2;
using HenryMod.Modules.Achievements;
using RoR2.Achievements;
using UnityEngine;
using R2API;

using System;
using Assets.RoR2.Scripts.Platform;

namespace HenryMod.Survivors.Henry.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, typeof(FistServerAchievement))]
    public class NapalmAchievement : BaseAchievement
    {
        public const string identifier = HenrySurvivor.HENRY_PREFIX + "ArtilleristBossFire";
        public const string unlockableIdentifier = HenrySurvivor.HENRY_PREFIX + "Skills.Artillerist.Napalm";

        public override BodyIndex LookUpRequiredBodyIndex()   
        { 
            return BodyCatalog.FindBodyIndex("ArtilleristBody"); 
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        public override void TryToCompleteActivity()
        {
            if (base.localUser.id == LocalUserManager.GetFirstLocalUser().id && this.shouldGrant)
            {
                BaseActivitySelector baseActivitySelector = new BaseActivitySelector();
                baseActivitySelector.activityAchievementID = identifier;
                PlatformSystems.activityManager.TryToCompleteActivity(baseActivitySelector, true, true);
            }
        }


        private class FistServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
   
                GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeath;
            }

            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeath;

                base.OnUninstall();
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {
                if (damageReport.attackerMaster == base.networkUser.master && damageReport.attackerMaster != null)
                {
                    if (damageReport.victimIsBoss)
                    {                
                        if (damageReport.dotType == DotController.DotIndex.Burn)
                        {
                            base.Grant();
                            base.ServerTryToCompleteActivity();
                        }
                    }
                    
                }
            }

        }
    }
}