using System;
using Exiled.Events.Handlers;
using RemoteKeycard.EventHandlers;

namespace RemoteKeycard
{
    public sealed class RemoteKeycardPlugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Name => "RemoteKeycard";
        public override string Prefix => "remote_keycard";
        public override string Author => "wexelsdev";
        
        public override Version Version => new(1, 1, 0);
        public override Version RequiredExiledVersion => new(9, 6, 0);
        
        private PlayerHandlers? _handlers;
        
        public override void OnEnabled()
        {
            _handlers = new();
            
            Player.UnlockingGenerator += _handlers.OnUnlockingGenerator;
            Player.InteractingLocker += _handlers.OnInteractingLocker;
            Player.InteractingDoor += _handlers.OnInteractingDoor;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.InteractingDoor -= _handlers.OnInteractingDoor;
            Player.InteractingLocker -= _handlers.OnInteractingLocker;
            Player.UnlockingGenerator -= _handlers!.OnUnlockingGenerator;
            
            _handlers = null;
            
            base.OnDisabled();
        }
    }
}