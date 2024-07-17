using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ChatAppBAL;
using ChatAppBAL.Interfaces;
using ChatAppBAL.Services;
using ChatAppDAL;
using ChatAppDAL.Interfaces;
using ChatAppDAL.Repositories;
using ChatAppPL.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAppTests.Common
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        
        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(options => { options.EnableDetailedErrors = true;});
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton(CreateMapperProfile());
            services.AddDbContext<ChatAppDBContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IChatService, ChatService>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            ApplicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app,
                              IApplicationLifetime applicationLifetime)
        {

            app.UseWebSockets();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });

            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

        IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}