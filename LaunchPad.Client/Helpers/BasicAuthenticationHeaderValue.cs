using System.Net.Http.Headers;

namespace LaunchPad.Client.Helpers
{
    internal class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string scheme, string parameter) : base(scheme, parameter)
        {
        }
    }
}