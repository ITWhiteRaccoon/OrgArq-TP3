using System;
using System.Collections.Generic;
using System.IO;

namespace TP3
{
    /// <summary>
    ///     Eduardo C. Andrade - 17111012-5
    ///     Michael L. S. Rosa - 17204042-0
    ///     Org. Arq. I - 2020/2 - TP3
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Processor proc = new Processor(new List<string> {"0x01095020"});
            proc.Start();
            return;
            if (args.Length >= 3 && File.Exists(args[1]))
            {
                if (args[0] == "-s")
                {
                    List<string> result = AssembleOperation(args[1], "-a");
                    Processor processor = new Processor(result);
                    processor.Start();
                }
                else
                {
                    List<string> result = AssembleOperation(args[1], args[0]);
                    File.WriteAllLines(args[2], result);
                }

                Console.WriteLine("Sucesso!");
                return;
            }

            Console.WriteLine(
                "Informe pela linha de comando a operação desejada e os caminhos de entrada e saída. (eg.: Program -a example.asm example.txt)");
        }

        private static List<string> AssembleOperation(string file, string operation)
        {
            var result = new List<string>();
            string[] input = File.ReadAllLines(file);
            result = operation switch
            {
                "-a" => Assembler.Assemble(input),
                "-d" => Assembler.Disassemble(input),
                _ => result
            };

            return result;
        }
    }
}