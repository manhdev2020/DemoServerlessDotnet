namespace DemoServerless.Configs
{
    public class AppConfigs
    {
        public string SecretKeyToken { get; set; }
        public string SecretKeyRefreshToken { get; set; }
        public int ExpiresToken { get; set; }
        public int ExpiresRefreshToken { get; set; }
    }
}
