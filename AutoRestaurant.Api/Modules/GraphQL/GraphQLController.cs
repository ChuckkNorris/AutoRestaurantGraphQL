using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRestaurant.Api.Modules.GraphQL {
    [Route("[controller]")]
    public class GraphQLController : Controller {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter) {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query) {
            var executionOptions = new ExecutionOptions {
                Schema = _schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs()
            };
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
