namespace AuthEduApi.Models
{
    public class Products
    {
        public Products()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
