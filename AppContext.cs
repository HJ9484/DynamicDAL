using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL
{
    public class AppContext : System.Data.Entity.DbContext
    {
        

        public AppContext() : base("DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<AppContext>());

            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<AppContext, Migrations.Configuration>());

        }

        public AppContext(System.Data.Common.DbConnection connection) : base(connection, false)
        {
            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<AppContext, Migrations.Configuration>());
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<AppContext>());
        }



        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
          
        }
    }
}
