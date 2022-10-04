using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Data.Models
{

	/// <summary>
	/// extension methods for IList, help insert into an ordered IList
	/// source: https://www.jacksondunstan.com/articles/3189
	/// </summary>
	public static class IListExtension
    {
		/// <summary>
		/// Insert a value into an IList{T} that is presumed to be already sorted such that sort
		/// ordering is preserved
		/// </summary>
		/// <param name="list">List to insert into</param>
		/// <param name="value">Value to insert</param>
		/// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
		public static void InsertIntoSortedList<T>(this IList<T> list, T value)
			where T : IComparable<T>
		{
			InsertIntoSortedList(list, value, (a, b) => a.CompareTo(b));
		}

		/// <summary>
		/// Insert a value into an IList<T> that is presumed to be already sorted such that sort
		/// ordering is preserved
		/// </summary>
		/// <param name="list">List to insert into</param>
		/// <param name="value">Value to insert</param>
		/// <param name="comparison">Comparison to determine sort order with</param>
		/// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
		public static void InsertIntoSortedList<T>(
			this IList<T> list,
			T value,
			Comparison<T> comparison
		)
		{
			var startIndex = 0;
			var endIndex = list.Count;
			while (endIndex > startIndex)
			{
				var windowSize = endIndex - startIndex;
				var middleIndex = startIndex + (windowSize / 2);
				var middleValue = list[middleIndex];
				var compareToResult = comparison(middleValue, value);
				if (compareToResult == 0)
				{
					list.Insert(middleIndex, value);
					return;
				}
				else if (compareToResult < 0)
				{
					startIndex = middleIndex + 1;
				}
				else
				{
					endIndex = middleIndex;
				}
			}
			list.Insert(startIndex, value);
		}
	}
}
