using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using RemoteKeycard.API;

namespace RemoteKeycard.EventHandlers
{
    internal sealed class PlayerHandlers
    {
        internal void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.IsAllowed || ev.Generator.IsUnlocked)
                return;

            ev.IsAllowed = ev.Player.IsPermitted(ev.Generator.KeycardPermissions);
        }

        internal void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player.IsBypassModeEnabled || ev.IsAllowed || ev.Door.IsLocked || ev.Door.KeycardPermissions == KeycardPermissions.None)
                return;
        
            if (ev.Player.IsPermitted(ev.Door.KeycardPermissions))
            {
                ev.IsAllowed = true;
                ev.CanInteract = true;
            
                return;
            }

            ev.IsAllowed = false;
            ev.CanInteract = true;
        }

        internal void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player.IsBypassModeEnabled || ev.IsAllowed || !ev.InteractingChamber.CanInteract)
                return;

            ev.IsAllowed = ev.Player.IsPermitted(ev.InteractingChamber.RequiredPermissions);
        }
    }
}