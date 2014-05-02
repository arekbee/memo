namespace memo.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class bazaEntities : DbContext
    {
        public bazaEntities()
            : base("name=bazaEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<rola> rola { get; set; }
        public virtual DbSet<slowko> slowko { get; set; }
        public virtual DbSet<statystykaUzytkownika> statystykaUzytkownika { get; set; }
        public virtual DbSet<ustawieniaZagadki> ustawieniaZagadki { get; set; }
        public virtual DbSet<uzytkownik> uzytkownik { get; set; }
    }
}