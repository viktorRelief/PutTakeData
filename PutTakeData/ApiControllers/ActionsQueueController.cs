using Microsoft.AspNetCore.Mvc;
using PutTakeData.Interfaces;
using System.Collections.Generic;

namespace PutTakeData.ApiControllers
{
    [Route("api/actionsqueue")]
    [ApiController]
    public class ActionsQueueController : ControllerBase
    {
        private readonly IActionsQueue _action;
        public ActionsQueueController(IActionsQueue actions)
        {
            _action = actions;
        }

        [HttpPost("enqueue")]
        public void Enqueue([FromBody] string str)
        {
            _action.Enqueue(str);
        }

        [HttpGet("dequeue")]
        public IEnumerable<string> Dequeue()
        {
            return _action.Dequeue();
        }
    }
}
