﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankingSystem.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
    
        public async Task<T> GetById(int id)
        {
            var result = await _dbSet.FindAsync(id);

            return result;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
             
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
