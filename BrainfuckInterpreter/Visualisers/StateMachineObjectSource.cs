using Microsoft.VisualStudio.DebuggerVisualizers;
using System.IO;
using System.Text;

namespace BrainfuckInterpreter.Visualisers
{
    public class StateMachineObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, System.IO.Stream outgoingData)
        {
            var stateMachine = target as StateMachine;
            var streamWriter = new StreamWriter(outgoingData);
            streamWriter.Write((char)stateMachine.MemoryAddress);
            streamWriter.Write(ASCIIEncoding.ASCII.GetString(stateMachine.Memory));
            streamWriter.Flush();
        }
    }
}
