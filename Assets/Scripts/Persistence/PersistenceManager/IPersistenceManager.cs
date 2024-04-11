using System;
using Persistence.Data;

namespace Persistence.PersistenceManager
{
    public interface IPersistenceManager : IDisposable
    {
        public void Save();
        public void Load(IPersistenceData persistenceData);
        public string GetPersistenceKey();
    }
}