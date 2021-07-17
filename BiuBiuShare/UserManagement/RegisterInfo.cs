namespace BiuBiuShare.UserManagement
{
    public class RegisterInfo
    {
        public string JobNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public bool Permissions { get; set; }//True:Administrator
    }
}