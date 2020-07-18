using AIkailo.Data.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    internal class ConceptsRecordStorage : IRecordStorage
    {
        // TODO: Implement Record caching

        public ConceptsRecordStorage(IBlockStorage storage)
        {
            throw new NotImplementedException();
        }

        public uint Create()
        {
            throw new NotImplementedException();
        }

        public uint Create(byte[] data)
        {
            throw new NotImplementedException();
        }

        public uint Create(Func<uint, byte[]> dataGenerator)
        {
            throw new NotImplementedException();
        }

        public void Delete(uint recordId)
        {
            throw new NotImplementedException();
        }

        public byte[] Find(uint recordId)
        {
            throw new NotImplementedException();
        }

        public void Update(uint recordId, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
