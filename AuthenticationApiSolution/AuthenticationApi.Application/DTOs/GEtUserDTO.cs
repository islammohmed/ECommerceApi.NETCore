using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs
{
    public record GEtUserDTO
    (
        int Id,

        [Required]   string? Name,

        [Required] string? TelephoneNumber,

        [Required] string? Email,


        [Required] string? Address,

        [Required] string? Role

    );
}
