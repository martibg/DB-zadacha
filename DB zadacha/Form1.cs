using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DB_zadacha
{
    public partial class Form1 : Form
    {
        string connStr = "server=localhost;user=root;database=StudentsDB;port=3306;password= Qwe1209poi!;";
        MySqlConnection conn;
        int selectedID = -1;
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connStr);
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadData()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM Students";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при зареждане: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out int age, out decimal grade)) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "INSERT INTO Students(Name, Age, Class, AverageGrade) VALUES (@name, @age, @class, @grade)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@class", txtClass.Text);
                    cmd.Parameters.AddWithValue("@grade", grade);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при добавяне: " + ex.Message);
            }
        }

        

        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedID = Convert.ToInt32(row.Cells["ID"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                txtClass.Text = row.Cells["Class"].Value.ToString();
                txtGrade.Text = row.Cells["AverageGrade"].Value.ToString();
            }
        }
        
        

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Моля, изберете ученик от таблицата.");
                return;
            }

            if (!ValidateInputs(out int age, out decimal grade)) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "UPDATE Students SET Name=@name, Age=@age, Class=@class, AverageGrade=@grade WHERE ID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@class", txtClass.Text);
                    cmd.Parameters.AddWithValue("@grade", grade);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при редактиране: " + ex.Message);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Моля, изберете ученик от таблицата.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = "DELETE FROM Students WHERE ID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при изтриване: " + ex.Message);
            }
        }
        private void ClearFields()
        {
            txtID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtClass.Clear();
            txtGrade.Clear();
        }

        private bool ValidateInputs(out int age, out decimal grade)
        {
            age = 0;
            grade = 0;

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtClass.Text))
            {
                MessageBox.Show("Име и паралелка са задължителни.");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out age))
            {
                MessageBox.Show("Невалидна възраст.");
                return false;
            }

            if (!decimal.TryParse(txtGrade.Text, out grade))
            {
                MessageBox.Show("Невалиден среден успех.");
                return false;
            }

            return true;
        }
    }
}

    


    

