﻿using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace DogePlugin
{
    public class ConsoleCommands
    {
        private readonly DogePlugin _pluginInstance;
        public ConsoleCommands(DogePlugin pluginInstance) => this._pluginInstance = pluginInstance;

        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            switch (ev.Name)
            {
                case "help":
                {
                    ev.Allow = false;
                    ev.Color = "green";
                    ev.ReturnMessage = "사용 가능한 명령어: \n" +
                                       ".help, .stats, .discord, .scp";
                    break;
                }
                case "stats":
                {
                    ev.Allow = false;
                    ev.Color = "green";
                    string name = ev.Player.GetDatabasePlayer().Name;
                    int exp = ev.Player.GetDatabasePlayer().Exp;
                    int level = ev.Player.GetDatabasePlayer().Level;
                    int totalKilled = ev.Player.GetDatabasePlayer().TotalKilled;
                    int totalScpKilled = ev.Player.GetDatabasePlayer().TotalScpKilled;
                    int totalEscaped = ev.Player.GetDatabasePlayer().TotalEscaped;
                    int totalDeath = ev.Player.GetDatabasePlayer().TotalDeath;
                    int totalGamesPlayed = ev.Player.GetDatabasePlayer().TotalGamesPlayed;
                    int totalScpPlayed = ev.Player.GetDatabasePlayer().TotalScpGamesPlayed;
                    ev.ReturnMessage =
                        $"{name}님의 현재 통계 \n" +
                        $"레벨: {level} | 경험치: {exp} \n" +
                        $"처치한 적: {totalKilled} | 격리한 SCP: {totalScpKilled} | 탈출한 횟수: {totalEscaped} | 죽은 횟수: {totalDeath} \n" +
                        $"총 플레이한 게임: {totalGamesPlayed} | SCP로 플레이한 게임: {totalScpPlayed} \n" +
                        $"레벨 계산 방법: (현재레벨+10)*2 만큼의 경험치를 얻을 시 레벨업";
                    break;
                }
            }
        }
    }
}