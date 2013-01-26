using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;

namespace BrainfuckInterpreter.Visualisers
{
    public class StateMachineVisualiser : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var streamReader = new StreamReader(objectProvider.GetData());
            var stateMachineSource = streamReader.ReadToEnd();

            var pointer = (int)stateMachineSource[0];

            var form = new StateMachineVisualiserForm(pointer, stateMachineSource.Substring(1));
            windowService.ShowDialog(form);
        }
    }
}
