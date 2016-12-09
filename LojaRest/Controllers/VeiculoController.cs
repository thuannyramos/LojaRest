using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LojaRest.Controllers
{
    public class VeiculoController : ApiController
    {
        // GET api/fabricante
        public IEnumerable<Models.Veiculo> Get()
        {
            Models.LojaDataContext dc = new Models.LojaDataContext();
            var r = from f in dc.Veiculos select f;
            return r.ToList();
        }

    [Route("API/VeicPost")]
        [HttpPost]
        public void Post([FromBody] string value)
        {
            List<Models.Veiculo> x = JsonConvert.DeserializeObject<List<Models.Veiculo>>(value);
            Models.LojaDataContext dc = new Models.LojaDataContext();
            dc.Veiculos.InsertAllOnSubmit(x);
            dc.SubmitChanges();
        }

        // PUT 
        public void Put(int id, [FromBody] string value)
        {
            Models.Veiculo x = JsonConvert.DeserializeObject<Models.Veiculo>(value);
            Models.LojaDataContext dc = new Models.LojaDataContext();
            Models.Veiculo fab = (from f in dc.Veiculos
                                     where f.Id == id
                                     select f).Single();
            fab.Modelo = x.Modelo;
            fab.Ano = x.Ano;
            fab.Preco = x.Preco;
            fab.Modelo = x.Modelo;
            fab.SituVenda = x.SituVenda;
            fab.IdFabricante = x.IdFabricante;
            dc.SubmitChanges();
        }

        // DELETE 

        public void Delete(int id)
        {
            Models.LojaDataContext dc = new Models.LojaDataContext();
            Models.Veiculo fab = (from f in dc.Veiculos where f.Id == id select f).Single();
            dc.Veiculos.DeleteOnSubmit(fab);
            dc.SubmitChanges();
        }
    }
}

