using ChatApplication.ViewModels.MessagesViewModels;

namespace ChatApplication.Interface
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagesUserListViewModel>> GetUsers();
        Task<ChatViewModel> GetMessages(string selectedUserId);
    }
}
