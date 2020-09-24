using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;

namespace DogePlugin
{
    public class EventHandlers
    {
        private readonly DogePlugin _pluginInstance;
        public EventHandlers(DogePlugin pluginInstance) => this._pluginInstance = pluginInstance;

        
        internal void OnRoundStarting()
        {
            DogePlugin.IsStarted = true;

            foreach (var player in Exiled.API.Features.Player.List)
            {
                AddExp(player, 50);
            }
            Log.Info("라운드 시작 경험치 전체에게 50 지급.");

            int CdpCount = 0;
            int RscCount = 0;
            int ChiCount = 0;
            int MtfCount = 0;
            int ScpCount = 0;
            foreach (var player in Exiled.API.Features.Player.List)
            {
                switch (player.Team)
                {
                    case Team.CDP:
                        CdpCount++;
                        break;
                    case Team.RSC:
                        RscCount++;
                        break;
                    case Team.CHI:
                        ChiCount++;
                        break;
                    case Team.MTF:
                        MtfCount++;
                        break;
                    case Team.SCP:
                        ScpCount++;
                        break;
                }
            }
            Log.Info($"라운드 시작.\nD계급:{CdpCount} | 과학자:{RscCount} | 혼돈의 반란:{ChiCount} | MTF:{MtfCount} | SCP:{ScpCount}");
        }

        internal void OnRoundEnding(RoundEndedEventArgs ev)
        {
            DogePlugin.IsStarted = false;
            
            foreach (var player in Exiled.API.Features.Player.List)
            {
                AddExp(player, 50);
            }
            Log.Info("라운드 종료 경험치 전체에게 50 지급.");
            
            Log.Info($"라운드 종료.\n승리한 팀:{ev.LeadingTeam}");
        }

        internal void OnRoundRestarting()
        {
            DogePlugin.IsStarted = false;
        }
        
        internal void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == ev.Player.GetRawUserId()))
            {
                Log.Info(ev.Player.Nickname + " 은(는) 데이터베이스에 등록되어 있지 않습니다!");
                _pluginInstance.DatabasePlayerData.AddPlayer(ev.Player);
            }
            
            var databasePlayer = ev.Player.GetDatabasePlayer();
            if (Database.PlayerData.ContainsKey(ev.Player))
            {
                Database.PlayerData.Add(ev.Player, databasePlayer);
                databasePlayer.LastSeen = DateTime.Now;
                databasePlayer.Name = ev.Player.Nickname;
                if (databasePlayer.FirstJoin == DateTime.MinValue) databasePlayer.FirstJoin = DateTime.Now;
            }
            
            if (_pluginInstance.Config.NicknameFilteringEnable)
            {
                string[] FilteringWords = new string[] {"유튜브_","유튜브","트위치_","트위치","Youtube_","Youtube","Twitch_","Twitch"};
                string nickname = ev.Player.Nickname;
                for (int i = 0; i < FilteringWords.Length; i++)
                {
                    nickname = Regex.Replace(nickname, FilteringWords[i], "", RegexOptions.IgnoreCase);
                }

                if (nickname != ev.Player.Nickname) ev.Player.DisplayNickname = nickname;
                Log.Info($"{ev.Player.Nickname} 에게 필터링 단어가 포함되어 삭제했습니다.\n현재 닉네임:{nickname}");
            }
        }

        internal void OnPlayerLeft(LeftEventArgs ev)
        {
            if (ev.Player.Nickname != "Dedicated Server" && ev.Player != null &&
                Database.PlayerData.ContainsKey(ev.Player))
            {
                ev.Player.GetDatabasePlayer().SetCurrentDayPlayTime();
                Database.LiteDatabase.GetCollection<Player>().Update(Database.PlayerData[ev.Player]);
            }
        }
        




      
        private static void AddExp(Exiled.API.Features.Player player, int exp)
        {
            int nowExp = player.GetDatabasePlayer().Exp;
            int nowLevel = player.GetDatabasePlayer().Level;
            if (nowExp + exp >= (nowLevel*nowLevel+10)*10)
            {
                 /*
                 * 1lv -> 2lv : 110
                 * 2lv -> 3lv : 140 (+30)
                 * 3lv -> 4lv : 190 (+50)
                 * 4lv -> 5lv : 260 (+70)
                 * 5lv -> 6lv : 350 (+90)
                 */
                player.GetDatabasePlayer().Level++;
                player.GetDatabasePlayer().Exp = 0;
                player.Broadcast(5,$"레벨업!\n당신의 레벨이 <color=green>{player.GetDatabasePlayer().Level}</color>레벨으로 올랐습니다.\n`를 눌러 콘솔창을 연 뒤 <color=green>.tats</color> 명령어로 확인이 가능합니다!");
                Log.Info($"{player}이(가) 레벨업했습니다. 현재 레벨: {player.GetDatabasePlayer().Level}");
            }
            else
            {
                player.GetDatabasePlayer().Exp += exp;
            }
            return;
        }
    }
}