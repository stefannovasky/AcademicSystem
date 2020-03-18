using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Response
    {
        public bool Success { get; set; } = true;
        public List<string> ErrorList; 

        public Response() 
        {
            this.ErrorList = new List<string>(); 
        }

        public bool HasError()
        {
            return ErrorList.Count > 0;
        }

        public string GetErrorMessage()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string error in this.ErrorList)
            {
                sb.AppendLine(error); 
            }

            return sb.ToString(); 
        }
    }
}
