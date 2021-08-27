using System;
using System.Collections.Generic;

namespace YeetOverFlow.Logging
{
    public class YeetSinkActionProvider
    {
        List<Action<YeetSinkEvent>> _actions = new List<Action<YeetSinkEvent>>();

        public void AddAction(Action<YeetSinkEvent> action)
        {
            _actions.Add(action);
        }

        public IEnumerable<Action<YeetSinkEvent>> GetActions()
        {
            return _actions;
        }
    }
}
