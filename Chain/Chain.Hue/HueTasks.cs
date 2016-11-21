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

        protected LightCommand Command;

        public HueLightTaskBase(string lightId)
        {
            LightId = lightId;
        }

        public override void OnExecute()
        {
            Hue = Context.CredentialProvider.Get<HueCredential, LocalHueClient>();

            var cmd = new LightCommand();
            cmd.TurnOn();

            var result = Hue.SendCommandAsync(
                cmd,
                new string[] { LightId }).Result;

            if (result.HasErrors())
                throw new InvalidOperationException("");
        }
    }

    public class GetAppKey : ChainTask
    {
        private LocalHueClient Hue;

        public GetAppKey()
        {
        }

        public override void OnExecute()
        {
            var locator = new HttpBridgeLocator();
            var bridgeIPs =
                locator.LocateBridgesAsync(TimeSpan.FromSeconds(5)).Result;

            Hue = new LocalHueClient(bridgeIPs.First());

            var appKey = Hue.RegisterAsync("Chain.Hue", "chain").Result;

            Console.WriteLine("Hue.GetAppKey : " + appKey);
        }
    }

    public class EnumLights : ChainTask
    {
        private HueClient Hue;

        public EnumLights()
        {
        }

        public override void OnExecute()
        {
            Hue = Context.CredentialProvider.Get<HueCredential, LocalHueClient>();

            var lights = Hue.GetLightsAsync().Result;

            foreach (var light in lights)
            {
                Console.WriteLine(light.Id);
            }
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
        public SetBrightness(string lightId, int value)
            : base(lightId)
        {
            Command = new LightCommand();
            Command.Brightness = (byte)value;
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
