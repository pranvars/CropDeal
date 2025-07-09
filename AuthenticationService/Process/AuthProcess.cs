using AuthenticationService;
using AuthenticationService.Process;
using AuthenticationService.EntityModel;
using AuthenticationService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AuthenticationService.Process
{
    public class AuthProcess
    {
        private readonly IUserRepository _repository;
        private readonly TokenManager _tokenManager;
        private readonly IHttpContextAccessor _context;

        public AuthProcess(IUserRepository repository, TokenManager mgr, IHttpContextAccessor ctx)
        {
            _repository = repository;
            _tokenManager = mgr;
            _context = ctx;
        }
        public async Task<AuthResponse> ValidateUserAndGenerateResponse(AuthRequest request)
        {
            var user = await _repository.Authenticate(request);
            Role role = null!;
            string token = string.Empty;
            if (user != null)
            {
                role = await _repository.GetRoleForUser(user.UserId);
                token =  _tokenManager.GetJwtToken(user, role);
                return new AuthResponse(user, role, token);
            }
            return null!;
        }
        public async Task<AuthResponse> ValidateTokenAndReturnUser()
        {
           
            var userEmail = _context.HttpContext?.Items["Email"]?.ToString(); 
            if(string.IsNullOrEmpty(userEmail))
            {
                throw new ArgumentException("Invalid token");
            }
            //Based on the email, call the repository to get the user
            var user = await _repository.GetUserByEmail(userEmail);
            if (user != null)
            {
                var role = await _repository.GetRoleForUser(user.UserId);
                
                return new AuthResponse(user, role, "");
            }
            return null!;
        }
        public async Task<AuthResponse> Signup(SignupRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Username, Email, and Password are required.");
            }

            // Check if user already exists
            var existingUser = await _repository.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            // Create new user object
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password // Ideally, hash this before saving
            };

            // Insert user into the database
            var createdUser = await _repository.CreateUser(newUser);

            // Assign role based on the request (Farmer or Dealer)
            var role = await _repository.GetRoleByName(request.Role);
            if (role == null)
            {
                throw new InvalidOperationException("Invalid role. Allowed roles: Farmer, Dealer.");
            }

            await _repository.AssignRoleToUser(createdUser.UserId, role.RoleId);

            // Call respective service to add user in FarmerDB or DealerDB
            using (var httpClient = new HttpClient())
            {
                if (request.Role.ToLower() == "farmer")
                {
                    var farmer = new
                    {
                        UserId = createdUser.UserId,
                        Location = request.Location,
                        AccountNumber = request.AccountNumber,
                        BankIFSCCode = request.BankIFSCCode,
                        IsActive = true
                    };
                    var response = await httpClient.PostAsJsonAsync("http://localhost:5159/api/Farmer/add", farmer);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to add farmer in FarmerDB.");
                    }
                }
                else if (request.Role.ToLower() == "dealer")
                {
                    var dealer = new
                    {
                        UserId = createdUser.UserId,
                        Location = request.Location,
                        AccountNumber = request.AccountNumber,
                        BankIFSCCode = request.BankIFSCCode,
                        IsActive = true
                    };
                    var response = await httpClient.PostAsJsonAsync("http://localhost:5201/api/Dealer/add", dealer);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to add dealer in DealerDB.");
                    }
                }
            }

            // Generate JWT token with the assigned role
            var token = _tokenManager.GetJwtToken(createdUser, role);

            return new AuthResponse(createdUser, role, token);
        }

        public async Task<string> GetUsernameByUserIdAsync(int userId)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            return user?.Username ?? string.Empty; 
        }

    }
}
