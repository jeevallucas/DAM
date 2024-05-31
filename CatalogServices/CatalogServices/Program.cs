using CatalogServices.DAL;
using CatalogServices.DAL.Interfaces;
using CatalogServices.DTO.Category;
using CatalogServices.DTO.CatProd;
using CatalogServices.DTO.Product;
using CatalogServices.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategory, CategoryDapper>();
builder.Services.AddScoped<IProduct, ProductDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/categories", (ICategory categoryDal) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetAll();
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    CategoryDTO categoryDto = new CategoryDTO();
    var categories = categoryDal.GetByID(id);
    if (categories == null)
    {
        return Results.NotFound();
    }
    categoryDto.CategoryName = categories.CategoryName;
    categoryDto.CategoryID = categories.CategoryID;

    return Results.Ok(categoryDto);
});

app.MapPost("/api/categories", (ICategory categoryDAL, CategoryCreateDTO category) =>
{
    try
    {
        Category categoryDto = new Category
        {
            CategoryName = category.CategoryName
        };

        categoryDAL.Insert(categoryDto);

        return Results.Created($"/api/categories/{category}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/categories/{id}", (ICategory categoryDAL, int id) =>
{
    try
    {
        categoryDAL.Delete(id);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/categories", (ICategory categoryDAL, CategoryUpdateDTO category) =>
{
    try
    {
        Category categoryDto = new Category
        {
            CategoryName = category.CategoryName,
            CategoryID = category.CategoryID,
        };

        categoryDAL.Update(categoryDto);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/products", (IProduct productDal) =>
{
    List<ProductDTO> productsDto = new List<ProductDTO>();
    var products = productDal.GetAll();
    foreach (var product in products)
    {
        productsDto.Add(new ProductDTO
        {
            ProductID = product.ProductID,
            CategoryID = product.CategoryID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
        });
    }
    return Results.Ok(productsDto);
});

app.MapGet("/api/products/{id}", (IProduct productDal, int id) =>
{
    ProductDTO productDto = new ProductDTO();
    var product = productDal.GetByID(id);
    if (product == null)
    {
        return Results.NotFound();
    }

    productDto.ProductID = product.ProductID;
    productDto.CategoryID = product.CategoryID;
    productDto.Name = product.Name;
    productDto.Description = product.Description;
    productDto.Price = product.Price;
    productDto.Quantity = product.Quantity;

    return Results.Ok(productDto);
});

app.MapPost("/api/products", (IProduct productDAL, ProductCreateDTO product) =>
{
    try
    {
        Product productDto = new Product
        {
            CategoryID = product.CategoryID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
        };

        productDAL.Insert(productDto);

        ProductDTO products = new ProductDTO
        {
            CategoryID = product.CategoryID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
        };

        return Results.Created($"/catalogservice/api/products/{product}", product);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/products/{id}", (IProduct productDAL, int id) =>
{
    try
    {
        productDAL.Delete(id);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/products", (IProduct productDAL, ProductUpdateDTO product) =>
{
    try
    {
        Product productDto = new Product
        {
            ProductID = product.ProductID,
            CategoryID = product.CategoryID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
        };

        productDAL.Update(productDto);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();