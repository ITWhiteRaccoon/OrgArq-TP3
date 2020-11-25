namespace TP3.Components
{
    public class Control
    {
        public ALU.Operation AluControlInput { get; private set; }
        public int AluOp { get; private set; }
        public bool AluSrc { get; private set; }
        public bool Branch { get; private set; }
        public bool Jump { get; private set; }
        public bool MemRead { get; private set; }
        public bool MemToReg { get; private set; }
        public bool MemWrite { get; private set; }
        public bool RegDst { get; private set; }
        public bool RegWrite { get; private set; }

        public void SetSignals(int opcode, int funct)
        {
            switch (opcode)
            {
                case 0: //R
                    RegDst = true;
                    AluSrc = false;
                    MemToReg = false;
                    RegWrite = true;
                    MemRead = false;
                    MemWrite = false;
                    Branch = false;
                    AluOp = 0b10;
                    Jump = false;
                    break;
                case 2: //j
                    RegWrite = false;
                    MemWrite = false;
                    Jump = true;
                    break;
                case 4: //beq
                    AluSrc = false;
                    RegWrite = false;
                    MemRead = false;
                    MemWrite = false;
                    Branch = true;
                    AluOp = 0b01;
                    Jump = false;
                    break;
                case 35: //lw
                    RegDst = false;
                    AluSrc = true;
                    MemToReg = true;
                    RegWrite = true;
                    MemRead = true;
                    MemWrite = false;
                    Branch = false;
                    AluOp = 0b00;
                    Jump = false;
                    break;
                case 43: //sw
                    AluSrc = false;
                    RegWrite = false;
                    MemRead = false;
                    MemWrite = true;
                    Branch = false;
                    AluOp = 0b00;
                    Jump = false;
                    break;
            }

            switch (AluOp)
            {
                case 0b00:
                    AluControlInput = ALU.Operation.Add;
                    break;
                case 0b01:
                    AluControlInput = ALU.Operation.Sub;
                    break;
                case 0b10:
                    AluControlInput = funct switch
                    {
                        0b000_000 => ALU.Operation.Sll,
                        0b100_000 => ALU.Operation.Add,
                        0b100_010 => ALU.Operation.Sub,
                        0b100_100 => ALU.Operation.And,
                        0b100_101 => ALU.Operation.Or,
                        0b101_010 => ALU.Operation.Slt,
                        _ => AluControlInput
                    };
                    break;
            }
        }

        public override string ToString()
        {
            return
                $"AluOp={AluOp}, AluSrc={AluSrc}, Branch={Branch}, Jump={Jump}, MemRead={MemRead}, MemToReg={MemToReg}, MemWrite={MemWrite}, RegDst={RegDst}, RegWrite={RegWrite}";
        }
    }
}