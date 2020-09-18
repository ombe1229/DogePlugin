using System;
using System.Security.Permissions;
using Exiled.Events.Extensions;
using UnityEngine;
using Log = Exiled.API.Features.Log;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MapEvents = Exiled.Events.Handlers.Map;
using Features = Exiled.API.Features;

namespace DogePlugin
{
    public class DogePlugin : Features.Plugin<Configs>
    {
        public static bool IsStarted { get; set; }
        public EventHandlers EventHandlers { get; private set; }
        public ConsoleCommands PlayerConsoleCommands { get; private set; }

        public void LoadEvents()
        {
            
        }

        public void LoadCommands()
        {
            ServerEvents.SendingConsoleCommand += PlayerConsoleCommands.OnConsoleCommand;
        }

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            EventHandlers = new EventHandlers(this);
            PlayerConsoleCommands = new ConsoleCommands(this);
            LoadEvents();
            LoadCommands();
            Log.Info("DogePlugin Enabled.");
        }

        public override void OnDisabled()
        {
            EventHandlers = null;
            PlayerConsoleCommands = null;
        }

        public override void OnReloaded()
        {
            
        }
    }
}