using Core;
using Core.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPTV.WebApi.Controller
{
    [Route("channels/v2")]
    [ApiController]
    public class ChannelControllerV2 : ControllerBase
    {
        private readonly IChannelService service;

        public ChannelControllerV2(IChannelService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Channels()
        {
            return Ok(await service.GetChannels());
        }
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannel([FromRoute] string channelId)
        {
            return Ok(await service.GetChannel(channelId));
        }
        [HttpGet("stream")]
        [Produces("audio/x-mpegurl")]
        public async Task<IActionResult> Stream()
        {
            return File(await service.GetStream(), "audio/x-mpegurl");
        }
        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannel([FromRoute] string channelId, [FromBody] Channel request)
        {
            await service.UpdateChannel(channelId, request);
            return Ok();
        }
        [HttpPut("{channelId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] string channelId)
        {
            await service.UpdateStatus(channelId);
            return Ok();
        }
        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannel([FromRoute] string channelId)
        {
            await service.DeleteChannel(channelId);
            return Ok();
        }

    }
}
