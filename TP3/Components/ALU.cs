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
        /// <param name="aluOp">The ALU operation</param>
        /// <param name="alu1">First ALU operand</param>
        /// <param name="alu2">Second ALU operand</param>
        public void Start(AluOp aluOp, int alu1, int alu2)
        {
            AluResult = aluOp switch
            {
                AluOp.Add => alu1 + alu2,
                AluOp.And => alu1 & alu2,
                AluOp.Or => alu1 | alu2,
                AluOp.Sll => alu1 << alu2,
                AluOp.Slt => alu1 < alu2 ? 1 : 0,
                AluOp.Sub => alu1 - alu2,
                _ => throw new InvalidEnumArgumentException()
            };

            Zero = AluResult == 0;
        }
    }
}