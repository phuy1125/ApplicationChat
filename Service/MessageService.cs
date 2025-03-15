using ChatApplication.Data;
using ChatApplication.Helpers;
using ChatApplication.Interface;
using ChatApplication.ViewModels.MessagesViewModels;
using Microsoft.EntityFrameworkCore;


namespace ChatApplication.Service
{
    public class MessageService: IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public MessageService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<ChatViewModel>GetMessages(string selectedUserId)
        {
            var currentUserId=_currentUserService.UserId;
            var selectedUser = await _context.Users.FirstOrDefaultAsync(i=>i.Id == selectedUserId);
            var selectedName = "";

            if (selectedUser != null)
            {
                selectedName = selectedUser.UserName;
            }
            var chatViewModel = new ChatViewModel()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectedUserId,
                ReceiverUserName = selectedName,
            };
            var messages = await _context.Messages
                .Where(i => (i.SenderId == currentUserId || i.SenderId == selectedUserId)
                         && (i.ReceiverId == currentUserId || i.ReceiverId == selectedUserId))
                .Select(i => new UserMessagesListViewModel()
                {
                    Id = i.Id,
                    Text = i.Text,
                    Date = i.Date.ToShortDateString(),
                    Time = i.Date.ToShortTimeString(),
                    IsCurrentUserSentMessage = i.SenderId == currentUserId,
                }).ToListAsync();
            chatViewModel.Messages=messages;
            return chatViewModel;
        }
        public async Task<IEnumerable<MessagesUserListViewModel>> GetUsers()
        {
            var currentUserId= _currentUserService.UserId;
            var users = await _context.Users.Where(i => i.Id != currentUserId).Select(i => new MessagesUserListViewModel()
            {
                Id = i.Id,
                UserName = i.UserName,
                LastMassage = _context.Messages.Where(m => (m.SenderId == currentUserId || m.SenderId == i.Id) && (m.ReceiverId == currentUserId || m.ReceiverId == i.Id)).OrderByDescending(m => m.Id).Select(m => m.Text).FirstOrDefault()
            }).ToListAsync();

            return users;
        }
    }
}
