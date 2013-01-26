using BrainfuckInterpreter.Visualisers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BrainfuckInterpreter
{
    [DebuggerVisualizer(typeof(StateMachineVisualiser), typeof(StateMachineObjectSource), Description = "Memory Visualiser")]
    public class StateMachine
    {
        public int InstructionAddress { get; set; }
        public byte[] Memory { get; set; }
        public int MemoryAddress { get; set; }
        public Stack<int> PreviousLoopStarts = new Stack<int>();
        private Dictionary<OpCode, Action> actions = new Dictionary<OpCode, Action>();
        private Runner _runner;

        public StateMachine(Runner runner)
        {
            _runner = runner;
            actions.Add(OpCode.BeginLoop, BeginLoop);
            actions.Add(OpCode.DecrementPointer, DecrementPointer);
            actions.Add(OpCode.DecrementValue, DecrementValue);
            actions.Add(OpCode.EndLoop, EndLoop);
            actions.Add(OpCode.IncrementPointer, IncrementPointer);
            actions.Add(OpCode.IncrementValue, IncrementValue);
            actions.Add(OpCode.InputValue, InputValue);
            actions.Add(OpCode.OutputValue, OutputValue);
        }

        public void ResetStateMachine()
        {
            MemoryAddress = 0;
            Memory = new byte[64 * 1024];
            PreviousLoopStarts.Clear();
            InstructionAddress = 0;
        }

        public void OutputValue()
        {
            Console.Write((char)Memory[MemoryAddress]);
            IncrementInstructionAddress();
        }

        public void InputValue()
        {
            var key = Console.ReadKey(true).KeyChar;
            Memory[MemoryAddress] = (byte)key;
            IncrementInstructionAddress();
        }

        public void ExecuteOpcode(OpCode opcode)
        {
            actions[opcode].Invoke();
        }

        public void IncrementInstructionAddress()
        {
            InstructionAddress++;
        }

        public void IncrementPointer()
        {
            if (MemoryAddress != Memory.Length - 1)
            {
                MemoryAddress++;
            }
            IncrementInstructionAddress();
        }

        public void DecrementPointer()
        {
            if (MemoryAddress != 0)
            {
                MemoryAddress--;
            }
            IncrementInstructionAddress();
        }

        public void IncrementValue()
        {
            if (Memory[MemoryAddress] != 255)
            {
                Memory[MemoryAddress]++;
            }
            else
            {
                Memory[MemoryAddress] = 0;
            }
            IncrementInstructionAddress();
        }

        public void DecrementValue()
        {
            if (Memory[MemoryAddress] != 0)
            {
                Memory[MemoryAddress]--;
            }
            else
            {
                Memory[MemoryAddress] = 255;
            }
            IncrementInstructionAddress();
        }

        public void BeginLoop()
        {
            if (Memory[MemoryAddress] != 0)
            {
                PreviousLoopStarts.Push(InstructionAddress + 1);
                IncrementInstructionAddress();
            }
            else
            {
                int endLoopValue = _runner.SeekEndLoop(InstructionAddress);
                if (endLoopValue == -1)
                {
                    Program.ErrorOut("Parse error");
                }
                else
                {
                    InstructionAddress = endLoopValue + 1;
                }
            }
        }

        public void EndLoop()
        {
            if (Memory[MemoryAddress] != 0)
            {
                if (PreviousLoopStarts.Count > 0)
                {
                    InstructionAddress = PreviousLoopStarts.Peek();
                }
                else
                {
                    Program.ErrorOut("Parse error");
                }
            }
            else
            {
                PreviousLoopStarts.Pop();
                IncrementInstructionAddress();
            }
        }
    }
}
