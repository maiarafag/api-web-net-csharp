using Instituicao.Models;
using Microsoft.EntityFrameworkCore;

namespace Instituicao.Context
{
    public class InstituicaoContext : DbContext
    {
        public InstituicaoContext(DbContextOptions<InstituicaoContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Turma> Turma { get; set; }


        //adicionais
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turma>().ToTable("Turma");

            modelBuilder.Entity<Aluno>().ToTable("Aluno");

            modelBuilder.Entity<Aluno>()
                .HasOne(e => e.Turma)
                .WithMany(e => e.Alunos)
                .HasForeignKey(e => e.Turma_Id);
        }
    }
}
