namespace AwesomePizzaBLL.Structure
{
    public interface ITokenCryptoSettings
    {
        string FormAuthToken { get; set; }
        string ApiSeed { get; set; }
        string PwdAccountSeedKey { get; set; }
        string XTokenSecretKey { get; set; }
    }
    public class TokenCryptoSettings : ITokenCryptoSettings
    {
        public string FormAuthToken { get; set; } = "spec77e409adsdvccdsdfio12$)(";
        public string ApiSeed { get; set; } = "bdjio%rtyhrgrgb%";
        public string PwdAccountSeedKey { get; set; } = "12ka66teaeerfdcd3mfri3";
        public string XTokenSecretKey { get; set; } = "sdsiRFpopakOOSffdsf£$&)";
    }
}
