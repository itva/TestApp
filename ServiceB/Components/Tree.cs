using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceB.Components.Helpers;

namespace ServiceB.Components
{
    public class Tree:ViewComponent
    {
        private TreeData _data;

        public async Task<IViewComponentResult> InvokeAsync(string htmlId, IEnumerable<TreeRow> data, Func<TreeRow, bool> filter)
        {
            _data = new TreeData(data);

            if (filter != null)
            {
                _data.Filter(filter);
            }

            var innerHtml = _data.ToHtml(htmlId);
            return View("Default", innerHtml);
        }
    }
}
