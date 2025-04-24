using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using MapGeneration.Distributors;

namespace RemoteKeycard.Patches
{
    [HarmonyPatch(typeof(Locker), nameof(Locker.CheckTogglePerms))]
    internal static class LockerPatch
    {
        internal static bool Prefix(Locker __instance, ref bool __result, int chamberId, ReferenceHub? ply)
        {
            if (__instance.Chambers[chamberId].RequiredPermissions == KeycardPermissions.None)
            {
                __result = true;
                return false;
            }
        
            if (ply != null)
            {
                if (ply.serverRoles.BypassMode)
                {
                    __result = true;
                    return false;
                }
            }
        
            Player player = Player.Get(ply);
        
            foreach (Item? keycardItem in player.Items.Where(x => x.IsKeycard))
            {
                if (keycardItem is Keycard keycard)
                {
                    if (keycard.Base.Permissions.HasFlagFast(__instance.Chambers[chamberId].RequiredPermissions))
                    {
                        __result = true;
                        return false;
                    }
                }
            }
        
            __result = false;
            return false;
        }
    }
}