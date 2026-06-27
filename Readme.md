# Thunderkit fix when cloning repo (for me)

- go to package manager delete fucking r2 editor kit
- reimport ror2 in thunderkit settings
- fuck you
- prevent unity crash: delete burst.dll ror2 and install from package manager

# Fix projectiles multiplayer

- when testing coop "connect localhost:7777" press enter not submit
- register proj prefab like 
   PrefabAPI.RegisterNetworkPrefab(edgeProjectilePrefab);
   Content.AddProjectilePrefab(edgeProjectilePrefab);

- check if (base.isAuthority) before ProjectileManager.instance.FireProjectile
- play anim and sound only in OnExit etc
- check NetworkServer.active in custom MonoBehaviours to call on server only/ spawn NetworkIdentity on server only