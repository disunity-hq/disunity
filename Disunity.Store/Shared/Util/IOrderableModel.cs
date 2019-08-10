using System.Collections.Generic;


namespace Disunity.Store.Util
{
    public enum Ordering
    {
        Asc,
        Desc
    }

    public interface IOrderableModel
    {
        string OrderBy { get; }
        Ordering? Order { get; }

        IEnumerable<string> OrderByChoices { get; }

    }
}
