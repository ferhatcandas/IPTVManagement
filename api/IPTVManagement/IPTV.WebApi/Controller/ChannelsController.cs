using Core;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Linq;
using System.Threading.Tasks;

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
        [HttpGet("stream")]
        [Produces("audio/x-mpegurl")]
        public async Task<IActionResult> Stream()
        {
            System.IO.Stream stream = manager.GetStream();
            return File(stream, "audio/x-mpegurl");
        }
        [HttpGet()]
        public async Task<IActionResult> GetChannels()
        {
            return Ok(manager.GetTVChannels(true).Select(x => new
            {
                Id = x.Id,
                ChannelName = x.Name,
                IsActive = x.IsActive,
                Logo = x.Logo,
                Language = x.Language,
                Category = x.Category,
                Country = x.Country,
                StreamUrl = x.Stream,
                IsFound = x.Found,
                Editable = x.Editable
            }
            ).OrderByDescending(x => x.IsFound).ThenBy(x => x.ChannelName).ToList());
        }
        [HttpPost()]
        public async Task<IActionResult> AddChannel([FromBody] TVChannel request)
        {
            manager.AddChannel(request);
            return Ok();
        }
        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannel([FromRoute] string channelId)
        {
            return Ok(manager.GetTVChannel(channelId));
        } 
        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannel([FromRoute] string channelId, [FromBody] TVChannel request)
        {
            manager.UpdateChannel(channelId, request);
            return Ok();
        }
        [HttpPut("{channelId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] string channelId)
        {
            manager.UpdateStatus(channelId);
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
