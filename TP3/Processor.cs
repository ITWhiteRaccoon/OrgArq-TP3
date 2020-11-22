using System;
using System.Collections.Generic;

namespace TP3
{
    public class Processor
    {
        private int _pc = 0;

        public void Start(List<string> instructions)
        {
            while (_pc <= instructions.Count - 1)
            {
                string hexInstr = instructions[_pc]; //Instruction memory

                string binInstr = Convert.ToString(Convert.ToInt32(hexInstr, 16), 2).PadLeft(32, '0');
                int opcode = Convert.ToInt32(binInstr[..7], 2);
                if (opcode == 0)
                {
                    int rs, rt, rd, shamt;
                    int funct = Convert.ToInt32(binInstr[^6..]);
                }
                else
                {
                }
            }
        }

        //private void R(string instruction,out int zero, out int alu)
        //{

        //}
    }
}