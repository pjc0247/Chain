using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Q42.HueApi;

namespace Chain.Hue
{
    public class HueCredential : IServiceCredentials
    {
        public string AppKey;

        public object CreateClient()
        {
            var locator = new HttpBridgeLocator();
            var bridgeIPs =
                locator.LocateBridgesAsync(TimeSpan.FromSeconds(5)).Result;

            return new LocalHueClient(bridgeIPs.First(), AppKey);
        }
    }
}
