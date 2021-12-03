namespace YeetOverFlow.Wpf.Ui
{
    public static class FilterHelper 
    {
        public static bool Evaluate(string filter, FilterMode filterMode, string targetValue)
        {
            switch (filterMode)
            {
                case FilterMode.CONTAINS:
                    if (!targetValue.Contains(filter, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
                case FilterMode.EQUALS:
                    if (!targetValue.Equals(filter, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
                case FilterMode.STARTS_WITH:
                    if (!targetValue.StartsWith(filter, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
                case FilterMode.ENDS_WITH:
                    if (!targetValue.EndsWith(filter, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
