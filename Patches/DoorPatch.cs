using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using PlayerRoles;

namespace RemoteKeycard.Patches
{
    [HarmonyPatch(typeof(DoorPermissions), nameof(DoorPermissions.CheckPermissions))]
    internal static class DoorPatch
    {
        internal static bool Prefix(DoorPermissions __instance, ref bool __result, ItemBase item, ReferenceHub? ply)
        {
            if (__instance.RequiredPermissions == KeycardPermissions.None)
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

                if (item == null)
                {
                    if (ply.IsSCP() && __instance.RequiredPermissions.HasFlagFast(KeycardPermissions.ScpOverride))
                    {
                        __result = true;
                        return false;
                    }
                }
            }
        
            if (item != null && !(item is KeycardItem))
            {
                __result = false;
                return false;
            }

            Player player = Player.Get(ply);
        
            foreach (Item? keycardItem in player.Items.Where(x => x.IsKeycard))
            {
                if (keycardItem is Keycard keycard)
                {
                    if (!__instance.RequireAll ? (keycard.Permissions & (Exiled.API.Enums.KeycardPermissions)__instance.RequiredPermissions) != 0 : (keycard.Permissions & (Exiled.API.Enums.KeycardPermissions)__instance.RequiredPermissions) == (Exiled.API.Enums.KeycardPermissions)__instance.RequiredPermissions)
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