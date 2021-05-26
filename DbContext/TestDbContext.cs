using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IDbContext;
using IDbContext.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestDbModel;

namespace DbContext
{
    public class TestDbContext:ITestDbContext
    {
        public async Task<IEnumerable<Subdivision>> GetSubdivisionsAsync()
        {
            using (var db = new TestDb())
            {
                var data = await db.Subdivisions.ToListAsync();
                var rslt=new List<Subdivision>();
                rslt = data.Select(rec => new Subdivision(){Id=rec.Id, ParentId = rec.ParentId, Name = rec.Name}).ToList();
                return rslt;
            }
        }

        public async Task<int> SaveSubDivisionsInDbAsync(List<Subdivision> src)
        {
            var doubles = src.Select(rec => rec.Id).GroupBy(rec => rec).Any(gr => gr.Count() > 1);
            if (doubles)
            {
                return -1;
            }

            var noParents = src.Where(rec => rec.ParentId.HasValue).Select(rec => rec.ParentId.Value).Distinct()
                .Except(src.Select(r => r.Id)).Any();
            if (noParents)
            {
                return -2;
            }

            var selfBinded = src.Any(rec => rec.Id == rec.ParentId);
            if (selfBinded)
            {
                return -3;
            }

            using (var db=new TestDb())
            {
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                db.Subdivisions.RemoveRange(db.Subdivisions);

                db.Subdivisions.AddRange(src.Select(rec=>new TestDbModel.Tables.Subdivision(){Id = rec.Id, ParentId = rec.ParentId, Name = rec.Name}).ToList());

                await db.SaveChangesAsync();
                return 0;
            }
        }


    }
}
