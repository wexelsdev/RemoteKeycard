using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace RemoteKeycard.API
{
    public static class Extensions
    {
        public static bool IsPermitted(this Player player, KeycardPermissions neededPermissions) => player.GetKeycards()
            .Any(x => (x.Permissions & neededPermissions) == neededPermissions);
    
        public static List<Keycard> GetKeycards(this Player player) =>
            player.Items.OfType<Keycard>().ToList();
    }
}