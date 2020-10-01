using System;
using System.IO;
using System.Threading;

namespace AIkailo.Data.DBCore
{
    class FileCopy
    {
        const int BUFFERS = 4; // number of outstanding requests
        const int BUFFER_SIZE = 1 << 20; // request size, one megabyte

        public static FileStream source;
        public static FileStream target;
        public static long totalBytes = 0;
        public static long bytesRead = 0;
        public static long bytesWritten = 0;
        public static object WriteCountMutex = new object[0];

        public static AsyncRequestState[] request = new AsyncRequestState[BUFFERS];

        public class AsyncRequestState
        {
            public byte[] Buffer;
            public long bufferOffset;
            public AutoResetEvent ReadLaunched;
            public IAsyncResult ReadAsyncResult;

            public AsyncRequestState(int i)
            {
                bufferOffset = i * BUFFER_SIZE;
                ReadLaunched = new AutoResetEvent(false);
                Buffer = new byte[BUFFER_SIZE];
            }
        }

        public static void WriteCompleteCallback(IAsyncResult ar)
        {
            lock (WriteCountMutex)
            {
                int i = Convert.ToInt32(ar.AsyncState);
                target.EndWrite(ar);
                bytesWritten += BUFFER_SIZE;
                request[i].bufferOffset += BUFFERS * BUFFER_SIZE;
                if (request[i].bufferOffset < totalBytes)
                {
                    source.Position = request[i].bufferOffset;
                    request[i].ReadAsyncResult = source.BeginRead(request[i].Buffer, 0, BUFFER_SIZE, null, i);
                    request[i].ReadLaunched.Set();
                }
            }
        }


        static void Main(string[] args)
        {
            source = new FileStream(@"C:\source.dat", FileMode.Open, FileAccess.Read, FileShare.Read, BUFFER_SIZE, true);
            target = new FileStream(@"C:\target.dat", FileMode.CreateNew, FileAccess.Write, FileShare.None, BUFFER_SIZE, true);

            totalBytes = source.Length;

            AsyncCallback writeCompleteCallback = new AsyncCallback(WriteCompleteCallback);

            for (int i = 0; i < BUFFERS; i++)
            {
                request[i] = new AsyncRequestState(i);
            }

            for (int i = 0; i < BUFFERS; i++)
            { 
                request[i].ReadAsyncResult = source.BeginRead(request[i].Buffer, 0, BUFFER_SIZE, null, i);
                request[i].ReadLaunched.Set();
            }

            for (int i = 0; bytesRead < totalBytes; i = (i + 1) % BUFFERS)
            {
                request[i].ReadLaunched.WaitOne();
                int bytes = source.EndRead(request[i].ReadAsyncResult);
                bytesRead += bytes;
                target.BeginWrite(request[i].Buffer, 0, bytes, writeCompleteCallback, i);
            }

            //while (pending > 0) Thread.Sleep(10);

            source.Close();
            target.Close();
        } 
    }
}
