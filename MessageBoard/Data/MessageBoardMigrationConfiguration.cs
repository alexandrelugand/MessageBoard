using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MessageBoard.Data
{
    public class MessageBoardMigrationConfiguration
        : DbMigrationsConfiguration<MessageBoardContext>
    {
        public MessageBoardMigrationConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MessageBoardContext context)
        {
            base.Seed(context);

#if DEBUG
            if (!context.Topics.Any())
            {
                var topic = new Topic()
                {
                    Title = "I like ASP.NET MVC",
                    Body = "It's is makin things easier... really!",
                    Created = DateTime.Now,
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Body = "I love it too!",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Me too",
                            Created = DateTime.Now
                        },
                        new Reply()
                        {
                            Body = "Aw shucks",
                            Created = DateTime.Now
                        }
                    }
                };

                context.Topics.Add(topic);

                var anotherTopic = new Topic()
                {
                    Title = "I like Ruby too!",
                    Body = "Ruby on Rails is popular",
                    Created = DateTime.Now
                };

                context.Topics.Add(anotherTopic);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
#endif
        }
    }
}