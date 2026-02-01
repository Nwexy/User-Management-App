using System;
using System.Windows.Forms;
using UserManagement.Controls;
using UserManagement.Helpers;

namespace UserManagement.Forms
{
    public partial class LoginForm : Form
    {
        private const string ValidUsername = "nwexy";
        private const string ValidPassword = "nwexy";
        private CustomTitleBar titleBar;

        public LoginForm()
        {
            InitializeComponent();
            SetupCustomTitleBar();
            ApplyLanguage();
        }

        private void SetupCustomTitleBar()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            
            titleBar = new CustomTitleBar();
            titleBar.SetTitle("User Management");
            titleBar.LanguageChanged += TitleBar_LanguageChanged;
            
            this.Controls.Add(titleBar);
            titleBar.BringToFront();
            
            // Adjust main panel position
            panelMain.Top = titleBar.Height;
            panelMain.Height = this.ClientSize.Height - titleBar.Height;
        }

        private void TitleBar_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            titleBar.SetTitle(LanguageManager.UserManagement);
            lblWelcome.Text = LanguageManager.WelcomeTitle;
            lblTitle.Text = LanguageManager.SignIn;
            lblSubtitle.Text = LanguageManager.SignInSubtitle;
            lblUsername.Text = LanguageManager.Username;
            lblPassword.Text = LanguageManager.Password;
            btnLogin.Text = LanguageManager.LoginButton;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(LanguageManager.LoginWarning, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username == ValidUsername && password == ValidPassword)
            {
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, args) => this.Close();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show(LanguageManager.LoginError, LanguageManager.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
