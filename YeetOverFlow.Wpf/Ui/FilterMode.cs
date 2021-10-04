using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeetOverFlow.Wpf.Ui
{
    public enum FilterMode
    {
        CONTAINS,
        EQUALS,
        STARTS_WITH,
        ENDS_WITH,
        BETWEEN_INCLUSIVE,
        BETWEEN_EXCLUSIVE,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL_TO,
        LESS_THAN_OR_EQUAL_TO,
        REGULAR_EXPRESSSION
    }
}
