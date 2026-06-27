using ArtilleristMod.Survivors.Artillerist.SkillStates;

namespace ArtilleristMod.Survivors.Artillerist
{
    public static class ArtilleristStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ThrowGas));
            Modules.Content.AddEntityState(typeof(Missile));
            Modules.Content.AddEntityState(typeof(Dash));
            Modules.Content.AddEntityState(typeof(ChargeNuke));
            Modules.Content.AddEntityState(typeof(ThrowNuke));
            Modules.Content.AddEntityState(typeof(Wait));
            Modules.Content.AddEntityState(typeof(NapalmDash));
            Modules.Content.AddEntityState(typeof(ThrowCluster));
            Modules.Content.AddEntityState(typeof(Crackle));
        }
    }
}
