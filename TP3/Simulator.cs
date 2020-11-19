using System;

namespace TP3
{
    public class Simulator
    {
        public static void Start(string[] instructions)
        {
            Processor processor = new Processor();
            foreach (string hexInstr in instructions)
            {
                string binInstr = Convert.ToString(Convert.ToInt32(hexInstr, 16), 2).PadLeft(32, '0');
            }
        }
    }
}