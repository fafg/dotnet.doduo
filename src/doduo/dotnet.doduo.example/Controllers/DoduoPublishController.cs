using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.doduo.MessageBroker.Contract;

namespace dotnet.doduo.example.Controllers
{
    [Route("api/[controller]")]
    public class DoduoPublishController : Controller
    {
        private readonly IDoduoPublish _doduoPublish;

        public DoduoPublishController(IDoduoPublish doduoPublish)
        {
            _doduoPublish = doduoPublish;
        }

        [Route("Send")]
        [HttpPost]
        public async void SendAsync([FromBody]DoduoPublishDto doduoPublishDto)
        {
           await _doduoPublish.PublishAsync("doduo.teste", doduoPublishDto);
        }
    }

    public class DoduoPublishDto
    {
        public string Name { get; set; }
    }
}
