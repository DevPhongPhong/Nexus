﻿using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models.Enums;
using Nexus.Models;
using Nexus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nexus.Controllers;

[ApiController]
[Route("api/[controller]")]
[RoleAccess(Role.Admin, Role.TechnicalEmployee)]
public class DeviceController : BaseController
{
    private readonly NexusDbContext _context;

    public DeviceController(NexusDbContext context)
    {
        _context = context;
    }

    // Get all devices
    [HttpGet]
    public IActionResult GetAllDevices(int page, int size, string? query)
    {
        var count = _context.Devices.AsQueryable().Query(query).Count();
        var devices = _context.Devices.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();

        // Lấy dữ liệu liên quan thủ công
        foreach (var device in devices)
        {
            device.Supplier = _context.Suppliers
                .FirstOrDefault(s => s.SupplierId == device.SupplierId);
            device.Store = _context.Stores
                .FirstOrDefault(st => st.StoreId == device.StoreId);
        }
        return Ok(new { devices, count });
    }

    // Get device by ID
    [HttpGet("{id}")]
    public IActionResult GetDeviceById(int id)
    {
        var device = _context.Devices.FirstOrDefault(d => d.DeviceId == id);

        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        // Lấy dữ liệu liên quan thủ công
        device.Supplier = _context.Suppliers
            .FirstOrDefault(s => s.SupplierId == device.SupplierId);
        device.Store = _context.Stores
            .FirstOrDefault(st => st.StoreId == device.StoreId);

        return Ok(device);
    }

    // Create a new device
    [HttpPost]
    public IActionResult CreateDevice([FromBody] Device device)
    {
        if (device == null)
        {
            return BadRequest("Invalid device data.");
        }
        device.CreatedAt = DateTime.Now;
        _context.Devices.Add(device);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            if (ex.InnerException.Message.Contains("supplier_id")) return BadRequest("no supplier");
        }

        return CreatedAtAction(nameof(GetDeviceById), new { id = device.DeviceId }, device);
    }

    // Update device
    [HttpPut("{id}")]
    public IActionResult UpdateDevice(int id, [FromBody] Device updatedDevice)
    {
        updatedDevice.UpdatedAt = DateTime.Now;
        if (updatedDevice == null || updatedDevice.DeviceId != id)
        {
            return BadRequest("Device data is invalid or mismatched.");
        }

        var device = _context.Devices.Find(id);
        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        // Update properties
        device.DeviceName = updatedDevice.DeviceName;
        device.SupplierId = updatedDevice.SupplierId;
        device.StoreId = updatedDevice.StoreId;
        device.Quantity = updatedDevice.Quantity;
        device.CreatedAt = updatedDevice.CreatedAt;
        device.UpdatedAt = updatedDevice.UpdatedAt;

        _context.Devices.Update(device);
        _context.SaveChanges();

        return NoContent();
    }

    // Delete device
    [HttpDelete("{id}")]
    public IActionResult DeleteDevice(int id)
    {
        var device = _context.Devices.Find(id);
        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        _context.Devices.Remove(device);
        _context.SaveChanges();

        return NoContent();
    }
}