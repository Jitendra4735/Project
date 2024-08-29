using WebApi.Business.Models;

namespace WebApi.Business.Business.Interface
{
    public interface ITerminalProcessor
    {
        Task<IEnumerable<TerminalVM>> GetTerminals();
        Task<TerminalVM> GetTerminal(int id);
        Task<TerminalVM> CreateTerminal(Terminal terminal);
        Task<bool> UpdateTerminal(TerminalEdit terminal);
        Task<bool> DeleteTerminal(int id);
    }
}
