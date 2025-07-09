using AuthenticationService;

namespace AuthenticationService.EntityModel
{
    public class AuthResponse
    {
        public User User { get; set; }
        public Role Role { get; set; }  
        public string Token { get; set; }
        public AuthResponse(User user, Role role, string token) => (User, Role, Token) = (user, role, token);   



    }
}
