using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.doduo.MessageBroker.Contract;
using dotnet.doduo.Attributes;
using dotnet.doduo.MessageBroker.Model;

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

        [Route("SendAsync")]
        [HttpPost]
        public async void SendAsync([FromBody]DoduoPublishDto doduoPublishDto)
        {
           await m_doduoPublish.PublishAsyncWithoutReturn("doduo.teste", doduoPublishDto, "sssssss");
        }

        [Route("SendSync")]
        [HttpPost]
        public async Task<DoduoPublishDto> SendSync([FromBody]DoduoPublishDto doduoPublishDto)
        {
           return await m_doduoPublish.PublishAsync<DoduoPublishDto>("doduo.teste.sync", doduoPublishDto, "sssssss");
        }

        [Route("Test")]
        [HttpPost]
        [DoduoTopic("doduo.teste")]
        public async void Test(DoduoPublishDto doduoPublishDto, string sss, int ssssssbdvfb)
        {
        }

        [Route("TestSync")]
        [HttpPost]
        [DoduoTopic("doduo.teste.sync")]
        public async Task<DoduoPublishDto> TestSync(DoduoPublishDto doduoPublishDto, string sss, int ssssssbdvfb)
        {
            doduoPublishDto.Name = $"Nome '{doduoPublishDto.Name}' Publicado";
            return doduoPublishDto;
        }

    }

    public class DoduoPublishDto
    {
        public string Name { get; set; }
    }
}
