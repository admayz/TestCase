using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestCase.Models;

namespace TestCase.Utlis
{
    public static class Consts
    {
        public static ConnSettings ConnOptions { get; set; }

        public static SysSettings SysSettings { get; set; }
    }

    public interface IConstsBuilder
    {
        void Initialize();
    }

    public class ConstsBuilder : IConstsBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public ConstsBuilder(IOptions<ConnSettings> connOp, IOptions<SysSettings> sysOp, IServiceProvider serviceProvider)
        {
            Consts.ConnOptions = connOp.Value;
            Consts.SysSettings = sysOp.Value;
            _serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var conn = scope.ServiceProvider.GetRequiredService<AppData>();
                if (conn.Database.GetPendingMigrations().Any())
                    conn.Database.Migrate();
            }
        }
    }
}