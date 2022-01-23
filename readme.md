## Lazily split and merge files

### This package provides two main classes, Splitter and Merger.

- Splitter: Loads file parts into memory in a lazy way by yielding each part on a `MoveNext()` call (using the Split() function). Splitting can be performed in the following way, you specify the amount of parts you want per file then you can choose if you want a trailing remainer or a merged remainder, this means, if you have a 100 bytes file and you divide it in 11 parts you can either have the remaining 1 byte as a dedicated part adding up to the total for 12 parts, or you can attach it to the last part making it 10 bytes instead of the original 9. This is purely on your preference.

- Merger: Takes a list of parts, they can be presented as files names (which can also be derived by file prefix) or a collection of parts (byte arrays). The way this class works depends on the contructor, if you've created it using a file prefix you can merge parts via byte arrays or file names, for the latter data must be stored on disk. Passing a prefix to constructor will make Merger look for parts on its own in a specified directory, every file that matches the prefix will be included for merging in an ordered by name manner. Once created the object you will have a `Merge()` method callable as `Merge()` or `Merge(IEnumerable<byte[]> parts)` if parts are not present in disk memory.

### Running the example

```bash
$ dotnet run --project FileSplitter.Example split test.txt 4
$ ls
.. . [some_files...] test.txt test.txt0 test.txt1 test.txt2 text.txt3
$ mv test.txt test.txt.orig
$ dotnet run --project FileSplitter.Example merge --prefix test.txt
$ diff test.txt test.txt.orig
```

Note: Merging was successful, diff exited on 0
