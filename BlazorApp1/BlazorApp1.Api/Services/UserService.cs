using BlazorApp1.Api.Data;
using BlazorApp1.Interfaces;
using BlazorApp1.Shared.Dtos;

namespace BlazorApp1.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<FormData> GetAllUsers() => _context.FormData.ToList();

        public FormData? GetUserByEmail(string email) =>
            _context.FormData.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        public void CreateUser(FormData userData)
        {
            if (_context.FormData.Any(u => u.Email.ToLower() == userData.Email.ToLower()))
                throw new InvalidOperationException("User already exists");

            _context.FormData.Add(userData);
            _context.SaveChanges();
        }

        public bool UpdateUser(string email, FormData userData)
        {
            var existingUser = GetUserByEmail(email);
            if (existingUser == null) return false;

            existingUser.Name = userData.Name;
            existingUser.Password = userData.Password;
            existingUser.Url = userData.Url;
            existingUser.Gender = userData.Gender;
            existingUser.PhoneNumber = userData.PhoneNumber;
            existingUser.Date = userData.Date;
            existingUser.Verify = userData.Verify;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(string email)
        {
            var user = GetUserByEmail(email);
            if (user == null) return false;

            _context.FormData.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
