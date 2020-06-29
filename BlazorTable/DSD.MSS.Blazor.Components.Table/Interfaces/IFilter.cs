using System;
using System.Linq.Expressions;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// Filter Component Interface
    /// </summary>
    /// <typeparam name="TableItem"></typeparam>
    public interface IFilter<TableItem>
    {
        /// <summary>
        /// Get Filter Expression
        /// </summary>
        /// <returns></returns>
        Expression<Func<TableItem, bool>> GetFilter();
    }
}
