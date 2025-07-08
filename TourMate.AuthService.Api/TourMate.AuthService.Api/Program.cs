using TourMate.AuthService.Services.IServices;
using TourMate.AuthService.Services.Services;
using TourMate.AuthService.Repositories.IRepositories;
using TourMate.AuthService.Repositories.Repositories;
using TourMate.AuthService.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TourMate.AuthService.Services.Utilities;
using TourMate.AuthService.Services.GrpcServices;
using TourMate.AuthService.Services.IGrpcServices;
using TourMate.AuthService.Api.Protos; // Nếu chưa dùng


var builder = WebApplication.CreateBuilder(args);

// Config Database
builder.Services.AddDbContext<TourMateAuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký gRPC client cho AuthService
builder.Services.AddGrpcClient<NotificationService.NotificationServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:Notification"]);
});

// Đăng ký gRPC client cho PublicService
builder.Services.AddGrpcClient<UserService.UserServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:User"]);
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
