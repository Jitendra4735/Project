using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Data.Models;

namespace WebApi.Business.Models
{
    public class TerminalVM : Terminal
    {
        public int Id { get; set; }

        [ForeignKey("PortId")]
        public PortVM Port { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastEditedDate { get; set; } = DateTime.UtcNow;
    }
}
