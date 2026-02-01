using System;
using System.Windows.Forms;
using UserManagement.Controls;
using UserManagement.Helpers;
using UserManagement.Models;

namespace UserManagement.Forms
{
    public partial class UserForm : Form
    {
        public User User { get; private set; }
        private bool _isEditMode;
        private CustomTitleBar titleBar;

        public UserForm()
        {
            InitializeComponent();
            _isEditMode = false;
            User = new User();
            
            SetupCustomTitleBar();
            
            // Hide status for new users
            lblStatus.Visible = false;
            cmbStatus.Visible = false;
            
            SetupPhoneMask();
            ApplyLanguage();
        }

        public UserForm(User user)
        {
            InitializeComponent();
            _isEditMode = true;
            User = user;
            
            SetupCustomTitleBar();
            
            // Show status for editing
            lblStatus.Visible = true;
            cmbStatus.Visible = true;
            
            SetupPhoneMask();
            ApplyLanguage();
            LoadUserData();
        }

        private void SetupCustomTitleBar()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            
            // Hide the duplicate purple header since we have custom title bar
            panelHeader.Visible = false;
            
            titleBar = new CustomTitleBar();
            titleBar.SetTitle(_isEditMode ? LanguageManager.EditUser : LanguageManager.AddNewUser);
            titleBar.HideMinimize();
            titleBar.LanguageChanged += TitleBar_LanguageChanged;
            titleBar.Dock = DockStyle.Top;
            
            // Add title bar directly to form (not to panelMain)
            this.Controls.Add(titleBar);
            this.Controls.SetChildIndex(titleBar, 0);
            
            // Adjust panelMain to not use Dock.Fill, position it below title bar
            panelMain.Dock = DockStyle.None;
            panelMain.Location = new System.Drawing.Point(0, titleBar.Height);
            panelMain.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height - titleBar.Height);
            panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void TitleBar_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            if (_isEditMode)
            {
                titleBar.SetTitle(LanguageManager.EditUser);
                lblFormTitle.Text = LanguageManager.EditUser;
            }
            else
            {
                titleBar.SetTitle(LanguageManager.AddNewUser);
                lblFormTitle.Text = LanguageManager.AddNewUser;
            }
            
            lblSubtitle.Text = LanguageManager.FillInfo;
            lblFullName.Text = LanguageManager.FullName;
            lblEmail.Text = LanguageManager.Email;
            lblPhoneNumber.Text = LanguageManager.PhoneFormat;
            lblJoinedDate.Text = LanguageManager.JoinedDate;
            lblEndingDate.Text = LanguageManager.EndingDate;
            lblStatus.Text = LanguageManager.Status;
            btnSave.Text = LanguageManager.SaveUser;
            btnCancel.Text = LanguageManager.Cancel;

            // Update status combobox
            int selectedIndex = cmbStatus.SelectedIndex;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add(LanguageManager.Active);
            cmbStatus.Items.Add(LanguageManager.Banned);
            cmbStatus.Items.Add(LanguageManager.Inactive);
            cmbStatus.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
        }

        private void SetupPhoneMask()
        {
            txtPhoneNumber.MaxLength = 15;
            txtPhoneNumber.KeyPress += TxtPhoneNumber_KeyPress;
            txtPhoneNumber.TextChanged += TxtPhoneNumber_TextChanged;
        }

        private void TxtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and control characters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            // Remove formatting to get raw digits
            string rawDigits = GetRawPhoneDigits(txtPhoneNumber.Text);
            
            // Limit to 10 digits
            if (rawDigits.Length > 10)
                rawDigits = rawDigits.Substring(0, 10);

            // Format the phone number
            string formatted = FormatPhoneNumber(rawDigits);
            
            // Prevent infinite loop
            if (txtPhoneNumber.Text != formatted)
            {
                int cursorPos = txtPhoneNumber.SelectionStart;
                txtPhoneNumber.TextChanged -= TxtPhoneNumber_TextChanged;
                txtPhoneNumber.Text = formatted;
                txtPhoneNumber.SelectionStart = Math.Min(cursorPos + 1, formatted.Length);
                txtPhoneNumber.TextChanged += TxtPhoneNumber_TextChanged;
            }
        }

        private string GetRawPhoneDigits(string text)
        {
            string result = "";
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    result += c;
            }
            return result;
        }

        private string FormatPhoneNumber(string digits)
        {
            // Format: xxx xxx xx xx
            if (string.IsNullOrEmpty(digits))
                return "";

            if (digits.Length <= 3)
                return digits;
            else if (digits.Length <= 6)
                return digits.Substring(0, 3) + " " + digits.Substring(3);
            else if (digits.Length <= 8)
                return digits.Substring(0, 3) + " " + digits.Substring(3, 3) + " " + digits.Substring(6);
            else
                return digits.Substring(0, 3) + " " + digits.Substring(3, 3) + " " + digits.Substring(6, 2) + " " + digits.Substring(8);
        }

        private void LoadUserData()
        {
            txtFullName.Text = User.FullName;
            txtEmail.Text = User.Email;
            
            // Load phone number - extract digits and reformat
            string rawDigits = GetRawPhoneDigits(User.PhoneNumber);
            txtPhoneNumber.Text = FormatPhoneNumber(rawDigits);
            
            dtpJoinedDate.Value = User.JoinedDate;
            dtpEndingDate.Value = User.EndingDate;
            cmbStatus.SelectedIndex = (int)User.Status;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            User.FullName = txtFullName.Text.Trim();
            User.Email = txtEmail.Text.Trim();
            User.PhoneNumber = txtPhoneNumber.Text.Trim(); // Save formatted phone number
            User.JoinedDate = dtpJoinedDate.Value;
            User.EndingDate = dtpEndingDate.Value;

            if (_isEditMode)
            {
                // Set status from combobox when editing
                User.Status = (UserStatus)cmbStatus.SelectedIndex;
            }
            else
            {
                // Set Active for new users
                User.Status = UserStatus.Active;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show(LanguageManager.EnterFullName, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(LanguageManager.EnterEmail, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show(LanguageManager.ValidEmail, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            string rawDigits = GetRawPhoneDigits(txtPhoneNumber.Text);
            if (rawDigits.Length < 10)
            {
                MessageBox.Show(LanguageManager.CompletePhone, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhoneNumber.Focus();
                return false;
            }

            if (dtpEndingDate.Value < dtpJoinedDate.Value)
            {
                MessageBox.Show(LanguageManager.EndingBeforeJoined, LanguageManager.WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndingDate.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
