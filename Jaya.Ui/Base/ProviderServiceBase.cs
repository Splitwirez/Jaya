﻿using Jaya.Ui.Models;
using Jaya.Ui.Services;
using System;
using System.Threading.Tasks;

namespace Jaya.Ui.Base
{
    public abstract class ProviderServiceBase
    {
        readonly MemoryCacheService _cache;

        protected ProviderServiceBase()
        {
            _cache = ServiceLocator.Instance.GetService<MemoryCacheService>();
        }

        #region properties

        public bool IsRootDrive
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public string ImagePath
        {
            get;
            protected set;
        }

        #endregion

        protected DirectoryModel GetFromCache(ProviderModel provider, string path)
        {
            var hash = provider.GetHashCode();
            if (!string.IsNullOrEmpty(path))
                hash += path.GetHashCode();

            if (_cache.TryGetValue(hash, out DirectoryModel directory))
                return directory;

            return null;
        }

        protected void AddToCache(ProviderModel provider, DirectoryModel directory)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            var hash = provider.GetHashCode();
            if (!string.IsNullOrEmpty(directory.Path))
                hash += directory.Path.GetHashCode();

            _cache.Set(hash, directory);
        }

        public async Task<ProviderModel> GetDefaultProviderAsync()
        {
            return await Task.Run(() => GetDefaultProvider());
        }

        public async Task<DirectoryModel> GetDirectoryAsync(ProviderModel provider, string path = null)
        {
            return await Task.Run(() => GetDirectory(provider, path));
        }

        protected abstract ProviderModel GetDefaultProvider();

        protected abstract DirectoryModel GetDirectory(ProviderModel provider, string path = null);

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}