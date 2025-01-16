using FinTrackPro.Abstraction;
using FinTrackPro.Models;
using FinTrackPro.Services.Interface;
using Newtonsoft.Json;
namespace FinTrackPro.Services
{
    public class UserService : UserBase,IUserService
    {
        private List<LoginModel> _users;

        // Default admin username and password for initial seeding.
        public const string SeedUsername = "admin";
        public const string SeedPassword = "password";
        public const int SeedBalance = 1000;

        public UserService()
        {
            // Load the list of users from the JSON file.
            _users = LoadUsers();

            // If no users are present, add a default admin user and save to the file.
            if (!_users.Any())
            {
                _users.Add(new LoginModel { Username = SeedUsername, Password = SeedPassword ,Balance=SeedBalance});
                SaveUsers(_users);
            }
        }
        
        public List<LoginModel> GetAllUsers()
        {
            return _users; // Return the in-memory list of users.
        }

        // Logs in a user by checking if their username and password exist in the list.
        // Returns true if the user is authenticated, false otherwise.
        public bool Login(LoginModel user)
        {
            // Validate input for null or empty values.
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return false; // Invalid input.
            }

            // Check if the username and password match any user in the list.
            return _users.Any(u => u.Username == user.Username && u.Password == user.Password);
        }

        // Registers a new user. Returns false if the username already exists, true if registration is successful.
        public bool Register(LoginModel user)
        {
            // Check if the username already exists in the list.
            if (_users.Any(u => u.Username == user.Username))
                return false; // Registration failed: user already exists.

            // Create a new user with a GUID and add it to the list.
            var newUser = new LoginModel
            {
                UserID = Guid.NewGuid(),  // Generate a unique GUID for the new user
                Username = user.Username,
                Password = user.Password
            };


            _users.Add(newUser);
            SaveUsers(_users);
            return true;
        }


        public bool addBalance(int amountToAdd)
        {
            var user = _users.FirstOrDefault(u => u.Username == SeedUsername); // Only one user "admin"
            if (user == null)
                return false;

            user.Balance += amountToAdd;
            SaveUsers(_users);
            return true;
        }

        public bool UpdateUserBalance(int newBalance)
        {
            var user = _users.FirstOrDefault(u => u.Username == SeedUsername); // Only one user "admin"
            if (user == null)
                return false;

            user.Balance = newBalance; // Update the balance for the user.
            SaveUsers(_users); // Save the updated list of users to the JSON file.
            return true;
        }

        public int GetUserBalance()
        {
            var user = _users.FirstOrDefault(u => u.Username == SeedUsername);
            return user?.Balance ?? 0; 
        }
        public string getCurrency()
        {
            var user = _users.FirstOrDefault(u => u.Username == SeedUsername);
            return user.PreferredCurrency;
        }
        public bool UpdatePreferredCurrency(string preferredCurrency)
        {
            var user = _users.FirstOrDefault(u => u.Username == SeedUsername); // Only one user "admin"
            if (user == null)
                return false;

            user.PreferredCurrency = preferredCurrency; // Update the preferred currency.
            SaveUsers(_users); // Save the updated list of users to the JSON file.
            return true;
        }
    }
}