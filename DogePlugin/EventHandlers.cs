using System;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using static DogePlugin.Functions;

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

                if (nickname != ev.Player.Nickname)
                {
                    ev.Player.DisplayNickname = nickname;
                    SendSubtitle(10,"<color=red>닉네임에 필터링 단어가 포함되어 삭제했습니다.</color>", ev.Player);
                    Log.Info($"{ev.Player.Nickname} 에게 필터링 단어가 포함되어 삭제했습니다.\n현재 닉네임:{nickname}");
                }
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
    }
}