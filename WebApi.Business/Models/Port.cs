using System.ComponentModel.DataAnnotations;

namespace WebApi.Business.Models
{
    public class Port
    {
        [Key]
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        //public ICollection<TerminalDto> Terminals { get; set; } = new List<TerminalDto>();
    }
}
