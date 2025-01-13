using FinTrackPro.Models;

namespace FinTrackPro.Services.Interface
{
    public interface IUserService
    {

        bool Login(LoginModel user);

        bool Register(LoginModel user);

        bool addBalance(int amountToAdd);

        bool UpdateUserBalance(int newBalance);

        int GetUserBalance();


        List<LoginModel> GetAllUsers();
    }
}
