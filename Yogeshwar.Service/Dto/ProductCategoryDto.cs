﻿namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ProductCategoryDto.
/// </summary>
public sealed class ProductCategoryDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    /// <value>The category identifier.</value>
    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>The product identifier.</value>
    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the category.
    /// </summary>
    /// <value>The category.</value>
    public CategoryDto? Category { get; set; }
}