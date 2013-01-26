using System;
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
                var columnSize = TextRenderer.MeasureText("    ", this.Font);
                var columnWidth = columnSize.Width - 8; //TODO: Find out why I have to munge the return value

                var labelSize = TextRenderer.MeasureText("Ascii: ", this.Font);
                var labelWidth = labelSize.Width - 8; //TODO: Find out why I have to munge the return value

                var startX = columnWidth * (float)_stateMachineViewModel.CurrentPosition + labelWidth;
                graphics.FillRectangle(Brushes.Red, startX, 0, columnWidth, columnSize.Height);

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
                SlotLabels = "Slot:  ";
                SlotValues = "Value: ";
                SlotAsciiValues = "Ascii: ";

                CurrentPosition = memoryLocation;

                var lastNon0Slot = GetLastNonZeroMemoryIndex(memory);
                var displayEnd = Math.Max(lastNon0Slot, memoryLocation);

                var slotsToDisplay = memory.Substring(0, displayEnd + 1);

                for (var memSlot = 0; memSlot < slotsToDisplay.Length; ++memSlot)
                {
                    SlotLabels += memSlot.ToString().PadRight(4);
                    SlotValues += ((int)slotsToDisplay[memSlot]).ToString().PadRight(4);
                    SlotAsciiValues += (char.IsControl(slotsToDisplay[memSlot]) ? ' ' : slotsToDisplay[memSlot]).ToString().PadRight(4);
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
