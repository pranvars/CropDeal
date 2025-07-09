using System;
using System.Collections.Generic;

namespace AuthenticationService;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}

public class FaultContract
{
    public int FaultId { get; set; }
    public string FaultName { get; set; } = string.Empty; //Exception Class name
    public string FaultDescription { get; set; }// exception message
    public string FaultType { get; set; } // controller, repository, external call, 

}
public class SignupRequest
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!; // Either "Farmer" or "Dealer"
    public string Location { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string BankIFSCCode { get; set; } = null!;
}

