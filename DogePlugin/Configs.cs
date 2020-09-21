using Exiled.API.Interfaces;
using System.ComponentModel;

namespace DogePlugin
{
    public class Configs : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Use Nickname Filtering?")]
        public bool NicknameFilteringEnable { get; private set; } = true;

        [Description("Database name")]
        public string DatabaseName { get; private set; } = "DogePlugin";

        [Description("In which folder database should be stored?")]
        public string DatabaseFolder { get; private set; } = "EXILED";
    }
}