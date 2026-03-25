using RoR2;
using ArtilleristMod.Modules.Achievements;

namespace ArtilleristMod.Survivors.Artillerist.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ArtilleristMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ArtilleristSurvivor.HENRY_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ArtilleristSurvivor.HENRY_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ArtilleristSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}