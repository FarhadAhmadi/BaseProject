using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Enums;
using BaseProject.Domain.Events;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace BaseProject.Infrastructure.Caching
{
    /// <summary>
    /// Represents a manager for memory caching
    /// </summary>
    public class MemoryCacheBase : ICacheBase, IDisposable
    {
        #region Fields

        private readonly IMemoryCache _cache;
        private readonly IMediator _mediator;

        private bool _disposed;
        private CancellationTokenSource _resetCacheToken = new CancellationTokenSource();

        protected static readonly ConcurrentDictionary<string, bool> _allCacheKeys = new();

        #endregion

        #region Ctor

        public MemoryCacheBase(IMemoryCache cache, IMediator mediator)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        #endregion

        #region Methods

        public virtual async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await _cache.GetOrCreateAsync(key, entry =>
            {
                AddKey(key);
                entry.SetOptions(GetMemoryCacheEntryOptions(CommonHelper.CacheTimeMinutes));
                return acquire();
            });
        }

        public T Get<T>(string key, Func<T> acquire)
        {
            return _cache.GetOrCreate(key, entry =>
            {
                AddKey(key);
                entry.SetOptions(GetMemoryCacheEntryOptions(CommonHelper.CacheTimeMinutes));
                return acquire();
            });
        }

        public virtual Task SetAsync(string key, object data, int cacheTime)
        {
            if (data != null)
            {
                _cache.Set(AddKey(key), data, GetMemoryCacheEntryOptions(cacheTime));
            }
            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(string key, bool publisher = true)
        {
            _cache.Remove(key);
            RemoveKey(key);

            if (publisher)
                _mediator.Publish(new EntityCacheEvent(key, CacheEvent.RemoveKey));

            return Task.CompletedTask;
        }

        public virtual Task RemoveByPrefix(string prefix, bool publisher = true)
        {
            List<string> keysToRemove = _allCacheKeys.Keys
                .Where(x => x.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (string key in keysToRemove)
            {
                _cache.Remove(key);
                RemoveKey(key);
            }

            if (publisher)
                _mediator.Publish(new EntityCacheEvent(prefix, CacheEvent.RemovePrefix));

            return Task.CompletedTask;
        }

        public virtual Task Clear(bool publisher = true)
        {
            ClearKeys();
            _resetCacheToken.Cancel();
            _resetCacheToken = new CancellationTokenSource(); // old one will be GC-ed
            if (publisher)
                _mediator.Publish(new EntityCacheEvent(string.Empty, CacheEvent.RemoveKey));
            return Task.CompletedTask;
        }


        #endregion

        #region Utilities

        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int cacheTime)
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime))
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token))
                .RegisterPostEvictionCallback(callback: PostEvictionCallback);
        }

        protected string AddKey(string key)
        {
            _allCacheKeys.TryAdd(key, true);
            return key;
        }

        protected string RemoveKey(string key)
        {
            _allCacheKeys.TryRemove(key, out _);
            return key;
        }

        private void ClearKeys()
        {
            _allCacheKeys.Clear();
        }

        private void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (reason is EvictionReason.Replaced or EvictionReason.TokenExpired)
                return;

            RemoveKey(key.ToString());
        }

        #endregion

        #region Dispose

        public void Dispose() => _disposed = true;

        #endregion
    }
}
