using Microsoft.EntityFrameworkCore;

using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;
using System.Reflection;

namespace Model.Context.Contexts
{
    public class SaalDigitalContext : DbContext
    {


        public SaalDigitalContext(DbContextOptions<SaalDigitalContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region Auth
       
        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<Session> Sessions { get; set; }

        #endregion

        #region Dbo

        public DbSet<User> User { get; set; }
        #endregion
    }
}
