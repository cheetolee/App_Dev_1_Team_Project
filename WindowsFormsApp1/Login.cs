﻿// Login.cs FINAL Cheeto 010

// private void button1_Click

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        private SqlConnection cn;
        private SqlCommand cmd;
        private SqlDataReader dr;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =\\Mac\Home\Documents\database.mdf; Integrated Security = True; Connect Timeout = 30; Integrated Security=True");
            cn.Open();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPwd.Text != string.Empty || txtUsername.Text != string.Empty)
            {

                cmd = new SqlCommand("select * from LoginTable where username='" + txtUsername.Text + "' and password='" + txtPwd.Text + "'", cn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    OrderManagement home = new OrderManagement();
                    home.ShowDialog();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("No Account avilable with this username and password ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registration registration = new Registration();
            registration.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

}


