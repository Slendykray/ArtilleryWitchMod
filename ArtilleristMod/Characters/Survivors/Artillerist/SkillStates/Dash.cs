using EntityStates;
using EntityStates.Merc;
using ArtilleristMod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static RoR2.SolusWing.SolusWingPodAI.Simulation.SimulationState;

namespace ArtilleristMod.Survivors.Artillerist.SkillStates
{

    public class Dash : BaseDash
    {
        public override void OnEnter()
        {

            damageCoefficient = ArtilleristStaticValues.dashDamageCoefficient;
            base.OnEnter();

           

        }

    }


}
