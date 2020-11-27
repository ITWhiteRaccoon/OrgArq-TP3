using System.Collections.Generic;
using System.Linq;

namespace TP3.Components
{
    /// <summary>
    ///     Eduardo C. Andrade - 17111012-5
    ///     Michael L. S. Rosa - 17204042-0
    ///     Org. Arq. I - 2020/2 - TP3
    /// </summary>
    public class DataMem
    {
        private readonly Dictionary<int, int> _memory;

        public DataMem()
        {
            _memory = new Dictionary<int, int>();
        }

        public int ReadData { get; private set; }

        /// <summary>
        ///     Starts Data Memory block operation
        /// </summary>
        /// <param name="memWrite">Indicates the block will read from memory</param>
        /// <param name="memRead">Indicates the block will write in memory</param>
        /// <param name="address">Memory location to be written or read</param>
        /// <param name="writeData">Data to be written in memory</param>
        public void Start(bool memWrite, bool memRead, int address, int writeData)
        {
            if (memWrite && !memRead)
            {
                _memory[address] = writeData;
            }
            else if (memRead && !memWrite)
            {
                _memory.TryGetValue(address, out int r);
                ReadData = r;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", _memory.Select(x => $"0x{x.Key:x}=0x{x.Value:x}"))}]";
        }
    }
}