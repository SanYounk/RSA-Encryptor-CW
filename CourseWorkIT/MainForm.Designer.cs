using System;
using System.Numerics;
using System.Windows.Forms;

public partial class MainForm : Form
{
    private RSAAlgorithm rsa;
    private FileManager fileManager;
    private Button btnLoadKeys;
    private TextBox txtPublicKey;
    private TextBox txtPrivateKey;

    public MainForm()
    {
        InitializeComponent();
        rsa = new RSAAlgorithm();
        fileManager = new FileManager();

    }

    private void btnGenerateKeys_Click(object sender, EventArgs e)
    {
        rsa.GenerateKeys(); // Генерируем связанную пару ключей
        fileManager.SaveKeysToFile("publicKey.txt", "privateKey.txt", rsa.PublicKey, rsa.PrivateKey, rsa.Modulus);

        txtPublicKey.Text = $"{rsa.PublicKey},{rsa.Modulus}";
        txtPrivateKey.Text = $"{rsa.PrivateKey},{rsa.Modulus}";

        MessageBox.Show("Keys generated and saved!");
    }



    public void LoadKeysFromFile(string publicKeyPath, string privateKeyPath, out BigInteger publicKey, out BigInteger privateKey, out BigInteger modulus)
    {
        string[] publicKeyData = File.ReadAllLines(publicKeyPath);
        string[] privateKeyData = File.ReadAllLines(privateKeyPath);

        publicKey = BigInteger.Parse(publicKeyData[0]);
        modulus = BigInteger.Parse(publicKeyData[1]);
        privateKey = BigInteger.Parse(privateKeyData[0]);
    }

