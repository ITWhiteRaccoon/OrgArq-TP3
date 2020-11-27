using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TP3
{
    /// <summary>
    ///     Eduardo C. Andrade - 17111012-5
    ///     Michael L. S. Rosa - 17204042-0
    ///     Org. Arq. I - 2020/2 - TP3
    ///     Provides methods to convert from MIPS Assembly to hex code and vice-versa.
    /// </summary>
    public static class Assembler
    {
        private const int StartInstrAddress = 0x00400000;
        private const int StartMemoryAddress = 0x10010000;

        #region Mappings

        private static readonly Dictionary<string, Instruction> NameToOp = new Dictionary<string, Instruction>
        {
            {"sll", new Instruction(0, 0)},
            {"jr", new Instruction(0, 8)},
            {"div", new Instruction(0, 0x1a)},
            {"addu", new Instruction(0, 0x21)},
            {"and", new Instruction(0, 0x24)},
            {"nor", new Instruction(0, 0x27)},
            {"slt", new Instruction(0, 0x2a)},

            {"j", new Instruction(0x02, 0)},
            {"jal", new Instruction(0x03, 0)},

            {"beq", new Instruction(0x04, 0)},
            {"blez", new Instruction(0x06, 0)},
            {"slti", new Instruction(0x0a, 0)},
            {"ori", new Instruction(0x0d, 0)},
            {"lui", new Instruction(0x0f, 0)},
            {"lw", new Instruction(0x23, 0)},
            {"sw", new Instruction(0x2b, 0)}
        };

        private static readonly Dictionary<string, int> RegToNum = new Dictionary<string, int>
        {
            {"$zero", 0},
            {"$at", 1},
            {"$v0", 2},
            {"$v1", 3},
            {"$a0", 4},
            {"$a1", 5},
            {"$a2", 6},
            {"$a3", 7},
            {"$t0", 8},
            {"$t1", 9},
            {"$t2", 10},
            {"$t3", 11},
            {"$t4", 12},
            {"$t5", 13},
            {"$t6", 14},
            {"$t7", 15},
            {"$s0", 16},
            {"$s1", 17},
            {"$s2", 18},
            {"$s3", 19},
            {"$s4", 20},
            {"$s5", 21},
            {"$s6", 22},
            {"$s7", 23},
            {"$t8", 24},
            {"$t9", 25},
            {"$k0", 26},
            {"$k1", 27},
            {"$gp", 28},
            {"$sp", 29},
            {"$fp", 30},
            {"$ra", 31},
            {"$0", 0},
            {"$1", 1},
            {"$2", 2},
            {"$3", 3},
            {"$4", 4},
            {"$5", 5},
            {"$6", 6},
            {"$7", 7},
            {"$8", 8},
            {"$9", 9},
            {"$10", 10},
            {"$11", 11},
            {"$12", 12},
            {"$13", 13},
            {"$14", 14},
            {"$15", 15},
            {"$16", 16},
            {"$17", 17},
            {"$18", 18},
            {"$19", 19},
            {"$20", 20},
            {"$21", 21},
            {"$22", 22},
            {"$23", 23},
            {"$24", 24},
            {"$25", 25},
            {"$26", 26},
            {"$27", 27},
            {"$28", 28},
            {"$29", 29},
            {"$30", 30},
            {"$31", 31}
        };

        #endregion

        #region Disassemble

        public static List<string> Disassemble(string[] input)
        {
            List<string> result = HexToInstr(input, out Dictionary<int, string> labels);

            result = AddLabels(result, labels);

            return result;
        }

        /// <summary>
        ///     Adds the missing labels in the MIPS Assembly code
        /// </summary>
        /// <param name="input">Assembly code</param>
        /// <param name="labels">Dictionary mapping labels to line of code</param>
        /// <returns>Assembly code with labels</returns>
        private static List<string> AddLabels(List<string> input, Dictionary<int, string> labels)
        {
            foreach (int index in labels.Keys)
            {
                input[index] = labels[index] + ":" + input[index];
            }

            return input;
        }

        /// <summary>
        ///     Converts hex code to MIPS Assembly without labels.
        /// </summary>
        /// <param name="input">Hex MIPS code</param>
        /// <param name="labels">Dictionary mapping labels to line of code</param>
        /// <returns>Assembly code without labels</returns>
        private static List<string> HexToInstr(string[] input, out Dictionary<int, string> labels)
        {
            var result = new List<string>();
            labels = new Dictionary<int, string>();

            const string labelPre = "L_";
            int labelCount = 1;

            for (int index = 0; index < input.Length; index++)
            {
                input[index] = Convert.ToString(Convert.ToInt64(input[index], 16), 2).PadLeft(32, '0');

                StringBuilder mips = new StringBuilder();

                int opcode = Convert.ToInt32(input[index][..6], 2);
                if (opcode == 0)
                {
                    int rs = Convert.ToInt32(input[index][6..11], 2);
                    int rt = Convert.ToInt32(input[index][11..16], 2);
                    int rd = Convert.ToInt32(input[index][16..21], 2);
                    int funct = Convert.ToInt32(input[index][26..], 2);
                    switch (funct)
                    {
                        case 0: //sll
                            int shamt = Convert.ToInt32(input[index][21..26], 2);
                            mips.Append("sll");
                            mips.Append($" ${rd}, ${rt}, {shamt}");
                            break;
                        case 8: //jr
                            mips.Append("jr");
                            mips.Append($" ${rs}");
                            break;
                        case 0x1a: //div
                            mips.Append("div");
                            mips.Append($" ${rs}, ${rt}");
                            break;
                        case 0x21: //addu
                            mips.Append("addu");
                            mips.Append($" ${rd}, ${rs}, ${rt}");
                            break;
                        case 0x24: //and
                            mips.Append("and");
                            mips.Append($" ${rd}, ${rs}, ${rt}");
                            break;
                        case 0x27: //nor
                            mips.Append("nor");
                            mips.Append($" ${rd}, ${rs}, ${rt}");
                            break;
                        case 0x2a: //slt
                            mips.Append("slt");
                            mips.Append($" ${rd}, ${rs}, ${rt}");
                            break;
                    }
                }
                else if (opcode == 2) //j
                {
                    mips.Append("j");
                    int address = Convert.ToInt32($"{input[index][6..]}00", 2);
                    if (!labels.ContainsKey((address - StartInstrAddress) / 4))
                    {
                        labels[(address - StartInstrAddress) / 4] = $"{labelPre}{labelCount}";
                        labelCount++;
                    }

                    mips.Append($" {labels[(address - StartInstrAddress) / 4]}");
                }
                else if (opcode == 3) //jal
                {
                    mips.Append("jal");
                    int address = Convert.ToInt32($"{input[index][6..]}00", 2);
                    if (!labels.ContainsKey((address - StartInstrAddress) / 4))
                    {
                        labels[(address - StartInstrAddress) / 4] = $"{labelPre}{labelCount}";
                        labelCount++;
                    }

                    mips.Append($" {labels[(address - StartInstrAddress) / 4]}");
                }
                else
                {
                    int rs = Convert.ToInt32(input[index][6..11], 2);
                    int rt = Convert.ToInt32(input[index][11..16], 2);
                    string immExtended = input[index][16..].PadLeft(32, input[index][16]);
                    int imm = Convert.ToInt32(immExtended, 2);
                    switch (opcode)
                    {
                        case 4: //beq
                            mips.Append("beq");
                            imm++;
                            if (!labels.ContainsKey(index + imm))
                            {
                                labels[index + imm] = $"{labelPre}{labelCount}";
                                labelCount++;
                            }

                            mips.Append($" ${rs}, ${rt}, {labels[index + imm]}");

                            break;
                        case 6: //blez
                            mips.Append("blez");
                            imm++;
                            if (!labels.ContainsKey(index + imm))
                            {
                                labels[index + imm] = $"{labelPre}{labelCount}";
                                labelCount++;
                            }

                            mips.Append($" ${rs}, {labels[index + imm]}");

                            break;
                        case 0xa: //slti
                            mips.Append("slti");
                            mips.Append($" ${rt}, ${rs}, {imm}");
                            break;
                        case 0xd: //ori
                            mips.Append("ori");
                            mips.Append($" ${rt}, ${rs}, {imm}");
                            break;
                        case 0xf: //lui
                            mips.Append("lui");
                            mips.Append($" ${rt}, {imm}");
                            break;
                        case 0x23: //lw
                            mips.Append("lw");
                            mips.Append($" ${rt}, {imm}(${rs})");
                            break;
                        case 0x2b: //sw
                            mips.Append("sw");
                            mips.Append($" ${rt}, {imm}(${rs})");
                            break;
                    }
                }

                result.Add(mips.ToString());
            }

            return result;
        }

        #endregion

        #region Assemble

        #region Checks

        /// <summary>
        ///     Checks if the provided array contains the number of arguments needed.
        /// </summary>
        /// <param name="args">Array with the arguments</param>
        /// <param name="nArgs">Number of arguments</param>
        /// <param name="code">Assembly code for exception message</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if there are less arguments than expected in array, the exception message
        ///     contains the code string.
        /// </exception>
        private static void CheckArgs<T>(T[] args, int nArgs, string code)
        {
            if (args.Length < nArgs)
            {
                throw new ArgumentException($"Faltam argumentos na linha '{code}'.");
            }
        }

        /// <summary>
        ///     Checks if the provided dictionary contains all the keys from the array.
        /// </summary>
        /// <param name="dict">Dictionary for search</param>
        /// <param name="keys">Keys to lookup in dictionary</param>
        /// <param name="keyType">Type of key to show in exception message</param>
        /// <param name="code">Assembly code for exception message</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if any of key from the array is not found in the dictionary, the exception
        ///     message contains the code string.
        /// </exception>
        private static void CheckKeys<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey[] keys, string keyType,
            string code)
        {
            foreach (TKey key in keys)
            {
                if (!dict.ContainsKey(key))
                {
                    throw new ArgumentException($"O {keyType} '{key}' na linha '{code}' não existe.");
                }
            }
        }

        /// <summary>
        ///     Tries to parse the string to a positive/negative hex/decimal integer
        /// </summary>
        /// <param name="number">The string to convert</param>
        /// <param name="code">Assembly code for exception message</param>
        /// <returns>Int64 containing the number represented in the string</returns>
        private static long GetInt(string number, string code)
        {
            if (Regex.IsMatch(number, @"^0x[\d|a-f]+$"))
            {
                return Convert.ToInt64(number, 16);
            }

            if (Regex.IsMatch(number, @"^-?\d+$"))
            {
                return Convert.ToInt64(number, 10);
            }

            throw new ArgumentException(
                $"O argumento da linha '{code}' deve ser um número inteiro, decimal somente dígitos ou hexadecimal começando em '0x'.");
        }

        #endregion

        /// <summary>
        ///     Assembles a MIPS code into hex.
        /// </summary>
        /// <param name="input">Array with the lines of code</param>
        /// <param name="dataLabels">A dictionary containing all .data labels with its values and assigned addresses</param>
        /// <returns>Array with the lines of hex representing the input code.</returns>
        public static List<string> Assemble(string[] input, out Dictionary<int, int> dataLabels)
        {
            //Procura por labels primeiro para poder completar (ex.: jal label)
            input = LabelsToDict(input, out Dictionary<string, int> instrLabels, out dataLabels);

            //Procura pelos comandos assembly para converter
            return InstrToHex(input, instrLabels);
        }

        /// <summary>
        ///     Searches for MIPS instructions
        ///     (sll, jr, div, addu, and, nor, slt, j, jal, beq, blez, slti, ori, lui, lw, sw)
        ///     and converts it into hex.
        /// </summary>
        /// <param name="input">Instructions (eg.: lw $t0, 0($t1)</param>
        /// <param name="labels">Mapping of labels addresses</param>
        /// <returns>Array of hex codes corresponding to the instructions entered.</returns>
        /// <exception cref="ArgumentException">A label is not found.</exception>
        private static List<string> InstrToHex(string[] input, Dictionary<string, int> labels)
        {
            var result = new List<string>();
            for (int index = 0; index < input.Length; index++)
            {
                StringBuilder hex = new StringBuilder();

                string[] content = input[index].Split(' ', ',');
                content = content.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                CheckKeys(NameToOp, new[] {content[0]}, "comando", input[index]);

                Instruction instr = NameToOp[content[0]];
                hex.Append(Convert.ToString(instr.OpCode, 2).PadLeft(6, '0')[^6..]);

                try
                {
                    if (instr.OpCode == 0)
                    {
                        long rs = 0, rt = 0, rd = 0, shamt = 0;
                        switch (instr.Funct)
                        {
                            case 0: //sll r,r,i
                                CheckArgs(content, 4, input[index]);
                                CheckKeys(RegToNum, content[1..3], "registrador", input[index]);
                                rt = RegToNum[content[2]];
                                rd = RegToNum[content[1]];
                                shamt = Convert.ToInt64(content[3], 10);
                                if (shamt <= 0 || shamt >= 32)
                                {
                                    throw new ArgumentException(
                                        $"O deslocamento informado na linha '{input[index]}' deve estar entre 0 e 31.");
                                }

                                break;
                            case 8: //jr r
                                CheckArgs(content, 2, input[index]);
                                CheckKeys(RegToNum, content[1..2], "registrador", input[index]);
                                rs = RegToNum[content[1]];
                                break;
                            case 0x1a: //div r,r
                                CheckArgs(content, 3, input[index]);
                                CheckKeys(RegToNum, content[1..3], "registrador", input[index]);
                                rs = RegToNum[content[1]];
                                rt = RegToNum[content[2]];
                                break;
                            case 0x21: //addu r,r,r
                            case 0x24: //and r,r,r
                            case 0x27: //nor r,r,r
                            case 0x2a: //slt r,r,r
                                CheckArgs(content, 4, input[index]);
                                CheckKeys(RegToNum, content[1..4], "registrador", input[index]);
                                rs = RegToNum[content[2]];
                                rt = RegToNum[content[3]];
                                rd = RegToNum[content[1]];
                                break;
                        }

                        hex.Append(Convert.ToString(rs, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(rt, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(rd, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(shamt, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(instr.Funct, 2).PadLeft(6, '0')[^6..]);
                    }
                    else if (instr.OpCode == 2 || instr.OpCode == 3) //j label OR jal label 
                    {
                        CheckArgs(content, 2, input[index]);
                        hex.Append(Convert.ToString(labels[content[1]], 2).PadLeft(32, '0')[4..^2]);
                    }
                    else
                    {
                        long rs = 0, rt = 0, imm = 0;
                        switch (instr.OpCode)
                        {
                            case 4: //beq r,r,l
                                CheckArgs(content, 4, input[index]);
                                CheckKeys(RegToNum, content[1..3], "registrador", input[index]);
                                rs = RegToNum[content[1]];
                                rt = RegToNum[content[2]];
                                imm = (labels[content[3]] - (StartInstrAddress + 4 * index + 4)) / 4;
                                break;
                            case 6: //blez r,l
                                CheckArgs(content, 3, input[index]);
                                CheckKeys(RegToNum, content[1..2], "registrador", input[index]);
                                rs = RegToNum[content[1]];
                                imm = (labels[content[2]] - (StartInstrAddress + 4 * index + 4)) / 4;
                                break;
                            case 0xa: //slti r,r,i
                            case 0xd: //ori r,r,i
                                CheckArgs(content, 4, input[index]);
                                CheckKeys(RegToNum, content[1..3], "registrador", input[index]);
                                rs = RegToNum[content[2]];
                                rt = RegToNum[content[1]];
                                imm = GetInt(content[3], input[index]);
                                break;
                            case 0xf: //lui r,i
                                CheckArgs(content, 3, input[index]);
                                CheckKeys(RegToNum, content[1..2], "registrador", input[index]);
                                rt = RegToNum[content[1]];
                                imm = GetInt(content[2], input[index]);
                                break;
                            case 0x23: //lw r,o(r)
                            case 0x2b: //sw r,o(r)
                                CheckArgs(content, 3, input[index]);
                                Match searchOffReg = Regex.Match(content[2], @"([^()]+)\(([^()]+)\)");
                                if (!searchOffReg.Success) { continue; }

                                string off = searchOffReg.Groups[1].Value, reg = searchOffReg.Groups[2].Value;
                                CheckKeys(RegToNum, new[] {content[1], reg}, "registrador", input[index]);
                                rs = RegToNum[reg];
                                rt = RegToNum[content[1]];
                                imm = GetInt(off, input[index]);
                                break;
                        }

                        hex.Append(Convert.ToString(rs, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(rt, 2).PadLeft(5, '0')[^5..]);
                        hex.Append(Convert.ToString(imm, 2).PadLeft(16, '0')[^16..]);
                    }
                }
                catch (KeyNotFoundException _)
                {
                    throw new ArgumentException($"The label in line '{input[index]}' was not found.");
                }

                result.Add($"0x{Convert.ToInt64(hex.ToString(), 2):x8}");
            }

            return result;
        }

        /// <summary>
        ///     Reads the labels in the code and stores them in a dictionary
        /// </summary>
        /// <param name="input">Code</param>
        /// <param name="instrLabels">Dictionary to store the labels to instruction memory address</param>
        /// <param name="dataLabels">Dictionary to store the labels to data memory address</param>
        /// <returns></returns>
        private static string[] LabelsToDict(string[] input, out Dictionary<string, int> instrLabels,
            out Dictionary<int, int> dataLabels)
        {
            instrLabels = new Dictionary<string, int>();
            dataLabels = new Dictionary<int, int>();

            input = input.Where(x => !string.IsNullOrWhiteSpace(x) && Regex.IsMatch(x, @"^\s*(\.(text|data))|\w*"))
                .ToArray();

            bool isInstr = true;
            int currentInstrAddress = 0;
            int currentDataAddress = 0;

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Trim();

                //If .data is found, switch label saving and address to data memory
                Match categorySearch = Regex.Match(input[i], @"^\s*\.(text|data)");
                if (categorySearch.Success)
                {
                    isInstr = categorySearch.Groups[1].Value switch
                    {
                        "text" => true,
                        "data" => false,
                        _ => isInstr
                    };
                }


                if (isInstr)
                {
                    Match labelSearch = Regex.Match(input[i], @"^([\w|\d]+):");
                    if (labelSearch.Success)
                    {
                        instrLabels[labelSearch.Groups[1].Value] = StartInstrAddress + 4 * currentInstrAddress;
                        input[i] = input[i][(labelSearch.Index + labelSearch.Length)..].Trim();
                        if (input[i].Length <= 0) { continue; }
                    }

                    currentInstrAddress++;
                }
                else
                {
                    Match labelSearch = Regex.Match(input[i], @"^\s*[\w|\d]+:\s+\.word\s+(\d+)");
                    if (labelSearch.Success)
                    {
                        dataLabels[StartMemoryAddress + 4 * currentDataAddress] =
                            Convert.ToInt32(labelSearch.Groups[1].Value);
                        currentDataAddress++;
                        input[i] = "";
                    }
                }
            }

            input = input.Where(x => !string.IsNullOrWhiteSpace(x) && !Regex.IsMatch(x, @"^\s*\.\w*")).ToArray();

            return input;
        }

        #endregion
    }
}