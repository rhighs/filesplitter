using System.IO;
using System.Collections.Generic;

namespace FileSplitter
{
    public class Merger
    {
        string filename;
        string[] partPaths;

        public Merger(string filenamePrefix, string searchDir, bool orderByName = true)
        {
            this.filename = filenamePrefix;
            var partPaths = new List<string>();

            string tempFile = searchDir + filenamePrefix;
            foreach (var part in Directory.GetFiles(searchDir))
            {
                if (part.Length <= tempFile.Length)
                    continue;

                if(part.Substring(0, tempFile.Length) == tempFile)
                    partPaths.Add(part);
            }

            if (orderByName)
                partPaths.Sort();

            this.partPaths = partPaths.ToArray();
        }

        public Merger(string[] partPaths)
        {
            this.partPaths = partPaths;
            this.filename = this.partPaths[0].Remove(this.partPaths[0].Length - 1);
        }

        public void Merge(IEnumerable<byte[]> parts, string path = "./")
        {
            using (var stream = new FileStream(path + filename, FileMode.Append))
            {
                foreach (var part in parts)
                    stream.Write(part, 0, part.Length);
            }
        }

        public void Merge(string path = "./")
        {
            using (var mainStream = new FileStream(path + this.filename, FileMode.Append))
            {
                foreach (var partPath in partPaths)
                {
                    using (var partStream = File.OpenRead(partPath))
                    {
                        byte[] payload = new byte[partStream.Length];
                        partStream.Read(payload);
                        mainStream.Write(payload);
                    }
                }
            }
        }
    }
}
