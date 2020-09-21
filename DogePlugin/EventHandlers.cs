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

        internal void OnPlayerJoin(JoinedEventArgs ev)
        {
            if (!Database.Database.LiteDatabase.GetCollection<Player>().Exists(player => player.Id == ev.Player.GetRawUserId()))
            {
                Log.Info(ev.Player.Nickname + " 은(는) 데이터베이스에 등록되어 있지 않습니다!");
                _pluginInstance.DatabasePlayerData.AddPlayer(ev.Player);
            }
            
            var databasePlayer = ev.Player.GetDatabasePlayer();
            if (Database.Database.PlayerData.ContainsKey(ev.Player))
            {
                Database.Database.PlayerData.Add(ev.Player, databasePlayer);
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
                    nickname = NicknameFiltering(ev.Player.Nickname, FilteringWords[i]);
                }

                if (nickname != ev.Player.Nickname) ev.Player.DisplayNickname = nickname;
            }
        }

        internal void OnPlayerLeft(LeftEventArgs ev)
        {
            if (ev.Player.Nickname != "Dedicated Server" && ev.Player != null &&
                Database.Database.PlayerData.ContainsKey(ev.Player))
            {
                ev.Player.GetDatabasePlayer().SetCurrentDayPlayTime();
                Database.Database.LiteDatabase.GetCollection<Player>().Update(Database.Database.PlayerData[ev.Player]);
            }
        }
        
        private string NicknameFiltering(string nickname, string FilteringWord)
        {
            return Regex.Replace(nickname, FilteringWord, "", RegexOptions.IgnoreCase);
        }
        
        private static void AddExp(Exiled.API.Features.Player player, int exp)
        {
            int nowExp = player.GetDatabasePlayer().Exp;
            int nowLevel = player.GetDatabasePlayer().Level;
            if (nowExp + exp >= (nowLevel + 10) * 2)
            {
                player.GetDatabasePlayer().Level++;
                player.GetDatabasePlayer().Exp = 0;
                player.Broadcast(5,$"레벨업!\n당신의 레벨이 <color=green>{player.GetDatabasePlayer().Level}</color>레벨으로 올랐습니다.\n`를 눌러 콘솔창을 연 뒤 <color=green>.tats</color> 명령어로 확인이 가능합니다!");
            }
            else
            {
                player.GetDatabasePlayer().Exp += exp;
            }
            return;
        }
    }
}