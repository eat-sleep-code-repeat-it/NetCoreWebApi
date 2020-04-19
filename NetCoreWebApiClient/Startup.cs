using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetCoreWebApiClient.Models;
using Polly;
using Polly.Extensions.Http;

namespace NetCoreWebApiClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookServiceClient", Version = "v1" });
            });

            /*
            services.AddHttpClient<IBookService, BookService>();

            services.AddHttpClient<IBookService, BookService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IBookService, BookService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseUrl"]);
            });
            */
            services.AddHttpClient<IBookService, BookService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Define a method for Http Retry Policy with exponential backoff.
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Retry policy to try six times with an exponential retry, starting at two seconds.
            int maxNumOfRetry = 6;  // six times with an exponential retry
            int maxNumOfRetryStartWith = 2; // seconds
            Random jitterer = new Random();

            // a regular Retry policy
            var regulatRetryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    maxNumOfRetry,
                    retryAttempt => {
                        return TimeSpan.FromSeconds(Math.Pow(maxNumOfRetryStartWith, retryAttempt));
                    }
                );
            // A regular Retry policy can impact your system in cases of high concurrency and scalability and under high contention.
            // To overcome peaks of similar retries coming from many clients in case of partial outages,
            // a good workaround is to add a jitter strategy to the retry algorithm/policy.
            // to improve the overall performance of the end-to-end system by adding randomness to the exponential backoff.
            // This spreads out the spikes when issues arise.
            var retryWithJitterPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    maxNumOfRetry,  // exponential back-off plus some jitter
                    retryAttempt => {
                        return TimeSpan.FromSeconds(Math.Pow(maxNumOfRetryStartWith, retryAttempt))
                        + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                    }
                );
            
            return retryWithJitterPolicy;
        }

        /// <summary>
        /// To prevents an application from performing an operation that's likely to fail
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            int numOfConsecutiveFaults = 5;
            int circuitBreakFor = 30;
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(numOfConsecutiveFaults, TimeSpan.FromSeconds(circuitBreakFor));
        }
    }
}
