using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Components.Helpers
{
    public class TreeData
    {
        public TreeData()
        {
            _data = new List<TreeRow>();
        }

        public TreeData(IEnumerable<TreeRow> data)
        {
            Populate(data);
        }

        public void Populate(IEnumerable<TreeRow> data)
        {
            var doubles = data.Select(rec => rec.Id).GroupBy(rec => rec).Where(gr => gr.Count() > 1)
                .Select(gr => gr.Key);
            if (doubles.Any())
            {
                throw new Exception($"Неуникальные идентификаторы записи Id {string.Join(", ", doubles)}");
            }

            var noParents = data.Where(rec => rec.ParentId.HasValue).Select(rec => rec.ParentId.Value).Distinct()
                .Except(data.Select(r => r.Id)).ToList();
            if (noParents.Any())
            {
                throw new Exception($"Отсутствует родительская запись для ParentId {string.Join(", ", noParents)}");
            }

            _data = new List<TreeRow>();
            foreach (var rec in data)
            {
                _data.Add(new TreeRow() { Id = rec.Id, ParentId = rec.ParentId, Text = rec.Text, Value = rec.Value});
            }

            foreach (var rec in _data)
            {
                rec.Host = _data;
            }
        }



        public IEnumerable<TreeRow> Roots => _data.Where(rec => !rec.ParentId.HasValue);
        public IEnumerable<TreeRow> RawData => _data.AsEnumerable();

        public IEnumerable<TreeRow> Filter(Func<TreeRow, bool> func)
        {
            foreach (var rec in _data)
            {
                rec.IsVisible = false;
            }

            foreach (var rec in _data.Where(func))
            {
                rec.IsVisible = true;
            }

            return _data.Where(rec => rec.IsVisible);
        }

        private List<TreeRow> _data;

        public string ToHtml(string htmlId) => Roots.Any(rec=>rec.IsVisible)
            ? $"<ul{(string.IsNullOrEmpty(htmlId) ? "" : $" id='{htmlId}'")}>{string.Join("", Roots.Select(rec => rec.ToHtml(htmlId)))}</ul>"
            : "Нет данных, удовлетворяющих условиям поиска";

    }
}
