using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UserManagement.Controls
{
    public class CustomTitleBar : Panel
    {
        private Label lblTitle;
        private Button btnClose;
        private Button btnMinimize;

        private Panel panelLanguage;
        private Button btnTR;
        private Button btnEN;
        private Form _parentForm;
        private Point _dragStart;
        private bool _isDragging;

        public event EventHandler LanguageChanged;

        public CustomTitleBar()
        {
            this.Height = 40;
            this.Dock = DockStyle.Top;
            this.BackColor = Color.FromArgb(30, 27, 75);
            
            InitializeComponents();
            
            this.MouseDown += TitleBar_MouseDown;
            this.MouseMove += TitleBar_MouseMove;
            this.MouseUp += TitleBar_MouseUp;
        }

        private void InitializeComponents()
        {
            // Language Panel
            panelLanguage = new Panel();
            panelLanguage.Size = new Size(80, 28);
            panelLanguage.Location = new Point(10, 6);
            panelLanguage.BackColor = Color.FromArgb(55, 48, 107);

            btnTR = new Button();
            btnTR.Text = "TR";
            btnTR.Size = new Size(38, 26);
            btnTR.Location = new Point(1, 1);
            btnTR.FlatStyle = FlatStyle.Flat;
            btnTR.FlatAppearance.BorderSize = 0;
            btnTR.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnTR.Cursor = Cursors.Hand;
            btnTR.BackColor = Color.FromArgb(55, 48, 107);
            btnTR.ForeColor = Color.White;
            btnTR.Click += BtnTR_Click;

            btnEN = new Button();
            btnEN.Text = "EN";
            btnEN.Size = new Size(38, 26);
            btnEN.Location = new Point(40, 1);
            btnEN.FlatStyle = FlatStyle.Flat;
            btnEN.FlatAppearance.BorderSize = 0;
            btnEN.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnEN.Cursor = Cursors.Hand;
            btnEN.BackColor = Color.FromArgb(79, 70, 229);
            btnEN.ForeColor = Color.White;
            btnEN.Click += BtnEN_Click;

            panelLanguage.Controls.Add(btnTR);
            panelLanguage.Controls.Add(btnEN);

            // Title
            lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(100, 10);
            lblTitle.Text = "User Management";
            
            lblTitle.MouseDown += TitleBar_MouseDown;
            lblTitle.MouseMove += TitleBar_MouseMove;
            lblTitle.MouseUp += TitleBar_MouseUp;

            // Close Button
            btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.Size = new Size(40, 40);
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.FromArgb(30, 27, 75);
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += BtnClose_Click;
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(239, 68, 68);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.FromArgb(30, 27, 75);



            // Minimize Button
            btnMinimize = new Button();
            btnMinimize.Text = "—";
            btnMinimize.Size = new Size(40, 40);
            btnMinimize.Dock = DockStyle.Right;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Font = new Font("Segoe UI", 10F);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.BackColor = Color.FromArgb(30, 27, 75);
            btnMinimize.Cursor = Cursors.Hand;
            btnMinimize.Click += BtnMinimize_Click;
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.FromArgb(55, 48, 107);
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.FromArgb(30, 27, 75);

            // Add buttons in reverse order (rightmost first due to Dock.Right)
            this.Controls.Add(btnClose);      // Rightmost - Close

            this.Controls.Add(btnMinimize);   // Left - Minimize
            this.Controls.Add(panelLanguage);
            this.Controls.Add(lblTitle);
            
            UpdateLanguageButtons();
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }



        public void HideMinimize()
        {
            btnMinimize.Visible = false;
        }

        private void UpdateLanguageButtons()
        {
            if (Helpers.LanguageManager.IsTurkish)
            {
                btnTR.BackColor = Color.FromArgb(79, 70, 229);
                btnEN.BackColor = Color.FromArgb(55, 48, 107);
            }
            else
            {
                btnEN.BackColor = Color.FromArgb(79, 70, 229);
                btnTR.BackColor = Color.FromArgb(55, 48, 107);
            }
        }

        private void BtnTR_Click(object sender, EventArgs e)
        {
            Helpers.LanguageManager.IsTurkish = true;
            UpdateLanguageButtons();
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnEN_Click(object sender, EventArgs e)
        {
            Helpers.LanguageManager.IsTurkish = false;
            UpdateLanguageButtons();
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            _parentForm?.Close();
        }



        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
                _parentForm.WindowState = FormWindowState.Minimized;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStart = e.Location;
            }
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _parentForm != null)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                _parentForm.Location = new Point(
                    currentScreenPos.X - _dragStart.X,
                    currentScreenPos.Y - _dragStart.Y
                );
            }
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            _parentForm = this.FindForm();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw gradient
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(30, 27, 75),
                Color.FromArgb(55, 48, 107),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
