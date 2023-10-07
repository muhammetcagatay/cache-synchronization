namespace memory_cache_synchronization.Models
{
    public class CustomerBalance
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
    }
}
