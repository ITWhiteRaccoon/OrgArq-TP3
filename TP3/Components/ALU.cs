using System.ComponentModel;

namespace TP3.Components
{
    public class ALU
    {
        public enum Operation
        {
            Add,
            And,
            Lui,
            Or,
            Sll,
            Slt,
            Sub
        }

        public int AluResult { get; private set; }
        public bool Zero { get; private set; }

        /// <summary>
        ///     Performs the informed ALU operation.
        /// </summary>
        /// <param name="aluControl">The ALU operation</param>
        /// <param name="alu1">First ALU operand</param>
        /// <param name="alu2">Second ALU operand</param>
        /// <param name="shamt">Shift amount</param>
        public void Start(Operation aluControl, int alu1, int alu2, int shamt)
        {
            AluResult = aluControl switch
            {
                Operation.Add => alu1 + alu2,
                Operation.And => alu1 & alu2,
                Operation.Lui => alu2 << 16,
                Operation.Or => alu1 | alu2,
                Operation.Sll => alu2 << shamt,
                Operation.Slt => alu1 < alu2 ? 1 : 0,
                Operation.Sub => alu1 - alu2,
                _ => throw new InvalidEnumArgumentException()
            };

            Zero = AluResult == 0;
        }
    }
}