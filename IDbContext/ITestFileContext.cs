using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDbContext.Data;

namespace IDbContext
{
    public interface ITestFileContext
    {
        Task<IEnumerable<Subdivision>> UploadSubdivisionsFromFileAsync(string fileSrc);

        Task<int> SaveSubDivisionsIntoFileAsync(List<Subdivision> src, string fileDest);
    }
}
