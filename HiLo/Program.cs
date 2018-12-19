using System;
using System.IO;

namespace HiLo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Wrong number of arguments. Call: HiLo [sourceFile] [oddFile] [evenFile] [verbose]");
                Console.WriteLine("[sourceFile] - path to source file");
                Console.WriteLine("[oddFile] - path to file where will be odd bytes stored");
                Console.WriteLine("[evenFile] - path to file where will be even bytes stored");
                Console.WriteLine("[verbose] - extended informations about program run use true or false value");
                Console.WriteLine("example: HiLo.exe \"c:/tmp/soutce.bin\" oddBin.bin evenBin.bin true");
                return;
            }

            try
            {
                using (var br = GetBinaryReader(args[0]))
                using (var bwOdd = GetBinaryWritter(args[1]))
                using (var bwEven = GetBinaryWritter(args[2]))
                {
                    var isVerbose = bool.Parse(args[3]);
                    var inputFile = args[0];
                    var counter = 1;
                    var oddBytesWritten = 0;
                    var evenBytesWritten = 0;

                    do
                    {
                        var currentByte = br.ReadByte();
                        if (isVerbose) Console.Write($"Reading {counter} byte of {inputFile}: {currentByte}");

                        if (currentByte % 2 == 0)
                        {
                            if (isVerbose) Console.Write(" - as Odd\n");
                            oddBytesWritten++;
                            bwOdd.Write(currentByte);
                        }
                        else
                        {
                            if (isVerbose) Console.Write(" - as Even\n");
                            evenBytesWritten++;
                            bwEven.Write(currentByte);
                        }

                        counter++;
                    } while (br.PeekChar() != -1);

                    Console.WriteLine($"{counter} bytes readed - {oddBytesWritten} odd - {evenBytesWritten} even.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Unreckognized verbose switch.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Specified file {ex.Message} not found.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong path.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private static BinaryReader GetBinaryReader(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException(nameof(fileName));
            if (!File.Exists(fileName)) throw new FileNotFoundException(fileName);

            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return new BinaryReader(fs);
        }

        private static BinaryWriter GetBinaryWritter(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException(nameof(fileName));

            var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            return new BinaryWriter(fs);
        }
    }
}