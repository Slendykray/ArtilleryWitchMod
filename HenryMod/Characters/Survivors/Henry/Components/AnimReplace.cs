using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry.Components
{
    internal class AnimReplace : MonoBehaviour
    {
        private void Awake()
        {
            GetAnimator().runtimeAnimatorController = Addressables.LoadAssetAsync<RuntimeAnimatorController>("RoR2/Base/Mage/animMage.controller").WaitForCompletion();
        }  

        Animator GetAnimator()
        {
            var body = GetComponent<CharacterBody>();

            var modelTransform = body.modelLocator.modelTransform;

           return modelTransform.GetComponent<Animator>();
        }
    }
}