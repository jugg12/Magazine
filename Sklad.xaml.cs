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
    /// Логика взаимодействия для Sklad.xaml
    /// </summary>
    public partial class Sklad : Window
    {
        Sqlcon sql;
        tabledata.sklad sklad;
        tabledata.sklad data;
        OtdelProdaj window;
        int ka = 7;
        PostTovaraOtProdaj window2;
        public Sklad(OtdelProdaj win, PostTovaraOtProdaj win2, Sqlcon sql)
        {
            InitializeComponent();

            this.sql = sql;
            window = win;
            window2 = win2;

            sklad = data;
        }

        List<SelectedData> ediz = new List<SelectedData>();
        List<SelectedData> kat = new List<SelectedData>();


        void LoadingShow()
        {
            if (LoadingGrid.Visibility == Visibility.Hidden)
            {
                LoadingGrid.Visibility = Visibility.Visible;
            }
            else { LoadingGrid.Visibility = Visibility.Hidden; }
        }

        public async Task UpdateTable()
        {
            LoadingShow();
            string qy = SR.SKLAD;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                SklViewer.ItemsSource = d.DefaultView;


            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        public async Task UpdateInfo()
        {
            ediz = new List<SelectedData>();
            kat = new List<SelectedData>();
            var a = await sql.CommnadWithQuery("SELECT id,[Название единицы измерения] FROM EdIzm");
            var b = a.Select();
            for (int i = 0; i < b.Length; i++)
            {
                ediz.Add(new SelectedData() { id = b[i]["id"].ToString(), title = b[i]["Название единицы измерения"].ToString() });
            }
            EdIzBoxList.ItemsSource = ediz;

            var c = await sql.CommnadWithQuery("SELECT id, [Название категории] FROM Kategorii");
            var d = c.Select();
            for (int i = 0; i < d.Length; i++)
            {
                kat.Add(new SelectedData() { id = d[i]["id"].ToString(), title = d[i]["Название категории"].ToString() });
            }
            KatBoxList.ItemsSource = kat;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        //public static string SKLAD = "SELECT [id] as [id], [Название товара] as [Название товара], [Количество на складе] as [Количество на складе], [Единица измерения] as [Единица измерения], [Категория] as [Категория], [Стоимость(руб.)] as [Стоимость(руб.)] FROM Sklad";

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await UpdateInfo();
            Skladgrid.Visibility = Visibility.Visible;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SklViewer.SelectedIndex != -1)
            {


                string id = (SklViewer.SelectedItem as DataRowView).Row["Код товара"].ToString();


                string table = "Sklad";


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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (SklViewer.SelectedItem != null)
            {
                edit = true;
                var row = (SklViewer.SelectedItem as DataRowView);


                Tovarbox.Text = row.Row["Название товара"].ToString();
                kolbox.Text = row.Row["Количество на складе"].ToString();
                EdIzBoxList.Text = row.Row["Единица измерения"].ToString();
                KatBoxList.Text = row.Row["Категория"].ToString();
                fbox.Text = row.Row["Стоимость(руб.)"].ToString();



                Skladgrid.Visibility = Visibility.Visible;
                //public static string SKLAD = "SELECT [id] as [id], [Название товара]  [Количество на складе]  [Единица измерения] [Категория]  [Стоимость(руб.)] FROM Sklad";

            }
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
            this.Close();
        }

        private async void Skl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (window != null && window2 != null)
            {
                window2.IsEnabled = true;

                await window2.UpdateInfo();
            }

            else
            {
                window.IsEnabled = true;

                await window.UpdateInfo();
            }
        }

        private async void Skl_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
        }
        bool edit = false;
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Tovarbox.Text != "" & kolbox.Text != "" & EdIzBoxList.Text != "" & KatBoxList.Text != "" && fbox.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO Sklad ([Название товара],[Количество на складе],[Единица измерения],[Категория],[Стоимость(руб.)]) VALUES ('" + Tovarbox.Text + "','" + kolbox.Text + "','" + EdIzBoxList.Text + "','" + KatBoxList.Text + "','" + fbox.Text + "')"))
                    {


                        await UpdateTable();

                        EdIzBoxList.SelectedIndex = -1;
                        KatBoxList.SelectedIndex = -1;

                        Skladgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");
                        //public static string SKLAD = "SELECT [id] as [id], [Название товара]  [Количество на складе]  [Единица измерения] [Категория]  [Стоимость(руб.)] FROM Sklad";

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {

                    if (await sql.CommnadWithNonQuery("UPDATE Sklad SET [Название товара] = '" + Tovarbox.Text + "',[Количество на складе] ='" + kolbox.Text + "' ,[Единица измерения] ='" + EdIzBoxList.Text + "' ,[Категория] ='" + KatBoxList.Text + "' ,[Стоимость(руб.)] ='" + fbox.Text + "' where id = " + (SklViewer.SelectedItem as DataRowView).Row["Код товара"].ToString()))
                    {


                        KatBoxList.SelectedIndex = -1;
                        EdIzBoxList.SelectedIndex = -1;


                        Skladgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно изменено.");
                        edit = false;
                        await UpdateTable();
                    }
                    else
                    {

                        informer.MessageQueue.Enqueue("Проблема в изменении данных.");
                    }
                }


                Tovarbox.Clear();
                kolbox.Clear();
                fbox.Clear();



            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Tovarbox.Clear();
            kolbox.Clear();
            fbox.Clear();
            KatBoxList.SelectedIndex = -1;
            EdIzBoxList.SelectedIndex = -1;
            Skladgrid.Visibility = Visibility.Hidden;
        }

        private void EdIzrbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            EdIzmer ed = new EdIzmer(window, this, sql);
            ed.Show();


        }

        private void Katerbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Kategorii ed = new Kategorii(window, this, sql);
            ed.Show();
        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
           PrintExcel.Print(SklViewer,ka);
        }
       
    }
}