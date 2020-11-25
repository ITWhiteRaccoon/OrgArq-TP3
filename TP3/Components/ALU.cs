using System.ComponentModel;

namespace TP3.Components
{
    public class ALU
    {
        public int AluResult { get; private set; }
        public bool Zero { get; private set; }

        /// <summary>
        /// Performs the informed ALU operation.
        /// </summary>
        /// <param name="aluControl">The ALU operation</param>
        /// <param name="alu1">First ALU operand</param>
        /// <param name="alu2">Second ALU operand</param>
        public void Start(AluControl aluControl, int alu1, int alu2)
        {
            AluResult = aluControl switch
            {
                AluControl.Add => alu1 + alu2,
                AluControl.And => alu1 & alu2,
                AluControl.Or => alu1 | alu2,
                AluControl.Sll => alu1 << alu2,
                AluControl.Slt => alu1 < alu2 ? 1 : 0,
                AluControl.Sub => alu1 - alu2,
                _ => throw new InvalidEnumArgumentException()
            };

            Zero = AluResult == 0;
        }
    }
}