using Microsoft.EntityFrameworkCore;
using PortalDeProdutosClientes.Domain.Entidades;
using PortalDeProdutosClientes.Domain.Entities;
using PortalDeProdutosClientes.Dominain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PortalDeProdutosClientes.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }

        public DbSet<LogAuditoria> LogAuditorias { get; set; }

        public DbSet<LogAuditoria> LogAuditoriass { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica automaticamente todas as classes que implementam IEntityTypeConfiguration neste projeto
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted))
                .ToList();

            foreach (var entry in entries)
            {
                var auditLog = new AuditLog
                {
                    Tabela = entry.Entity.GetType().Name,
                    Acao = entry.State.ToString(),
                    Usuario = "Sistema",
                    DataHora = DateTime.UtcNow,
                    ValoresNovos = System.Text.Json.JsonSerializer.Serialize(
                        entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p])
                    )
                };

                await AuditLogs.AddAsync(auditLog, cancellationToken);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
