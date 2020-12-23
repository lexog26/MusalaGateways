using AutoMapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogic.Services
{
    /// <summary>
    /// Groups all services with data persistances features based on Unit of Work design pattern
    /// </summary>
    public abstract class TransactionalService<TDto, TEntity, TKey> : ServiceBase, ITransactionalService<TDto, TKey>
                                                           where TDto : BaseDto<TKey>
                                                           where TEntity : Entity<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalService(IMapper mapper, IUnitOfWork unitOfWork, IRepository repository)
                                   : base(repository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Create<TEntity, TKey>(entity);
            await SaveChangesAsync();
            dto.Id = entity.Id;
            return dto;
        }

        public virtual async Task<TDto> DeleteAsync(TKey id)
        {
            var entity = _repository.Delete<TEntity, TKey>(id);
            await SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync(int limit = int.MaxValue)
        {
            return _mapper.Map<IEnumerable<TDto>>(await _repository.GetAllAsync<TEntity>(take: limit));
        }

        public virtual async Task<TDto> GetByIdAsync(TKey id)
        {
            var entity = await _repository.GetEntityByIdAsync<TEntity, TKey>(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            if (_repository.GetCount<TEntity>(x => x.Id.Equals(dto.Id)) > 0)
            {
                var entity = _mapper.Map<TEntity>(dto);
                entity.ModifiedDate = DateTime.UtcNow;
                _repository.Update(entity);
                await SaveChangesAsync();
                return _mapper.Map<TDto>(entity);
            }
            return default;
        }

        public virtual void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _unitOfWork.CommitAsync();
        }
        
    }
}
