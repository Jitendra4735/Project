using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Data.Models;

namespace WebApi.Business.Models
{
    public class TerminalEdit : Terminal
    {
        public int Id { get; set; }
    }
}
