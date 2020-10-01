using AIkailo.Data.DBCore;
using AIkailo.Common;
using AIkailo.Model.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Data
{
    internal class AssociationDataProvider : IDisposable
    {
        private const int BLOCK_HEADER_SIZE = 48;
        private const int DISK_SECTOR_SIZE = 512;
        private const int BUFFER_SIZE = 4096;

        private bool disposed = false;

        private ulong _nextConceptId = 0;
        private const uint NEXT_CONCEPT_ID_PK = 0;
        private TreeULongSerializer uLongSerializer = new TreeULongSerializer();

        // {definition, conceptId}
        // {conceptId, {definitionPosition, parentsPosition, childrenPosition, processModelsPosition, tagsPosition}}        
        // {definitionBytes}, {parents{weight, id}[]}, {children[]{weight, id}}, {processModels[]{weight, direction[in|out], id}}, {tags[]{weight, id}}

        //private readonly Tree<Primitive, uint> _settingsIndex; // Setting Name => Setting Position
        private readonly RecordStorage _settingsDb; // Setting Position => Setting Value
        private readonly Tree<Primitive, ulong> _definitionsIndex; // Definition => Concept Id
        private readonly Tree<ulong, uint> _conceptsIndex; // Concept Id => Concept Position
        private readonly RecordStorage _conceptsDb; // Concept Position => Concept Record

        //private readonly Stream _settingsIndexFile;
        private readonly Stream _settingsDbFile;
        private readonly Stream _definitionsIndexFile;
        private readonly Stream _conceptsIndexFile;
        private readonly Stream _conceptsDbFile;


        internal AssociationDataProvider(string path)
        {
            //_settingsIndexFile = new FileStream(Path.Combine(path, "settings.idx"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _settingsDbFile = new FileStream(Path.Combine(path, "settings.db"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _definitionsIndexFile = new FileStream(Path.Combine(path, "definitions.db"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptsIndexFile = new FileStream(Path.Combine(path, "concepts.idx"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);
            _conceptsDbFile = new FileStream(Path.Combine(path, "concepts.db"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE);

            /*
            _settingsIndex = new Tree<Primitive, uint>(
                    new TreeDiskNodeManager<Primitive, uint>(
                        new PrimitiveSerializer(),
                        new TreeUIntSerializer(),
                        new RecordStorage(new BlockStorage(_settingsIndexFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE)),
                        new PrimitiveComparer()
                    )
                );*/

            _settingsDb = new RecordStorage(new BlockStorage(_settingsDbFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE));

            _definitionsIndex = new Tree<Primitive, ulong>(
                    new TreeDiskNodeManager<Primitive, ulong>(
                        new PrimitiveSerializer(),
                        new TreeULongSerializer(),
                        new RecordStorage(new BlockStorage(_definitionsIndexFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE)),
                        new PrimitiveComparer()
                    )
                );

            _conceptsIndex = new Tree<ulong, uint>(
                   new TreeDiskNodeManager<ulong, uint>(
                       new TreeULongSerializer(),
                       new TreeUIntSerializer(),
                       new RecordStorage(new BlockStorage(_conceptsIndexFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE))
                    )
                );

            _conceptsDb = new RecordStorage(new BlockStorage(_conceptsDbFile, BUFFER_SIZE, BLOCK_HEADER_SIZE, DISK_SECTOR_SIZE));
        }

        internal ulong NextConceptId()
        {   
            if (_nextConceptId == 0)
            {
                var bytes = _settingsDb.Find(NEXT_CONCEPT_ID_PK);                
                _nextConceptId = bytes == null ? _nextConceptId : uLongSerializer.Deserialize(bytes, 0, 8);

            }
            SaveNextConceptId(++_nextConceptId);
            return _nextConceptId;
        }

        private void SaveNextConceptId(ulong nextConceptId)
        {
            _settingsDb.Update(NEXT_CONCEPT_ID_PK, uLongSerializer.Serialize(nextConceptId));
        }

        // ** FIND **


        internal IConcept Find(Primitive definition)
        {
            Tuple<Primitive, ulong> result = _definitionsIndex.Get(definition);
            return result == null ? null : new Concept(result.Item1, result.Item2);
        }

        internal IScene Find(IConcept[] concepts)
        {
            throw new NotImplementedException();
        }

        // ** CREATE **

        internal IConcept Create(Primitive definition)
        {
            var newId = NextConceptId();
            _definitionsIndex.Insert(definition, newId);
            return new Concept(definition, newId);
        }

        internal IScene Create(IConcept[] concepts)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //SaveNextConceptId(_nextConceptId);
                    //_settingsIndexFile.Dispose();
                    _settingsDbFile.Dispose();
                    _definitionsIndexFile.Dispose();
                    _conceptsIndexFile.Dispose();
                    _conceptsDbFile.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposed = true;
            }
        }

        
         ~AssociationDataProvider()
         {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(false);
         }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
