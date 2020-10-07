using System;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using Mirror;
using RemoteAdmin;
using Respawning;
using UnityEngine;

namespace DogePlugin
{
    internal static class Functions
    {
        public static void SendSubtitle(ushort time, string text, Exiled.API.Features.Player target = null)
        {
            if (target != null)
            {
                target.ClearBroadcasts();
                target.Broadcast(time, text);
            }
            else
            {
                Map.ClearBroadcasts();
                Map.Broadcast(time, text);
            }
        }
        
        public static void AddExp(Exiled.API.Features.Player player, int exp)
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
                SendSubtitle(5,$"레벨업!\n당신의 레벨이 <color=green>{player.GetDatabasePlayer().Level}</color>레벨으로 올랐습니다.\n`를 눌러 콘솔창을 연 뒤 <color=green>.stats</color> 명령어로 확인이 가능합니다!", player);
                Log.Info($"{player}이(가) 레벨업했습니다. 현재 레벨: {player.GetDatabasePlayer().Level}");
            }
            else
            {
                player.GetDatabasePlayer().Exp += exp;
            }
            return;
        }

        public static int GetMTFTickets()
        {
            if(CustomLiteNetLib4MirrorTransport.DelayConnections) return -1;
            return RespawnTickets.Singleton.GetAvailableTickets(SpawnableTeamType.NineTailedFox);
        }
        
        public static int GetCHIickets()
        {
            if(CustomLiteNetLib4MirrorTransport.DelayConnections) return -1;
            return RespawnTickets.Singleton.GetAvailableTickets(SpawnableTeamType.ChaosInsurgency);
        }
        
        public static bool IsEnemy(this Exiled.API.Features.Player player, Team target)
        {
            if(player.Role == RoleType.Spectator || player.Role == RoleType.None || player.Team == target)
                return false;

            return target == Team.SCP ||
                   ((player.Team != Team.MTF && player.Team != Team.RSC) || (target != Team.MTF && target != Team.RSC))
                   &&
                   ((player.Team != Team.CDP && player.Team != Team.CHI) || (target != Team.CDP && target != Team.CHI))
                ;
        }
        
        public static T GetRandomOne<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static void ShowHitmarker(this Exiled.API.Features.Player player)
        {
            player.ReferenceHub.GetComponent<Scp173PlayerScript>().CallTargetHitMarker(player.Connection);
        }

        public static void SendTextHint(this Exiled.API.Features.Player player, string text, ushort time)
        {
            player.ReferenceHub.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter("") }, new HintEffect[] { HintEffectPresets.TrailingPulseAlpha(0.5f,1f,0.5f,2f,0f,2)}, time));
        }
    }
}