    private void btnEncrypt_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtMessage.Text))
        {
            string encrypted = FileManager.EncryptText(txtMessage.Text, rsa.PublicKey, rsa.Modulus);
            txtEncrypted.Text = encrypted;
            File.WriteAllText("encrypted.txt", encrypted); // Сохраняем в файл
        }
        else
        {
            MessageBox.Show("Enter text to encrypt!");
        }

    }

    private void btnDecrypt_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEncrypted.Text))
        {
            try
            {
                string decrypted = FileManager.DecryptText(txtEncrypted.Text, rsa.PrivateKey, rsa.Modulus);
                txtDecrypted.Text = decrypted;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while decrypting: " + ex.Message);
            }
        }
        else
        {
            MessageBox.Show("Enter encrypted text!");
        }
    }

    private void btnLoadKeys_Click(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists("publicKey.txt") && File.Exists("privateKey.txt"))
            {
                string[] publicKeyData = File.ReadAllLines("publicKey.txt");
                string[] privateKeyData = File.ReadAllLines("privateKey.txt");

                if (publicKeyData.Length < 2 || privateKeyData.Length < 2)
                {
                    throw new Exception("The key file is not in the correct format.");
                }

                txtPublicKey.Text = $"{publicKeyData[0]},{publicKeyData[1]}";
                txtPrivateKey.Text = $"{privateKeyData[0]},{privateKeyData[1]}";

                SetKeysFromTextFields();
                MessageBox.Show("Keys loaded!");
            }
            else
            {
                MessageBox.Show("Key files not found!");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading keys: " + ex.Message);
        }
    }


    private void SetKeysFromTextFields()
    {
        try
        {
            string[] publicParts = txtPublicKey.Text.Split(',');
            string[] privateParts = txtPrivateKey.Text.Split(',');

            if (publicParts.Length != 2 || privateParts.Length != 2)
            {
                throw new Exception("Invalid key format. Expected two numbers separated by a comma.");
            }

            rsa.PublicKey = BigInteger.Parse(publicParts[0]);
            rsa.Modulus = BigInteger.Parse(publicParts[1]);
            rsa.PrivateKey = BigInteger.Parse(privateParts[0]);

            MessageBox.Show("Keys installed successfully!");
        }
        catch (FormatException)
        {
            MessageBox.Show("Format error: Please enter valid numbers in the text fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (OverflowException)
        {
            MessageBox.Show("Error: The number entered is too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }



    private void btnSetKeys_Click(object sender, EventArgs e)
    {
        try
        {
            // Проверяем, что пользователь ввел корректные ключи
            string[] publicParts = txtPublicKey.Text.Split(',');
            string[] privateParts = txtPrivateKey.Text.Split(',');

            if (publicParts.Length != 2 || privateParts.Length != 2)
            {
                MessageBox.Show("Incorrect key format. Expected two numbers separated by a comma.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Преобразуем строки в BigInteger
            BigInteger publicKey = BigInteger.Parse(publicParts[0]);
            BigInteger modulus = BigInteger.Parse(publicParts[1]);
            BigInteger privateKey = BigInteger.Parse(privateParts[0]);

            // Устанавливаем новые ключи в объект rsa
            rsa.PublicKey = publicKey;
            rsa.PrivateKey = privateKey;
            rsa.Modulus = modulus;

            // Сохраняем обновленные ключи в файлы
            File.WriteAllText("publicKey.txt", $"{publicKey}\n{modulus}");
            File.WriteAllText("privateKey.txt", $"{privateKey}\n{modulus}");

            MessageBox.Show("Keys installed and saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (FormatException)
        {
            MessageBox.Show("Format error: Please enter valid numbers in the text fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (OverflowException)
        {
            MessageBox.Show("Error: The number entered is too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error installing keys: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }






    private Button btnGenerateKeys;
    private Button btnEncrypt;
    private Button btnDecrypt;
    private TextBox txtMessage;
    private TextBox txtEncrypted;
    private TextBox txtDecrypted;

    private void InitializeComponent()
    {
        btnGenerateKeys = new Button();
        btnEncrypt = new Button();
        btnDecrypt = new Button();
        txtMessage = new TextBox();
        txtEncrypted = new TextBox();
        txtDecrypted = new TextBox();
        btnLoadKeys = new Button();
        txtPublicKey = new TextBox();
        txtPrivateKey = new TextBox();
        btnSetKeys = new Button();
        labelPublicKey = new Label();
        labelPrivateKey = new Label();
        pictureBox1 = new PictureBox();
        pictureBox2 = new PictureBox();
        labelHELLOW = new Label();
        labelMessage = new Label();
        labelInputMessage = new Label();
        labelEncryptedMassage = new Label();
        labelDecryptedMassage = new Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
        SuspendLayout();
        // 
        // btnGenerateKeys
        // 
        btnGenerateKeys.BackColor = Color.DodgerBlue;
        btnGenerateKeys.BackgroundImageLayout = ImageLayout.None;
        btnGenerateKeys.Cursor = Cursors.Hand;
        btnGenerateKeys.FlatAppearance.BorderColor = Color.GhostWhite;
        btnGenerateKeys.FlatAppearance.CheckedBackColor = Color.DodgerBlue;
        btnGenerateKeys.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
        btnGenerateKeys.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
        btnGenerateKeys.FlatStyle = FlatStyle.Flat;
        btnGenerateKeys.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        btnGenerateKeys.ForeColor = Color.White;
        btnGenerateKeys.Location = new Point(60, 265);
        btnGenerateKeys.Margin = new Padding(0);
        btnGenerateKeys.Name = "btnGenerateKeys";
        btnGenerateKeys.Size = new Size(180, 25);
        btnGenerateKeys.TabIndex = 0;
        btnGenerateKeys.Text = "GENERATE KEYS";
        btnGenerateKeys.UseVisualStyleBackColor = false;
        btnGenerateKeys.Click += btnGenerateKeys_Click;
        // 
        // btnEncrypt
        // 
        btnEncrypt.BackColor = Color.DodgerBlue;
        btnEncrypt.BackgroundImageLayout = ImageLayout.None;
        btnEncrypt.Cursor = Cursors.Hand;
        btnEncrypt.FlatAppearance.BorderColor = Color.DodgerBlue;
        btnEncrypt.FlatAppearance.BorderSize = 0;
        btnEncrypt.FlatAppearance.CheckedBackColor = Color.DodgerBlue;
        btnEncrypt.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
        btnEncrypt.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
        btnEncrypt.FlatStyle = FlatStyle.Flat;
        btnEncrypt.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        btnEncrypt.ForeColor = Color.White;
        btnEncrypt.Location = new Point(275, 170);
        btnEncrypt.Margin = new Padding(0);
        btnEncrypt.Name = "btnEncrypt";
        btnEncrypt.Size = new Size(80, 25);
        btnEncrypt.TabIndex = 1;
        btnEncrypt.Text = "ENCRYPT";
        btnEncrypt.TextAlign = ContentAlignment.TopLeft;
        btnEncrypt.UseVisualStyleBackColor = false;
        btnEncrypt.Click += btnEncrypt_Click;
        // 
        // btnDecrypt
        // 
        btnDecrypt.BackColor = Color.DodgerBlue;
        btnDecrypt.BackgroundImageLayout = ImageLayout.None;
        btnDecrypt.Cursor = Cursors.Hand;
        btnDecrypt.FlatAppearance.BorderColor = Color.DodgerBlue;
        btnDecrypt.FlatAppearance.BorderSize = 0;
        btnDecrypt.FlatAppearance.CheckedBackColor = Color.DodgerBlue;
        btnDecrypt.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
        btnDecrypt.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
        btnDecrypt.FlatStyle = FlatStyle.Flat;
        btnDecrypt.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        btnDecrypt.ForeColor = Color.White;
        btnDecrypt.Location = new Point(275, 258);
        btnDecrypt.Margin = new Padding(0);
        btnDecrypt.Name = "btnDecrypt";
        btnDecrypt.Size = new Size(80, 25);
        btnDecrypt.TabIndex = 2;
        btnDecrypt.Text = "DECRYPT";
        btnDecrypt.TextAlign = ContentAlignment.TopLeft;
        btnDecrypt.UseVisualStyleBackColor = false;
        btnDecrypt.Click += btnDecrypt_Click;
        // 
        // txtMessage
        // 
        txtMessage.BackColor = SystemColors.InactiveBorder;
        txtMessage.BorderStyle = BorderStyle.FixedSingle;
        txtMessage.Cursor = Cursors.IBeam;
        txtMessage.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        txtMessage.ForeColor = Color.FromArgb(64, 64, 64);
        txtMessage.Location = new Point(275, 140);
        txtMessage.Margin = new Padding(0);
        txtMessage.Name = "txtMessage";
        txtMessage.Size = new Size(294, 22);
        txtMessage.TabIndex = 3;
        // 
        // txtEncrypted
        // 
        txtEncrypted.BackColor = SystemColors.InactiveBorder;
        txtEncrypted.BorderStyle = BorderStyle.FixedSingle;
        txtEncrypted.Cursor = Cursors.IBeam;
        txtEncrypted.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        txtEncrypted.ForeColor = Color.FromArgb(64, 64, 64);
        txtEncrypted.Location = new Point(275, 228);
        txtEncrypted.Margin = new Padding(0);
        txtEncrypted.Name = "txtEncrypted";
        txtEncrypted.Size = new Size(294, 22);
        txtEncrypted.TabIndex = 4;
        // 
        // txtDecrypted
        // 
        txtDecrypted.BackColor = SystemColors.InactiveBorder;
        txtDecrypted.BorderStyle = BorderStyle.FixedSingle;
        txtDecrypted.Cursor = Cursors.IBeam;
        txtDecrypted.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        txtDecrypted.ForeColor = Color.FromArgb(64, 64, 64);
        txtDecrypted.Location = new Point(275, 318);
        txtDecrypted.Margin = new Padding(0);
        txtDecrypted.Name = "txtDecrypted";
        txtDecrypted.Size = new Size(294, 22);
        txtDecrypted.TabIndex = 5;
        // 
        // btnLoadKeys
        // 
        btnLoadKeys.BackColor = Color.DodgerBlue;
        btnLoadKeys.BackgroundImageLayout = ImageLayout.None;
        btnLoadKeys.Cursor = Cursors.Hand;
        btnLoadKeys.FlatAppearance.BorderColor = Color.GhostWhite;
        btnLoadKeys.FlatAppearance.CheckedBackColor = Color.DodgerBlue;
        btnLoadKeys.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
        btnLoadKeys.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
        btnLoadKeys.FlatStyle = FlatStyle.Flat;
        btnLoadKeys.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        btnLoadKeys.ForeColor = Color.White;
        btnLoadKeys.Location = new Point(60, 305);
        btnLoadKeys.Margin = new Padding(0);
        btnLoadKeys.Name = "btnLoadKeys";
        btnLoadKeys.Size = new Size(180, 25);
        btnLoadKeys.TabIndex = 6;
        btnLoadKeys.Text = "LOAD KEYS";
        btnLoadKeys.UseVisualStyleBackColor = false;
        btnLoadKeys.Click += btnLoadKeys_Click;
        // 
        // txtPublicKey
        // 
        txtPublicKey.BackColor = Color.White;
        txtPublicKey.BorderStyle = BorderStyle.FixedSingle;
        txtPublicKey.Cursor = Cursors.IBeam;
        txtPublicKey.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        txtPublicKey.ForeColor = Color.FromArgb(64, 64, 64);
        txtPublicKey.Location = new Point(60, 140);
        txtPublicKey.Margin = new Padding(0);
        txtPublicKey.Name = "txtPublicKey";
        txtPublicKey.Size = new Size(180, 22);
        txtPublicKey.TabIndex = 7;
        // 
        // txtPrivateKey
        // 
        txtPrivateKey.BackColor = Color.White;
        txtPrivateKey.BorderStyle = BorderStyle.FixedSingle;
        txtPrivateKey.Cursor = Cursors.IBeam;
        txtPrivateKey.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        txtPrivateKey.ForeColor = Color.FromArgb(64, 64, 64);
        txtPrivateKey.Location = new Point(60, 190);
        txtPrivateKey.Margin = new Padding(0);
        txtPrivateKey.Name = "txtPrivateKey";
        txtPrivateKey.Size = new Size(180, 22);
        txtPrivateKey.TabIndex = 8;
        // 
        // btnSetKeys
        // 
        btnSetKeys.BackColor = Color.DodgerBlue;
        btnSetKeys.BackgroundImageLayout = ImageLayout.None;
        btnSetKeys.Cursor = Cursors.Hand;
        btnSetKeys.FlatAppearance.BorderColor = Color.GhostWhite;
        btnSetKeys.FlatAppearance.CheckedBackColor = Color.DodgerBlue;
        btnSetKeys.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
        btnSetKeys.FlatAppearance.MouseOverBackColor = Color.DodgerBlue;
        btnSetKeys.FlatStyle = FlatStyle.Flat;
        btnSetKeys.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold);
        btnSetKeys.ForeColor = Color.White;
        btnSetKeys.Location = new Point(60, 225);
        btnSetKeys.Margin = new Padding(0);
        btnSetKeys.Name = "btnSetKeys";
        btnSetKeys.Size = new Size(180, 25);
        btnSetKeys.TabIndex = 9;
        btnSetKeys.Text = "SET KEYS\n";
        btnSetKeys.UseVisualStyleBackColor = false;
        btnSetKeys.Click += btnSetKeys_Click;
        // 
        // labelPublicKey
        // 
        labelPublicKey.AutoSize = true;
        labelPublicKey.BackColor = Color.DodgerBlue;
        labelPublicKey.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelPublicKey.ForeColor = Color.White;
        labelPublicKey.Location = new Point(108, 120);
        labelPublicKey.Name = "labelPublicKey";
        labelPublicKey.Size = new Size(83, 18);
        labelPublicKey.TabIndex = 10;
        labelPublicKey.Text = "PUBLIC KEY";
        // 
        // labelPrivateKey
        // 
        labelPrivateKey.AutoSize = true;
        labelPrivateKey.BackColor = Color.DodgerBlue;
        labelPrivateKey.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelPrivateKey.ForeColor = Color.White;
        labelPrivateKey.Location = new Point(104, 170);
        labelPrivateKey.Name = "labelPrivateKey";
        labelPrivateKey.Size = new Size(92, 18);
        labelPrivateKey.TabIndex = 11;
        labelPrivateKey.Text = "PRIVATE KEY";
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = Color.White;
        pictureBox1.Enabled = false;
        pictureBox1.Location = new Point(100, 50);
        pictureBox1.Margin = new Padding(0);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(494, 318);
        pictureBox1.TabIndex = 12;
        pictureBox1.TabStop = false;
        // 
        // pictureBox2
        // 
        pictureBox2.BackColor = Color.DodgerBlue;
        pictureBox2.Enabled = false;
        pictureBox2.Location = new Point(50, 75);
        pictureBox2.Margin = new Padding(0);
        pictureBox2.Name = "pictureBox2";
        pictureBox2.Size = new Size(200, 268);
        pictureBox2.TabIndex = 13;
        pictureBox2.TabStop = false;
        // 
        // labelHELLOW
        // 
        labelHELLOW.AutoSize = true;
        labelHELLOW.BackColor = Color.DodgerBlue;
        labelHELLOW.Font = new Font("Montserrat ExtraBold", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelHELLOW.ForeColor = Color.White;
        labelHELLOW.Location = new Point(108, 80);
        labelHELLOW.Name = "labelHELLOW";
        labelHELLOW.Size = new Size(84, 38);
        labelHELLOW.TabIndex = 14;
        labelHELLOW.Text = "KEYS";
        // 
        // labelMessage
        // 
        labelMessage.AutoSize = true;
        labelMessage.BackColor = Color.White;
        labelMessage.Font = new Font("Montserrat ExtraBold", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelMessage.ForeColor = Color.DodgerBlue;
        labelMessage.Location = new Point(275, 80);
        labelMessage.Margin = new Padding(0);
        labelMessage.Name = "labelMessage";
        labelMessage.Size = new Size(160, 38);
        labelMessage.TabIndex = 15;
        labelMessage.Text = "MASSAGES";
        // 
        // labelInputMessage
        // 
        labelInputMessage.AutoSize = true;
        labelInputMessage.BackColor = Color.White;
        labelInputMessage.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelInputMessage.ForeColor = Color.DodgerBlue;
        labelInputMessage.Location = new Point(275, 120);
        labelInputMessage.Name = "labelInputMessage";
        labelInputMessage.Size = new Size(113, 18);
        labelInputMessage.TabIndex = 16;
        labelInputMessage.Text = "INPUT MASSAGE";
        // 
        // labelEncryptedMassage
        // 
        labelEncryptedMassage.AutoSize = true;
        labelEncryptedMassage.BackColor = Color.White;
        labelEncryptedMassage.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelEncryptedMassage.ForeColor = Color.DodgerBlue;
        labelEncryptedMassage.Location = new Point(275, 208);
        labelEncryptedMassage.Name = "labelEncryptedMassage";
        labelEncryptedMassage.Size = new Size(151, 18);
        labelEncryptedMassage.TabIndex = 17;
        labelEncryptedMassage.Text = "ENCRYPTED MESSAGE";
        // 
        // labelDecryptedMassage
        // 
        labelDecryptedMassage.AutoSize = true;
        labelDecryptedMassage.BackColor = Color.White;
        labelDecryptedMassage.Font = new Font("Montserrat ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 204);
        labelDecryptedMassage.ForeColor = Color.DodgerBlue;
        labelDecryptedMassage.Location = new Point(275, 298);
        labelDecryptedMassage.Name = "labelDecryptedMassage";
        labelDecryptedMassage.Size = new Size(152, 18);
        labelDecryptedMassage.TabIndex = 18;
        labelDecryptedMassage.Text = "DECRYPTED MASSAGE";
        // 
        // MainForm
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(64, 64, 64);
        BackgroundImage = CourseWorkIT.Properties.Resources._360_F_216875193_vU9z9BoOyZwEiPjpJlVIxvKCDtiHVjcG;
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(644, 418);
        Controls.Add(labelDecryptedMassage);
        Controls.Add(labelEncryptedMassage);
        Controls.Add(labelInputMessage);
        Controls.Add(labelMessage);
        Controls.Add(btnSetKeys);
        Controls.Add(btnLoadKeys);
        Controls.Add(btnGenerateKeys);
        Controls.Add(txtPrivateKey);
        Controls.Add(labelPrivateKey);
        Controls.Add(txtPublicKey);
        Controls.Add(labelHELLOW);
        Controls.Add(labelPublicKey);
        Controls.Add(pictureBox2);
        Controls.Add(txtDecrypted);
        Controls.Add(txtEncrypted);
        Controls.Add(txtMessage);
        Controls.Add(btnDecrypt);
        Controls.Add(btnEncrypt);
        Controls.Add(pictureBox1);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = "MainForm";
        Opacity = 0.9D;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    private Button btnSetKeys;
    private Label labelPublicKey;
    private Label labelPrivateKey;
    private PictureBox pictureBox1;
    private PictureBox pictureBox2;
    private Label labelHELLOW;
    private Label labelMessage;
    private Label labelInputMessage;
    private Label labelEncryptedMassage;
    private Label labelDecryptedMassage;
}