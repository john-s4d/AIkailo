namespace AIkailo.Data.DBCore
{
    public class AssociationRecordHeader
    {
        public const short SIZE = 20;
        public short Position { get; private set; }
        public ulong Key { get; private set; }
        public short Length { get; private set; }
        public long ContinuesAtBlock { get; private set; }
        public short RecordNumber { get; private set; }

        public AssociationRecordHeader(short position, ulong key, short length, long continuesAtBlock) //, short recordNumber)
        {
            Position = position;
            Key = key;
            Length = length;
            ContinuesAtBlock = continuesAtBlock;
            RecordNumber = RecordNumber;
        }
    }
}
