using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private static readonly ConcurrentDictionary<long, bool> _data = new ConcurrentDictionary<long, bool>();

        private static object _lock=new {};

        private static readonly Timer _timer = new Timer(state =>
        {
            var rand=new Random();
            lock (_lock)
            {
                foreach (var (key, _) in _data)
                {
                    _data[key] = rand.Next(0,100)>50?_data[key]:!_data[key];
                }
            }
        }, null, 0, 3000);



        [HttpGet]
        [Route("/api/Status/GetData")]
        public ActionResult<IDictionary<long, bool>> GetData()
        {
            return _data;
        }

        [HttpPost]
        [Route("/api/Status/UploadNewData")]
        public ActionResult<IDictionary<long, bool>> UploadNewData([FromBody]List<long> src)
        {

            lock (_lock)
            {

                _data.Clear();
                var rnd = new Random();
                foreach (var id in src)
                {
                    _data.TryAdd(id, rnd.Next(1, 100) > 70);
                }
            }

            return _data;
        }

    }
}
