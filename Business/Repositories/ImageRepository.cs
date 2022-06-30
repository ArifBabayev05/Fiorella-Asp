using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Repositories
{
    public class ImageRepository : IImageService
    {
        private readonly AppDbContext _context;
        public ImageRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<Image> Get(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException("id");
            }
            var entity = await _context.Images.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new NullReferenceException();
            }
            return entity;
        }

        public async Task<List<Image>> GetAll()
        {
            var entity = await _context.Images.ToListAsync();

            if (entity is null)
            {
                throw new NullReferenceException();
            }
            return entity;
        }
        public async Task Create(Image entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Image");
            };

            await _context.Images.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Image entity)
        {
            var dbEntity = await Get(id);

            if (dbEntity is null)
            {
                throw new NullReferenceException("entity");
            }
            dbEntity.Name = entity.Name;
            await _context.SaveChangesAsync();
        }
        public async  Task Delete(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException("id");
            }
            var entity = await _context.Images.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new NullReferenceException();
            }

            _context.Images.Remove(entity);
            await _context.SaveChangesAsync(); 
        }

    }
}

