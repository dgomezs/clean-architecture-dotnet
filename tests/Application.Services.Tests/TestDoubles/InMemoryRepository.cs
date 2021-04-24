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
            Elements.Add(id, Copy(value));
            return Task.CompletedTask;
        }

        protected abstract TKey GetId(TVal value);
        protected abstract TVal Copy(TVal value);

        public Task<TVal?> GetById(TKey id)
        {
            var valueOrDefault = Elements.GetValueOrDefault(id);
            // Return a copy of the value so we simulate the full creation of the object as if it was done from a persistence store
            return Task.FromResult(valueOrDefault != null ? Copy(valueOrDefault) : null);
        }
    }
}