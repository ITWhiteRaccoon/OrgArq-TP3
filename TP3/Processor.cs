using System;
using System.Collections.Generic;
using TP3.Components;

namespace TP3
{
    public class Processor
    {
        private int _pc;
        private Registers _regs;
        private ALU _alu;

        public Processor()
        {
            _pc = 0;
            _regs = new Registers();
            _alu = new ALU();
        }

        /// <summary>
        /// Simulates a MIPS processor
        /// Assumes PC starting at 
        /// </summary>
        /// <param name="instructions">List of hex assembly instructions</param>
        public void Start(List<string> instructions)
        {
            while (_pc <= instructions.Count - 1)
            {
                string hexInstr = instructions[_pc]; //Instruction memory

                string binInstr = Convert.ToString(Convert.ToInt32(hexInstr, 16), 2).PadLeft(32, '0');
                int opcode = Convert.ToInt32(binInstr[..7], 2);
                int nextPc = _pc++;
                switch (opcode)
                {
                    case 0:
                        R(binInstr);
                        break;
                    case 2:
                    case 3:
                        J(binInstr);
                        break;
                    default:
                        I(binInstr);
                        break;
                }
            }
        }

        private void R(string instruction)
        {
            int rs = Convert.ToInt32(instruction[6..11], 2);
            int rt = Convert.ToInt32(instruction[11..16], 2);
            int rd = Convert.ToInt32(instruction[16..21], 2);
            int shamt = Convert.ToInt32(instruction[21..26], 2);
            int funct = Convert.ToInt32(instruction[26..], 2);
            AluOp aluOp = funct switch
            {
                0b100_000 => AluOp.add,
                0b100_010 => AluOp.sub,
                0b100_100 => AluOp.and,
                0b100_101 => AluOp.or,
                0b101_010 => AluOp.slt,
                _ => throw new ArgumentException(
                    $"The informed funct in instruction opcode {0}, funct {funct} is invalid.")
            };

            _regs.Start(false, rs, rt, rd, null, out int r1, out int? r2);
            _alu.Start(aluOp, r1, r2.GetValueOrDefault(), out bool zero, out int result);
            _regs.Start(true, rs, rt, rd, result, out _, out _);
            Console.WriteLine($"{aluOp.ToString()}");
        }

        private void I(string instruction)
        {
            int rs = Convert.ToInt32(instruction[6..11], 2);
            int rt = Convert.ToInt32(instruction[11..16], 2);
            int imm = Convert.ToInt32(instruction[16..], 2);
        }

        private void J(string instruction)
        {
            int address = Convert.ToInt32($"{instruction[6..]}00", 2);
        }
    }
}