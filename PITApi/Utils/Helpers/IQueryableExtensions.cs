using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Operapp.Infra.Helpers
{
    public static class IQueryableExtensions
    {
        public async static Task<PaginatedObject> ReturnPaginated<T>(this IQueryable<T> items, int? currentPage = 1, int pageSize = 10)
        {
            currentPage = currentPage <= 0 ? 1 : currentPage;
            pageSize = pageSize <= 0 ? 1 : pageSize;
            Pager pager = new Pager(items.Count(), currentPage, pageSize);

            return new PaginatedObject(await items.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync(), pager);
        }
    }
  public class PaginatedObject
  {
    public object Entity { get; set; }
    public Pager Pager { get; set; }

    public PaginatedObject(object entity, Pager pager)
    {
      Entity = entity;
      Pager = pager;
    }
  }
  public class Pager
  {
    public int TotalItems { get; private set; }

    public int CurrentPage { get; private set; }

    public int PageSize { get; private set; }

    public int TotalPages { get; private set; }

    public int StartPage { get; private set; }

    public int EndPage { get; private set; }

    public Pager(int totalItems, int? page, int pageSize)
    {
      int num = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
      int num2 = ((!page.HasValue) ? 1 : page.Value);
      int num3 = num2 - 5;
      int num4 = num2 + 4;
      if (num3 <= 0)
      {
        num4 -= num3 - 1;
        num3 = 1;
      }

      if (num4 > num)
      {
        num4 = num;
        if (num4 > 10)
        {
          num3 = num4 - 9;
        }
      }

      TotalItems = totalItems;
      CurrentPage = num2;
      PageSize = pageSize;
      TotalPages = num;
      StartPage = num3;
      EndPage = num4;
    }
  }
}
