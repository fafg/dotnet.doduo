using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.Attributes;

namespace dotnet.doduo.example.Controllers
{
    [Route("api/[controller]")]
    public class DoduoPublishController : Controller
    {
        private readonly IDoduoPublish m_doduoPublish;

        public DoduoPublishController(IDoduoPublish doduoPublish)
        {
            m_doduoPublish = doduoPublish;
        }

        [Route("Send")]
        [HttpPost]
        public async void SendAsync([FromBody]DoduoPublishDto doduoPublishDto)
        {
           await m_doduoPublish.PublishAsync("doduo.teste", doduoPublishDto);
        }

        [Route("Test")]
        [HttpPost]
        [DoduoTopicAttribute("doduo.teste")]
        public async void Test([FromBody]DoduoPublishDto doduoPublishDto)
        {
        }

    }

    public class DoduoPublishDto
    {
        public string Name { get; set; }
    }
}
