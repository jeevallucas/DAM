using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServices.Models;
using CatalogServices.DTO.Product;

namespace CatalogServices.DAL.Interfaces
{
    public interface IProduct : IProductCrud<Product>
    {
        Task UpdateStockAfterOrder(ProductUpdateQuantityDTO productUpdateQuantityDTO);
    }
}