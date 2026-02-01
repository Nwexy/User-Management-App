using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserRepository
    {
        private static UserRepository _instance;
        private List<User> _users;
        private int _nextId;

        public static UserRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserRepository();
                return _instance;
            }
        }

        private UserRepository()
        {
            _users = new List<User>();
            _nextId = 1;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            AddUser(new User(0, "User", "Active@gmail.com", "xxx xxx xx xx", new DateTime(2026, 1, 1), new DateTime(2077, 1, 1), UserStatus.Active));
            AddUser(new User(0, "User", "Inactive@gmail.com", "xxx xxx xx xx", new DateTime(2026, 1, 1), new DateTime(2077, 1, 1), UserStatus.Inactive));
            AddUser(new User(0, "User", "Banned@gmail.com", "xxx xxx xx xx", new DateTime(2026, 1, 1), new DateTime(2077, 1, 1), UserStatus.Banned));
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public List<User> SearchUsers(string searchText, UserStatus? statusFilter)
        {
            var query = _users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(u =>
                    u.FullName.ToLower().Contains(searchText) ||
                    u.Email.ToLower().Contains(searchText) ||
                    u.PhoneNumber.ToLower().Contains(searchText));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(u => u.Status == statusFilter.Value);
            }

            return query.ToList();
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void AddUser(User user)
        {
            user.Id = _nextId++;
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.JoinedDate = user.JoinedDate;
                existingUser.EndingDate = user.EndingDate;
                existingUser.Status = user.Status;
            }
        }

        public void DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public int GetTotalCount()
        {
            return _users.Count;
        }
    }
}
