using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPTV.WebApi.Controller
{
    [Route("channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly Manager manager;

        public ChannelsController(Manager manager)
        {
            this.manager = manager;
        }
        [HttpGet()]
        public async Task<IActionResult> GetChannels()
        {
            return Ok(manager.GetTVChannels().Select(x => new
            {
                Id = x.Id,
                ChannelName = x.ShowChannelName,
                IsActive = x.IsActive,
                Logo = x.Logo,
                Language = x.Language,
                Category = x.Category,
                Country = x.Country,
                StreamUrl = x.StreamLink,
                IsFound = x.Found
            }
            ).ToList());
        }
        [HttpPost()]
        public async Task<IActionResult> AddChannel()
        {
            return Ok();
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateChannel()
        {
            return Ok();
        }
        [HttpGet("/fetch")]
        public async Task<IActionResult> FetchLink()
        {
            return Ok();
        }

    }
}
