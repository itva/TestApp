using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace IDbContext.Data
{
    public class Subdivision
    {
        public long Id { get; set; }


        public long? ParentId { get; set; }

        public string Name { get; set; }
    }
}
