using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortin.Common.Dtos
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int currentPage, int pageSize, int totalCount)
        {
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages 
        { 
            get
            {
                if (PageSize == 0) return 0;
                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new(items, pageNumber, pageSize, count);
        }
    }
}
