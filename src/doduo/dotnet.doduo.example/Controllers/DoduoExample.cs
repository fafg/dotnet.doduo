using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.doduo.example.Controllers
{
    [Route("api/[controller]")]
    public class DoduoExample : DoduoController
    {
        [Route("{id}")]
        public async Task<DoduoResult<MyDto>> Get(int id)
        {
            return null;
        }
    }

    public class MyDto
    {
    }
}
