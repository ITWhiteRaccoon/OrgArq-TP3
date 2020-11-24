﻿using System;
using System.Collections.Generic;

namespace TP3.Components
{
    public class DataMem
    {
        private Dictionary<int, int> _memory;
        public int ReadData { get; private set; }

        public DataMem()
        {
            _memory = new Dictionary<int, int>();
        }

        /// <summary>
        /// Starts Data Memory block operation
        /// </summary>
        /// <param name="memWrite">Indicates the block will read from memory</param>
        /// <param name="memRead">Indicates the block will write in memory</param>
        /// <param name="address">Memory location to be written or read</param>
        /// <param name="writeData">Data to be written in memory</param>
        public void Start(bool memWrite, bool memRead, int address, int? writeData)
        {
            if (memWrite && !memRead)
            {
                if (writeData == null)
                {
                    throw new ArgumentNullException(nameof(writeData),
                        "Can't perform write operation if data is null.");
                }

                _memory[address] = writeData.Value;
            }
            else if (memRead && !memWrite)
            {
                _memory.TryGetValue(address, out int r);
                ReadData = r;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(',', _memory)}]";
        }
    }
}