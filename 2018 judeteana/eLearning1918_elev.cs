using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace _2018_judeteana
{
    public partial class eLearning1918_elev : Form
    {
        public string email;
        private int[] questions;
        public int indexEx = 0;
        public string connx;
        public eLearning1918_elev(string emailElev)
        {
            email = emailElev;
            InitializeComponent();
        }

        private void eLearning1918_elev_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'eLearning1918DataSet3.carnet' table. You can move, or remove it, as needed.
            //this.carnetTableAdapter.Fill(this.eLearning1918DataSet3.carnet);
            // TODO: This line of code loads data into the 'eLearning1918DataSet1.Evaluari' table. You can move, or remove it, as needed.
            //this.evaluariTableAdapter.Fill(this.eLearning1918DataSet1.Evaluari);
            // TODO: This line of code loads data into the 'eLearning1918DataSet.Table' table. You can move, or remove it, as needed.
            //this.tableTableAdapter.Fill(this.eLearning1918DataSet.Table);

            string path = System.IO.Directory.GetCurrentDirectory();
            connx = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=" + path + @"\eLearning1918.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection conn = new SqlConnection(connx);
            conn.Open();




            string id;
            SqlDataAdapter ids = new SqlDataAdapter("select idutilizator from utilizatori where emailutilizator = @email", conn);
            ids.SelectCommand.Parameters.AddWithValue("@email", email);
            DataTable idss = new DataTable();
            ids.Fill(idss);
            id = idss.Rows[0][0].ToString();
            label11.Text = "Carnetul de note al elevului " + numeprenume;

            string cls;
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("select dataevaluare, notaevaluare from evaluari where idelev = @id", conn);
            sda.SelectCommand.Parameters.AddWithValue("@id", id);
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            DataTable clasa = new DataTable();
            SqlDataAdapter cl = new SqlDataAdapter("select clasautilizator from utilizatori where idutilizator = @id", conn);
            cl.SelectCommand.Parameters.AddWithValue("@id", id);
            cl.Fill(clasa);
            cls = clasa.Rows[0][0].ToString();

            SqlCommand sda3 = new SqlCommand("select avg(notaevaluare) as 'media' from evaluari inner join utilizatori on evaluari.idelev=utilizatori.idutilizator where clasautilizator = (select clasautilizator from utilizatori where idutilizator = @id1); select notaevaluare, idevaluare from evaluari where idelev = @id order by idevaluare", conn);
            sda3.Parameters.AddWithValue("@id1", id);
            sda3.Parameters.AddWithValue("@id", id);
            var reader2 = sda3.ExecuteReader();
            int w = 0;
            reader2.Read();
            string sdad = reader2["media"].ToString();
            //
            reader2.NextResult();
            while (reader2.Read())
            {
                chart1.Series["medie"].Points.AddXY(reader2["idevaluare"].ToString(), sdad);
                chart1.Series["note"].Points.AddXY(reader2["idevaluare"].ToString(), reader2["notaevaluare"].ToString());
            }



            
        }

        public void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        public void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        public void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        public int[] getQuestions()
        {
            Random random = new Random();
            int[] ex = new int[9];
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                {
                    int no = random.Next(0, 8);
                    while (ex.Contains(no))
                    {
                        no = random.Next(0, 8);

                    }
                    ex[i] = no;
                }
                else
                {
                    if (i < 6)
                    {
                        int no = random.Next(9, 18);
                        while (ex.Contains(no))
                        {
                            no = random.Next(9, 18);

                        }
                        ex[i] = no;
                    }
                    else
                    {
                        if (i < 7)
                        {
                            int no = random.Next(19, 24);
                            while (ex.Contains(no))
                            {
                                no = random.Next(19, 24);

                            }
                            ex[i] = no;
                        }
                        else
                        {
                            if (i < 9)
                            {
                                int no = random.Next(25, 31);
                                while (ex.Contains(no))
                                {
                                    no = random.Next(25, 31);

                                }
                                ex[i] = no;
                            }
                        }
                    }
                }
            }

            return ex;
        }

        public void renderQuestionForm()
        {
            SqlConnection con = new SqlConnection(connx);
            con.Open();
            SqlDataAdapter idtip = new SqlDataAdapter("select tipitem from itemi where iditem=@id", con);
            idtip.SelectCommand.Parameters.AddWithValue("@id", this.questions[indexEx]);

            string tip;
            DataTable dt = new DataTable();
            idtip.Fill(dt);
            tip = dt.Rows[0][0].ToString();
            switch (tip)
            {
                case "1":
                    {
                        groupBox1.Visible = true;
                        button2.Enabled = true;
                        break;
                    };
                case "2":
                    {
                        groupBox2.Visible = true;
                        button3.Enabled = true;
                        break;
                    };
                case "3":
                    {
                        groupBox3.Visible = true;
                        button4.Enabled = true;
                        break;
                    };
                case "4":
                    {
                        groupBox4.Visible = true;
                        button5.Enabled = true;
                        break;
                    }
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.questions = getQuestions();
            renderQuestionForm();
        }

        public void button4_Click(object sender, EventArgs e)
        {
            indexEx++;
            renderQuestionForm();
        }
        public void button5_Click(object sender, EventArgs e)
        {
            indexEx++;
            renderQuestionForm();
        }
        public void button2_Click(object sender, EventArgs e)
        {
            indexEx++;
            renderQuestionForm();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }


        public string numeprenume;
        private void tabPage2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connx);
            conn.Open();
            
            SqlDataAdapter numeprenume1 = new SqlDataAdapter("select numeprenumeutilizator from utilizatori where emailutilizator = @email", conn);
            numeprenume1.SelectCommand.Parameters.AddWithValue("@email", email);
            DataTable dt = new DataTable();
            numeprenume1.Fill(dt);
            numeprenume=dt.Rows[0][0].ToString();
            label11.Text = "Carnetul de note al elevului " + numeprenume;


            string id;
            SqlDataAdapter ids = new SqlDataAdapter("select idutilizator from utilizatori where emailutilizator = @email",conn);
            ids.SelectCommand.Parameters.AddWithValue("@email", email);
            DataTable idss = new DataTable();
            ids.Fill(idss);
            id = idss.Rows[0][0].ToString();



            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("select data dataevaluare, notaevaluare from evaluari where idelev = @id", conn);
            sda.SelectCommand.Parameters.AddWithValue("@id", id);
            

        }

        public void label11_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
