using BlazorApp1.Shared.Dtos;

namespace BlazorApp1.Interfaces
{
    public interface IUserService
    {
        IEnumerable<FormData> GetAllUsers();
        FormData? GetUserByEmail(string email);
        void CreateUser(FormData userData);
        bool UpdateUser(string email, FormData userData);
        bool DeleteUser(string email);
    }
}
