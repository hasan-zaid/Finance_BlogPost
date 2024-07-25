using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Finance_BlogPost.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        public readonly string BucketName;



        public MediaRepository()
        {
            this.BucketName = "fininsight-media-bucket";

        }

        public async Task<string> UploadAsync(IFormFile file)
        {

            var client = new AmazonS3Client("AKIAYS2NQA6BXDGBAPGR", "2tWw0oFUlYheUi3dVTJ76QVIP2ixg+gDuP+I+vYN", RegionEndpoint.APSoutheast1);

            // Check if the bucket exists
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, BucketName);
            if (!bucketExists)
            {
                var bucketRequest = new PutBucketRequest()
                {
                    BucketName = BucketName,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(bucketRequest);
            }

            // Upload the file and make it public
            var objectRequest = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                CannedACL = S3CannedACL.PublicRead
            };
            await client.PutObjectAsync(objectRequest);

            // Construct the URL
            string fileUrl = $"https://{BucketName}.s3.amazonaws.com/{file.FileName}";

            return fileUrl;
        }
    }
}
