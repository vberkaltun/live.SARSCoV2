using System.Threading.Tasks;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.SqlAdapter
{
    class InheritSqlAdapter: SqlAdapter
    {
        public override async Task ConnectAsync()
        {
            // print message
            Extension.PrintMessage(ClassName, JobType.Succesfull);

            // call base
            await base.ConnectAsync();
        }
        public override async Task DisconnectAsync()
        {
            // print message
            Extension.PrintMessage(ClassName, JobType.Error);

            // call base
            await base.DisconnectAsync();
        }
    }
}
