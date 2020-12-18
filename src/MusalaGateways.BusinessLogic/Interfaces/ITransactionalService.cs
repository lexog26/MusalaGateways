using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogic.Interfaces
{
    /// <summary>
    /// Services that persists data with a transactional flow
    /// </summary>
    public interface ITransactionalService : IServiceBase
    {
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
