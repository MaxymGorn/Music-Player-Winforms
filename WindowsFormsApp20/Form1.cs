using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Maxs_Gorn;
namespace WindowsFormsApp20
{

    public partial class Form1 : Form
    {
        string currentfilename;
        string dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Catalog\"; //Директория куда скидываем файлы
        public Form1()
        {
            InitializeComponent();
            LoadFile();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("All");
            comboBox1.SelectedIndex = 0;
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void LoadFile()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            DatManage datManage = new DatManage();
            ListMusic listMusic = new ListMusic();

            listMusic = datManage.DeserializeXML<ListMusic>("MusicL.xml");

            listView1.Items.Clear();
            List<string> vs = new List<string>();
            foreach (var el in listMusic.Musics)
            {
                var item = listView1.Items.Add(el.Filename);
                item.SubItems.Add(el.Category);
                vs.Add(el.Category);
            }
            comboBox1.Items.Clear();
            comboBox1.Items.Add("All");
            List<string> res = new List<string>();
            for (int i = 0; i < vs.Count; i++)
            {
                for (int j = vs.Count - 1; j >= 0; j--)
                {
                    if (vs[i] == vs[j])
                    {

                        if (!res.Contains(vs[i]))
                            res.Add(vs[i]);
                    }
                }
            }
            foreach (var el in res)
            {
                comboBox1.Items.Add(el);
            }
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void SaveFile(Music music)
        {
            DatManage datManage = new DatManage();
            ListMusic listMusic = new ListMusic();

            listMusic = datManage.DeserializeXML<ListMusic>("MusicL.xml");
            bool e = false;
            foreach (var el in listMusic.Musics)
            {
                if (el.Filename == music.Filename)
                {
                    e = true;
                }
            }
            if (e == false)
            {
                listMusic.Musics.Add(music);

                datManage.SerializeXML(listMusic, "MusicL.xml");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Files WAV (*.wav) |*.wav" };


            openFileDialog.Multiselect = true; //Мультиселект
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

              
                MessageBox.Show("","");
                string[] filenames = openFileDialog.FileNames;
                //Получаем путь к выделенным файлам
                FileInfo fi = new FileInfo(openFileDialog.FileName);
                string dirSource = fi.DirectoryName;
                //Сохранение файла в указанную директорию
                foreach (var file in filenames)
                {
                    string fname = file.Substring(dirSource.Length + 1);
                
                        SaveFile(new Music() { Filename = fname, Category = "default" });
                        File.Copy(Path.Combine(dirSource, fname), Path.Combine(dir, fname), true);
                    
                }
               
            
                LoadFile();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {


            axWindowsMediaPlayer1.URL = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Catalog\" + listView1.Items[listView1.FocusedItem.Index].Text;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int ind = listView1.FocusedItem.Index;
                if (ind < 0)
                {
                    throw new Exception("Eror index selected!");
                }
                DatManage datManage = new DatManage();
                ListMusic listMusic = new ListMusic();

                XElement doc = XElement.Load("MusicL.xml");
                doc.Elements("Musics").Elements("Music").Where(ee => ee.Element("Filename").Value == listView1.Items[listView1.FocusedItem.Index].Text).Remove();
                doc.Save("MusicL.xml");

                LoadFile();

            }
            catch (Exception eror)
            {
                MessageBox.Show(eror.Message, "Notifications", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int ind = comboBox1.SelectedIndex;
                if (ind < 0)
                {
                    throw new Exception("Eror index selected!");
                }
                if (ind == 0)
                {
                    throw new ArgumentException();
                }

                DatManage datManage = new DatManage();
                ListMusic listMusic = new ListMusic();

                listMusic = datManage.DeserializeXML<ListMusic>("MusicL.xml");

                listView1.Items.Clear();
                foreach (var el in listMusic.Musics)
                {
                    if (el.Category == comboBox1.Text)
                    {
                        var item = listView1.Items.Add(el.Filename);
                        item.SubItems.Add(el.Category);
                    }

                }



            }
            catch (ArgumentException eror)
            {
                LoadFile();
            }
            catch (Exception eror)
            {
                MessageBox.Show(eror.Message, "Notifications", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            try
            {
                if (listView1.FocusedItem.Index <= 0)
                {

                }
                if (DialogResult.OK == form2.ShowDialog())
                {
                    DatManage datManage = new DatManage();
                    ListMusic listMusic = new ListMusic();
                   
                    listMusic = datManage.DeserializeXML<ListMusic>("MusicL.xml");
                    File.Delete("MusicL.xml");
                    listMusic.Musics[listView1.FocusedItem.Index].Category= form2.text;
                    datManage.SerializeXML(listMusic, "MusicL.xml");
                    LoadFile();
                }

            }
            catch (Exception eror)
            {
                MessageBox.Show(eror.Message, "Notifications", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
            }


        }
    }
}
