using AdvancedProjectMVC.Models;

namespace AdvancedProjectMVC.Repositories
{
    public interface IChatRepository
    {
        public void AddMessage(string username, string message);
    }
}
