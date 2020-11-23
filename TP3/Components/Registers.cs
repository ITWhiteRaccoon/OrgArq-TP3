using System;

namespace TP3.Components
{
    public class Registers
    {
        private int[] _registers;

        public Registers()
        {
            _registers = new int[32];
            _registers[28] = 0x1000_8000;//$gp - global pointer
            _registers[29] = 0x7fff_fffc;//$sp - stack pointer
        }
        
        /// <summary>
        /// Reads and/or writes data to the chosen registers.
        /// </summary>
        /// <param name="write">If true writes writeData to writeReg </param>
        /// <param name="readReg1">First register number</param>
        /// <param name="readReg2">Second register number or null</param>
        /// <param name="writeReg">Register to be written or null</param>
        /// <param name="writeData">Data to write in writeReg or null</param>
        /// <param name="readData1">Outputs the content of the first chosen register</param>
        /// <param name="readData2">Outputs the content of the second chosen register or null</param>
        /// <exception cref="ArgumentOutOfRangeException">If the informed registers are out of the 0-31 range</exception>
        private void Start(bool write, int readReg1, int? readReg2, int? writeReg, int? writeData,
            out int readData1, out int? readData2)
        {
            if (readReg1 < 0 || readReg1 > 31 ||
                (readReg2 != null && (readReg2 < 0 || readReg2 > 31)) ||
                (writeReg != null && (writeReg < 0 || writeReg > 31)))
            {
                throw new ArgumentOutOfRangeException($"The informed registers must be between 0 and 31");
            }

            if (write && writeReg != null && writeData != null)
            {
                _registers[writeReg.Value] = writeData.Value;
            }

            readData1 = _registers[readReg1];
            readData2 = readReg2 == null ? null : (int?) _registers[readReg2.Value];
        }
    }
}