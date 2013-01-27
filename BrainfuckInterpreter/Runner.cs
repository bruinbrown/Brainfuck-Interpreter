using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainfuckInterpreter
{
    public class Runner
    {
        public string Program { get; set; }
        StateMachine sm;
        private bool programLoaded = false;

        public Runner()
        {
            sm = new StateMachine(this);
        }

        public Runner(string program)
            : this()
        {
            LoadNewProgram(program);
        }

        private string ParseProgram(string program)
        {
            StringBuilder sb = new StringBuilder();
            HashSet<char> validChars = new HashSet<char>();
            validChars.Add('>');
            validChars.Add('<');
            validChars.Add('.');
            validChars.Add(',');
            validChars.Add('+');
            validChars.Add('-');
            validChars.Add('[');
            validChars.Add(']');
            Stack<char> brackets = new Stack<char>();
            foreach (char c in program)
            {
                if (validChars.Contains(c))
                {
                    sb.Append(c);
                    if (c == '[')
                    {
                        brackets.Push(c);
                    }
                    if (c == ']')
                    {
                        if (brackets.Count > 0)
                        {
                            brackets.Pop();
                        }
                        else
                        {
                            BrainfuckInterpreter.Program.ErrorOut("Invalid bracket syntax. You have too many [ or too few ].");
                        }
                    }
                }
            }
            if (brackets.Count != 0)
            {
                BrainfuckInterpreter.Program.ErrorOut("Invalid bracket syntax. You have too many [ or too few ]");
            }
            return sb.ToString();
        }

        public void LoadAndRun(string program)
        {
            LoadNewProgram(program);
            Run();
        }

        public void LoadNewProgram(string program)
        {
            programLoaded = false;
            Program = ParseProgram(program);
            programLoaded = true;
        }

        public void Run()
        {
            if (programLoaded)
            {
                sm.ResetStateMachine();
                while (sm.InstructionAddress != Program.Length)
                {
                    sm.ExecuteOpcode((OpCode)Program[sm.InstructionAddress]);
                }
            }
            else
            {
                Console.WriteLine("No program has yet been loaded");
            }
        }

        private int SeekChar(char c)
        {
            for (int i = sm.InstructionAddress; i < Program.Length; i++)
            {
                if (i == c)
                {
                    return i;
                }
            }
            return -1;
        }

        public int SeekEndLoop(int startPointer)
        {
            Stack<bool> nestedLoops = new Stack<bool>();
            for (int i = startPointer+1; i < Program.Length; i++)
            {
                if (Program[i] == '[')
                {
                    nestedLoops.Push(true);
                }
                else if (Program[i] == ']')
                {
                    if (nestedLoops.Count == 0)
                    {
                        return i;
                    }
                    else
                    {
                        nestedLoops.Pop();
                    }
                }
            }
            return -1;
        }

        public int SeekBeginLoop()
        {
            return SeekChar('[');
        }
    }
}
