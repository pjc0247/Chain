using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Amazon;
using Amazon.Util;
using Amazon.S3;

namespace Chain.AWS.S3
{
    public class GetPresignedUrl : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_BucketName = nameof(BucketName);
        public static readonly string IN_RemoteFilePath = nameof(RemoteFilePath);
        public static readonly string IN_Expires = nameof(Expires);
        #endregion
        #region OUT_KEYS
        public static readonly string OUT_Url = $"S3.{nameof(GetPresignedUrl)}.Url";
        #endregion

        private string BucketName;
        private string RemoteFilePath;
        private DateTime Expires;

        private AmazonS3Client Client;

        public GetPresignedUrl(string bucketName, string remoteFilePath, DateTime expires)
        {
            BucketName = bucketName;
            RemoteFilePath = remoteFilePath;
            Expires = expires;
        }

        public override void OnExecute()
        {
            Client = Context.CredentialProvider.Get<S3Credential, AmazonS3Client>();

            var response = Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest()
            {
                 BucketName = BucketName,
                 Key = RemoteFilePath,
                 Expires = Expires
            });

            if (string.IsNullOrEmpty(response))
                throw new InvalidOperationException($"S3GetPresignedUrl : invalid response");

            Context.Out(OUT_Url, response);
        }
    }
}
