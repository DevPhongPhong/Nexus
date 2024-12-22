using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Nexus.Models.Enums;

namespace Nexus.Models
{

    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [Column("role_id")]
        public Role? RoleId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        public Employee? Employee { get; set; }
    }

    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [ForeignKey("Store")]
        [Column("store_id")]
        public int? StoreId { get; set; }

        public Store? Store { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }
    }

    [Table("stores")]
    public class Store
    {
        [Key]
        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("store_name")]
        public string? StoreName { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }

    [Table("suppliers")]
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("supplier_name")]
        public string? SupplierName { get; set; }

        [Column("contact_name")]
        public string? ContactName { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Device>? Devices { get; set; }
    }

    [Table("devices")]
    public class Device
    {
        [Key]
        [Column("device_id")]
        public int? DeviceId { get; set; }

        [Column("device_name")]
        public string? DeviceName { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("purchase_date")]
        public DateTime? PurchaseDate { get; set; }

        [Column("warranty_period")]
        public int? WarrantyPeriod { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Supplier? Supplier { get; set; }

        public ICollection<ApplyDevice>? ApplyDevices { get; set; }
    }

    [Table("packages")]
    public class Package
    {
        [Key]
        [Column("package_id")]
        public int? PackageId { get; set; }

        [Column("package_name")]
        public string? PackageName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }

    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<Connection>? Connections { get; set; }
    }

    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("order_date")]
        public DateTime? OrderDate { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Customer? Customer { get; set; }

        public Store? Store { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    [Table("order_details")]
    public class OrderDetail
    {
        [Key]
        [Column("order_detail_id")]
        public int? OrderDetailId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("product_name")]
        public string? ProductName { get; set; }

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Order? Order { get; set; }
    }

    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int? PaymentId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("payment_date")]
        public DateTime? PaymentDate { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("payment_method")]
        public string? PaymentMethod { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Order? Order { get; set; }
    }

    [Table("connections")]
    public class Connection
    {
        [Key]
        [Column("connection_id")]
        public int? ConnectionId { get; set; }

        [ForeignKey("Customer")]
        [Column("customer_id")]
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Column("connection_name")]
        public string? ConnectionName { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public ICollection<ApplyDevice>? ApplyDevices { get; set; }
    }

    [Table("apply_devices")]
    public class ApplyDevice
    {
        [Key]
        [Column("apply_device_id")]
        public int? ApplyDeviceID { get; set; }

        [Column("device_id")]
        public int? DeviceId { get; set; }

        public Device? Device { get; set; }

        [Column("connection_id")]
        public int? ConnectionId { get; set; }

        public Connection? Connection { get; set; }
    }
}
