using System;
using System.IO;
using System.Collections.Generic;

namespace FileSplitter
{
    class Splitter
    {
        private string path;
        private FileStream stream;

        public Splitter(string path)
        {
            this.stream = File.OpenRead(path);
            this.path = path;
        }

        private byte[] MergeParts(byte[] first, byte[] second)
        {
            byte[] merged = new byte[first.Length + second.Length];
            Array.Copy(first, merged, first.Length);
            Array.Copy(second, 0, merged, first.Length, second.Length);
            return merged;
        }

        private byte[] MakeTail(byte[] lastBuffer, int remaining)
        {
            byte[] tail = new byte[remaining];
            int readBytes = stream.Read(tail);

            if (readBytes == 0)
            {
                Array.Copy(lastBuffer, tail, remaining);
                return tail;
            }
            else
            {
                return MergeParts(lastBuffer, tail);
            }
        }

        public IEnumerable<byte[]> Split(int nparts, bool mergeLast = true)
        {
            long step = stream.Length / nparts;
            long remaining = stream.Length % nparts;
            byte[] outBuffer = new byte[step];
            byte[] last = new byte[step];

            while (stream.Read(outBuffer, 0, (int) step) == step)
            {
                if (stream.Length - stream.Position < step
                        && stream.Position < stream.Length - 1
                        && mergeLast)
                    break;

                yield return outBuffer;
                Array.Fill<byte>(outBuffer, 0);
            }

            if (remaining > 0)
                yield return MakeTail(outBuffer, (int)remaining);
        }

        public void SplitToFiles(int nparts, bool mergeLast = true)
        {
            int partNumber = 0;
            foreach (var part in Split(nparts, mergeLast))
            {
                using (var stream = new FileStream(path + partNumber, FileMode.Append))
                {
                    stream.Write(part);
                }
                partNumber++;
            }
        }
    }
}