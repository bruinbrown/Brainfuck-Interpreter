﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrainfuckInterpreter.Visualisers
{
    public partial class StateMachineVisualiserForm : Form
    {
        private StateMachineViewModel _stateMachineViewModel;

        public StateMachineVisualiserForm()
        {
            InitializeComponent();
        }

        public StateMachineVisualiserForm(int memoryLocation, string memory)
            : this()
        {
            _stateMachineViewModel = new StateMachineViewModel(memoryLocation, memory);
            this.Paint += FormPaint;
        }

        private void FormPaint(object sender, PaintEventArgs e)
        {
            using (var graphics = this.CreateGraphics())
            {
                // Calculate the width of 4 spaces in the current font; use this to highlight the current pointer
                var stringSize = TextRenderer.MeasureText("    ", this.Font);
                var stringWidth = stringSize.Width - 8; //TODO: Find out why I have to mmunge the return value

                var startX = stringWidth * (float)_stateMachineViewModel.CurrentPosition;
                graphics.FillRectangle(Brushes.Red, startX, 0, stringWidth, stringSize.Height);

                var memoryInfo = string.Format("{1}{0}{2}{0}{3}"
                    , Environment.NewLine
                    , _stateMachineViewModel.SlotLabels
                    , _stateMachineViewModel.SlotValues
                    , _stateMachineViewModel.SlotAsciiValues);

                graphics.DrawString(memoryInfo, this.Font, Brushes.Black, 0, 0);
            }
        }

        private class StateMachineViewModel
        {
            public StateMachineViewModel(int memoryLocation, string memory)
            {
                CurrentPosition = memoryLocation;
                var lastNon0Slot = GetLastNonZeroMemoryIndex(memory);
                var slots = memory.Substring(0, lastNon0Slot + 1);

                for (var memSlot = 0; memSlot < slots.Length; ++memSlot)
                {
                    SlotLabels += memSlot.ToString().PadRight(4);
                    SlotValues += ((int)slots[memSlot]).ToString().PadRight(4);
                    SlotAsciiValues += (char.IsControl(slots[memSlot]) ? ' ' : slots[memSlot]).ToString().PadRight(4);
                }
            }

            private int GetLastNonZeroMemoryIndex(string memory)
            {
                return Array.FindLastIndex(memory.ToArray(), b => b > 0);
            }

            public string SlotLabels { get; private set; }
            public string SlotValues { get; private set; }
            public string SlotAsciiValues { get; private set; }
            public int CurrentPosition { get; private set; }
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
