using YeetOverFlow.Core.Application.Data.Commands;

namespace YeetOverFlow.Core.Application.Commands
{
    public abstract class YeetCommand : Command
    {
        //public Guid LibraryGuid { get; }  //don't see a need yet
        public abstract YeetCommandKind Kind { get; }
        public bool DeferCommit { get; set; }
    }
}
