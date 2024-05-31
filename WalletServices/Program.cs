using Microsoft.OpenApi.Models;
using WalletServices.DAL.Interfaces;
using WalletServices.DAL;
using WalletServices.DTO.Wallet;
using WalletServices.DTO.Transfer;
using WalletServices.Models;
using BCryptHash = BCrypt.Net.BCrypt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWallet, WalletDapper>();
builder.Services.AddScoped<ITransfer, TransferDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/customerwallets", (IWallet wallet) =>
{
    try
    {
        List<WalletReadDTO> walletDTO = new List<WalletReadDTO>();
        var walletFromDb = wallet.GetAll();
        foreach (var w in walletFromDb)
        {
            walletDTO.Add(new WalletReadDTO
            {
                WalletId = w.WalletId,
                Username = w.Username,
                Email = w.Email,
                FullName = w.FullName,
                Password = w.Password,
                Balance = w.Balance
            });
        }
        return Results.Ok(walletDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);

    }
});

app.MapGet("/api/customerwallets/{id}", (IWallet wallet, int id) =>
{
    try
    {
        WalletReadDTO walletDTO = new WalletReadDTO();
        var walletFromDb = wallet.GetByWalletId(id);
        if (walletFromDb == null)
        {
            return Results.NotFound();
        }

        walletDTO.WalletId = walletFromDb.WalletId;
        walletDTO.Username = walletFromDb.Username;
        walletDTO.Email = walletFromDb.Email;
        walletDTO.FullName = walletFromDb.FullName;
        walletDTO.Password = walletFromDb.Password;
        walletDTO.Balance = walletFromDb.Balance;

        return Results.Ok(walletDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);

    }
});

app.MapPut("/api/topupdetails", (IWallet wallet, int id, float balance) =>
{
    try
    {
        var walletFromDb = wallet.GetByWalletId(id);

        if (walletFromDb == null)
        {
            return Results.NotFound();
        }

        if (balance <= 0)
        {
            return Results.BadRequest("Balance must be greater than 0");
        }

        wallet.TopUpWallet(id, balance);

        return Results.Ok(wallet.GetByWalletId(id));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();