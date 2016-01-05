using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class RepliesController : ApiController
    {
        private readonly IMessageBoardRepository _messageBoardRepository;

        public RepliesController(IMessageBoardRepository messageBoardRepository)
        {
            _messageBoardRepository = messageBoardRepository;
        }

        public IEnumerable<Reply> Get(int topicId)
        {
            return _messageBoardRepository.GetRepliesByTopic(topicId)
                .OrderByDescending(r => r.Created)
                .Take(5)
                .ToList();
        }

        public HttpResponseMessage Post(int topicId, [FromBody] Reply reply)
        {
            if (reply.Created == default(DateTime))
                reply.Created = DateTime.Now;

            reply.TopicId = topicId;

            if (_messageBoardRepository.AddReply(reply) &&
                _messageBoardRepository.Save())
                return Request.CreateResponse(HttpStatusCode.Created, reply);

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
