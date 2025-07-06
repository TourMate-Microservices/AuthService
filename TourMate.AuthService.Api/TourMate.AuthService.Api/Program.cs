using NotificationService.Grpc;
using UserService.Grpc;
using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Services.Services;
using TourMate.AuthService.Repositories.IRepositories;
using TourMate.AuthService.Repositories.Repositories;
using TourMate.AuthService.Repositories.Context;
using TourMate.AuthService.Api.GrpcServices;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Config Database
builder.Services.AddDbContext<TourMateAuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Config gRPC client
builder.Services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcServices:User"] ?? "https://localhost:7001");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

builder.Services.AddGrpcClient<NotificationGrpc.NotificationGrpcClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcServices:Notification"] ?? "https://localhost:7002");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Register services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<TokenService>();

// Register gRPC service wrappers
builder.Services.AddScoped<IUserGrpcService, UserGrpcService>();
builder.Services.AddScoped<INotificationGrpcService, NotificationGrpcService>();

// Register repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Controllers, Swagger...
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
