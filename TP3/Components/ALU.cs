using System.ComponentModel;

namespace TP3.Components
{
    public class ALU
    {
        public void Start(AluOp aluOp, int alu1, int alu2, out int zero, out int aluResult)
        {
            switch (aluOp)
            {
                case AluOp.add:
                    aluResult = alu1 + alu2;
                    break;
                case AluOp.and:
                    aluResult = alu1 & alu2;
                    break;
                case AluOp.or:
                    aluResult = alu1 | alu2;
                    break;
                case AluOp.slt:
                    aluResult = alu1 < alu2 ? 1 : 0;
                    break;
                case AluOp.sub:
                    aluResult = alu1 - alu2;
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            zero = aluResult == 0 ? 1 : 0;
        }
    }
}