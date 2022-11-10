using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly MyAppContext _context;

        public ProdutosController(MyAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Produto>> GetProdutos()
        {
            var a = _context.Produtos.ToList();
            return Ok(a);
        }

        [HttpPut("{id}")]
        public ActionResult<List<Produto>> PutProdutos([FromRoute] int id, [FromBody] Produto prod)
        {

            if (id != prod.Id)
            {
                return BadRequest();
            }

            _context.Entry(prod).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(prod);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> PostProdutos(Produto prod)
        {
            try
            {
                prod.Status = "Ativo";
                _context.Produtos.Add(prod);
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                throw;
            }
            return Ok(prod);
        }

        [HttpDelete("{id}")]
        public ActionResult<List<Produto>> DeleteProdutos([FromRoute] int id)
        {
            var produtos = _context.Produtos.Where(i => i.Id == id).FirstOrDefault();
            if (produtos == null)
            {
                return NotFound();
            }
            try
            {

                _context.Produtos.Remove(produtos);
                _context.SaveChanges();
            }
            catch (Exception err)
            {
                throw;
            }
            return Ok(produtos);
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
