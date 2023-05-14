﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDo_List
{
    public partial class frmRegister : Form
    {
        private JsonFileManager jsonManager;

        public frmRegister()
        {
            InitializeComponent();

            //Создание экземпляра класса JsonFileManager с указанием пути к файлу JSON
            jsonManager = new JsonFileManager("users.json");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "" || txtPassword.Text == "" || txtComPas.Text == "") // Пустые поля
            {
                MessageBox.Show("Username or Passwords fields are empty", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassword.Text == txtComPas.Text)
            {
                if (jsonManager.IsEmailExists(txtEmail.Text))
                {
                    MessageBox.Show("Your Account has been Created Earlier, Please Try to Login", "Registration Succes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    jsonManager.AddUser(new User(txtEmail.Text, txtPassword.Text));
                    MessageBox.Show("Your Account has been Successfully Created", "Registration Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }     
            }
            else
            {
                MessageBox.Show("Passwords does not match, Please Re-enter", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtComPas.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
                txtComPas.PasswordChar = '•';
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtComPas.Text = "";
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            this.Hide();
        }
    }
}