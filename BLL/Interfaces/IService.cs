﻿using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IService<T> where T : class, new()
    {
        Task<Response> Create(T item);
        Task<Response> Delete(int id);
        Task<DataResponse<T>> GetAll();
        Task<DataResponse<T>> GetByID(int id);
        Task<DataResponse<T>> Update(T item);
    }
}
