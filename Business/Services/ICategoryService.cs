using System;
using Business.Base;
using DAL.Models;

namespace Business.Services
{
	public interface ICategoryService : IBaseRepository<Category>
	{
	}
}

