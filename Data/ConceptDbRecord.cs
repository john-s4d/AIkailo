using AIkailo.Data.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIkailo.Data
{
    public class ConceptDbRecord
    {
        public ulong ConceptId { get; set; }
        public int Definition_Position { get; set; }
        public int Parents_Position { get; set; }
        public int Children_Position { get; set; }
        public int ProcessModels_Position { get; set; }
        public int Tags_Position { get; set; }

        public byte[] Definition { get; set; }

        public AssociationDbRecordCollection Parents { get; set; }
        public AssociationDbRecordCollection Children { get; set; }
        public AssociationDbRecordCollection ProcessModels { get; set; }
        public AssociationDbRecordCollection Tags { get; set; }
    }
}
