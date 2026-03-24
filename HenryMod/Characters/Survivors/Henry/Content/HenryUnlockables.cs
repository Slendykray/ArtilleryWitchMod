using HenryMod.Survivors.Henry.Achievements;
using RoR2;
using UnityEngine;

namespace HenryMod.Survivors.Henry
{
    public static class HenryUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;
        public static UnlockableDef clusterUnlockableDef = null;
        public static UnlockableDef fistUnlockableDef = null;

        public static void Init()
        {
            //if no icon = break :((((
            //HenrySurvivor.instance.assetBundle.LoadAsset<Sprite>("dulich")

            characterUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ArtilleristUnlockAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ArtilleristUnlockAchievement.identifier),
                null);

            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                HenryMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier),
                HenrySurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));

            clusterUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
               ClusterAchievement.unlockableIdentifier,
               Modules.Tokens.GetAchievementNameToken(ClusterAchievement.identifier),
               null);

            fistUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
               NapalmAchievement.unlockableIdentifier,
               Modules.Tokens.GetAchievementNameToken(NapalmAchievement.identifier),
               null);
        }
    }
}
