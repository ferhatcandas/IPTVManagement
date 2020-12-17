using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

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
            ).OrderByDescending(x => x.IsFound).ThenBy(x => x.ChannelName).ToList());
        }
        [HttpPost()]
        public async Task<IActionResult> AddChannel(TVChannelModel request)
        {
            manager.AddChannel(request.ToTvChannel(Guid.NewGuid().ToString()));
            return Ok();
        }
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannel([FromRoute] string channelId)
        {
            return Ok(manager.GetTVChannel(channelId));
        }
        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannel([FromRoute] string channelId, [FromBody] TVChannelModel request)
        {
            manager.UpdateChannel(channelId, request.ToTvChannel(channelId));
            return Ok();
        }
        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannel([FromRoute] string channelId)
        {
            manager.RemoveChannel(channelId);
            return Ok();
        }
        [HttpGet("/fetch")]
        public async Task<IActionResult> FetchLink()
        {
            return Ok();
        }

    }
}
