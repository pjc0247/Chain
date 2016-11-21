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
    public class PutObject : ChainTask
    {
        private string LocalFilePath;
        private string BucketName;
        private string RemoteFilePath;

        private AmazonS3Client Client;

        public PutObject(string localFilePath, string bucketName, string remoteFilePath)
        {
            LocalFilePath = localFilePath;
            BucketName = bucketName;
            RemoteFilePath = remoteFilePath;
        }

        public override void OnExecute()
        {
            Client = Context.CredentialProvider.Get<S3Credential, AmazonS3Client>();

            if (File.Exists(LocalFilePath) == false)
                throw new InvalidOperationException($"File not exists. {LocalFilePath}");

            var response = Client.PutObject(new Amazon.S3.Model.PutObjectRequest()
            {
                BucketName = BucketName,
                FilePath = LocalFilePath,
                Key = RemoteFilePath
            });
            
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new InvalidOperationException($"S3Response => {response.HttpStatusCode}");
        }
    }
}
