using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;

namespace Chain.Hue
{
    public class HueLightTaskBase : ChainTask
    {
        private string LightId;

        private HueClient Hue;
        private Light Light;

        protected LightCommand Command;

        public HueLightTaskBase(string lightId)
        {
            LightId = lightId;

            var appKey = Config.Get("hue.app_key", "");
            var locator = new HttpBridgeLocator();
            var bridgeIPs =
                locator.LocateBridgesAsync(TimeSpan.FromSeconds(5)).Result;

            Hue = new LocalHueClient(bridgeIPs.First(), appKey);
            Light = Hue.GetLightAsync(LightId).Result;
        }

        public override void OnExecute()
        {
            var cmd = new LightCommand();
            cmd.TurnOn();

            var result = Hue.SendCommandAsync(cmd).Result;
        }
    }

    public class TurnOnLight : HueLightTaskBase
    {
        public TurnOnLight(string lightId)
            : base (lightId)
        {
            Command = new LightCommand();
            Command.TurnOn();
        }
    }
    public class TurnOffLight : HueLightTaskBase
    {
        public TurnOffLight(string lightId)
            : base(lightId)
        {
            Command = new LightCommand();
            Command.TurnOff();
        }
    }
    public class SetBrightness : HueLightTaskBase
    {
        public SetBrightness(string lightId, byte value)
            : base(lightId)
        {
            Command = new LightCommand();
            Command.Brightness = value;
        }
    }
    public class SetColor : HueLightTaskBase
    {
        public SetColor(string lightId, int r, int g ,int b)
            : base(lightId)
        {
            Command = new LightCommand();
            Command.SetColor(new RGBColor(r, g, b));
        }
    }
}
