using InformationProtocolSubSystem.Api.Infrastructure;
using LiveChat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace LiveChat.Controllers
{
    [Route("api/Message")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet, Authorize]
        public IActionResult Get()
        {
            //var claims = TokenIdentityHelper.GetClaimValues(User, "messages");

            var query = _context.Messages.Select(ac =>
                new MessageDTO
                {
                    Text = ac.Text,
                    User = ac.User
                }
            ).AsQueryable();

            return Ok(query);

        }
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement obj)
        {
            try
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.ChangeTracker.AutoDetectChangesEnabled = false;

                MessageDTO messageDTO = JsonSerializer.Deserialize<MessageDTO>(obj.ToString());
                Message message = new Message();

                message.Text = messageDTO.Text;
                message.User = messageDTO.User;

                _context.Messages.Add(message);
                _context.SaveChanges();

                return Created("Message", new Message() { Id = message.Id });
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpPatch]
        public IActionResult Patch(Guid key, [FromBody] Message patch)
        {
            try
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                Message message = _context.Messages.FirstOrDefault(m => m.Id == key);
                if (message == null)
                {
                    return NotFound();
                }
                else
                {
                    message.Text = patch.Text;
                    message.User = patch.User;
                    _context.SaveChanges();
                    return Ok(message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid key)
        {
            try
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                Message message = _context.Messages.FirstOrDefault(m => m.Id == key);
                if (message == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Messages.Remove(message);
                    _context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}

