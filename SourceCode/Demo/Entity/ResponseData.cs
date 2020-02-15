using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Entity
{
    public class ResponseData
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class ResponseData<T>
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public T ReturnData { get; set; }
    }
}
