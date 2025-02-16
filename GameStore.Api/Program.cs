using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//Connection to database
builder.Services.AddSqlite<GameStoreContext>(builder.Configuration.GetConnectionString("GameStore"));

var app = builder.Build();

//Adding endpoints
app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();