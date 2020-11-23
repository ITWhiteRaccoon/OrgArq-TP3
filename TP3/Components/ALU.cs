using System.ComponentModel;

namespace TP3.Components
{
    public class ALU
    {
        /// <summary>
        /// Performs the informed ALU operation.
        /// </summary>
        /// <param name="aluOp">The ALU operation</param>
        /// <param name="alu1">First ALU operand</param>
        /// <param name="alu2">Second ALU operand</param>
        /// <param name="zero">Outputs true if result is 0 and false if not</param>
        /// <param name="aluResult">Outputs the result of the aluOp operation with the two operands</param>
        public void Start(AluOp aluOp, int alu1, int alu2, out bool zero, out int aluResult)
        {
            aluResult = aluOp switch
            {
                AluOp.add => alu1 + alu2,
                AluOp.and => alu1 & alu2,
                AluOp.or => alu1 | alu2,
                AluOp.slt => alu1 < alu2 ? 1 : 0,
                AluOp.sub => alu1 - alu2,
                _ => throw new InvalidEnumArgumentException()
            };

            zero = aluResult == 0;
        }
    }
}