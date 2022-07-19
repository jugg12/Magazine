using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Magazine.OtdelProdaj;

namespace Magazine
{
    /// <summary>
    /// Логика взаимодействия для PostTovaraOtProdaj.xaml
    /// </summary>
    public partial class PostTovaraOtProdaj : Window
    {
        Sqlcon sql;
        tabledata.posttov posttov;
        tabledata.posttov data;
        OtdelProdaj window;
        int ka = 6;
        PostTovaraOtProdaj window2;

       
        public PostTovaraOtProdaj(OtdelProdaj win,Sqlcon sql)
        {


            InitializeComponent();
            this.sql = sql;
            window = win;
            



            posttov = data;
        }
        List<SelectedData> kod = new List<SelectedData>();
        List<SelectedData> otvzapost = new List<SelectedData>();
        List<SelectedData> indpost = new List<SelectedData>();
        void LoadingShow()
        {
            if (LoadingGrid.Visibility == Visibility.Hidden)
            {
                LoadingGrid.Visibility = Visibility.Visible;
            }
            else { LoadingGrid.Visibility = Visibility.Hidden; }
        }
        async Task UpdateTable()
        {
            LoadingShow();
            string qy = SR.POSTAVKATOVARA;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                PTViewer.ItemsSource = d.DefaultView;
                PTViewer.Columns[0].Visibility = Visibility.Hidden;

            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        private async void PostTovara_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
        }

       public async Task UpdateInfo()
        {
            kod = new List<SelectedData>();
            otvzapost = new List<SelectedData>();
            indpost = new List<SelectedData>();
            var a = await sql.CommnadWithQuery("SELECT id,[Название товара] as [Название товара] FROM Sklad");
            var b = a.Select();
            for (int i = 0; i < b.Length; i++)
            {
                kod.Add(new SelectedData() { id = b[i]["id"].ToString(), title = b[i]["id"].ToString() });
            }
            KodBoxList.ItemsSource = kod;

            var c = await sql.CommnadWithQuery("SELECT id, [Фамилия]+' '+[Имя]+' '+[Отчество] as ФИО FROM Sotrudniki");
            var d = c.Select();
            for (int i = 0; i < d.Length; i++)
            {
                otvzapost.Add(new SelectedData() { id = d[i]["id"].ToString(), title = d[i]["ФИО"].ToString()});
            }
            SotrBoxList.ItemsSource = otvzapost;

            var e = await sql.CommnadWithQuery("SELECT id, [id] FROM Postavshik");
            var f = e.Select();
            for (int i = 0; i < f.Length; i++)
            {
                indpost.Add(new SelectedData() { id = f[i]["id"].ToString(), title = f[i]["id"].ToString() });
            }
            IndexPost.ItemsSource = indpost;

        }

        private async void PostTovara_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.IsEnabled = true;
            await window.UpdateInfo();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await UpdateInfo();
            PostTovaragrid.Visibility = Visibility.Visible;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PTViewer.SelectedIndex != -1)
            {


                string id = (PTViewer.SelectedItem as DataRowView).Row["id"].ToString();


                string table = "PostavkiTovara";


                if (await sql.Delete("id", id, table))
                {
                    await UpdateTable();
                    informer.MessageQueue.Enqueue("Удалено!");

                }
                else
                {
                    informer.MessageQueue.Enqueue("Проблемма при удалении.");
                }


            }
        }
        bool edit = false;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (PTViewer.SelectedItem != null)
            {
                edit = true;
                var row = (PTViewer.SelectedItem as DataRowView);

                
                IndexPost.Text = row.Row["Индекс поставщика"].ToString();
                KodBoxList.Text = row.Row["Код товара"].ToString();
                Sbox.Text = row.Row["Стоимость поставки"].ToString();
                Kbox.Text = row.Row["Количество"].ToString();
                SotrBoxList.Text = row.Row["Ответственный за поставку"].ToString();
                beginDate.Text = row.Row["Дата поставки"].ToString();


                PostTovaragrid.Visibility = Visibility.Visible;


            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (IndexPost.Text != "" & KodBoxList.Text != "" & Sbox.Text != "" & Kbox.Text != "" && SotrBoxList.Text != "" && beginDate.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO PostavkiTovara ([Индекс поставщика],[Код товара],[Стоимость поставки],[Количество],[Ответственный за поставку],[Дата поставки]) VALUES ('" + IndexPost.Text + "','" + KodBoxList.Text + "','" + Sbox.Text + "','" + Kbox.Text + "','" + SotrBoxList.Text + "','" + beginDate.Text + "')"))
                    {


                        await UpdateTable();
                        IndexPost.SelectedIndex = -1;
                        KodBoxList.SelectedIndex = -1;
                        SotrBoxList.SelectedIndex = -1;
                        beginDate.Text = "";
                        PostTovaragrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {
                   // public static string POSTAVKATOVARA = "SELECT [id] as [id], [Индекс поставщика] as [Индекс поставщика], [Код товара] as [Код товара], [Стоимость поставки] as [Стоимость поставки], [Количество] as [Количество], [Ответственный за поставку] as [Ответственный за поставку], [Дата поставки] as [Дата поставки] FROM PostavkiTovara";

                    if (await sql.CommnadWithNonQuery("UPDATE PostavkiTovara SET [Индекс поставщика] = '" + IndexPost.Text + "',[Код товара] ='" + KodBoxList.Text + "' ,[Стоимость поставки] ='" + Sbox.Text + "' ,[Количество] ='" + Kbox.Text + "' ,[Ответственный за поставку] ='" + SotrBoxList.Text + "' , [Дата поставки] ='" + beginDate.Text + "' where id = " + (PTViewer.SelectedItem as DataRowView).Row["id"].ToString()))
                    {
                        //"SELECT id as [ПоставкаТовара.id], [Индекс поставщика] as [Индекс поставщика], [Код товара] as [Код товара], [Стоимость поставки] as [Стоимость поставки], [Количество] as [Количество], [Ответственный за поставку] as [Ответственный за поставку], [Дата поставки] as [Дата поставки] FROM PostavkiTovara";

                        IndexPost.SelectedIndex = -1;
                        KodBoxList.SelectedIndex = -1;
                        SotrBoxList.SelectedIndex = -1;
                        beginDate.Text = "";

                        PostTovaragrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно изменено.");
                        edit = false;
                        await UpdateTable();
                    }
                    else
                    {

                        informer.MessageQueue.Enqueue("Проблема в изменении данных.");
                    }
                }

               
                Sbox.Clear();
                Kbox.Clear();
                


            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            IndexPost.SelectedIndex = -1;
            KodBoxList.SelectedIndex = -1;
            SotrBoxList.SelectedIndex = -1;
            
            Sbox.Clear();
            Kbox.Clear();
            beginDate.Text = "";
            PostTovaragrid.Visibility = Visibility.Hidden;
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
            this.Close();
        }

        private void indbtn_Click(object sender, RoutedEventArgs e)
        {
            
            this.IsEnabled = false;
            PostOtdProdaj p = new PostOtdProdaj(window,this,sql);
            p.Show();
        }

        private void Kodrbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Sklad p = new Sklad(window, this, sql);
            p.Show();
        }

        private void Sotrrbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            SotrOtProdaj p = new SotrOtProdaj(window, this, sql);
            p.Show();
        }

        private void PTViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(PTViewer,ka);
        }
    }
}


