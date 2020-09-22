using Exiled.API.Interfaces;
using System.ComponentModel;

namespace DogePlugin
{
    public class Configs : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("닉네임 필터링 사용 여부")]
        public bool NicknameFilteringEnable { get; private set; } = true;

        [Description("데이터베이스 이름(기본적으로 변경X)")]
        public string DatabaseName { get; private set; } = "DogePlugin";

        [Description("데이터베이스를 저장할 폴더(기본적으로 변경X)")]
        public string DatabaseFolder { get; private set; } = "EXILED";
    }
}