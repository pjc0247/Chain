using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.Util;
using Amazon.S3;

namespace Chain.AWS.S3
{
    public class S3Credential : IServiceCredential
    {
        public string AccessKey;
        public string Secret;
        public string Region;

        public object CreateClient()
        {
            return new AmazonS3Client(
                AccessKey, Secret,
                RegionEndpoint.GetBySystemName(Region));
        }
    }
}
