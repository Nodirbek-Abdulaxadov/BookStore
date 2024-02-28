using Ocelot.Middleware;
using Ocelot.DependencyInjection;

new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("ocelot.json")
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(s => {
                s.AddOcelot();
                s.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                                               builder =>
                                               {
                            builder.AllowAnyOrigin()
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                });
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                //add your logging
            })
            .UseIISIntegration()
            .Configure(app =>
            {
                app.UseCors("AllowAll");
                app.UseOcelot().Wait();
            })
            .Build()
            .Run();