using AuthSystem.App.Models.Amigo;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.App.Data
{
    public class AmigoDbContext : DbContext
    {
        private readonly string conn = "workstation id=MVCAuthDB.mssql.somee.com;packet size=4096;user id=kynusasan_SQLLogin_2;pwd=idpmz8jksu;data source=MVCAuthDB.mssql.somee.com;persist security info=False;initial catalog=MVCAuthDB";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(conn);

        public DbSet<Amigo> Amigos { get; set; }
    }
}
