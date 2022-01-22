using System.IO;
using System.Collections.Generic;

namespace FileSplitter
{
    class Merger
    {
        string path;
        string[] partPaths;

        public Merger(string prefixName, bool orderByName = true)
        {
            this.path = "./" + prefixName;
            var partPaths = new List<string>();

            foreach (var part in Directory.GetFiles("./"))
            {
                if (part.Length <= this.path.Length)
                    continue;

                if(part.Substring(0, this.path.Length) == this.path)
                    partPaths.Add(part);
            }

            if (orderByName)
                partPaths.Sort();

            this.partPaths = partPaths.ToArray();
        }

        public Merger(string[] partPaths)
        {
            this.partPaths = partPaths;
            this.path = this.partPaths[0].Remove(this.partPaths[0].Length - 1);
        }

        public void Merge(IEnumerable<byte[]> parts)
        {
            using (var stream = new FileStream(path, FileMode.Append))
            {
                foreach (var part in parts)
                    stream.Write(part, 0, part.Length);
            }
        }

        public void Merge()
        {
            using (var mainStream = new FileStream(path, FileMode.Append))
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