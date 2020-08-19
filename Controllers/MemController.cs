using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotnetCoreWCF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemController : ControllerBase
    {
        // GET: api/<MemController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            int xnCode = 180;
            int companyId = 797;
            string groupIDs = "";
            string accessToken = Guid.NewGuid().ToString();


            var datas = new GPSMemoryService().InitVehicleOnline(xnCode, companyId, groupIDs, accessToken);

            var jsonString = JsonSerializer.Serialize(datas);

            var count = datas != null ? datas.Count : 0;

            return new string[] { "COUNT:", count.ToString()};
        }
    }
}
