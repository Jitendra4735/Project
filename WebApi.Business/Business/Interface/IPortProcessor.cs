using WebApi.Business.Models;

namespace WebApi.Business.Business.Interface
{
    public interface IPortProcessor
    {
        Task<IEnumerable<PortVM>> GetPorts();
        Task<PortVM> GetPort(int id);
        Task<PortVM> CreatePort(Port port);
        Task<bool> UpdatePort( EditPort port);
        Task<bool> DeletePort(int id);
    }
}
