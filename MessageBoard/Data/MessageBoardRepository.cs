using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageBoard.Data
{
    public class MessageBoardRepository : IMessageBoardRepository
    {
        private readonly MessageBoardContext _ctx;

        public MessageBoardRepository(MessageBoardContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<Topic> GetTopics()
        {
            return _ctx.Topics;
        }

        public IQueryable<Topic> GetTopicsIncludingReplies()
        {
            return _ctx.Topics.Include("Replies");
        }

        public IQueryable<Reply> GetRepliesByTopic(int topicId)
        {
            return _ctx.Replies.Where(r => r.TopicId == topicId);
        }

        public bool Save()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddTopic(Topic topic)
        {
            try
            {
                _ctx.Topics.Add(topic);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddReply(Reply reply)
        {
            try
            {
                _ctx.Replies.Add(reply);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}