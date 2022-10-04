// Login.cs Cheeto 004

// if else, login table, message box

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPwd.Text != string.Empty || txtUsername.Text != string.Empty)
            {

                cmd = new SqlCommand("select * from LoginTable where username='" + txtUsername.Text + "' and password='" + txtPwd.Text + "'", cn);
                dr = cmd.ExecuteReader();

            }
            else
            {
                MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}


