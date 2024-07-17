using AutoMapper;
using ChatAppBAL;
using ChatAppBAL.Interfaces;
using ChatAppBAL.Services;
using ChatAppDAL;
using ChatAppDAL.Interfaces;
using ChatAppDAL.Repositories;
using ChatAppPL.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(CreateMapperProfile());
builder.Services.AddDbContext<ChatAppDBContext>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMessagingService, MessagingService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

app.MapHub<ChatHub>("/chat");

app.Run();



IMapper CreateMapperProfile()
{
    var myProfile = new AutomapperProfile();
    var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

    return new Mapper(configuration);
}