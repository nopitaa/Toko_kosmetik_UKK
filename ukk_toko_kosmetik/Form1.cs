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
using System.IO;

namespace ukk_toko_kosmetik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=toko_kosmetik_ukk";
            string query = "INSERT INTO tbl_barang(Kode_Barang,Nama,Stok,Kategori,Harga,Image)VALUES('" + this.KodeBarang.Text + "','" + this.Nama.Text + "','" + this.Stok.Text + "','" + this.Kategori.Text + "','" + this.Harga.Text + "','" + Path.GetFileName(pictureBox.ImageLocation) + "')";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query,conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Data Berhasil Disimpan");
            File.Copy(TextImage.Text, Application.StartupPath + @"\Image\" + Path.GetFileName(pictureBox.ImageLocation), true);
            conn.Close();
            

            // Refresh data grid view
            string selectQuery = "SELECT * FROM tbl_barang";
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt.Columns.Add("gambar", Type.GetType("System.Byte[]"));
            foreach (DataRow row in dt.Rows)
            {
                row["gambar"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["Image"].ToString()));
            }
            dataGridView1.DataSource = dt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=toko_kosmetik_ukk";
            string query = "UPDATE tbl_barang SET Kode_Barang='" + this.KodeBarang.Text + "', Nama='" + this.Nama.Text + "', Stok='" + this.Stok.Text + "', Kategori='" + this.Kategori.Text + "', Harga='" + this.Harga.Text + "' WHERE Kode_Barang='"+ this.KodeBarang.Text +"' "; // gweh masih ragu sm  where kode barang nya deh 
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Data Berhasil di Update");
            conn.Close();

            // Refresh data grid view
            string selectQuery = "SELECT * FROM tbl_barang";
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt.Columns.Add("gambar", Type.GetType("System.Byte[]"));
            foreach (DataRow row in dt.Rows)
            {
                row["gambar"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["Image"].ToString()));
            }
            dataGridView1.DataSource = dt;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=toko_kosmetik_ukk";
            string query = " DELETE FROM tbl_barang WHERE Kode_Barang= '" + this.KodeBarang.Text +"'"; 
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Data Berhasil di Hapus !!");
            conn.Close();

            // Refresh data grid view
            string selectQuery = "SELECT * FROM tbl_barang";
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt.Columns.Add("gambar", Type.GetType("System.Byte[]"));
            foreach (DataRow row in dt.Rows)
            {
                row["gambar"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["Image"].ToString()));
            }
            dataGridView1.DataSource = dt;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=toko_kosmetik_ukk";
            MySqlConnection con = new MySqlConnection(connection);
            MySqlDataAdapter da;
            DataTable dt;
            con.Open();
            da = new MySqlDataAdapter("SELECT * FROM tbl_barang WHERE Nama LIKE'" + this.Search.Text + "%' OR Kategori LIKE'" + this.Search.Text + "%' ", con); //search di atur berdasarkan nama barang atau kategori 
            dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Add("gambar", Type.GetType("System.Byte[]"));
            foreach (DataRow row in dt.Rows)
            {
                row["gambar"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["Image"].ToString()));
            }
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=toko_kosmetik_ukk";
            string query = "SELECT * FROM tbl_barang";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt.Columns.Add("gambar", Type.GetType("System.Byte[]"));
            foreach (DataRow row in dt.Rows)
            {
                row["gambar"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["Image"].ToString()));
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Image"].Visible = false;
            dataGridView1.Columns["ID"].Visible = false;

        }



        private void pictureBox_Click(object sender, EventArgs e)
        {

            OpenFileDialog openfd = new OpenFileDialog();
            openfd.Filter = "Image Files(*.jpg;*.jpeg;*.png;) | *.jpg;*.jpeg;*.png; ";
            if (openfd.ShowDialog() == DialogResult.OK)
            {
                TextImage.Text = openfd.FileName;
                pictureBox.Image = new Bitmap(openfd.FileName);
                pictureBox.ImageLocation = openfd.FileName;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

        }
    }
}
