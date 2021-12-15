using Microsoft.AspNetCore.Mvc;
using Stack.Models;
using StackOverFlow.Services;
using StackOverFlowData.Models;

namespace Stack.Controllers
{
    public class QuestionController : Controller
    {
        IVotingService _votingservice;
        IQuestionService _questionService;
        IUserService _userService;
        IAnswerService _answerService;
        ICategoryService _categoryService;
        public QuestionController(IVotingService votingservice,IQuestionService questionService, IUserService userService, IAnswerService answerService, ICategoryService categoryService)
        {
            _votingservice = votingservice;
            _questionService = questionService;
            _answerService = answerService;
            _userService = userService;
            _categoryService = categoryService;
        }
        // GET: Question
        public IActionResult Index()
        {
            var res = _questionService.ShowAll();
            return View(res);
        }
        public IActionResult Add()
        {
            ViewBag.category = _categoryService.ShowAll();
            ViewBag.user = _userService.ShowAll();
            Question q = new Question();
            return View(q);
        }
        [HttpPost]
        public IActionResult Add(Question Q)
        {
           
            _questionService.SaveQuestion(Q);
            return RedirectToAction("Index");
        }
        public IActionResult Show(int id)
        {
            var q = _questionService.FindQuestion(id);
            ViewBag.answers = _answerService.ShowAll();
            ViewBag.users = _userService.ShowAll();
            var votes = _votingservice.Showall();
            ViewBag.votes = votes;
            ViewBag.curr= _userService.FindUser(User.Identity.Name);
            return View(q);
        }

        public IActionResult Edit(int id)
        {
            var q = _questionService.FindQuestion(id);
            return View(q);
        }

        [HttpPost]
        public IActionResult Edit(Question q)
        {
            _questionService.Edit(q);
            return RedirectToAction("Index");
        }
    }
}
