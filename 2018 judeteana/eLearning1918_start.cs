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
using System.IO;

namespace _2018_judeteana
{
    public partial class eLearning1918_start : Form
    {
        public int indexLoadBar = 0;
        public int buton = 1;
        public string email;
        public string pass;
        public Timer timer = new Timer();
        public string connx;
        
        public eLearning1918_start()
        {
            InitializeComponent();
            //baza de date
            string path = System.IO.Directory.GetCurrentDirectory();
            connx=@"Data Source=(LocalDB)\v11.0;AttachDbFilename="+path+@"\eLearning1918.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connx);
            con.Open();
            SqlCommand init1 = new SqlCommand("delete from utilizatori", con);
            init1.ExecuteNonQuery();
            int indexUtilizatori = 1;
            SqlCommand init2 = new SqlCommand("delete from itemi", con);
            init2.ExecuteNonQuery();
            int indexItemi = 1;
            SqlCommand init3 = new SqlCommand("delete from evaluari", con);
            init3.ExecuteNonQuery();
            int indexEvaluari = 1;
            string[] linii = File.ReadAllLines(@"../../date.txt");
            int i=0;
                if (linii[i].Equals("Utilizatori:"))
                {
                    i++;
                    while (!linii[i].Equals("Itemi:"))
                    {
                        Utilizator utilizator = new Utilizator(linii[i]);
                        SqlCommand ins = new SqlCommand("insert into utilizatori(idutilizator,numeprenumeutilizator, parolautilizator, emailutilizator, clasautilizator) values (@id, @nume, @parola, @email, @clasa)", con);
                        ins.Parameters.AddWithValue("@nume", utilizator.nume);
                        ins.Parameters.AddWithValue("@id", indexUtilizatori++);
                        ins.Parameters.AddWithValue("@parola", utilizator.parola);
                        ins.Parameters.AddWithValue("@email", utilizator.email);
                        ins.Parameters.AddWithValue("@clasa", utilizator.clasa);
                        ins.ExecuteNonQuery();
                        i++;
                    }
                    i++;
                    while (!linii[i].Equals("Evaluari:"))
                    {
                        Item item = new Item(linii[i]);
                        if (item.tip.Equals("1") || item.tip.Equals("4"))
                        {
                            SqlCommand a1 = new SqlCommand("insert into itemi(iditem, tipitem, EnuntItem, Raspuns1Item, Raspuns2Item, Raspuns3Item, Raspuns4Item, RaspunsCorectItem) values (@id, @tip, @enunt,NULL,NULL,NULL,NULL, @raspuns)", con);
                            a1.Parameters.AddWithValue("@id", indexItemi++);
                            a1.Parameters.AddWithValue("@tip", item.tip);
                            a1.Parameters.AddWithValue("@enunt", item.enunt);
                            a1.Parameters.AddWithValue("@raspuns", item.answCorect);
                            a1.ExecuteNonQuery();
                            i++;

                        }
                        if (item.tip.Equals("2") || item.tip.Equals("3"))
                        {
                            SqlCommand ins2 = new SqlCommand("insert into itemi(iditem, tipitem, EnuntItem, Raspuns1Item, Raspuns2Item, Raspuns3Item, Raspuns4Item, RaspunsCorectItem) values (@id, @tip, @enunt, @r1, @r2, @r3, @r4, @rcorect)", con);
                            ins2.Parameters.AddWithValue("@tip", item.tip);
                            ins2.Parameters.AddWithValue("@id", indexItemi++);
                            ins2.Parameters.AddWithValue("@enunt", item.enunt);
                            ins2.Parameters.AddWithValue("@r1", item.answ1);
                            ins2.Parameters.AddWithValue("@r2", item.answ2);
                            ins2.Parameters.AddWithValue("@r3", item.answ3);
                            ins2.Parameters.AddWithValue("@r4", item.answ4);
                            ins2.Parameters.AddWithValue("@rcorect", item.answCorect);
                            ins2.ExecuteNonQuery();
                            i++;
                        }

                    }
                    i++;
                    while(i!=linii.Length)
                    {
                        Evaluare ev = new Evaluare(linii[i]);
                        SqlCommand ins3 = new SqlCommand("insert into evaluari(idevaluare, idelev, dataevaluare, notaevaluare) values (@id, @id2, @data, @nota)", con);
                        ins3.Parameters.AddWithValue("@id", indexEvaluari++);
                        ins3.Parameters.AddWithValue("@id2", ev.id);
                        ins3.Parameters.AddWithValue("@data", ev.data);
                        ins3.Parameters.AddWithValue("@nota", ev.nota);
                        ins3.ExecuteNonQuery();
                        i++;
                    }
            }
            //final baza de date

            //slideshow

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public class Utilizator
        {
            public string nume;
            public string parola;
            public string email;
            public string clasa;
            public Utilizator(string linie)
            {
                string[] r = linie.Split(';');
                nume = r[0];
                parola = r[1];
                email = r[2];
                clasa = r[3];
            }
        };
        public class Item
        {
            public string tip;
            public string enunt;
            public string answ1;
            public string answ2;
            public string answ3;
            public string answ4;
            public string answCorect;
            public Item(string linie)
            {
                string[] r = linie.Split(';');
                tip = r[0];
                enunt = r[1];
                answ1 = r[2];
                answ2 = r[3];
                answ3 = r[4];
                answ4 = r[5];
                answCorect = r[6];
            }
        }
        public class Evaluare
        {
            public string id;
            public string data;
            public string nota;
            public Evaluare(string linie)
            {
                string[] r = linie.Split(';');
                id = r[0];
                data = r[1];
                nota = r[2];
            }
        }

        public void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            email = textBox1.Text;
            pass = textBox2.Text;
            SqlConnection con = new SqlConnection(connx);
            con.Open();
            SqlDataAdapter log = new SqlDataAdapter("select count (*) from utilizatori where emailutilizator = @email and parolautilizator= @parola", con);
            log.SelectCommand.Parameters.AddWithValue("@email", email);
            log.SelectCommand.Parameters.AddWithValue("@parola", pass);
            DataTable dt = new DataTable();
            log.Fill(dt);
            if (dt.Rows[0][0].ToString().Equals("1"))
            {
                eLearning1918_elev elev = new eLearning1918_elev(email);
                elev.ShowDialog();
            }
            else
            {
                MessageBox.Show("Eroare de autentificare!");
            }
        }

        public void button3_Click(object sender, EventArgs e)
        {
            timer.Interval = 2000;
            if (buton == 1)
            {
                button3.Text = "Auto";
                button4.Enabled = true;
                button2.Enabled = true;
                buton = 0;
            }
            else
            {
                button3.Text = "Manual";
                button4.Enabled = false;
                button2.Enabled = false;
                buton = 1;
            }

        }

        public void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (indexLoadBar == 4)
            {
                indexLoadBar = 0;
            }
            else
            {
                indexLoadBar++;
            }

            progressBar1.Value = indexLoadBar;
            pictureBox1.ImageLocation = "../../" + (indexLoadBar + 1) + ".jpg";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (indexLoadBar == 0)
            {
                indexLoadBar = 4;
            }
            else
            {
                indexLoadBar--;
            }
            progressBar1.Value = indexLoadBar;
            pictureBox1.ImageLocation = "../../" + (indexLoadBar + 1) + ".jpg";

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
