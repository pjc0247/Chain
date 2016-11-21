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
    public class PutACL : ChainTask
    {
        #region IN_KEYS
        public static readonly string IN_BucketName = nameof(BucketName);
        public static readonly string IN_RemoteFilePath = nameof(RemoteFilePath);
        public static readonly string IN_Acl = nameof(Acl);
        #endregion

        private string BucketName;
        private string RemoteFilePath;
        private string Acl;

        private AmazonS3Client Client;

        public PutACL(string bucketName, string remoteFilePath, string acl)
        {
            BucketName = bucketName;
            RemoteFilePath = remoteFilePath;
            Acl = acl;
        }

        public override void OnExecute()
        {
            Client = Context.CredentialProvider.Get<S3Credential, AmazonS3Client>();

            var response = Client.PutACL(new Amazon.S3.Model.PutACLRequest()
            {
                BucketName = BucketName,
                Key = RemoteFilePath,
                CannedACL = S3CannedACL.FindValue(Acl)
            });

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                throw new InvalidOperationException($"S3Response => {response.HttpStatusCode}");
        }
    }
}
