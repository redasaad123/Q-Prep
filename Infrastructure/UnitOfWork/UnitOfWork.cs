using Core.Interfaces;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly AppDbContext context;
        private IGenericRepository<T> entity;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        public IGenericRepository<T> Entity
        {
            get
            {
                return entity ?? (entity = new GenericRepository<T>(context));
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
