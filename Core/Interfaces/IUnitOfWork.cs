using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork <T> where T : class
    {
        public IGenericRepository<T> Entity { get; }

        void Save();

    }
}
