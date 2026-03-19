using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry.Components
{
    internal class AnimReplaceDisplay : MonoBehaviour
    {      
        private void Awake()
        {
            Log.Message(transform.name);
            GetComponent<Animator>().runtimeAnimatorController = Addressables.LoadAssetAsync<RuntimeAnimatorController>("RoR2/Base/Mage/animMageDisplay.controller").WaitForCompletion();
        }




    }
}