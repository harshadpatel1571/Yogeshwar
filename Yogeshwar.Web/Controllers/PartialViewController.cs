namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class PartialViewController.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public class PartialViewController : Controller
{
    /// <summary>
    /// The accessories service
    /// </summary>
    private readonly IAccessoriesService _accessoriesService;

    public PartialViewController(IAccessoriesService accessoriesService)
    {
        _accessoriesService = accessoriesService;
    }

    /// <summary>
    /// Accessories the add popup view.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult AccessoriesAddPopupView()
    {
        return PartialView("~/Views/Accessories/_AccessoriesAddEdit.cshtml");
    }

    /// <summary>
    /// Categories the add popup view.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult CategoryAddPopupView()
    {
        return PartialView("~/Views/Categories/_CategoryAddEdit.cshtml");
    }

    /// <summary>
    /// Accessories the quantity view.
    /// </summary>
    /// <param name="accessoryIds">The accessory ids.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AccessoryQuantityView(int[] accessoryIds, CancellationToken cancellationToken)
    {
        var data = await _accessoriesService.GetByIdsAsync(accessoryIds, cancellationToken);

        var productAccessories = data.Select(x => new ProductAccessoryDto
        {
            AccessoryId = x.Id,
            Accessory = x
        }).ToArray();

        var model = new ProductDto
        {
            ProductAccessories = productAccessories
        };

        return PartialView("~/Views/Product/_AccessoryQuantity.cshtml", model);
    }

    /// <summary>
    /// Customers the address view.
    /// </summary>
    /// <param name="customerAddresses">The customer addresses.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public IActionResult CustomerAddressView(CustomerAddressDto[] customerAddresses)
    {
        return PartialView("~/Views/Customer/_Address.cshtml", new CustomerDto
        {
            CustomerAddresses = customerAddresses
        });
    }
}