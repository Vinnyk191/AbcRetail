namespace AbcRetail.Services
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; set; }
        public string BlobContainer { get; set; }
        public string TableCustomers { get; set; }
        public string TableProducts { get; set; }
        public string QueueOrders { get; set; }
        public string FileShareName { get; set; }
    }
}
