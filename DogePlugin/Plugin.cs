﻿using System;
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
        public new Commands Commands { get; private set; }
        public ConsoleCommands PlayerConsoleCommands { get; private set; }
        public Database DatabasePlayerData { get; private set; }
        public Player Player { get; private set; }

        public void LoadEvents()
        {
            ServerEvents.RoundStarted += EventHandlers.OnRoundStarting;
            ServerEvents.RoundEnded += EventHandlers.OnRoundEnding;
            ServerEvents.RestartingRound += EventHandlers.OnRoundRestarting;
            
            PlayerEvents.Joined += EventHandlers.OnPlayerJoin;
            PlayerEvents.Left += EventHandlers.OnPlayerLeft;
        }

        public void LoadCommands()
        {
            ServerEvents.SendingConsoleCommand += PlayerConsoleCommands.OnConsoleCommand;
        }

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            EventHandlers = new EventHandlers(this);
            Commands = new Commands();
            PlayerConsoleCommands = new ConsoleCommands(this);
            DatabasePlayerData = new Database(this);
            LoadEvents();
            LoadCommands();
            DatabasePlayerData.CreateDatabase();
            DatabasePlayerData.OpenDatabase();
            Log.Info("DogePlugin 활성화");
        }

        public override void OnDisabled()
        {
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStarting;
            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnding;
            ServerEvents.RestartingRound -= EventHandlers.OnRoundRestarting;
            
            PlayerEvents.Joined -= EventHandlers.OnPlayerJoin;
            PlayerEvents.Left -= EventHandlers.OnPlayerLeft;
            
            EventHandlers = null;
            Commands = null;
            PlayerConsoleCommands = null;
            Database.LiteDatabase.Dispose();
        }

        public override void OnReloaded()
        {
            
        }
    }
}