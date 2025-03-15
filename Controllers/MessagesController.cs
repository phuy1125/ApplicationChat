using ChatApplication.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _messageService.GetUsers();
            return View(users);
        }
        public async Task<IActionResult> Chat(string selectedUserId)
        {
            var chatViewModel = await _messageService.GetMessages(selectedUserId);
            return View(chatViewModel);
        }
    }
}
