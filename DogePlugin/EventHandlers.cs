using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;

namespace DogePlugin
{
    public class EventHandlers
    {
        private readonly DogePlugin _pluginInstance;
        public EventHandlers(DogePlugin pluginInstance) => this._pluginInstance = pluginInstance;
    }
}