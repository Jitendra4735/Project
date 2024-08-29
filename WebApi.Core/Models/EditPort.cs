using System.ComponentModel.DataAnnotations;

namespace WebApi.Business.Models
{
    public class EditPort : Port
    {
        public int Id { get; set; }

        //public ICollection<TerminalDto> Terminals { get; set; } = new List<TerminalDto>();
    }
}
