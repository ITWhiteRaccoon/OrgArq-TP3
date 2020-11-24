namespace TP3.Components
{
    public class Control
    {
        public int AluOp { get; private set; }
        public int AluSrc { get; private set; }
        public int Branch { get; private set; }
        public int Jump { get; private set; }
        public int MemRead { get; private set; }
        public int MemToReg { get; private set; }
        public int MemWrite { get; private set; }
        public int RegDst { get; private set; }
        public int RegWrite { get; private set; }

        public void SetSignals(int opcode)
        {
            switch (opcode)
            {
                case 0: //R
                    RegDst = 1;
                    AluSrc = 0;
                    MemToReg = 0;
                    RegWrite = 1;
                    MemRead = 0;
                    MemWrite = 0;
                    Branch = 0;
                    AluOp = 0b10;
                    Jump = 0;
                    break;
                case 2: //j
                    RegWrite = 0;
                    MemWrite = 0;
                    Jump = 1;
                    break;
                case 4: //beq
                    AluSrc = 0;
                    RegWrite = 0;
                    MemRead = 0;
                    MemWrite = 0;
                    Branch = 1;
                    AluOp = 0b01;
                    Jump = 0;
                    break;
                case 35: //lw
                    RegDst = 0;
                    AluSrc = 1;
                    MemToReg = 1;
                    RegWrite = 1;
                    MemRead = 1;
                    MemWrite = 0;
                    Branch = 0;
                    AluOp = 0b00;
                    Jump = 0;
                    break;
                case 43: //sw
                    AluSrc = 0;
                    RegWrite = 0;
                    MemRead = 0;
                    MemWrite = 1;
                    Branch = 0;
                    AluOp = 0b00;
                    Jump = 0;
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