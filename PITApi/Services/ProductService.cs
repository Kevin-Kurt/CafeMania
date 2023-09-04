using keener.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using PTI.Data;
using PTI.Models;
using PTI.Utils;
using PTI.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Sentry;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Operapp.Infra.Helpers;
using PTI.Migrations;

namespace PTI.Services
{
  public class ProductService
  {
    public IConfiguration configuration { get; }
    private readonly AppDbContext db;



    public ProductService(IConfiguration Configuration, AppDbContext context)
    {
      configuration = Configuration;
      db = context;
    }

    public async Task<ResponseModel> AddAsync(Product product)
    {
      db.Products.Add(product);
      await db.SaveChangesAsync();

      return ResponseModel.BuildResponse("Produto adicionado com sucesso!");
    }
    public async Task<ResponseModel> GetProductAsync(Guid id)
    {
      var product = await db.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
      return ResponseModel.BuildOkResponse(product);
    }

    public async Task<ResponseModel> GetListAsync(PagerModel pager)
    {
      var products = db.Products.AsNoTracking();

      if (pager.Order == "Preço")
      {
        products = products.OrderBy(x => x.Price);
      }
      else if (pager.Order == "Tipo")
      {
        products = products.OrderBy(x => x.Type);
      }

      if (pager.Filter != null)
      {
        products = products.Where(x => x.Type == pager.Filter);
      }
      
      PaginatedObject result = await products.ReturnPaginated(pager.Page, pager.Pagesize);

      return ResponseModel.BuildOkResponse(result);
    }
  }
}
