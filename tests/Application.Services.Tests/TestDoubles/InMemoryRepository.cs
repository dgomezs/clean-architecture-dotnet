using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Shared.ValueObjects;

namespace Application.Services.Tests.TestDoubles
{
    public abstract class InMemoryRepository<TKey, TVal>
        where TKey : GuidId
        where TVal : class
    {
        protected readonly Dictionary<TKey, TVal> Elements = new();


        public Task Save(TVal value)
        {
            var id = GetId(value);
            Elements.Remove(id);
            Elements.Add(id, Clone(value));
            return Task.CompletedTask;
        }

        protected abstract TKey GetId(TVal value);
        protected abstract TVal Clone(TVal value);

        public Task<TVal?> GetById(TKey id)
        {
            var valueOrDefault = Elements.GetValueOrDefault(id);
            Elements.Clear(); // force the entity to be saved again
            return Task.FromResult(valueOrDefault);
        }
    }
}