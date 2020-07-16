using Quiz.Api.Models.Display;

namespace Quiz.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public void CreateUser(User user);
        public void UpdateUserName(string id, string name);
        public User GetUser(string id);
    }
}
