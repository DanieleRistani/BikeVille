﻿using System;
using System.Collections.Generic;

namespace BikeVille.Entity;

/// <summary>
/// Products sold or used in the manfacturing of sold products.
/// </summary>
public partial class ProductDto
{
    /// <summary>
    /// Primary key for Product records.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Unique product identification number.
    /// </summary>
    public string ProductNumber { get; set; } = null!;

    /// <summary>
    /// Product color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Standard cost of the product.
    /// </summary>
    public decimal StandardCost { get; set; }

    /// <summary>
    /// Selling price.
    /// </summary>
    public decimal ListPrice { get; set; }

    /// <summary>
    /// Product size.
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Product weight.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Product is a member of this product category. Foreign key to ProductCategory.ProductCategoryID. 
    /// </summary>
    public int? ProductCategoryId { get; set; }

    /// <summary>
    /// Product is a member of this product model. Foreign key to ProductModel.ProductModelID.
    /// </summary>
    public int? ProductModelId { get; set; }

    /// <summary>
    /// Date the product was available for sale.
    /// </summary>
    public DateTime SellStartDate { get; set; }

    /// <summary>
    /// Date the product was no longer available for sale.
    /// </summary>
    public DateTime? SellEndDate { get; set; }

  
   
}
