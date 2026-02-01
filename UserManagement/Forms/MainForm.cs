using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using UserManagement.Controls;
using UserManagement.Data;
using UserManagement.Helpers;
using UserManagement.Models;

namespace UserManagement.Forms
{
    public partial class MainForm : Form
    {
        private UserRepository _repository;
        private List<User> _currentUsers;
        private int _currentPage = 1;
        private int _rowsPerPage = 10;
        private string _searchText = "";
        private CustomTitleBar titleBar;

        public MainForm()
        {
            InitializeComponent();
            _repository = UserRepository.Instance;
            SetupCustomTitleBar();
            SetupDataGridView();
            ApplyLanguage();
            LoadUsers();
        }

        private void SetupCustomTitleBar()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            
            titleBar = new CustomTitleBar();
            titleBar.SetTitle(LanguageManager.UserManagement);
            titleBar.LanguageChanged += TitleBar_LanguageChanged;
            titleBar.Dock = DockStyle.Top;
            
            this.Controls.Add(titleBar);

            // Reorder controls for correct Docking layout.
            // Docking priority goes from Back (Highest Index) to Front (Lowest Index).
            // We want TitleBar (Most Back/Outer) -> Header -> Filters -> Footer -> Grid (Most Front/Inner).
            
            titleBar.BringToFront();
            panelHeader.BringToFront();
            panelFilters.BringToFront();
            panelFooter.BringToFront();
            panelGrid.BringToFront();
        }

