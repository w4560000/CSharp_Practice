namespace MongoDBWebSample.Models
{
    public class EmployeeViewModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public decimal Salary { get; set; } = 0;
        public IFormFile File { get; set; }
    }
}
