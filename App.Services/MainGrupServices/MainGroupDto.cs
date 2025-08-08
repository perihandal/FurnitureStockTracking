namespace App.Services.MainGroupServices
{
    public record MainGroupDto
    {
        public int Id { get; init; }
        public string Code { get; init; }
        public string Name { get; init; }
        public bool IsActive { get; init; }
        public int? UserId { get; init; }
        
        //public List<string> SubGroups { get; init; }
        //public List<string> StockCards { get; init; }
    }
}