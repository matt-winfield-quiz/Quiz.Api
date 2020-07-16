using Quiz.Api.Models.Display;
using Quiz.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Quiz.Api.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();

        public void CreateUser(User user)
        {
            _users.Add(user);
        }

        public User GetUser(string id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }

        public void UpdateUserName(string id, string name)
        {
            var user = GetUser(id);

            if (user != null)
            {
                user.Name = name;
            }
        }
    }
}
