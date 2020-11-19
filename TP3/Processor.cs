using System;
using System.Collections.Generic;

namespace TP3
{
    public class Processor
    {
        private int[] _registers = new int[32];
        private int _pc = 0;

        public void Start(List<string> instructions)
        {
            while (_pc <= instructions.Count - 1)
            {
                string hexInstr = instructions[index];
                string binInstr = Convert.ToString(Convert.ToInt32(hexInstr, 16), 2).PadLeft(32, '0');
                int opcode = Convert.ToInt32(binInstr[..7], 2);
                if (opcode == 0)
                {
                    int rs , rt , rd , shamt ;
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

        private void Registers(bool regWrite, int readReg1, int readReg2, int writeReg, int writeData,
            out int readData1, out int readData2)
        {
            if (readReg1 < 0 || readReg1 > 31 || readReg2 < 0 || readReg2 > 31 || writeReg < 0 || writeReg > 31)
            {
                throw new ArgumentOutOfRangeException($"readReg1, readReg2 and writeReg must be between 0 and 31");
            }

            if (regWrite)
            {
                _registers[writeReg] = writeData;
            }

            readData1 = _registers[readReg1];
            readData2 = _registers[readReg2];
        }
    }
}