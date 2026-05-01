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
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ArtilleristUnlockAchievement : BaseAchievement
    {
        public const string identifier = ArtilleristSurvivor.HENRY_PREFIX + "ArtilleristUnlock";
        public const string unlockableIdentifier = ArtilleristSurvivor.HENRY_PREFIX + "Characters.Artillerist";

        public override void OnInstall()
        {
            base.OnInstall();
            base.localUser.onMasterChanged += this.OnMasterChanged;
            this.SetMasterController(base.localUser.cachedMasterController);
        }

        public override void OnUninstall()
        {
            this.SetMasterController(null);
            base.localUser.onMasterChanged -= this.OnMasterChanged;
            base.OnUninstall();
        }

        private void SetMasterController(PlayerCharacterMasterController newMasterController)
        {
            if (this.currentMasterController == newMasterController)
            {
                return;
            }
            if (this.currentInventory != null)
            {
                this.currentInventory.onInventoryChanged -= this.OnInventoryChanged;
            }
            this.currentMasterController = newMasterController;
            PlayerCharacterMasterController playerCharacterMasterController = this.currentMasterController;
            Inventory inventory;
            if (playerCharacterMasterController == null)
            {
                inventory = null;
            }
            else
            {
                CharacterMaster master = playerCharacterMasterController.master;
                inventory = ((master != null) ? master.inventory : null);
            }
            this.currentInventory = inventory;
            if (this.currentInventory != null)
            {
                this.currentInventory.onInventoryChanged += this.OnInventoryChanged;
            }
        }

        private void OnInventoryChanged()
        {
            if (this.currentInventory)
            {
                //Log.Message("CHANGED!!!");

                string[] items = { "StickyBomb", "StunChanceOnHit", "Firework", "Missile" };
                
                for (int i = 0; i < items.Length; i++)
                {
                    if (!CheckItem(items[i]))
                    {
                        //Log.Message("FUCK YOU");
                        return;
                    }
                }

                string[] itemsOption = { "Behemoth", "MoreMissile" };

                bool flag = false;

                for (int i = 0; i < itemsOption.Length; i++)
                {
                    if (CheckItem(itemsOption[i]))
                    {
                        flag = true;
                    }
                }
       
                if (flag)
                {
                    base.Grant();
                }
                //else
                //{
                //    Log.Message("BROKEE");
                //}
            }
        }

        bool CheckItem(string itemName)
        {
            return currentInventory.GetItemCountEffective(ItemCatalog.FindItemIndex(itemName)) > 0;
        }

        private void OnMasterChanged()
        {
            this.SetMasterController(base.localUser.cachedMasterController);
        }

        private PlayerCharacterMasterController currentMasterController;

        private Inventory currentInventory;
    }
    
}