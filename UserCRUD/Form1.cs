using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UserCRUD
{
    public partial class Form1 : Form
    {
        private Connect conn = new Connect();
        public Form1()
        {
            InitializeComponent();
            ControlBox = false;
            radioButton1.Checked = true;
            hideReg();
            feltolt();
        }

        private static int userId = 0;
        private void button1_Click(object sender, System.EventArgs e)
        {
            string[] darabol = textBox1.Text.Split(' ');

            if (beleptet(darabol[1], darabol[0], textBox2.Text) == true)
            {
                MessageBox.Show("Regisztrált tag.");
            }
            else
            {
                MessageBox.Show("Nem regisztrált tag.");
                showReg();
                string[] darabol2 = textBox1.Text.Split(' ');
                textBox3.Text = darabol2[1];
                textBox4.Text = darabol2[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true && textBox5.Text == textBox6.Text)
            {
                MessageBox.Show(frissit(userId, textBox3.Text, textBox4.Text, textBox5.Text));
                listBox1.Items.Clear();
                feltolt();
                hideReg();
            }
            else if(textBox5.Text == textBox6.Text && radioButton1.Checked == false)
            {
                MessageBox.Show(regisztral(textBox3.Text, textBox4.Text, textBox5.Text));
                hideReg();
            }
          
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string[] getId = listBox1.SelectedItem.ToString().Split('.');
            userId = int.Parse(getId[0].TrimEnd());

            if (radioButton2.Checked == true)
            {
                MessageBox.Show(torol());
                listBox1.Items.Clear();
                feltolt();
            }
            else
            {
                showReg();
                string[] darabolNev = listBox1.SelectedItem.ToString().Split(' ');
                textBox4.Text = darabolNev[1];
                textBox3.Text = darabolNev[2];
            }
        }

        private bool beleptet(string firstName, string lastName, string pass)
        {
            conn.Connection.Open();
            string sql = $"SELECT `Id` FROM `data` WHERE `FirstName`= '{firstName}' and `LastName`= '{lastName}' and `Password`= '{pass}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            bool van = dr.Read();
            conn.Connection.Close();
            return van;
        }

        private string regisztral(string firstName, string lastName, string pass)
        {
            conn.Connection.Open();
            string sql = $"INSERT INTO `data`(`FirstName`, `LastName`, `Password`, `CreatedTime`, `UpdatedTime`) VALUES ('{firstName}','{lastName}','{pass}','{DateTime.Now.ToString("yyyy-MM-dd")}','{DateTime.Now.ToString("yyyy-MM-dd")}')";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            var result = cmd.ExecuteNonQuery();
            conn.Connection.Close();
            listBox1.Items.Clear();
            feltolt();
            return result > 0 ? "Sikeres regisztráció" : "Sikertelen Regisztráció.";
        }

        private string torol()
        {
            conn.Connection.Open();
            string sql = $"DELETE FROM `data` WHERE `Id`= '{userId}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            var result = cmd.ExecuteNonQuery();
            conn.Connection.Close();
            return result > 0 ? "Sikeres törlés" : "Sikertelen törlés.";
        }


        private string frissit(int Id, string FirstName, string LastName, string Password)
        {
            conn.Connection.Open();
            string sql = $"UPDATE `data` SET `FirstName`='{FirstName}',`LastName`='{LastName}',`Password`='{Password}' WHERE `Id`= {Id}";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            var result = cmd.ExecuteNonQuery();
            conn.Connection.Close();
            return result > 0 ? "Sikeres frissítés" : "Sikertelen frissítés.";
        }

        private void feltolt()
        {
            conn.Connection.Open();
            string sql = $"SELECT `Id`,`LastName`,`FirstName`,`CreatedTime`,`UpdatedTime` FROM `data`; ";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read()) 
            { 
                listBox1.Items.Add($"{dr.GetInt32(0)}. {dr.GetString(1)} {dr.GetString(2)}  {dr.GetDateTime(3).ToString("yyyy-MM-dd")} {dr.GetDateTime(4).ToString("yyy-MM-dd")}");
            }
            conn.Connection.Close();
        }
        private void hideReg()
        {
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = false;
            textBox3.Visible = textBox4.Visible = textBox5.Visible = textBox6.Visible = false;
            button2.Visible = false;
        }

        private void showReg()
        {
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = true;
            textBox3.Visible = textBox4.Visible = textBox5.Visible = textBox6.Visible = true;
            button2.Visible = true;
        }

       
    }
}
