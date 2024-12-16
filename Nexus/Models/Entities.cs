using Nexus.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Nexus.Models
{
    [Table("employees")]  // Tên bảng là "employees" (số nhiều của "employee")
    public class Employee
    {
        [Column("employee_id")]  // Tên thuộc tính trong bảng là "employee_id"
        public int EmployeeId { get; set; }

        [Column("name")]  // Tên thuộc tính trong bảng là "name"
        public string Name { get; set; }

        [Column("username")]  // Tên thuộc tính trong bảng là "username"
        public string Username { get; set; }

        [Column("password")]  // Tên thuộc tính trong bảng là "password"
        public string Password { get; set; }

        [Column("updated_time")]  // Tên thuộc tính trong bảng là "updated_time"
        public DateTime UpdatedTime { get; set; }

        [Column("updated_by")]  // Tên thuộc tính trong bảng là "updated_by"
        public int? UpdatedBy { get; set; }

        [Column("employee_type")]  // Tên thuộc tính trong bảng là "employee_type"
        public EmployeeType EmployeeType { get; set; }
    }

    [Table("rental_shop_employees")]  // Tên bảng là "rental_shop_employees"
    public class RentalShopEmployee
    {
        [Column("rental_shop_employee_id")]  // Tên thuộc tính trong bảng là "rental_shop_employee_id"
        public int RentalShopEmployeeId { get; set; }

        [Column("rental_shop_id")]  // Tên thuộc tính trong bảng là "rental_shop_id"
        public int RentalShopId { get; set; }
    }

    [Table("admin_employees")]  // Tên bảng là "admin_employees"
    public class AdminEmployee
    {
        [Column("admin_employee_id")]  // Tên thuộc tính trong bảng là "admin_employee_id"
        public int AdminEmployeeId { get; set; }
    }

    [Table("technical_employees")]  // Tên bảng là "technical_employees"
    public class TechnicalEmployee
    {
        [Column("technical_employee_id")]  // Tên thuộc tính trong bảng là "technical_employee_id"
        public int TechnicalEmployeeId { get; set; }
    }

    [Table("cities")]  // Tên bảng là "cities"
    public class City
    {
        [Column("city_id")]  // Tên thuộc tính trong bảng là "city_id"
        public int CityId { get; set; }

        [Column("name")]  // Tên thuộc tính trong bảng là "name"
        public string Name { get; set; }
    }

    [Table("rental_shops")]  // Tên bảng là "rental_shops"
    public class RentalShop
    {
        [Column("rental_shop_id")]  // Tên thuộc tính trong bảng là "rental_shop_id"
        public int RentalShopId { get; set; }

        [Column("city_id")]  // Tên thuộc tính trong bảng là "city_id"
        public int CityId { get; set; }

        [Column("address")]  // Tên thuộc tính trong bảng là "address"
        public string Address { get; set; }
    }
}
