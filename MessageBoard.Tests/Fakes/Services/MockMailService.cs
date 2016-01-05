using System;
using System.Diagnostics;
using MessageBoard.Services;

namespace MessageBoard.Tests.Fakes.Services
{
    public class MockMailService : IMailService
    {
        public bool SendMail(string @from, string to, string subject, string body)
        {
            Debug.WriteLine(string.Format("# SendMail #{0}From: {1}{0}To: {2}{0}Subject: {3}{0}Body:{0}{0}{4}",
                Environment.NewLine,
                @from,
                to,
                subject,
                body));
            return true;
        }
    }
}