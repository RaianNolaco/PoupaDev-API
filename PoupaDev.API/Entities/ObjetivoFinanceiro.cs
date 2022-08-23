using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoupaDev.API.Enums;
using PoupaDev.API.Exceptions;

namespace PoupaDev.API.Entities
{
    public class ObjetivoFinanceiro
   { 
       private const decimal RENDIMENTO = 0.01m; 

       public int Id { get; private set; }
       public string? Titulo { get; private set; }
       public string? Descricao { get; private set; }
       public decimal? ValorObjetivo { get; private set; }
       public DateTime DataCriacao { get; private set; }
       public List<Operacao> Operacoes { get; private set; }
       public decimal Saldo => ObterSaldo(); 

       public ObjetivoFinanceiro(string? titulo, string? descricao, decimal? valorObjetivo)
       {
           Titulo = titulo;
           Descricao = descricao;
           ValorObjetivo = valorObjetivo;
           DataCriacao = DateTime.Now;
           Operacoes = new List<Operacao>();
       }
       
       public decimal ObterSaldo(){
            var totalDeposito = Operacoes.Where(x => x.Tipo == TipoOperacao.Deposito).Sum(x => x.Valor);
            var totalSaque = Operacoes.Where(y => y.Tipo == TipoOperacao.Saque).Sum(y => y.Valor);

            return totalDeposito - totalSaque;
       }

       public void RealizarOperacao(Operacao operacao){
            if(operacao.Tipo == TipoOperacao.Saque && Saldo < operacao.Valor)
                throw new SaldoInsuficienteException();

                Operacoes.Add(operacao);         
       }

       public void Render(){
            var valorRendimento = Saldo * RENDIMENTO;
            Console.WriteLine($"Saldo{Saldo}, Redimento{valorRendimento}");

            var Deposito = new Operacao(valorRendimento, TipoOperacao.Deposito,Id);

            Operacoes.Add(Deposito);
       } 
   }
}