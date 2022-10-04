using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data.Interfaces;
using WebUI.ViewModels;

namespace WebUI.Data.Models
{
    /// <summary>
    /// defines extension methods for IQueryable<T>
    /// </summary>
    public static class IQueryableExtension
    {
        /// <summary>
        /// gets the first element of q or a new item if there are no elements
        /// </summary>
        public static T FirstOrNew<T>(this IQueryable<T> q) where T : new()
        {
            return q.FirstOrDefault() ?? new T();
        }

        /// <summary>
        /// asynchronously gets the first element of q or a new item if there are no elements
        /// </summary>
        public async static Task<T> FirstOrNewAsync<T>(this IQueryable<T> q) where T : new()
        {
            return await q.FirstOrDefaultAsync() ?? new T();
        }

        /// <summary>
        /// projects all the elements of a sequence into SelectOptions
        /// </summary>
        /// <typeparam name="T">The enumerated entity in the queryable</typeparam>
        /// <typeparam name="TKey">The key type used by the type T's concrete implementation of ISelectOption</typeparam>
        public static IQueryable<SelectOption<TKey>> AsSelectOptions<T, TKey>(this IQueryable<T> q) where T : ISelectOption<TKey>
        {
            // specifically get formatted select options when applicable
            if (typeof(IFormattedSelectOption<TKey>).IsAssignableFrom(typeof(T)))
            {
                return q.Select(x => (x as IFormattedSelectOption<TKey>).ToFormattedSelectOption());
            }
            else
            {
                return q.Select(x => x.ToSelectOption());
            }
        }
    }
}
