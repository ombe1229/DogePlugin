using System.Collections.Generic;
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
                case "doge":
                {
                    break;
                }
            }
        }
    }
}