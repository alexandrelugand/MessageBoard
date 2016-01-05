using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class TopicsController : ApiController
    {
        private readonly IMessageBoardRepository _messageBoardRepository;

        public TopicsController(IMessageBoardRepository messageBoardRepository)
        {
            _messageBoardRepository = messageBoardRepository;
        }

        public IEnumerable<Topic> Get(bool includeReplies = false)
        {
            IQueryable<Topic> results;
            if (includeReplies)
            {
                results = _messageBoardRepository.GetTopicsIncludingReplies();
            }
            else
            {
                results = _messageBoardRepository.GetTopics();
            }
            return results.OrderByDescending(t => t.Created)
                .Take(25)
                .ToList();
        }

        public HttpResponseMessage Post([FromBody]Topic topic)
        {
             if(topic.Created == default(DateTime))
                topic.Created = DateTime.Now;

            if (_messageBoardRepository.AddTopic(topic) &&
                _messageBoardRepository.Save())
                return Request.CreateResponse(HttpStatusCode.Created, topic);

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
