using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageBoard.Data;
using MessageBoard.Services;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IMessageBoardRepository _repository;

        public HomeController(IMailService mailService, IMessageBoardRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }

        public ActionResult Index()
        {
            var topics = _repository.GetTopics()
                .OrderByDescending(t => t.Created)
                .Take(25)
                .ToList();

            return View(topics);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
           
        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            var msg = string.Format("Comment From: {1}{0}Email: {2}{0}Website: {3}{0}Comment: {4}{0}",
                Environment.NewLine, 
                model.Name, 
                model.Email, 
                model.Website, 
                model.Comment);

            if (_mailService.SendMail("noreply@messageBoard.com",
                "alexandre.lugand@gmail.com",
                "Website contact",
                msg))
            {
                ViewBag.MailSent = true;
            }

            return View();
        }
                  
        [Authorize]  
        public ActionResult MyMessages()
        {
            return View();
        }


        [Authorize(Roles="Admin")]
        public ActionResult Moderation()
        {
            return View();
        }


    }
}