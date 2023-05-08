using CoreMVC_Project.Repository.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;

namespace CoreMVC_Project.Repository
{
    public class MemoryCacheRepository : IMemoryCacheRepository
    {
        private readonly IDistributedCache _iDistributedCache;
        public MemoryCacheRepository(IDistributedCache iDistributedCache)
        {
            _iDistributedCache = iDistributedCache;
        }
        public T Get<T>(string key) where T : class
        {
            return ByteArrayToObj<T>(_iDistributedCache.Get(key));
        }

        public void Remove(string key)
        {
            _iDistributedCache.Remove(key);
        }

        public void Set<T>(string key, T value) where T : class
        {
            _iDistributedCache.Set(key, ObjectToByteArray(value), new DistributedCacheEntryOptions()
            { 
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
            });

        }

        private byte[] ObjectToByteArray(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }
    
        private T ByteArrayToObj<T>(byte[] bytes) where T : class
        {
            return bytes is null ? null : JsonSerializer.Deserialize<T>(bytes);
        }
    
    }
}
