using BrainfuckInterpreter;
using BrainfuckInterpreter.Visualisers;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace VisualiserDevelopmentHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var stateMachine = new StateMachine(null);
            stateMachine.Memory = new byte[] { 1, 2, 3, 4, 0, 0, 65, 36, 1, 1, 87, 5, 32, 9, 122, 0, 0, 7 };
            stateMachine.MemoryAddress = 17;
            var host = new VisualizerDevelopmentHost(stateMachine, typeof(StateMachineVisualiser), typeof(StateMachineObjectSource));
            host.ShowVisualizer();
        }
    }
}
