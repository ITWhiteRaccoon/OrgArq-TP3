using System;
using System.Collections.Generic;
using TP3.Components;

namespace TP3
{
    public class Processor
    {
        private readonly Alu _alu;
        private readonly Dictionary<int, string> _binInstr;
        private readonly Control _control;
        private readonly DataMem _dataMem;
        private readonly int _pcLimit;
        private readonly Registers _regs;
        private int _pc;

        public Processor(List<string> instructions)
        {
            _alu = new Alu();
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
        ///     Simulates the MIPS blocks and signals
        /// </summary>
        public void Simulate()
        {
            while (_pc <= _pcLimit)
            {
                string instruction = _binInstr[_pc];

                //Divide the instruction properly into all the necessary parts
                int opcode = Convert.ToInt32(instruction[..6], 2);
                int rs = Convert.ToInt32(instruction[6..11], 2);
                int rt = Convert.ToInt32(instruction[11..16], 2);
                int rd = Convert.ToInt32(instruction[16..21], 2);
                int imm = ImmSignExtend(Convert.ToInt32(instruction[16..], 2));
                int funct = Convert.ToInt32(instruction[26..], 2);
                int shamt = Convert.ToInt32(instruction[21..26], 2);

                //Set controller signals
                _control.SetSignals(opcode, funct);

                //Start register-ALU-memory-register sending all the control signals
                _regs.Start(false, rs, rt, null, null);
                _alu.Start(_control.AluControlInput, _regs.ReadData1, _control.AluSrc ? imm : _regs.ReadData2, shamt);
                _dataMem.Start(_control.MemWrite, _control.MemRead, _alu.AluResult, _regs.ReadData2);
                _regs.Start(_control.RegWrite, rs, rt, _control.RegDst ? rd : rt,
                    _control.MemToReg ? _dataMem.ReadData : _alu.AluResult);

                //Calculating all the PC signal alternatives
                int pcAdd = _pc + 4;
                int address = Convert.ToInt32(
                    $"{Convert.ToString(pcAdd, 2).PadLeft(32, '0')[..4]}{instruction[6..]}00", 2);
                int pcBranch = pcAdd + (imm << 2);
                int nextPc = _control.Jump ? address :
                    _control.Branch && _alu.Zero ? pcBranch : pcAdd;

                Console.WriteLine($"\t{_binInstr[_pc]} pc=0x{_pc:x} opcode=0x{opcode:x} rs=0x{rs:x} rt=0x{rt:x} " +
                                  $"rd=0x{rd:x} imm=0x{imm:x} shamt=0x{shamt:x} funct=0x{funct:x}\n" +
                                  $"\tcontrol=\t{_control}\n" +
                                  $"\tregisters=\t{_regs}\n" +
                                  $"\tdataMemory=\t{_dataMem}\n");
                _pc = nextPc;
            }
        }

        /// <summary>
        ///     Pads number with 0s to make sure it's at least 16 bits, then sign extend it to 32 bits.
        /// </summary>
        /// <param name="number">The number to be sign extended</param>
        /// <returns>Sign extended int</returns>
        private static int ImmSignExtend(int number)
        {
            string num = Convert.ToString(number, 2);
            num = num.PadLeft(16, '0');
            num = num.PadLeft(32, num[0]);
            return Convert.ToInt32(num, 2);
        }
    }
}