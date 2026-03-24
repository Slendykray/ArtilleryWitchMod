using System;
using HenryMod.Modules;
using HenryMod.Survivors.Henry.Achievements;

namespace HenryMod.Survivors.Henry
{
    public static class HenryTokens
    {
        public static void Init()
        {
            AddHenryTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddHenryTokens()
        {
            string prefix = HenrySurvivor.HENRY_PREFIX;

            //string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
            // + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
            // + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
            // + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
            // + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string desc = "";

            string outro = "..and so she left with more questions than answers.";
            string outroFailure = "..and so she left, and empire in ruins and hunt abandoned.";

            string lore = "The High court was wrong, how could heaven be a planet when the worst monsters ever known came from one? Heaven couldn't be so easy for someone like them to reach, it was just impossible.\r\n\r\n" +
                "The seeds of doubt easily bear fruit when logic and questioning is outright rejected.\r\n\r\n" +
                "And from the courts that now burned and lay in ruin came something that knew it didn't belong, knew that what they stood for were a lie, and with no where to go there was only one place that could possibly give the answers that were promised to the Artificers.\r\n\r\n" +
                "Pertrichor V.";

            Language.Add(prefix + "NAME", "Artillerist");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Betrayer of the High Court");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Henry passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_MISSILE_NAME", "Missile");
            Language.Add(prefix + "PRIMARY_MISSILE_DESCRIPTION", $"Fire a small missile for {Tokens.DamageValueText(HenryStaticValues.missileDamageCoefficient)}. Hold up to 3.");
            #endregion

            #region Secondary

            Language.Add(prefix + "SECONDARY_GAS_NAME", "Gas grenade");
            Language.Add(prefix + "SECONDARY_GAS_DESCRIPTION", Tokens.poisonousPrefix + $"Throw a gas bomb. Can be ignited with Missile for {Tokens.DamageValueText(HenryStaticValues.gasExplosionDamageCoefficient)}.");

            Language.Add(prefix + "SECONDARY_CLUSTER_NAME", "Cluster grenade");
            Language.Add(prefix + "SECONDARY_CLUSTER_DESCRIPTION", Tokens.stunningPrefix + $"Throw a cluster bomb for {Tokens.MultDamageValueText(HenryStaticValues.clusterBomblets + 1, HenryStaticValues.clusterDamageCoefficient)}.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_DASH_NAME", "Dash");
            Language.Add(prefix + "UTILITY_DASH_DESCRIPTION", Tokens.stunningPrefix + $"Dash forward through enemies and damages them for {Tokens.DamageValueText(HenryStaticValues.dashDamageCoefficient)}");

            Language.Add(prefix + "UTILITY_FIREDASH_NAME", "Fire Dash");
            Language.Add(prefix + "UTILITY_FIREDASH_DESCRIPTION", $"Dash backwards leaving a napalm bomb for {Tokens.DamageValueText(HenryStaticValues.napalmDamageCoefficient)} in your place igniting a small area.");
            #endregion
             
            #region Special
            Language.Add(prefix + "SPECIAL_NUKE_NAME", "Nuke");
            Language.Add(prefix + "SPECIAL_NUKE_DESCRIPTION", Tokens.stunningPrefix + $"Charge up a massive bomb for {Tokens.RangeDamageValueText(HenryStaticValues.nukeMinDamageCoefficient, HenryStaticValues.nukeMaxDamageCoefficient)}. At max charge will ignite a big area and {Tokens.RedText($"disable abilities for {HenryStaticValues.nukeWaitTime}s")}.");

            Language.Add(prefix + "SPECIAL_FIST_NAME", "Fist");
            Language.Add(prefix + "SPECIAL_FIST_DESCRIPTION", Tokens.stunningPrefix + $"Slam your fist into the ground to explode for {Tokens.DamageValueText(HenryStaticValues.fistDamageCoefficient)}.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ArtilleristUnlockAchievement.identifier), "Walking Artillery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ArtilleristUnlockAchievement.identifier), "Have a stun grenade, a sticky bomb, a bundle of fireworks, an atg missile, and either a brilliant behemoth or pocket icbm at the same time.");

            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Artillerist: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Artillerist, beat the game or obliterate on Monsoon.");

            Language.Add(Tokens.GetAchievementNameToken(ClusterAchievement.identifier), "Cluster");
            Language.Add(Tokens.GetAchievementDescriptionToken(ClusterAchievement.identifier), "Kill 5 wisps while airborne");

            Language.Add(Tokens.GetAchievementNameToken(NapalmAchievement.identifier), "Napalm");
            Language.Add(Tokens.GetAchievementDescriptionToken(NapalmAchievement.identifier), "Kill a boss with ignite effects");
            #endregion
        }
    }
}
