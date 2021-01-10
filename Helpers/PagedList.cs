using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Helpers
{
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int number, int size)
        {
            TotalCount = count;
            PageSize = size;
            CurrentPage = number;
            TotalPages = (int)Math.Ceiling(count/(double)size);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int number, int size)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((number - 1) * size).Take(size).ToListAsync();
            return new PagedList<T>(items, count, number, size);
        }
    }
}
