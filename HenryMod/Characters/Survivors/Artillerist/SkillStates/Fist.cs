using EntityStates.Loader;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{

    public class Fist : GroundSlam
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
        }

        public override void OnExit()
        {
      

            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            base.OnExit();
        }
    }


}
