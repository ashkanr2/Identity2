using System.ComponentModel.DataAnnotations;

namespace AuthEduApi.DTOs
{
  public record UpdateProductPriceDto(
   [Required]
      Guid Id,
   [Required]
    decimal Price
  );


    public record UpdateProductNameDto(
   [Required]
      Guid Id,
   [Required]
    string Name
  );
}
