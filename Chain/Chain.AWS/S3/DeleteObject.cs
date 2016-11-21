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
    public class DeleteObject : ChainTask
    {
        private string BucketName;
        private string RemoteFilePath;

        private AmazonS3Client Client;

        public DeleteObject(string bucketName, string remoteFilePath)
        {
            BucketName = bucketName;
            RemoteFilePath = remoteFilePath;
        }

        public override void OnExecute()
        {
            Client = Context.CredentialProvider.Get<S3Credential, AmazonS3Client>();

            var response = Client.DeleteObject(new Amazon.S3.Model.DeleteObjectRequest()
            {
                BucketName = BucketName,
                Key = RemoteFilePath
            });
            
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                throw new InvalidOperationException($"S3Response => {response.HttpStatusCode}");
        }
    }
}
