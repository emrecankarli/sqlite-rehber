using System.Data;
using System.Data.SQLite;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sqlite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SQLiteConnection baglanti = new SQLiteConnection("Data Source = c:\\rehber\\rehber.db3;Version=3;");

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void Listele()
        {
            try
            {
                baglanti.Open();
                string sorgu = "Select * from Kisiler";
                SQLiteDataAdapter sqda = new SQLiteDataAdapter(sorgu, baglanti);
                DataSet ds = new DataSet();
                ds.Clear();
                sqda.Fill(ds, "kisiler");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "kisiler";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Baðlantý Kurulamadý" + ex, "Uyarý");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            maskedTextBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            label8.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();

            if (label8.Text != "")
            {
                string hedefYolAd = resimyolu + label8.Text;
                if (File.Exists(hedefYolAd) == true)
                    pictureBox1.Image = Image.FromFile(hedefYolAd);
            }
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Text = "Resim Deðiþtir";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //yeni kayýt
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            label8.Text = "";
            button2.Enabled = true;//kaydet
            button3.Enabled = false;//güncelle
            button4.Enabled = false;//sil
            pictureBox1.Image = null;
            button5.Text = "Resim Seç";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //bilgileri kaydet
            if (textBox2.Text == "" || textBox3.Text == "" || maskedTextBox1.Text == "")
            {
                MessageBox.Show("Ad, Soyad ve Telefon1 boþ geçilemez!");
                return;
            }
            try
            {
                baglanti.Open();
                SQLiteCommand komut = new SQLiteCommand();
                komut.Connection = baglanti;
                komut.CommandText = "Insert into kisiler (ad,soyad,tel1,tel2,mail,ekbilgiler,resim) " + " values (@t2,@t3,@mt1,@mt2,@t4,@t5,@l8)";
                komut.Parameters.AddWithValue("t2", textBox2.Text);
                komut.Parameters.AddWithValue("t3", textBox3.Text);
                komut.Parameters.AddWithValue("mt1", maskedTextBox1.Text);
                komut.Parameters.AddWithValue("mt2", maskedTextBox2.Text);
                komut.Parameters.AddWithValue("t4", textBox4.Text);
                komut.Parameters.AddWithValue("t5", textBox5.Text);
                komut.Parameters.AddWithValue("l8", label8.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kayýt Eklendi.");
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayýt Eklenemedi" + ex, "Uyarý");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //güncelle
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Güncellemek için önce kayýt seçin", "Uyarý");
                return;
            }
            try
            {
                baglanti.Open();
                string sql = "update kisiler set ad=@t2,soyad=@t3,tel1=@mt2,mail=@t4,ekbilgiler=@t5,resim=@t6" + " where id=" + int.Parse(textBox1.Text);
                SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
                komut.Parameters.AddWithValue("t2", textBox2.Text);
                komut.Parameters.AddWithValue("t3", textBox3.Text);
                komut.Parameters.AddWithValue("mt1", maskedTextBox1.Text);
                komut.Parameters.AddWithValue("mt2", maskedTextBox2.Text);
                komut.Parameters.AddWithValue("t4", textBox4.Text);
                komut.Parameters.AddWithValue("t5", textBox5.Text);
                if (resimyuklendi)
                    komut.Parameters.AddWithValue("t6", label8.Text);
                else
                    komut.Parameters.AddWithValue("t6", "");
                komut.ExecuteNonQuery();
                baglanti.Close();
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayýt Güncellenemedi" + ex, "Uyarý");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Kaydý sil
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Silmek için bir kayýt seçin.", "Uyarý");
                return;
            }
            try
            {
                baglanti.Open();
                string sql = "delete from kisiler" + " where id=" + int.Parse(textBox1.Text);
                SQLiteCommand komut = new SQLiteCommand(sql, baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayýt Silinemedi\n" + ex, "Uyarý");
            }
        }
        bool resimyuklendi = false;
        string resimyolu = "c:\\rehber\\resimler\\";

        private void button5_Click(object sender, EventArgs e)
        {
            //resim seç
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }

                string dosyaAd = openFileDialog1.SafeFileName;
                string kaynakYoluAdi = openFileDialog1.FileName;

                string uzanti = Path.GetExtension(openFileDialog1.FileName);
                string yeniAd = dosyaAd;
                string hedefYolAd = resimyolu + yeniAd;

                if (textBox1.Text != "" || textBox2.Text != "")
                {
                    yeniAd = textBox1.Text + "_" + textBox2.Text + uzanti;
                    hedefYolAd = resimyolu + yeniAd;
                }
                label8.Text = yeniAd;

                File.Copy(kaynakYoluAdi, hedefYolAd, true);
                if (File.Exists(hedefYolAd) == true)
                {
                    pictureBox1.Image = Image.FromFile(hedefYolAd);
                }
                resimyuklendi = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //arama butonu
            if (button6.Text == "Ara")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                maskedTextBox1.Text = "";
                maskedTextBox2.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                label8.Text = "";
                button1.Enabled = false;
                button2.Enabled = false;//kaydet
                button3.Enabled = false;//güncelle
                button4.Enabled = false;//sil
                pictureBox1.Image = null;
                button5.Enabled = false;

                button6.Text = "Aramayý Durdur";
            }
            else
            {
                button1.Enabled = true;
                button6.Text = "Ara";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (button6.Text == "Ara")
            {
                // Arama butonu pasifse, arama yapma
                return;
            }

            // Arama butonu aktifse, arama yapýlacak kelime
            string aramaKelimesi = textBox2.Text.Trim();

            // Arama kelimesi boþsa tüm kayýtlarý göster
            if (string.IsNullOrEmpty(aramaKelimesi))
            {
                Listele();
                return;
            }

            try
            {
                // Baðlantýyý aç
                baglanti.Open();

                // Arama sorgusu
                string sorgu = "SELECT * FROM kisiler WHERE ad LIKE @aramaKelimesi";

                // Parametrelerle sorguyu hazýrla
                SQLiteCommand komut = new SQLiteCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@aramaKelimesi", "%" + aramaKelimesi + "%");

                // Verileri çek
                SQLiteDataAdapter sqda = new SQLiteDataAdapter(komut);
                DataSet ds = new DataSet();
                ds.Clear();
                sqda.Fill(ds, "kisiler");

                // DataGridView'e verileri yükle
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "kisiler";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Baðlantýyý kapat
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama iþlemi baþarýsýz oldu: " + ex.Message, "Hata");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (button6.Text == "Ara")
            {
                // Arama butonu pasifse, arama yapma
                return;
            }

            // Arama butonu aktifse, arama yapýlacak kelime
            string aramaKelimesi = textBox3.Text.Trim();

            // Arama kelimesi boþsa tüm kayýtlarý göster
            if (string.IsNullOrEmpty(aramaKelimesi))
            {
                Listele();
                return;
            }

            try
            {
                // Baðlantýyý aç
                baglanti.Open();

                // Arama sorgusu
                string sorgu = "SELECT * FROM kisiler WHERE ad LIKE @aramaKelimesi OR soyad LIKE @aramaKelimesi OR tel1 LIKE @aramaKelimesi";

                // Parametrelerle sorguyu hazýrla
                SQLiteCommand komut = new SQLiteCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@aramaKelimesi", "%" + aramaKelimesi + "%");

                // Verileri çek
                SQLiteDataAdapter sqda = new SQLiteDataAdapter(komut);
                DataSet ds = new DataSet();
                ds.Clear();
                sqda.Fill(ds, "kisiler");

                // DataGridView'e verileri yükle
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "kisiler";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Baðlantýyý kapat
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama iþlemi baþarýsýz oldu: " + ex.Message, "Hata");
            }
        }

        private void Ara(string alanAdi, string aramaKelimesi)
        {
            try
            {
                // Baðlantýyý aç
                baglanti.Open();

                // Arama sorgusu
                string sorgu = "SELECT * FROM kisiler WHERE " + alanAdi + " LIKE @aramaKelimesi";

                // Parametrelerle sorguyu hazýrla
                SQLiteCommand komut = new SQLiteCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@aramaKelimesi", "%" + aramaKelimesi + "%");

                // Verileri çek
                SQLiteDataAdapter sqda = new SQLiteDataAdapter(komut);
                DataSet ds = new DataSet();
                ds.Clear();
                sqda.Fill(ds, "kisiler");

                // DataGridView'e verileri yükle
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "kisiler";
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Baðlantýyý kapat
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama iþlemi baþarýsýz oldu: " + ex.Message, "Hata");
            }
        }
    }
}
