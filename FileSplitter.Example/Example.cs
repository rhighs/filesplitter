namespace FileSplitter.Example
{
    class Example
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: <command> [args...]");
                Console.WriteLine("\nCommands:");
                Console.WriteLine("\tmerge [(options, value)...] | [parts...]");
                Console.WriteLine("\t\t--prefix <partPrefix>\t Looks for parts using a prefix, all matching files will be merged.\n");
                Console.WriteLine("\tsplit <filename> <parts>");
                return;
            }

            if (args[0] == "split")
            {
                var splitter = new FileSplitter.Splitter(args[1]);
                int nparts = Convert.ToInt32(args[2]);
                splitter.SplitToFiles(nparts);
            }
            else if (args[0] == "merge" && args[1] == "--prefix")
            {
                var merger = new FileSplitter.Merger(args[2]);
                merger.Merge();
            }
            else if (args[0] == "merge")
            {
                var partPaths = new ArraySegment<string>(args, 1, args.Length - 1);
                var merger = new FileSplitter.Merger(partPaths.ToArray());
                merger.Merge();
            }
        }
    }
}
