namespace TP3.Components
{
    public class Control
    {
        public int AluOp { get; private set; }
        public bool AluSrc { get; private set; }
        public bool Branch { get; private set; }
        public bool Jump { get; private set; }
        public bool MemRead { get; private set; }
        public bool MemToReg { get; private set; }
        public bool MemWrite { get; private set; }
        public bool RegDst { get; private set; }
        public bool RegWrite { get; private set; }

        public void SetSignals(int opcode)
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
        }

        public override string ToString()
        {
            return
                $"AluOp={AluOp}, AluSrc={AluSrc}, Branch={Branch}, Jump={Jump}, MemRead={MemRead}, MemToReg={MemToReg}, MemWrite={MemWrite}, RegDst={RegDst}, RegWrite={RegWrite}";
        }
    }
}