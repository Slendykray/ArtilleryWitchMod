using EntityStates.Mage.Weapon;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry.SkillStates
{
    public class ChargeNuke : BaseChargeBombState
    {
        public override void OnEnter()
        {
            chargeEffectPrefab = HenryAssets.chargeNuke;
            minBloomRadius = 0.5f;
            maxBloomRadius = 1f;
            baseDuration = 2f;
            chargeSoundString = "Play_mage_m2_iceSpear_charge";

            minChargeDuration = 1f;  
            base.OnEnter();
        }
        public override BaseThrowBombState GetNextState()
        {
            return new ThrowNuke();
        }


    }
}
