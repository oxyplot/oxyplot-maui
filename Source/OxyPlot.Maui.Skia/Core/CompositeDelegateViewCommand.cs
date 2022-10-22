namespace OxyPlot.Maui.Skia.Core
{
    /// <summary>
    /// Combine multiple commands to one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompositeDelegateViewCommand<T> : IViewCommand<T>
        where T : OxyInputEventArgs
    {
        private readonly IViewCommand<T>[] commands;
        public CompositeDelegateViewCommand(params IViewCommand<T>[] commands)
        {
            this.commands = commands;
        }

        public void Execute(IView view, IController controller, T args)
        {
            foreach (var cmd in commands)
            {
                cmd.Execute(view, controller, args);
                if (args.Handled)
                    break;
            }
        }

        public void Execute(IView view, IController controller, OxyInputEventArgs args)
        {
            foreach (var cmd in commands)
            {
                cmd.Execute(view, controller, args);
                if (args.Handled)
                    break;
            }
        }
    }
}