using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickShop.WebApp.Model;

namespace QuickShop.WebApp.Controllers
{
    [ApiController]
    [Route("graphql")]
    [Authorize]
    public class GraphQLController : Controller
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;

        public GraphQLController(ISchema schema, IDocumentExecuter executer)
        {
            _schema = schema;
            _executer = executer;
        }

        // POST /graphql
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQueryModel query)
        {
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = query.Query;
                _.Inputs = query.Variables?.ToInputs();

            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result.Errors.First().Message);
            }
            
            return Ok(new
            {
                data = result.Data
            });
        }
    }
}