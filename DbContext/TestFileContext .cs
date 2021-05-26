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
    public class TestFileContext:ITestFileContext
    {

        public async Task<IEnumerable<Subdivision>> UploadSubdivisionsFromFileAsync(string fileSrc)
        {
            await Task.Yield();
            if (!File.Exists(fileSrc))
            {
                return null;
            }

            var str = File.ReadAllText(fileSrc);
            try
            {
                var rslt = JsonConvert.DeserializeObject<List<Subdivision>>(str);
                return rslt;
            }
            catch (Exception ex)
            {
                // ignored
            }

            return null;
        }

        public async Task<int> SaveSubDivisionsIntoFileAsync(List<Subdivision> src, string fileDest)
        {
            await Task.Yield();

            try
            {
                var str = JsonConvert.SerializeObject(src);
                File.WriteAllText(fileDest, str);
                return 0;
            }
            catch (UnauthorizedAccessException ex)
            {
                return -1;
            }
            catch (Exception ex)
            {
                return -2;
            }
        }


    }
}
