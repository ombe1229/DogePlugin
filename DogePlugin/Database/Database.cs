using Exiled.API.Features;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace DogePlugin
{
    public class Database
    {
        private readonly DogePlugin _pluginInstance;
        public Database(DogePlugin pluginInstance) => this._pluginInstance = pluginInstance;

        public static LiteDatabase LiteDatabase { get; private set; }

        public string DatabaseDirectory =>
            Path.Combine(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    _pluginInstance.Config.DatabaseFolder), _pluginInstance.Config.DatabaseName);

        public string DatabaseFullPath => Path.Combine(DatabaseDirectory, $"{_pluginInstance.Config.DatabaseName}.db");
        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData = new Dictionary<Exiled.API.Features.Player, Player>();

        public void CreateDatabase()
        {
            if (Directory.Exists(DatabaseDirectory)) return;

            try
            {
                Directory.CreateDirectory(DatabaseDirectory);
                Log.Warn("데이터베이스를 찾지 못했습니다. 데이터베이스를 생성합니다.");
            }
            catch (Exception e)
            {
                Log.Error($"데이터베이스를 생성할 수 없습니다.\n{e.ToString()}");
            }
        }

        public void OpenDatabase()
        {
            try
            {
                LiteDatabase = new LiteDatabase(DatabaseFullPath);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Id);
                LiteDatabase.GetCollection<Player>().EnsureIndex(x => x.Name);
                Log.Info("데이터베이스를 불러왔습니다.");
            }
            catch (Exception e)
            {
                Log.Error($"데이터베이스를 불러오지 못했습니다.\n{e.ToString()}");
            }
        }
        
        public void AddPlayer(Exiled.API.Features.Player player)
        {
            try
            {
                if (LiteDatabase.GetCollection<Player>().Exists(x => x.Id == DatabasePlayer.GetRawUserId(player))) return;

                LiteDatabase.GetCollection<Player>().Insert(new Player()
                {
                    Id = DatabasePlayer.GetRawUserId(player),
                    Name = player.Nickname,
                    TotalGamesPlayed = 0,
                    TotalScpGamesPlayed = 0,
                    TotalKilled = 0,
                    TotalScpKilled = 0,
                    TotalDeath = 0,
                    FirstJoin = DateTime.Now,
                    LastSeen = DateTime.Now,
                    PlayTimeRecords = null,
                    Exp = 0,
                    Level = 1,
                    TotalEscaped = 0,
                    TotalWin = 0,
                    DisplayBadge = false
                });
                Log.Info("Trying to add ID: " + player.UserId.Split('@')[0] + " Discriminator: " + player.UserId.Split('@')[1] + " to Database");
            }
            catch (Exception ex)
            {
                Log.Error($"유저를 데이터베이스에 추가할 수 없습니다: {player.Nickname} ({player.UserId.Split('@')[0]})!\n{ex.ToString()}");
            }
        }
    }
}