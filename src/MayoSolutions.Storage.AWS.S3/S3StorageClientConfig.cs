namespace MayoSolutions.Storage.AWS.S3
{
    public class S3StorageClientConfig
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string RegionEndpoint { get; set; }
        public int? PageSize { get; set; }
    }
}