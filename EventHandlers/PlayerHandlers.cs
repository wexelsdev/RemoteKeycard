using System;
using System.Linq;
using System.Reflection;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using MapGeneration.Distributors;

namespace RemoteKeycard.EventHandlers
{
    internal sealed class PlayerHandlers
    {
        internal void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            foreach (Item? keycardItem in ev.Player.Items.Where(x => x.IsKeycard))
            {
                if (keycardItem is Keycard keycard && keycard.Permissions >= ev.Generator.KeycardPermissions)
                {
                    Type type = ev.Generator.Base.GetType();
                    MethodInfo? method = type.GetMethod("ServerSetFlag", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (method == null) return;
                    
                    ev.IsAllowed = false;
            
                    method.Invoke(ev.Generator.Base, new object[] { Scp079Generator.GeneratorFlags.Unlocked, true });

                    return;
                }
            }
        }
    }
}