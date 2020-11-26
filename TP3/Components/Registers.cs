using System;
using System.Linq;

namespace TP3.Components
{
    public class Registers
    {
        private readonly int[] _registers;

        public Registers()
        {
            _registers = new int[32];
            _registers[28] = 0x1000_8000; //$gp - global pointer
            _registers[29] = 0x7fff_fffc; //$sp - stack pointer
        }

        public int ReadData1 { get; private set; }
        public int ReadData2 { get; private set; }

        /// <summary>
        ///     Reads and/or writes data to the chosen registers.
        /// </summary>
        /// <param name="write">If true, writes writeData to writeReg </param>
        /// <param name="readReg1">First register number</param>
        /// <param name="readReg2">Second register number</param>
        /// <param name="writeReg">Register to be written or null</param>
        /// <param name="writeData">Data to write in writeReg or null</param>
        /// <exception cref="IndexOutOfRangeException">If the informed registers are out of the 0-31 range</exception>
        public void Start(bool write, int readReg1, int readReg2, int? writeReg, int? writeData)
        {
            if (readReg1 < 0 || readReg1 > 31 ||
                readReg2 < 0 || readReg2 > 31 ||
                writeReg < 0 || writeReg > 31)
            {
                throw new IndexOutOfRangeException("The informed registers must be between 0 and 31");
            }

            ReadData1 = _registers[readReg1];
            ReadData2 = _registers[readReg2];


            if (write && writeReg != null && writeData != null)
            {
                _registers[writeReg.Value] = writeData.Value;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", _registers.Select((d, i) => $"{i}=0x{d:x}"))}]";
        }
    }
}