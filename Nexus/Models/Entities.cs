using Nexus.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Models
{
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
    }

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

        [Column("security_deposit")]
        public decimal? SecurityDeposit { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

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
        public ICollection<Device>? Devices { get; set; }
    }

    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("supplier_name")]
        public string? SupplierName { get; set; }

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

    public class Device
    {
        [Key]
        [Column("device_id")]
        public int? DeviceId { get; set; }

        [Column("device_name")]
        public string? DeviceName { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Supplier? Supplier { get; set; }
        public Store? Store { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [Column("role_id")]
        public Role? RoleId { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Store? Store { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }

    public class Order
    {
        [Key]
        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Column("total_price")]
        public decimal? TotalPrice { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }

    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int? PaymentId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public Order? Order { get; set; }
        public ICollection<Connection>? Connections { get; set; }
    }

    public class Connection
    {
        [Key]
        [Column("connection_id")]
        public int? ConnectionId { get; set; }

        [Column("payment_id")]
        public int? PaymentId { get; set; }

        [Column("connection_name")]
        public string? ConnectionName { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        public Payment? Payment { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        [Column("order_detail_id")]
        public int? OrderDetailId { get; set; }

        [Column("order_id")]
        public int? OrderId { get; set; }

        [Column("package_id")]
        public int? PackageId { get; set; }

        [Column("package_name")]
        public string? PackageName { get; set; }

        [Column("package_quantity")]
        public int? PackageQuantity { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("device_id")]
        public int? DeviceId { get; set; }

        [Column("device_quantity")]
        public int? DeviceQuantity { get; set; }

        public Order? Order { get; set; }
        public Package? Package { get; set; }
        public Device? Device { get; set; }
    }
}
