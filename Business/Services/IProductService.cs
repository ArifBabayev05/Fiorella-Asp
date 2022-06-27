using System;
using Business.Base;
using DAL.Models;

namespace Business.Services
{
	public interface IProductService : IBaseRepository<Product>
	{
	}
}

