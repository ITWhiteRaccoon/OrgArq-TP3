namespace TP3.Components
{
    public class Control
    {
        public ALU.Operation AluControlInput { get; private set; }
        private int AluOp { get; set; }
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
                    AluOp = 0b010;
                    AluSrc = false;
                    Branch = false;
                    Jump = false;
                    MemRead = false;
                    MemToReg = false;
                    MemWrite = false;
                    RegDst = true;
                    RegWrite = true;
                    break;
                case 2: //j
                    Jump = true;
                    MemWrite = false;
                    RegWrite = false;
                    break;
                case 4: //beq
                    AluOp = 0b001;
                    AluSrc = false;
                    Branch = true;
                    Jump = false;
                    MemRead = false;
                    MemWrite = false;
                    RegWrite = false;
                    break;
                case 13: //ori
                    AluOp = 0b100;
                    AluSrc = true;
                    Branch = false;
                    Jump = false;
                    MemRead = false;
                    MemToReg = false;
                    MemWrite = false;
                    RegDst = false;
                    RegWrite = true;
                    break;
                case 15: //lui
                    AluOp = 0b011;
                    AluSrc = true;
                    Branch = false;
                    Jump = false;
                    MemRead = false;
                    MemToReg = false;
                    MemWrite = false;
                    RegDst = false;
                    RegWrite = true;
                    break;
                case 35: //lw
                    AluOp = 0b000;
                    AluSrc = true;
                    Branch = false;
                    Jump = false;
                    MemRead = true;
                    MemToReg = true;
                    MemWrite = false;
                    RegDst = false;
                    RegWrite = true;
                    break;
                case 43: //sw
                    AluOp = 0b000;
                    AluSrc = false;
                    Branch = false;
                    Jump = false;
                    MemRead = false;
                    MemWrite = true;
                    RegWrite = false;
                    break;
            }

            AluControlInput = AluOp switch
            {
                0b000 => //lw, sw
                    ALU.Operation.Add,
                0b001 => //beq
                    ALU.Operation.Sub,
                0b010 => //R-type
                    funct switch
                    {
                        0b000_000 => //sll
                            ALU.Operation.Sll,
                        0b100_001 => //addu
                            ALU.Operation.Add,
                        0b100_010 => //sub
                            ALU.Operation.Sub,
                        0b100_100 => //and
                            ALU.Operation.And,
                        0b100_101 => //or
                            ALU.Operation.Or,
                        0b101_010 => //slt
                            ALU.Operation.Slt,
                        _ => AluControlInput
                    },
                0b011 => //lui
                    ALU.Operation.Lui,
                0b100 => //ori
                    ALU.Operation.Or,
                _ => AluControlInput
            };
        }

        public override string ToString()
        {
            return
                $"AluOp={AluOp}, AluSrc={AluSrc}, Branch={Branch}, Jump={Jump}, MemRead={MemRead}, MemToReg={MemToReg}, MemWrite={MemWrite}, RegDst={RegDst}, RegWrite={RegWrite}";
        }
    }
}