using HenryMod.Survivors.Henry.SkillStates;

namespace HenryMod.Survivors.Henry
{
    public static class HenryStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(Missile));
            Modules.Content.AddEntityState(typeof(Dash));
            Modules.Content.AddEntityState(typeof(ChargeNuke));
            Modules.Content.AddEntityState(typeof(ThrowNuke));
            Modules.Content.AddEntityState(typeof(Wait));
        }
    }
}
