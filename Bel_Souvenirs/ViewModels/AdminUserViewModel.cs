namespace Bel_Souvenirs.ViewModels
{
    public class AdminUserViewModel
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class AdminIndexViewModel
    {
        public List<AdminUserViewModel> Users { get; set; } = [];
    }
}
