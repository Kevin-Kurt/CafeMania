using keener.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.XWPF.Extractor;
using PTI.Data;
using PTI.Models;
using PTI.Services;
using PTI.Utils;
using PTI.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PTI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProductController
  {
    private readonly ProductService _service; private readonly AppDbContext db;


    public ProductController(ProductService service, AppDbContext context)
    {
      _service = service;
      db = context;
    }


    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(Product product)
    {
      return new ResponseHelper().CreateResponse(await _service.AddAsync(product));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
      return new ResponseHelper().CreateResponse(await _service.GetProductAsync(id));
    }

    [HttpGet]
    [Route("list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetList([FromQuery] PagerModel pager)
    {
      return new ResponseHelper().CreateResponse(await _service.GetListAsync(pager));
    }
  }
}