        private void TitleBar_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLanguage();
            LoadUsers();
        }

        private void ApplyLanguage()
        {
            titleBar.SetTitle(LanguageManager.UserManagement);
            lblTitle.Text = LanguageManager.UserManagement;
            lblDescription.Text = LanguageManager.MainDescription;
            btnAddUser.Text = LanguageManager.AddUser;
            lblRowsPerPage.Text = LanguageManager.RowsPerPage;

            // Update status filter
            int selectedIndex = cmbStatus.SelectedIndex;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add(LanguageManager.AllStatus);
            cmbStatus.Items.Add(LanguageManager.Active);
            cmbStatus.Items.Add(LanguageManager.Banned);
            cmbStatus.Items.Add(LanguageManager.Inactive);
            cmbStatus.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;

            // Update column headers
            if (dgvUsers.Columns.Count > 0)
            {
                dgvUsers.Columns["colFullName"].HeaderText = LanguageManager.FullName;
                dgvUsers.Columns["colEmail"].HeaderText = LanguageManager.Email;
                dgvUsers.Columns["colPhone"].HeaderText = LanguageManager.PhoneNumber;
                dgvUsers.Columns["colJoinedDate"].HeaderText = LanguageManager.JoinedDate;
                dgvUsers.Columns["colEndingDate"].HeaderText = LanguageManager.EndingDate;
                dgvUsers.Columns["colStatus"].HeaderText = LanguageManager.Status;
            }
        }

        private void SetupDataGridView()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            // Style the header
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgvUsers.ColumnHeadersHeight = 45;

            // Style rows
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvUsers.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 65, 85);
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            // Make entire grid read-only
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Full Name
            var fullNameColumn = new DataGridViewTextBoxColumn();
            fullNameColumn.HeaderText = LanguageManager.FullName;
            fullNameColumn.DataPropertyName = "FullName";
            fullNameColumn.Width = 160;
            fullNameColumn.Name = "colFullName";
            fullNameColumn.ReadOnly = true;
            dgvUsers.Columns.Add(fullNameColumn);

            // Email
            var emailColumn = new DataGridViewTextBoxColumn();
            emailColumn.HeaderText = LanguageManager.Email;
            emailColumn.DataPropertyName = "Email";
            emailColumn.Width = 200;
            emailColumn.Name = "colEmail";
            emailColumn.ReadOnly = true;
            dgvUsers.Columns.Add(emailColumn);

            // Phone Number
            var phoneColumn = new DataGridViewTextBoxColumn();
            phoneColumn.HeaderText = LanguageManager.PhoneNumber;
            phoneColumn.DataPropertyName = "PhoneNumber";
            phoneColumn.Width = 150;
            phoneColumn.Name = "colPhone";
            phoneColumn.ReadOnly = true;
            dgvUsers.Columns.Add(phoneColumn);

            // Joined Date
            var joinedDateColumn = new DataGridViewTextBoxColumn();
            joinedDateColumn.HeaderText = LanguageManager.JoinedDate;
            joinedDateColumn.DataPropertyName = "JoinedDate";
            joinedDateColumn.Width = 120;
            joinedDateColumn.Name = "colJoinedDate";
            joinedDateColumn.ReadOnly = true;
            dgvUsers.Columns.Add(joinedDateColumn);

            // Ending Date
            var endingDateColumn = new DataGridViewTextBoxColumn();
            endingDateColumn.HeaderText = LanguageManager.EndingDate;
            endingDateColumn.DataPropertyName = "EndingDate";
            endingDateColumn.Width = 120;
            endingDateColumn.Name = "colEndingDate";
            endingDateColumn.ReadOnly = true;
            dgvUsers.Columns.Add(endingDateColumn);

            // Status
            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.HeaderText = LanguageManager.Status;
            statusColumn.DataPropertyName = "Status";
            statusColumn.Width = 100;
            statusColumn.Name = "colStatus";
            statusColumn.ReadOnly = true;
            dgvUsers.Columns.Add(statusColumn);

            // Edit Button
            var editColumn = new DataGridViewButtonColumn();
            editColumn.HeaderText = "";
            editColumn.Text = LanguageManager.Edit;
            editColumn.UseColumnTextForButtonValue = true;
            editColumn.Width = 70;
            editColumn.Name = "colEdit";
            editColumn.FlatStyle = FlatStyle.Flat;
            dgvUsers.Columns.Add(editColumn);

            // Delete Button
            var deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "";
            deleteColumn.Text = LanguageManager.Delete;
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.Width = 70;
            deleteColumn.Name = "colDelete";
            deleteColumn.FlatStyle = FlatStyle.Flat;
            dgvUsers.Columns.Add(deleteColumn);

            dgvUsers.CellClick += DgvUsers_CellClick;
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvUsers.Columns[e.ColumnIndex].Name == "colJoinedDate" || 
                dgvUsers.Columns[e.ColumnIndex].Name == "colEndingDate")
            {
                if (e.Value is DateTime dt)
                {
                    e.Value = dt.ToString("MMM dd, yyyy");
                    e.FormattingApplied = true;
                }
            }

            // Style status cell based on value
            if (dgvUsers.Columns[e.ColumnIndex].Name == "colStatus")
            {
                var cell = dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.Value != null)
                {
                    var status = (UserStatus)e.Value;
                    switch (status)
                    {
                        case UserStatus.Active:
                            cell.Style.BackColor = Color.FromArgb(220, 252, 231);
                            cell.Style.ForeColor = Color.FromArgb(22, 101, 52);
                            break;
                        case UserStatus.Banned:
                            cell.Style.BackColor = Color.FromArgb(254, 226, 226);
                            cell.Style.ForeColor = Color.FromArgb(185, 28, 28);
                            break;
                        case UserStatus.Inactive:
                            cell.Style.BackColor = Color.FromArgb(229, 231, 235);
                            cell.Style.ForeColor = Color.FromArgb(75, 85, 99);
                            break;
                    }
                    cell.Style.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // Style the buttons
            if (dgvUsers.Columns[e.ColumnIndex].Name == "colEdit")
            {
                var cell = dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.BackColor = Color.FromArgb(79, 70, 229);
                cell.Style.ForeColor = Color.White;
                cell.Style.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvUsers.Columns[e.ColumnIndex].Name == "colDelete")
            {
                var cell = dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.BackColor = Color.FromArgb(239, 68, 68);
                cell.Style.ForeColor = Color.White;
                cell.Style.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _currentUsers.Count) return;

            var user = _currentUsers[e.RowIndex];

            if (dgvUsers.Columns[e.ColumnIndex].Name == "colEdit")
            {
                EditUser(user);
            }
            else if (dgvUsers.Columns[e.ColumnIndex].Name == "colDelete")
            {
                DeleteUser(user);
            }
        }

        private void LoadUsers()
        {
            UserStatus? statusFilter = null;

            if (cmbStatus.SelectedIndex > 0)
            {
                statusFilter = (UserStatus)(cmbStatus.SelectedIndex - 1);
            }

            _currentUsers = _repository.SearchUsers(_searchText, statusFilter);
            
            int totalCount = _currentUsers.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / _rowsPerPage);
            
            if (_currentPage > totalPages && totalPages > 0)
                _currentPage = totalPages;
            if (_currentPage < 1)
                _currentPage = 1;

            int skip = (_currentPage - 1) * _rowsPerPage;
            int take = Math.Min(_rowsPerPage, Math.Max(0, _currentUsers.Count - skip));
            
            List<User> pagedUsers;
            if (skip < _currentUsers.Count)
                pagedUsers = _currentUsers.GetRange(skip, take);
            else
                pagedUsers = new List<User>();

            dgvUsers.DataSource = null;
            dgvUsers.DataSource = pagedUsers;
            _currentUsers = pagedUsers;

            lblRowInfo.Text = $"of {totalCount} {LanguageManager.OfRows}";
            UpdatePagination(totalPages);
        }

        private void UpdatePagination(int totalPages)
        {
            panelPagination.Controls.Clear();

            if (totalPages <= 1) return;

            // Previous button
            AddPaginationButton("◀", _currentPage - 1, _currentPage > 1);

            // Page numbers
            int startPage = Math.Max(1, _currentPage - 2);
            int endPage = Math.Min(totalPages, _currentPage + 2);

            if (startPage > 1)
            {
                AddPaginationButton("1", 1, true);
                if (startPage > 2)
                    AddPaginationButton("...", 0, false);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                AddPaginationButton(i.ToString(), i, true, i == _currentPage);
            }

            if (endPage < totalPages)
            {
                if (endPage < totalPages - 1)
                    AddPaginationButton("...", 0, false);
                AddPaginationButton(totalPages.ToString(), totalPages, true);
            }

            // Next button
            AddPaginationButton("▶", _currentPage + 1, _currentPage < totalPages);
        }

        private void AddPaginationButton(string text, int page, bool enabled, bool isActive = false)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Size = new Size(38, 32);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = enabled ? Cursors.Hand : Cursors.Default;
            btn.Enabled = enabled && page > 0;
            btn.Tag = page;
            btn.Margin = new Padding(2);

            if (isActive)
            {
                btn.BackColor = Color.FromArgb(79, 70, 229);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderSize = 0;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(71, 85, 105);
                btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            }

            btn.Click += (s, e) =>
            {
                if (btn.Tag is int p && p > 0)
                {
                    _currentPage = p;
                    LoadUsers();
                }
            };

            panelPagination.Controls.Add(btn);
        }

        private void EditUser(User user)
        {
            using (var form = new UserForm(user))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repository.UpdateUser(form.User);
                    LoadUsers();
                }
            }
        }

        private void DeleteUser(User user)
        {
            var message = string.Format(LanguageManager.DeleteConfirm, user.FullName);
            var result = MessageBox.Show(
                message,
                LanguageManager.DeleteUserTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _repository.DeleteUser(user.Id);
                LoadUsers();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (var form = new UserForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repository.AddUser(form.User);
                    LoadUsers();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchText = txtSearch.Text.Trim();
            _currentPage = 1;
            LoadUsers();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            LoadUsers();
        }

        private void cmbRowsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbRowsPerPage.Text, out int rows))
            {
                _rowsPerPage = rows;
                _currentPage = 1;
                LoadUsers();
            }
        }
    }
}
