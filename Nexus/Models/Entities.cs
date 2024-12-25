using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Nexus.Models.Enums;

namespace Nexus.Models
{

    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CustomerId { get; set; }

        [Column("full_name")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("phone_number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Column("address")]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<Connection>? Connections { get; set; }
        [NotMapped]
        public ICollection<Order>? Orders { get; set; }
        [NotMapped]
        public ICollection<Payment>? Payments { get; set; }
    }

    [Table("packages")]
    public class Package
    {
        [Key]
        [Column("package_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PackageId { get; set; }

        [Column("package_name")]
        [MaxLength(100)]
        public string? PackageName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    [Table("stores")]
    public class Store
    {
        [Key]
        [Column("store_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? StoreId { get; set; }

        [Column("store_name")]
        [MaxLength(100)]
        public string? StoreName { get; set; }

        [Column("address")]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Column("phone_number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<Employee>? Employees { get; set; }
        [NotMapped]
        public ICollection<Order>? Orders { get; set; }
    }

    [Table("suppliers")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? SupplierId { get; set; }

        [Column("supplier_name")]
        [MaxLength(100)]
        public string? SupplierName { get; set; }

        [Column("phone_number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("address")]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<Device>? Devices { get; set; }
    }

    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserId { get; set; }

        [Column("username")]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Column("password_hash")]
        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Column("role_id")]
        public Role? RoleId { get; set; }

        [Column("full_name")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("phone_number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public ICollection<Employee>? Employees { get; set; }
    }

    [Table("connections")]
    public class Connection
    {
        [Key]
        [Column("connection_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ConnectionId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("connection_name")]
        [MaxLength(100)]
        public string? ConnectionName { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [NotMapped]
        public Customer? Customer { get; set; }
        [NotMapped]
        public ICollection<ApplyDevice>? ApplyDevices { get; set; }
    }

    [Table("devices")]
    public class Device
    {
        [Key]
        [Column("device_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? DeviceId { get; set; }

        [Column("device_name")]
        [MaxLength(100)]
        public string? DeviceName { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("purchase_date")]
        public DateTime? PurchaseDate { get; set; }

        [ForeignKey("SupplierId")]
        [NotMapped]
        public Supplier? Supplier { get; set; }
        [NotMapped]
        public ICollection<ApplyDevice>? ApplyDevices { get; set; }
    }

    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? EmployeeId { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }
        [NotMapped]
        public Store? Store { get; set; }
        [NotMapped]
        public User? User { get; set; }
    }

    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("total_price")]
        public decimal? TotalPrice { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public Customer? Customer { get; set; }
        [NotMapped]
        public Store? Store { get; set; }
        [NotMapped]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PaymentId { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("description")]
        public string Description { get; set; }
        [NotMapped]
        public Customer? Customer { get; set; }
    }

    [Table("apply_devices")]
    public class ApplyDevice
    {
        [Key]
        [Column("apply_device_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ApplyDeviceId { get; set; }

        [Column("connection_id")]
        public int? ConnectionId { get; set; }

        [Column("device_id")]
        public int? DeviceId { get; set; }
        [NotMapped]
        public Connection? Connection { get; set; }
        [NotMapped]
        public Device? Device { get; set; }
    }

    [Table("order_details")]
    public class OrderDetail
    {
        [Key]
        [Column("order_detail_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderDetailId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("package_name")]
        [MaxLength(100)]
        public string? PackageName { get; set; }

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("package_id")]
        public int? PackageId { get; set; }
        [NotMapped]
        public Order? Order { get; set; }
        [NotMapped]
        public Package? Package { get; set; }
    }
}
