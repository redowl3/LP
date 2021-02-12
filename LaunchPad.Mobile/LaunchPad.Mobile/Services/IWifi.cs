using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPad.Mobile.Services
{
    public interface IWifi
    {
        Task<IEnumerable<string>> GetAvailableNetworksAsync();
    }
}
