using System;
using Exiled.Events.Handlers;
using HarmonyLib;
using RemoteKeycard.EventHandlers;

namespace RemoteKeycard
{
    public sealed class RemoteKeycardPlugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Name => "RemoteKeycard";
        public override string Prefix => "remote_keycard";
        public override string Author => "wexelsdev";
        
        public override Version Version => new(1, 0, 0);
        public override Version RequiredExiledVersion => new(9, 5, 0);
        
        private Harmony? _harmony;
        private PlayerHandlers? _handlers;
        
        public override void OnEnabled()
        {
            _harmony = new($"remotekeycard.wexelsdev.{DateTime.Now.Ticks}");
            _handlers = new();

            _harmony.PatchAll();
            
            Player.UnlockingGenerator += _handlers.OnUnlockingGenerator;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.UnlockingGenerator -= _handlers!.OnUnlockingGenerator;
            
            _harmony!.UnpatchAll();
            
            _handlers = null;
            _harmony = null;
            
            base.OnDisabled();
        }
    }
}