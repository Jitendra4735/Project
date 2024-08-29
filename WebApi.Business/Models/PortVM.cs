using System.ComponentModel.DataAnnotations;

namespace WebApi.Business.Models
{
    public class PortVM : Port
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedDate { get; set; }

        public ICollection<TerminalVM> Terminals { get; set; } = new List<TerminalVM>();
    }
}
