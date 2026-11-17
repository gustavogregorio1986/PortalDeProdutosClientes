using Microsoft.EntityFrameworkCore;
using PortalDeProdutosClientes.Application.Interfaces;
using PortalDeProdutosClientes.Application.Services;
using PortalDeProdutosClientes.Data.Data;
using PortalDeProdutosClientes.Domain.Interfaces;
using PortalDeProdutosClientes.Dominain.Repository.Interface;
using PortalDeProdutosClientes.Dominain.Services;
using PortalDeProdutosClientes.Infrastructure.Repositories;
using PortalDeProdutosClientes.Infrastructure.Services;
using PortalDeProdutosClientes.Repository;
using PortalDeProdutosClientes.Repository.Repositorio;
using PortalDeProdutosClientes.Service.Services;
using PortalDeProdutosClientes.Services;
using QuestPDF.Infrastructure;


namespace PortalDeProdutosClientesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
            builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            builder.Services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();
            builder.Services.AddScoped<IAuditRepository, AuditRepository>();

            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IPagamentoService, PagamentoService>();
            builder.Services.AddScoped<ILogAuditoriaService, LogAuditoriaService>();
            builder.Services.AddScoped<IAuditService, AuditService>();

            // Logo no topo do Program.cs
            QuestPDF.Settings.License = LicenseType.Community;

            // Junto com os outros serviços (builder.Services...)
            builder.Services.AddScoped<IRelatorioPdfService, RelatorioPdfService>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "PortalCache_";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
