namespace Finance_BlogPost.Models.ViewModels
{
    public class AddUserRequest
    {
    
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AuthorRoleCheckbox { get; set; }

        public bool AdminRoleCheckbox { get; set; }

        public bool UserRoleCheckbox { get; set; }
    }
}
