using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoupaDev.API.Entities;
using PoupaDev.API.Models;
using PoupaDev.API.Persistence;

namespace PoupaDev.API.Controllers
{
    [ApiController]
    [Route("api/objetivos-finaceiros")]
    public class ObjetivosFinanceirosController : ControllerBase
    {
        private readonly PoupaDevContext _poupaDevContext;
        public ObjetivosFinanceirosController(PoupaDevContext context)
        {
            _poupaDevContext = context;
        }

        //GET api/api/objetivos-financeiros
        [HttpGet]
        public IActionResult GetTodos(){
            var objetivo = _poupaDevContext.Objetivos;
            return Ok(objetivo);
        }

        //GET api/api/objetivos-financeiros/1
        [HttpGet("{id}")]
        public IActionResult GetPorId(int id){
           //Se não achar , retornar NotFund();
            var objetivo = _poupaDevContext
            .Objetivos
            .Include(o => o.Operacoes)
            .SingleOrDefault(x => x.Id == id);
           
            if(objetivo == null){
                 return NotFound();   
            }

            return Ok(objetivo);
        }

        //POST api/objetivos-financeiros
        [HttpPost]
        public IActionResult Post(ObjetivoFinanceiroInputModel model){
            //Se dados forem invalidos Retornar BadRequest();
            var objetivo = new ObjetivoFinanceiro(
                model.Titulo,
                model.Descricao,
                model.ValorObjetivo);

            _poupaDevContext.Objetivos.Add(objetivo);
            _poupaDevContext.SaveChanges();

            var id = objetivo.Id;
            return CreatedAtAction(
                "GetPorId",
                new {id = id},
                model
            );
        }

        //GET api/api/objetivos-financeiros/1
        [HttpPost("{id}/operacoes")]
        public IActionResult PostOperacoe(int id, OperacaoInputModel model){
            var operacao =new Operacao(model.Valor,model.Tipo, id);

            var objetivo = _poupaDevContext
                .Objetivos
                .Include(x => x.Operacoes)
                .SingleOrDefault(x => x.Id == id);

            if(objetivo == null)
                return NotFound();
            
            objetivo.RealizarOperacao(operacao);
            _poupaDevContext.SaveChanges();
            return NoContent();
        }
    }
}