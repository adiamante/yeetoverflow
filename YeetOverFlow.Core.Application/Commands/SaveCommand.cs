namespace YeetOverFlow.Core.Application.Commands
{
    public class SaveCommand<TChild> : YeetCommand
    {
        public override YeetCommandKind Kind => YeetCommandKind.Save;
        public SaveCommand()
        {
        }
    }
}
