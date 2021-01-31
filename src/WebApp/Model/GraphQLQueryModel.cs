using System.Collections.Generic;

namespace QuickShop.WebApp.Model
{
    public class GraphQLQueryModel
    {
        public string Query { get; set; }
        public Dictionary<string, object> Variables { get; set; }
    }
}