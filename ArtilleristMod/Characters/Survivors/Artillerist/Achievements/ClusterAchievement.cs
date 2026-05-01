using RoR2;
using ArtilleristMod.Modules.Achievements;
using RoR2.Achievements;
using UnityEngine;
using R2API;

using System;
using Assets.RoR2.Scripts.Platform;

namespace ArtilleristMod.Survivors.Artillerist.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, typeof(ClusterServerAchievement))]
    public class ClusterAchievement : BaseAchievement
    {
        public const string identifier = ArtilleristSurvivor.HENRY_PREFIX + "ArtilleristAirWisp";
        public const string unlockableIdentifier = ArtilleristSurvivor.HENRY_PREFIX + "Skills.Artillerist.Cluster";

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

        private static readonly int requirement = 5;


        private class ClusterServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                RoR2Application.onFixedUpdate += this.OnFixedUpdate;
                GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeath;
            }

            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeath;
                RoR2Application.onFixedUpdate -= this.OnFixedUpdate;
                base.OnUninstall();
            }

            private bool CharacterIsInAir()
            {
                CharacterBody currentBody = base.networkUser.GetCurrentBody();
                return currentBody && currentBody.characterMotor && !currentBody.characterMotor.isGrounded;
            }

            private void OnFixedUpdate()
            {
                if (!this.CharacterIsInAir())
                {
                    this.killCount = 0;
                }
            }

            private void OnCharacterDeath(DamageReport damageReport)
            {
                if (damageReport.attackerMaster == base.networkUser.master && damageReport.attackerMaster != null && this.CharacterIsInAir())
                {
                    //Log.Message(damageReport.victimBody.name);
                    if (damageReport.victimBody.bodyIndex == BodyCatalog.FindBodyIndex("WispBody"))
                    {
                        this.killCount++;
                        if (ClusterAchievement.requirement <= this.killCount)
                        {
                            base.Grant();
                            base.ServerTryToCompleteActivity();
                        }
                    }
                    
                }
            }

            private int killCount;
        }
    }
}