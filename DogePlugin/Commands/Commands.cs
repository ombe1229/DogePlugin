using Exiled.Events.EventArgs;
using static DogePlugin.Functions;

namespace DogePlugin
{
    public class Commands
    {
        public void OnRaCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            switch (ev.Name)
            {
                case "d_help":
                {
                    ev.ReplyMessage = "명령어 목록 : chit, mtft";
                    break;
                }
                case "chit":
                {
                    int Ticket = GetCHIickets();
                    ev.ReplyMessage = $"혼돈의 반란 티켓 수 : {Ticket}";
                    break;
                }
                case "mtft":
                {
                    int Ticket = GetMTFTickets();
                    ev.ReplyMessage = $"MTF 티켓 수 : {Ticket}";
                    break;
                }
            }
        }
    }
}