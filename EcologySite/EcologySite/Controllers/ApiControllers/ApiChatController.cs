using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcologySite.Localizations;
using EcologySite.Models.Ecology;
using EcologySite.Services;

namespace EcologySite.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiChatController : ControllerBase
    {
        private IChatMessageRepositryReal _chatMessageRepositry;

        public ApiChatController(IChatMessageRepositryReal chatMessageRepositry)
        {
            _chatMessageRepositry = chatMessageRepositry;
        }

        public List<string> GetLastMessages()
        {
            return _chatMessageRepositry.GetLastMessages();
        }
    }
}
