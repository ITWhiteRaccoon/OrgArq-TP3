using System;
using System.Collections.Generic;
using TP3.Components;

namespace TP3
{
    public class Processor
    {
        private readonly ALU _alu;
        private readonly Control _control;
        private readonly Dictionary<int, string> _binInstr;
        private DataMem _dataMem;
        private int _pc;
        private readonly int _pcLimit;
        private readonly Registers _regs;

        public Processor(List<string> instructions)
        {
            _alu = new ALU();
            _binInstr = new Dictionary<int, string>();
            _control = new Control();
            _dataMem = new DataMem();
            _pc = 0x400_000;
            _pcLimit = _pc + (instructions.Count - 1) * 4;
            _regs = new Registers();
            for (int i = 0; i < instructions.Count; i++)
            {
                _binInstr[_pc + i * 4] = Convert.ToString(Convert.ToInt32(instructions[i], 16), 2).PadLeft(32, '0');
            }
        }

        /// <summary>
        ///     Simulates a MIPS processor
        ///     Assumes PC starting at
        /// </summary>
        public void Start()
        {
            while (_pc <= _pcLimit)
            {
                int opcode = Convert.ToInt32(_binInstr[_pc][..6], 2);
                _control.SetSignals(opcode);
                int nextPc = _pc + 4;
                switch (opcode)
                {
                    case 0: //R
                        R(_binInstr[_pc]);
                        break;
                    case 2: //j
                        J(_binInstr[_pc]);
                        break;
                    case 4: //beq
                        B(_binInstr[_pc]);
                        break;
                    case 35: //lw
                        Lw(_binInstr[_pc]);
                        break;
                    case 43: //sw
                        Sw(_binInstr[_pc]);
                        break;
                    default:
                        I(_binInstr[_pc]);
                        break;
                }

                _pc = nextPc;
                Console.WriteLine($"{_binInstr[_pc]}\t control={_control}\t registers={_regs}\t dataMemory={_dataMem}");
            }
        }

        private void B(string instruction)
        {
            AluOp aluOp = AluOp.Sub;
            //TODO
        }

        private void I(string instruction)
        {
            int rs = Convert.ToInt32(instruction[6..11], 2);
            int rt = Convert.ToInt32(instruction[11..16], 2);
            int imm = Convert.ToInt32(instruction[16..], 2);
            _regs.Start(_control.RegWrite, rs, null, rt, null);
            imm = SignExtend(imm);
            _alu.Start(_regs.ReadData1);
            //TODO
        }

        private void J(string instruction)
        {
            int address = Convert.ToInt32($"{instruction[6..]}00", 2);
            //TODO
        }

        private void Lw(string instruction)
        {
            //TODO (LW stores the loaded content in rt, not rd)
        }

        private void R(string instruction)
        {
            int rs = Convert.ToInt32(instruction[6..11], 2);
            int rt = Convert.ToInt32(instruction[11..16], 2);
            int rd = Convert.ToInt32(instruction[16..21], 2);
            int funct = Convert.ToInt32(instruction[26..], 2);
            int shamt = Convert.ToInt32(instruction[21..26], 2);
            AluOp? aluOp = funct switch
            {
                0b000_000 => AluOp.Sll,
                0b100_000 => AluOp.Add,
                0b100_010 => AluOp.Sub,
                0b100_100 => AluOp.And,
                0b100_101 => AluOp.Or,
                0b101_010 => AluOp.Slt,
                _ => null
            };
            switch (funct)
            {
                case 0: //sll
                {
                    _regs.Start(_control.RegWrite, rt, null, null, null);
                    _alu.Start(aluOp.Value, _regs.ReadData1, shamt);
                    _regs.Start(_control.RegWrite, rt, null, rd, _alu.AluResult);
                    break;
                }
                default:
                {
                    _regs.Start(_control.RegWrite, rs, rt, null, null);
                    _alu.Start(aluOp.Value, _regs.ReadData1, _regs.ReadData2);
                    _regs.Start(_control.RegWrite, rs, rt, rd, _alu.AluResult);
                    break;
                }
            }
        }

        private void Sw(string instruction)
        {
            //TODO
        }

        private static int SignExtend(int number)
        {
            string num = Convert.ToString(number, 2);
            num = num.PadLeft(32, num[0]);
            return Convert.ToInt32(num, 2);
        }
    }
}