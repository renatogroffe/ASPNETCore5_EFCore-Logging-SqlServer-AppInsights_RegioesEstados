using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using APIRegioes.Data;

namespace APIRegioes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegioesController : ControllerBase
    {
        private readonly ILogger<RegioesController> _logger;
        private readonly RegioesContext _context;

        public RegioesController(ILogger<RegioesController> logger,
            RegioesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Regiao> GetRegioes()
        {
            var dados = _context.Regioes
                .Include(r => r.Estados).ToArray();

            _logger.LogInformation(
                $"{nameof(GetRegioes)}: {dados.Length} registro(s) encontrado(s)");

            return dados;
        }

        [HttpGet("{codRegiao}/Estados")]
        [ProducesResponseType(typeof(Regiao), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult<Regiao> GetEstadosPorRegiao(string codRegiao)
        {
            var dados = _context.Regioes
                .Where(r => r.CodRegiao == codRegiao)
                .Include(r => r.Estados)
                .SingleOrDefault();
            _logger.LogInformation(
                $"{nameof(GetEstadosPorRegiao)}: {dados?.Estados?.Count() ?? 0} registro(s) encontrado(s)");
 
            if (dados is null)
                return NotFound();
            
            return dados;
        }
    }
}