using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class DataResponse<T> : Response
    {
        public List<T> Data { get; set; }

        public DataResponse()
        {
            this.Data = new List<T>();
        }
    }
}
