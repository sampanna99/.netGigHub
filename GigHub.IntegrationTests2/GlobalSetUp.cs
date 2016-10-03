using NUnit.Framework;
using System.Data.Entity.Migrations;

namespace GigHub.IntegrationTests2
{
    [SetUpFixture]
    public class GlobalSetUp
    {
        public void SetUp()
        {
            var configuration = new GigHub.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}
