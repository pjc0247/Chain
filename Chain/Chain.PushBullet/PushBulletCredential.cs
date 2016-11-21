using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PushbulletSharp;

namespace Chain.PushBullet
{
    public class PushBulletCredential : IServiceCredentials
    {
        public string AccessKey;
        public string EncryptionPassword;

        public object CreateClient()
        {
            if (string.IsNullOrEmpty(EncryptionPassword))
                return new PushbulletClient(AccessKey);
            else
                return new PushbulletClient(AccessKey, EncryptionPassword);
        }
    }
}
