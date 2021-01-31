using System.Diagnostics.CodeAnalysis;
using GraphQL.Types;
using QuickShop.Domain.Ping.Model;

namespace QuickShop.WebApp.GraphQL.Ping
{
    public class PingCheckType : ObjectGraphType<PingCheck>
    {
        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public PingCheckType()
        {
            Name = nameof(PingCheck);

            Field(_ => _.DatabasePing, true).Description("Ping to the database in milliseconds, null if not reachable");
            Field(_ => _.DatabaseStatus).Description("Status of the database, true means reachable, true means unreachable");
            Field(_ => _.ServerTime).Description("Server time");
        }
    }
}
