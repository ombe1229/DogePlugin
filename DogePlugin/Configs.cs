using Exiled.API.Interfaces;
using System.ComponentModel;

namespace DogePlugin
{
    public class Configs : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}