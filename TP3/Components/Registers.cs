using System;

namespace TP3.Components
{
    public class Registers
    {
        private int[] _registers = new int[32];

        private void Start(bool regWrite, int readReg1, int readReg2, int writeReg, int writeData,
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