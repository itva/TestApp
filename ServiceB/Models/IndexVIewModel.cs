using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceB.Components.Helpers;

namespace ServiceB.Models
{
    public class IndexViewModel
    {
        public string Filter { get; set; }

        public IEnumerable<TreeRow> Data { get; set; }
    }
}
