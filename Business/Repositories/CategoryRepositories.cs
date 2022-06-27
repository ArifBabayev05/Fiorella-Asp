using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Base;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Repositories
{
	public class CategoryRepositories : ICategoryService 
	{
		private readonly AppDbContext _context;
		public CategoryRepositories(AppDbContext context)
		{
			_context = context;
		}

        public async Task<Category> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException("Id");
            }
            var data = await _context.Categories.Where(n => n.Id == id)
                                                .FirstOrDefaultAsync();

            if (data is null)
            {
                throw new NullReferenceException("Data Could Not Be Found!");
            }
            return data;
        }

        public async Task<List<Category>> GetAll()
        {
            var data = await _context.Categories.Where(n => !n.IsDeleted)
                                             .OrderByDescending(n => n.CreatedDate)
                                             .ToListAsync();

            if (data is null)
            {
                throw new NullReferenceException();
            }
            return data; 
        }

        public async Task Create(Category entity)
        {
            entity.CreatedDate = DateTime.Now;

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Category entity)
        {
            var dbEntity = await Get(id);
            if (dbEntity is null)
            {
                throw new NullReferenceException("Product is null!");
            }
            dbEntity.Name = entity.Name;
       
            dbEntity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }
            var data = await Get(id);
            if (data is null)
            {
                throw new NullReferenceException();
            }
            data.IsDeleted = true;

            await _context.SaveChangesAsync(); 
        }
    }
}

