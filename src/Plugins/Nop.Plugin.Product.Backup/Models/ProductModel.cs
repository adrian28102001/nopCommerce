﻿using System;

namespace Nop.Plugin.Product.Backup.Models;

public class ProductModel 
{
    public int ProductTypeId { get; set; } 
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public string Sku { get; set; }
    public int StockQuantity { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public bool Exported { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
}