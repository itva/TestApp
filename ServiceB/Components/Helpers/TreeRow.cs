using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Components.Helpers
{
    public class TreeRow
    {
        public TreeRow()
        {
            IsVisible = true;
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                if (value&&ParentId.HasValue)
                {
                    Parent.IsVisible = true;
                }
            }}
        private bool isVisible;

        public IEnumerable<TreeRow> Host { get; set; }
        public IEnumerable<TreeRow> Child => Host.Where(r => r.ParentId == Id);
        public TreeRow Parent => Host.FirstOrDefault(rec => rec.Id == ParentId);

        public string ToHtml(string htmlId) => IsVisible == false
            ? ""
            : $"<li{(string.IsNullOrEmpty(htmlId) ? "" : $" id='{htmlId}_{Id}'")}>{Text}</li>{(Child.Any() ? $"<ul>{String.Join("", Child.Select(r => r.ToHtml(htmlId)).ToList())}</ul>" : "")}";

    }
}
