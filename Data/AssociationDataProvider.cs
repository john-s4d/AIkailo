using AIkailo.Data.DBCore;
using AIkailo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Data
{
    internal class AssociationDataProvider : IDisposable
    {
        //private const int DISK_SECTOR_SIZE = 512;
        private const int BUFFER_SIZE = 4096;
        //private const int BLOCK_HEADER_SIZE = 8;
        private const string BASE_FILENAME = "associations";

        private readonly string _path;
        
        // Variable length. Not contingous. Not sorted.
        // Each field and array defines its own length at the beginning.
        // {definitionBytes}, {parents{weight, id}[]}, {children[]{weight, id}}, {processModels[]{weight, direction[in|out], id}}, {tags[]{weight, id}}
        private readonly FileStream _conceptsDbFile;

        // ConceptId == Position
        // Fixed Length. Sorted. Contingous for performance. Unique. 
        // Data points to positions in the dbFile        
        // {conceptId, definitionPosition, parentsPosition, childrenPosition, processModelsPosition, tagsPosition}
        private readonly FileStream _conceptsDbMapFile;

        // Fixed Length. Sorted
        // -1 indicates unlimited space
        // {length, position}
        private readonly FileStream _conceptsFreeSpaceFile;

        // Single Record
        // {availableId}
        private readonly FileStream _conceptsAvailableIdsFile;

        // Fixed Length. Sorted.
        // {definitionHash, conceptId}
        private readonly FileStream _definitionsIndexFile;

        //private readonly ConceptDbRecord _conceptRecords; // Main DB
        private readonly Tree<ulong, uint> _conceptsIndex; // Get Concept position by Id
        private readonly Tree<IConvertible, ulong> _definitionsIndex; // Get conceptId by Definition
        private readonly Tree<int, uint> _freeSpaceCatalog; // Get next available free space

        internal AssociationDataProvider(string path)
        {   
            _path = Path.Combine(path, BASE_FILENAME);

            _conceptsDbFile = new FileStream(_path + ".concepts.db", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptsDbMapFile = new FileStream(_path + ".concepts.map", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);            
            _conceptsFreeSpaceFile = new FileStream(_path + ".fs", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptsAvailableIdsFile = new FileStream(_path + ".id", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _definitionsIndexFile = new FileStream(_path + ".definitions.idx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);

            /*
            _conceptRecords = new ConceptDbRecord(
                    new BlockStorage(_conceptsDbFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE)
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
            */


        }

        ///**** Concept Id Generator ****///

        private ulong _nextConceptId;

        private ulong GetNextConceptIdFromDisk()
        {
            ulong result = 1;  // intentional. zero is like null

            using (StreamReader sr = new StreamReader(Path.Combine(_path, BASE_FILENAME + ".id")))
            {
                string value = sr.ReadToEnd();

                if (!string.IsNullOrEmpty(value))
                {
                    if (!ulong.TryParse(value, out result))
                    {
                        throw new FormatException(BASE_FILENAME + ".id");
                    }
                }
            }
            return result;
        }

        // Called during Dispose()
        private void WriteConceptIdToDisk(ulong conceptId)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(_path, BASE_FILENAME + ".id")))
            {
                sw.Write(conceptId.ToString());
            }
        }

        internal ulong NextConceptId()
        {   
            if (_nextConceptId == 0)
            {
                _nextConceptId = GetNextConceptIdFromDisk();
            }
            return _nextConceptId++;
        }


        ///**** Concept Lookup ****//

        // Parameters: Depth, weightThreshold

        internal Concept Find(int threshold, IConvertible item)
        {
            return Find(threshold, new IConvertible[] { item });
        }

        internal Scene Find(int threshold, params IConvertible[] items)
        {
            return null;
        }

        internal Concept Create(IConvertible item)
        {
            return Create(new IConvertible[] { item });
        }

        internal Scene Create(IConvertible[] items)
        {   
            return null;
            
        }

        internal Scene FindOrCreate(int threshold, params IConvertible[] items)
        {
            Scene result = Find(threshold, items);
            if (result == null)
            {
                result = Create(items);
            }
            return result;
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

                    WriteConceptIdToDisk(_nextConceptId);
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
