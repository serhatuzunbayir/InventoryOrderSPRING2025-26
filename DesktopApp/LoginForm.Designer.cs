namespace DesktopApp;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;

    private Label lblBaseUrl;
    private TextBox txtBaseUrl;
    private TabControl tabAuth;
    private TabPage tabLogin;
    private TabPage tabRegister;

    private Label lblLoginUser;
    private TextBox txtLoginUser;
    private Label lblLoginPass;
    private TextBox txtLoginPass;
    private Button btnLogin;
    private Label lblLoginHint;

    private Label lblRegUser;
    private TextBox txtRegUser;
    private Label lblRegPass;
    private TextBox txtRegPass;
    private Label lblRegEmail;
    private TextBox txtRegEmail;
    private Label lblRegPhone;
    private TextBox txtRegPhone;
    private Label lblRegFirst;
    private TextBox txtRegFirst;
    private Label lblRegLast;
    private TextBox txtRegLast;
    private Button btnRegister;
    private Label lblRegHint;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblBaseUrl = new Label();
        txtBaseUrl = new TextBox();
        tabAuth = new TabControl();
        tabLogin = new TabPage();
        tabRegister = new TabPage();

        lblLoginUser = new Label();
        txtLoginUser = new TextBox();
        lblLoginPass = new Label();
        txtLoginPass = new TextBox();
        btnLogin = new Button();
        lblLoginHint = new Label();

        lblRegUser = new Label();
        txtRegUser = new TextBox();
        lblRegPass = new Label();
        txtRegPass = new TextBox();
        lblRegEmail = new Label();
        txtRegEmail = new TextBox();
        lblRegPhone = new Label();
        txtRegPhone = new TextBox();
        lblRegFirst = new Label();
        txtRegFirst = new TextBox();
        lblRegLast = new Label();
        txtRegLast = new TextBox();
        btnRegister = new Button();
        lblRegHint = new Label();

        SuspendLayout();

        lblBaseUrl.AutoSize = true;
        lblBaseUrl.Location = new Point(16, 16);
        lblBaseUrl.Name = "lblBaseUrl";
        lblBaseUrl.Size = new Size(84, 15);
        lblBaseUrl.Text = "API Base URL";

        txtBaseUrl.Location = new Point(120, 12);
        txtBaseUrl.Name = "txtBaseUrl";
        txtBaseUrl.Size = new Size(420, 23);
        txtBaseUrl.Text = "http://localhost:5000";

        tabAuth.Location = new Point(16, 48);
        tabAuth.Name = "tabAuth";
        tabAuth.Size = new Size(524, 300);
        tabAuth.TabPages.Add(tabLogin);
        tabAuth.TabPages.Add(tabRegister);

        tabLogin.Text = "Staff Login";
        tabLogin.UseVisualStyleBackColor = true;

        lblLoginUser.AutoSize = true;
        lblLoginUser.Location = new Point(24, 28);
        lblLoginUser.Text = "Username";

        txtLoginUser.Location = new Point(120, 24);
        txtLoginUser.Size = new Size(280, 23);

        lblLoginPass.AutoSize = true;
        lblLoginPass.Location = new Point(24, 68);
        lblLoginPass.Text = "Password";

        txtLoginPass.Location = new Point(120, 64);
        txtLoginPass.Size = new Size(280, 23);
        txtLoginPass.UseSystemPasswordChar = true;

        btnLogin.Location = new Point(120, 104);
        btnLogin.Size = new Size(120, 28);
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = true;

        lblLoginHint.AutoSize = true;
        lblLoginHint.Location = new Point(120, 148);
        lblLoginHint.Size = new Size(240, 15);
        lblLoginHint.Text = "Only staff accounts can access this app.";

        tabLogin.Controls.Add(lblLoginUser);
        tabLogin.Controls.Add(txtLoginUser);
        tabLogin.Controls.Add(lblLoginPass);
        tabLogin.Controls.Add(txtLoginPass);
        tabLogin.Controls.Add(btnLogin);
        tabLogin.Controls.Add(lblLoginHint);

        tabRegister.Text = "Staff Register";
        tabRegister.UseVisualStyleBackColor = true;

        lblRegUser.AutoSize = true;
        lblRegUser.Location = new Point(24, 20);
        lblRegUser.Text = "Username";

        txtRegUser.Location = new Point(120, 16);
        txtRegUser.Size = new Size(280, 23);

        lblRegPass.AutoSize = true;
        lblRegPass.Location = new Point(24, 56);
        lblRegPass.Text = "Password";

        txtRegPass.Location = new Point(120, 52);
        txtRegPass.Size = new Size(280, 23);
        txtRegPass.UseSystemPasswordChar = true;

        lblRegEmail.AutoSize = true;
        lblRegEmail.Location = new Point(24, 92);
        lblRegEmail.Text = "Email";

        txtRegEmail.Location = new Point(120, 88);
        txtRegEmail.Size = new Size(280, 23);

        lblRegPhone.AutoSize = true;
        lblRegPhone.Location = new Point(24, 128);
        lblRegPhone.Text = "Phone";

        txtRegPhone.Location = new Point(120, 124);
        txtRegPhone.Size = new Size(280, 23);

        lblRegFirst.AutoSize = true;
        lblRegFirst.Location = new Point(24, 164);
        lblRegFirst.Text = "First Name";

        txtRegFirst.Location = new Point(120, 160);
        txtRegFirst.Size = new Size(280, 23);

        lblRegLast.AutoSize = true;
        lblRegLast.Location = new Point(24, 200);
        lblRegLast.Text = "Last Name";

        txtRegLast.Location = new Point(120, 196);
        txtRegLast.Size = new Size(280, 23);

        btnRegister.Location = new Point(120, 232);
        btnRegister.Size = new Size(120, 28);
        btnRegister.Text = "Register";
        btnRegister.UseVisualStyleBackColor = true;

        lblRegHint.AutoSize = true;
        lblRegHint.Location = new Point(250, 238);
        lblRegHint.Text = "Creates a staff-only account. For testing";

        tabRegister.Controls.Add(lblRegUser);
        tabRegister.Controls.Add(txtRegUser);
        tabRegister.Controls.Add(lblRegPass);
        tabRegister.Controls.Add(txtRegPass);
        tabRegister.Controls.Add(lblRegEmail);
        tabRegister.Controls.Add(txtRegEmail);
        tabRegister.Controls.Add(lblRegPhone);
        tabRegister.Controls.Add(txtRegPhone);
        tabRegister.Controls.Add(lblRegFirst);
        tabRegister.Controls.Add(txtRegFirst);
        tabRegister.Controls.Add(lblRegLast);
        tabRegister.Controls.Add(txtRegLast);
        tabRegister.Controls.Add(btnRegister);
        tabRegister.Controls.Add(lblRegHint);

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(560, 370);
        Controls.Add(lblBaseUrl);
        Controls.Add(txtBaseUrl);
        Controls.Add(tabAuth);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Staff Authentication";

        ResumeLayout(false);
        PerformLayout();
    }
}

