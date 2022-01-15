using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Models.Response
{
    public class Result<T>
    {
        public int Status { get; set; }

        public T Message { get; set; }
    }
}
