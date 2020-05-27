using System;
using System.IO;
using AutoMapper;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PaymentGatewayService.AcquiringBank;
using PaymentGatewayService.Common.Security;
using PaymentGatewayService.Payments;
using PaymentGatewayService.Payments.Commands;
using PaymentGatewayService.Payments.Validators;

namespace PaymentGatewayService.Api
{
    public class Startup
    {
        public Startup()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var contentRoot = Path.GetDirectoryName(path);

            var config = new ConfigurationBuilder();
            config.AddJsonFile(contentRoot + "/appsettings.json", optional: false);
            config.AddJsonFile(contentRoot + "/appsettings.development.json", optional: true);

            config.AddEnvironmentVariables();
            Configuration = config.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var dbConnectionString = Configuration.GetSection("PaymentGatewayDatabase:ConnectionString").Value;
            var dbName = Configuration.GetSection("PaymentGatewayDatabase:DatabaseName").Value;

            var client = new MongoClient(dbConnectionString);

            services.AddSingleton<IMongoClient>(client);
            services.AddSingleton(client.GetDatabase(dbName));
            services.AddSingleton<IPaymentRepository, PaymentRepository>();
            services.AddSingleton<IPaymentService, PaymentService>();

            var bankServiceParameters = new AcquiringBankServiceParameters()
            {
                BankTotalBalance = int.Parse(Configuration["AcquiringBankParameters:BankTotalBalance"]),
                TransactionMinTimeMs = int.Parse(Configuration["AcquiringBankParameters:TransactionMinTimeMs"]),
                TransactionMaxTimeMs = int.Parse(Configuration["AcquiringBankParameters:TransactionMaxTimeMs"]),
                TransactionTimeoutTimeMs = int.Parse(Configuration["AcquiringBankParameters:TransactionTimeoutTimeMs"])
            };

            services.AddSingleton(bankServiceParameters);
            services.AddSingleton<IAcquiringBankService, AcquiringBankService>();

            services.AddAutoMapper(
                c=>c.AddProfile<AutoMapperApiProfile>(),
                typeof(Startup));

            services.AddSingleton(new EncryptorParameters
            {
                Iv = Convert.FromBase64String(Configuration["Encryption:Iv"]),
                Key = Convert.FromBase64String(Configuration["Encryption:Key"]),
            });

            services.AddSingleton<IEncryptor, Encryptor>();

            services.AddLogging();

            services.AddControllers().AddFluentValidation();

            services.AddTransient<IValidator<CreatePaymentCommand>, CreatePaymentCommandValidator>();
            services.AddTransient<IValidator<GetPaymentCommand>, GetPaymentCommandValidator>();
            services.AddTransient<IValidator<SetPaymentStatusAcceptedCommand>, SetPaymentStatusAcceptedCommandValidator>();
            services.AddTransient<IValidator<SetPaymentStatusRejectedCommand>, SetPaymentStatusRejectedCommandValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}