using App.Repositories.Users;
using App.Repositories.Warehouses;
using App.Repositories.StockCards;

namespace App.Repositories.TransferRequests
{
    public class TransferRequest
    {
        public int Id { get; set; }
        public int FromWarehouseId { get; set; }
        public Warehouse FromWarehouse { get; set; } = default!;
        
        public int ToWarehouseId { get; set; }
        public Warehouse ToWarehouse { get; set; } = default!;
        
        public int StockCardId { get; set; }
        public StockCard StockCard { get; set; } = default!;
        
        public decimal Quantity { get; set; }
        public string? Description { get; set; }
        
        public int RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; } = default!;
        
        public int? ApprovedByUserId { get; set; }
        public User? ApprovedByUser { get; set; }
        
        public TransferRequestStatus Status { get; set; } = TransferRequestStatus.Pending;
        public string? ApprovalNotes { get; set; }
        
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    public enum TransferRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3,
        Cancelled = 4
    }
}