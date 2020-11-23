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
            _pc = 0x400_000;
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
                int nextPc = _pc + 4;
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