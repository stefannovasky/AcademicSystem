using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Response Create(T item);
        Response Delete(int id);
        DataResponse<T> GetAll();
        DataResponse<T> GetByID(int id);
        DataResponse<T> Update(T item);
    }
}
