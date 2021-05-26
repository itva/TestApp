using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDbContext.Data;

namespace IDbContext
{
    public interface ITestDbContext
    {
        Task<IEnumerable<Subdivision>> GetSubdivisionsAsync();

        Task<int> SaveSubDivisionsInDbAsync(List<Subdivision> src);
    }
}
