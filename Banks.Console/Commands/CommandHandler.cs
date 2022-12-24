namespace Banks.Console.Commands
{
    public abstract class CommandHandler
    {
        private CommandHandler? _handler;

        public virtual CommandHandler SetNext(CommandHandler handler)
        {
            _handler = handler;
            return handler;
        }

        public virtual void Handle(string command)
        {
            if (_handler is not null)
                _handler.Handle(command);
            else throw new InvalidOperationException("Unable to handler provided request.");
        }
    }
}