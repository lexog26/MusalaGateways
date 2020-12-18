using MusalaGateways.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogic.Interfaces
{
    /// <summary>
    /// Services that persists data with a transactional flow
    /// </summary>
    public interface ITransactionalService<TDto, TKey> : IServiceBase where TDto : BaseDto<TKey>
    {
        Task<TDto> CreateAsync(TDto dto);

        Task<TDto> DeleteAsync(TKey id);

        Task<IEnumerable<TDto>> GetAllAsync(int limit = int.MaxValue);

        Task<TDto> GetByIdAsync(TKey id);

        Task<TDto> UpdateAsync(TDto dto);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
