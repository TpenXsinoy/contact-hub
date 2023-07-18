using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Addresses
{
    public class AddressCreationDto
    {
        [Required(ErrorMessage = "Address Type is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the address type is 50 characters.")]
        public string AddressType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(100, ErrorMessage = "Maximum lenghth for the street is 100 characters.")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the city is 50 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the state is 50 characters.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal Code is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the postal code is 50 characters.")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact ID is required.")]
        public Guid ContactId { get; set; }
    }
}
