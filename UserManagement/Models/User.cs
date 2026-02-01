using System;

namespace UserManagement.Models
{
    public enum UserStatus
    {
        Active,
        Banned,
        Inactive
    }

    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime EndingDate { get; set; }
        public UserStatus Status { get; set; }

        public User()
        {
            JoinedDate = DateTime.Now;
            EndingDate = DateTime.Now.AddYears(1);
            Status = UserStatus.Active;
        }

        public User(int id, string fullName, string email, string phoneNumber, DateTime joinedDate, DateTime endingDate, UserStatus status)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            JoinedDate = joinedDate;
            EndingDate = endingDate;
            Status = status;
        }
    }
}
