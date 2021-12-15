using Microsoft.AspNetCore.Mvc;
using Stack.Models;
using StackOverFlow.Services;
using StackOverFlowData.Models;

namespace Stack.Controllers
{
        public class AnswerController : Controller
        {
            IVotingService _votingservice;
            IQuestionService _questionService;
            IUserService _userService;
            IAnswerService _answerService;
            ICategoryService _categoryService;
        public AnswerController(IVotingService votingservice, IQuestionService questionService, IUserService userService, IAnswerService answerService , ICategoryService categoryService)
        {
            _votingservice = votingservice;
            _questionService = questionService;
            _answerService = answerService;
            _userService = userService;
            _categoryService = categoryService;
        }
            public IActionResult Index()
            {
                return View();
            }
            public IActionResult Add(int id)
            {
                ViewBag.Qid = id;
                ViewBag.user = _userService.ShowAll();
                return View();
            }
            [HttpPost]
            public IActionResult Add(Answer answer)
            {
                _answerService.SaveAnswer(answer);
                return RedirectToAction("Index", "Question");
            }
            public IActionResult Edit(int id)
            { 
                var obj = _answerService.FindAnswer(id);
                ViewBag.user = _userService.ShowAll();
                TempData["Qid"] = obj.QuestionQId.ToString();
                return View(obj);
            }
            [HttpPost]
            public IActionResult Edit(Answer q)
            {
                var ID = TempData["Qid"];
                ID= Convert.ToInt32(ID);
                _answerService.Edit(q);
                return RedirectToAction("Show", "Question", new { id = ID});
            }

        public ActionResult Delete(int id)
        {
            var ans = _answerService.FindAnswer(id);
            ViewBag.user = _userService.ShowAll();
            return View(ans);
        }

        [HttpPost]
        public ActionResult Delete(Answer q)
        {
            int ID = q.QuestionQId;
            _answerService.Delete(q);
            return RedirectToAction("Show", "Question", new { id = ID });
        }

        public IActionResult Vote(int id)
        {
            Answer q = _answerService.FindAnswer(id);
            int ID = q.QuestionQId;
            var user = _userService.FindUser(User.Identity.Name);
            var userid = user.UId;

            if (_votingservice.Find(id, userid)!=null)
            {
                _votingservice.Unlike(id, userid);
            }
            else 
            {
                _votingservice.Like(id, userid);
            }
            return RedirectToAction("Show", "Question", new { id = ID });
        }

    }
    }
