using BepInEx.Configuration;
using HenryMod.Modules;
using HenryMod.Modules.Characters;
using HenryMod.Survivors.Henry.Components;
using HenryMod.Survivors.Henry.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry
{
    public class HenrySurvivor : SurvivorBase<HenrySurvivor> 
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "artilleristbundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "ArtilleristBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "ArtilleristMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlArtillerist";
        public override string displayPrefabName => "ArtilleristDisplay";

        public const string HENRY_PREFIX = HenryPlugin.DEVELOPER_PREFIX + "_ARTILLERIST_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => HENRY_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = HENRY_PREFIX + "NAME",
            subtitleNameToken = HENRY_PREFIX + "SUBTITLE",

            //characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            characterPortrait = null,
            bodyColor = new Color32(255, 196, 0, 255),
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            //crosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageCrosshair.prefab").WaitForCompletion(),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 125f, 
            healthRegen = 1.5f,
            armor = 10f,
            moveSpeed = 8f,
            damage = 12f,
            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
               new CustomRendererInfo
                {
                    childName = "Model",
                },
        };

        public override UnlockableDef characterUnlockableDef => HenryUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => null;

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Henry");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            HenryUnlockables.Init();

            base.InitializeCharacter();

            HenryConfig.Init();
            HenryStates.Init();
            HenryTokens.Init();

            HenryAssets.Init(assetBundle);
            HenryBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            //bodyPrefab.AddComponent<HenryWeaponComponent>();
            //bodyPrefab.AddComponent<AnimReplace>();
            //displayPrefab.AddComponent<AnimReplaceDisplay>(); 

            //bodyPrefab.GetComponent<CharacterBody>().modelLocator.modelTransform.GetComponent<Animator>().runtimeAnimatorController = Addressables.LoadAssetAsync<RuntimeAnimatorController>("RoR2/Base/Mage/animMage.controller").WaitForCompletion();
            displayPrefab.GetComponent<Animator>().runtimeAnimatorController = Addressables.LoadAssetAsync<RuntimeAnimatorController>("RoR2/Base/Mage/animMageDisplay.controller").WaitForCompletion();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            //Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.Mage.MageCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Backpack");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Jet");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = HENRY_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = HENRY_PREFIX + "PASSIVE_DESCRIPTION",
                //keywordToken = "KEYWORD_STUNNING",
                icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryPassive",
            //    skillNameToken = HENRY_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = HENRY_PREFIX + "PASSIVE_DESCRIPTION",
            //    keywordTokens = new string[] { "KEYWORD_AGILE" },
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            //    //unless you're somehow activating your passive like a skill, none of the following is needed.
            //    //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //    //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    //activationStateMachineName = "Weapon1",
            //    //interruptPriority = EntityStates.InterruptPriority.Skill,

            //    //baseRechargeInterval = 1f,
            //    //baseMaxStock = 1,

            //    //rechargeStock = 1,
            //    //requiredStock = 1,
            //    //stockToConsume = 1,

            //    //resetCooldownTimerOnUse = false,
            //    //fullRestockOnAssign = true,
            //    //dontAllowPastMaxStocks = false,
            //    //mustKeyPress = false,
            //    //beginSkillCooldownOnSkillEnd = false,

            //    //isCombatSkill = true,
            //    //canceledFromSprinting = false,
            //    //cancelSprintingOnActivation = false,
            //    //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            //SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            //    (
            //        "HenrySlash",
            //        HENRY_PREFIX + "PRIMARY_SLASH_NAME",
            //        HENRY_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
            //        assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //        new EntityStates.SerializableEntityStateType(typeof(SkillStates.Missile)),
            //        "Weapon",
            //        true
            //    ));
            //custom Skilldefs can have additional fields that you can set manually
            //primarySkillDef1.stepCount = 2;
            //primarySkillDef1.stepGraceDuration = 0.5f;

            SkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            {
                skillName = "ArtilleristMissile",
                skillNameToken = HENRY_PREFIX + "PRIMARY_MISSILE_NAME",
                skillDescriptionToken = HENRY_PREFIX + "PRIMARY_MISSILE_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Missile)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 2f,
                baseMaxStock = 3,

                rechargeStock = 3,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristGas",
                skillNameToken = HENRY_PREFIX + "SECONDARY_GAS_NAME",
                skillDescriptionToken = HENRY_PREFIX + "SECONDARY_GAS_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                keywordTokens = new string[] { "KEYWORD_POISON" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristCluster",
                skillNameToken = HENRY_PREFIX + "SECONDARY_CLUSTER_NAME",
                skillDescriptionToken = HENRY_PREFIX + "SECONDARY_CLUSTER_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                keywordTokens = new string[] { "KEYWORD_STUNNING" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowCluster)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,
                
                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
                
            });


            SkillFamily secondarySkillFamily = bodyPrefab.GetComponent<SkillLocator>().secondary.skillFamily;
            Skills.AddSkillToFamily(secondarySkillFamily, secondarySkillDef2, HenryUnlockables.clusterUnlockableDef);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristDash",
                skillNameToken = HENRY_PREFIX + "UTILITY_DASH_NAME",
                skillDescriptionToken = HENRY_PREFIX + "UTILITY_DASH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                keywordTokens = new string[] { "KEYWORD_STUNNING" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Dash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);

            SkillDef utilitySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristFireDash",
                skillNameToken = HENRY_PREFIX + "UTILITY_FIREDASH_NAME",
                skillDescriptionToken = HENRY_PREFIX + "UTILITY_FIREDASH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.NapalmDash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            SkillFamily utilitySkillFamily = bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily;
            Skills.AddSkillToFamily(utilitySkillFamily, utilitySkillDef2, HenryUnlockables.fistUnlockableDef);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristNuke",
                skillNameToken = HENRY_PREFIX + "SPECIAL_NUKE_NAME",
                skillDescriptionToken = HENRY_PREFIX + "SPECIAL_NUKE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_STUNNING" },

                //activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Mage.Weapon.ChargeIcebomb)),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ChargeNuke)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon", 
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 15f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);


            SkillDef specialSkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ArtilleristFist",
                skillNameToken = HENRY_PREFIX + "SPECIAL_FIST_NAME",
                skillDescriptionToken = HENRY_PREFIX + "SPECIAL_FIST_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_STUNNING" },

                //activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Mage.Weapon.ChargeIcebomb)),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fist)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef2);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
                //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
                //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            
            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            HenryAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            //On.RoR2.GlobalEventManager.OnHitAllProcess += GlobalEventManager_OnHitAllProcess;
        }
         
        //private void GlobalEventManager_OnHitAllProcess(On.RoR2.GlobalEventManager.orig_OnHitAllProcess orig, GlobalEventManager self, DamageInfo damageInfo, GameObject hitObject)
        //{
        //    Transform rootObject = hitObject.transform.root;
        //    bool flag = rootObject.name.Contains("Gas");

        //    if (flag)
        //    { 
        //        Log.Message(rootObject.name);
        //    }
        //}

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(HenryBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }
        }
    }
}