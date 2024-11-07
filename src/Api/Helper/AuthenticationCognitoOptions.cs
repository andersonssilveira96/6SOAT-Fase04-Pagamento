namespace Api.Helper
{
    public class AuthenticationCognitoOptions
    {
        public string Region { get; set; }
        public string CognitoPoolId { get; set; }
        public string CognitoClientId { get; set; }
        public string CognitoClientSecret { get; set; }
        public string CognitoUri { get => $"https://cognito-idp.{Region}.amazonaws.com/{CognitoPoolId}"; }
    }
}
