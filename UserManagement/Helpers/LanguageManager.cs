using System;

namespace UserManagement.Helpers
{
    public static class LanguageManager
    {
        public static event EventHandler LanguageChanged;
        
        private static bool _isTurkish = false;
        
        public static bool IsTurkish
        {
            get => _isTurkish;
            set
            {
                if (_isTurkish != value)
                {
                    _isTurkish = value;
                    LanguageChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        // Login Form
        public static string WelcomeTitle => IsTurkish ? "Kullanıcı Yönetim\nSistemine Hoş Geldiniz" : "Welcome to\nUser Management System";
        public static string SignIn => IsTurkish ? "Giriş Yap" : "Sign In";
        public static string SignInSubtitle => IsTurkish ? "Panele erişmek için bilgilerinizi girin" : "Enter your credentials to access panel";
        public static string Username => IsTurkish ? "Kullanıcı Adı" : "Username";
        public static string Password => IsTurkish ? "Şifre" : "Password";
        public static string LoginButton => IsTurkish ? "Giriş Yap" : "Sign In";
        public static string LoginError => IsTurkish ? "Kullanıcı adı veya şifre hatalı!" : "Invalid username or password!";
        public static string LoginWarning => IsTurkish ? "Lütfen kullanıcı adı ve şifre giriniz." : "Please enter username and password.";
        public static string ErrorTitle => IsTurkish ? "Hata" : "Error";
        public static string WarningTitle => IsTurkish ? "Uyarı" : "Warning";

        // Main Form
        public static string UserManagement => IsTurkish ? "Kullanıcı Yönetimi" : "User Management";
        public static string MainDescription => IsTurkish ? "Tüm kullanıcıları tek yerden yönetin. Platformunuzdaki erişimi ve aktiviteyi kontrol edin." : "Manage all users in one place. Control access and monitor activity across your platform.";
        public static string AllStatus => IsTurkish ? "Hepsi" : "All Status";
        public static string Active => IsTurkish ? "Aktif" : "Active";
        public static string Banned => IsTurkish ? "Yasaklı" : "Banned";
        public static string Inactive => IsTurkish ? "Pasif" : "Inactive";
        public static string AddUser => IsTurkish ? "+ Kullanıcı Ekle" : "+ Add User";
        public static string FullName => IsTurkish ? "Ad Soyad" : "Full Name";
        public static string Email => IsTurkish ? "E-posta" : "Email";
        public static string PhoneNumber => IsTurkish ? "Telefon Numarası" : "Phone Number";
        public static string JoinedDate => IsTurkish ? "Kayıt Tarihi" : "Joined Date";
        public static string EndingDate => IsTurkish ? "Bitiş Tarihi" : "Ending Date";
        public static string Status => IsTurkish ? "Durum" : "Status";
        public static string Edit => IsTurkish ? "Düzenle" : "Edit";
        public static string Delete => IsTurkish ? "Sil" : "Delete";
        public static string RowsPerPage => IsTurkish ? "Sayfa başına satır" : "Rows per page";
        public static string OfRows => IsTurkish ? "satır" : "rows";
        public static string DeleteConfirm => IsTurkish ? "'{0}' kullanıcısını silmek istediğinizden emin misiniz?" : "Are you sure you want to delete '{0}'?";
        public static string DeleteUserTitle => IsTurkish ? "Kullanıcı Sil" : "Delete User";

        // User Form
        public static string AddNewUser => IsTurkish ? "Yeni Kullanıcı Ekle" : "Add New User";
        public static string EditUser => IsTurkish ? "Kullanıcı Düzenle" : "Edit User";
        public static string FillInfo => IsTurkish ? "Aşağıdaki bilgileri doldurun" : "Fill in the information below";
        public static string PhoneFormat => IsTurkish ? "Telefon Numarası (xxx) xxx xx xx" : "Phone Number (xxx) xxx xx xx";
        public static string SaveUser => IsTurkish ? "Kaydet" : "Save User";
        public static string Cancel => IsTurkish ? "İptal" : "Cancel";
        
        // Validation Messages
        public static string EnterFullName => IsTurkish ? "Lütfen ad soyad giriniz." : "Please enter the full name.";
        public static string EnterEmail => IsTurkish ? "Lütfen e-posta adresini giriniz." : "Please enter the email address.";
        public static string ValidEmail => IsTurkish ? "Lütfen geçerli bir e-posta adresi giriniz." : "Please enter a valid email address.";
        public static string CompletePhone => IsTurkish ? "Lütfen tam telefon numarası giriniz (10 hane)." : "Please enter a complete phone number (10 digits).";
        public static string EndingBeforeJoined => IsTurkish ? "Bitiş tarihi, kayıt tarihinden önce olamaz." : "Ending date cannot be before the joined date.";
    }
}
