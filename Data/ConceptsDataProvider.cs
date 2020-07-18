using AIkailo.Data.DBCore;
using AIkailo.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    internal class ConceptsDataProvider : IDisposable
    {
        private const int BUFFER_SIZE = 4096;
        private const int BLOCK_HEADER_SIZE = 8;

        private readonly string _path;
        private readonly string _providerName;

        // {typeof(IConcept), (Definition | Weight | ProcessModel | Sentiment), parents[], children[]}
        private readonly FileStream _conceptsDbFile;

        // ConceptId => {BlockId, define_pos, parent_pos, child_pos, length}
        private readonly FileStream _conceptsIndexFile;

        // hash(definition<byte[]>) => ConceptId
        private readonly FileStream _definitionsIndexFile;

        // {length, blockId, position}
        private readonly FileStream _freeSpaceCatalogFile;

        //
        private readonly FileStream _conceptIdFile;

        private readonly ConceptsRecordStorage _conceptRecords; // Main DB
        private readonly Tree<ulong, uint> _conceptsIndex; // Get IConcept position by Id
        private readonly Tree<IConvertible, ulong> _definitionsIndex; // Get IConcept Id by Definition
        private readonly Tree<int, uint> _freeSpaceCatalog; // Get next available free space

        internal ConceptsDataProvider(string path, string providerName)
        {
            _providerName = providerName;
            _path = Path.Combine(path, _providerName);

            _conceptsDbFile = new FileStream(_path + ".main", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptsIndexFile = new FileStream(_path + ".pidx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _definitionsIndexFile = new FileStream(_path + ".sidx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _freeSpaceCatalogFile = new FileStream(_path + ".fs", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptIdFile = new FileStream(_path + ".id", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);

            _conceptRecords = new ConceptsRecordStorage(
                    new BlockStorage(_conceptsDbFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DataManager.DISK_SECTOR_SIZE)
                    );

            _conceptsIndex = new Tree<ulong, uint>(
                    new TreeDiskNodeManager<ulong, uint>(null, null, _conceptRecords)
                    );

            _definitionsIndex = new Tree<IConvertible, ulong>(
                    new TreeDiskNodeManager<IConvertible, ulong>(null, null, _conceptRecords)
                    );

            _freeSpaceCatalog = new Tree<int, uint>(
                    new TreeDiskNodeManager<int, uint>(null, null, _conceptRecords)
                    );

            _nextIConceptId = GetNextConceptIdFromDisk();
        }

        /* Concept Id Generator */

        private ulong _nextIConceptId;

        private ulong GetNextConceptIdFromDisk()
        {
            ulong result;

            using (StreamReader sr = new StreamReader(Path.Combine(_path, _providerName + ".id")))
            {
                if (!ulong.TryParse(sr.ReadToEnd(), out result))
                {
                    throw new FormatException(_providerName + ".id");
                }
            }

            return result;
        }

        private void WriteNextConceptIdToDisk(ulong id)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(_path, _providerName + ".id")))
            {
                sw.Write(id);
            }
        }

        internal ulong NextIConceptId()
        {
            return _nextIConceptId++;
        }

        /* IConcept Lookup */

        // Parameters: Depth, Threshold

        internal T Find<T>(ulong? id) where T : IConceptBase
        {
            throw new NotImplementedException();
            // T determines which structure to return
            // Association
            // Concept
            // Scene
            // Frame

        }

        internal T Find<T>(params IConvertible[] items) where T : IConceptBase
        {
            throw new NotImplementedException();
        }

        internal T FindOrCreate<T>(params IConvertible[] items) where T : IConceptBase
        {
            T result = Find<T>(items);
            if (result == null)
            {
                result = Create<T>(items);
            }
            return result;
        }

        private T Create<T>(IConvertible[] items) where T : IConceptBase
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    WriteNextConceptIdToDisk(_nextIConceptId);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AssociationsDataProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
