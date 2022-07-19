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

namespace Magazine
{
    /// <summary>
    /// Логика взаимодействия для PostOtdProdaj.xaml
    /// </summary>
    public partial class PostOtdProdaj : Window
    {
        Sqlcon sql;
        tabledata.postavshik post;
        tabledata.postavshik data;
        OtdelProdaj window;
        PostTovaraOtProdaj window2;
        int ka = 4;
        
        
        public PostOtdProdaj(OtdelProdaj win,PostTovaraOtProdaj win2, Sqlcon sql)
        {
            this.sql = sql;
            window = win;
            window2 = win2;
            
                  


            post = data;

            InitializeComponent();

        }
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
            string qy = SR.POSTAVSHIKI;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                PostViewer.ItemsSource = d.DefaultView;
              



            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            Postgrid.Visibility = Visibility.Visible;
        }

        

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PostViewer.SelectedIndex != -1)
            {


                string id = (PostViewer.SelectedItem as DataRowView).Row["Индекс поставщика"].ToString();


                string table = "Postavshik";


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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (PostViewer.SelectedItem != null)
            {
                edit = true;
                var row = (PostViewer.SelectedItem as DataRowView);

                fbox.Text = row.Row["ФИО"].ToString();
                Telbox.Text = row.Row["Номер телефона"].ToString();
                adressbox.Text = row.Row["Адрес"].ToString();
               
                Postgrid.Visibility = Visibility.Visible;


            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            this.Close();
        }

        private async void Post_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        bool edit = false;

        private async void Post_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            

        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (fbox.Text != "" & Telbox.Text != "" & adressbox.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO Postavshik ([ФИО],[Номер телефона],[Адрес]) VALUES ('" + fbox.Text + "','" + Telbox.Text + "','" + adressbox.Text + "')"))
                    {


                        await UpdateTable();

                        Postgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {

                    if (await sql.CommnadWithNonQuery("UPDATE Postavshik  SET [ФИО] = '" + fbox.Text + "',[Номер телефона] ='" + Telbox.Text + "' ,[Адрес] ='" + adressbox.Text +"' where id = " + (PostViewer.SelectedItem as DataRowView).Row["Индекс поставщика"].ToString()))
                    {

                        //"SELECT id as [Индекс поставщика], [ФИО] as [ФИО], [Номер телефона] as [Номер телефона], [Адрес] as [Адрес] FROM Postavshik"
                        Postgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно изменено.");
                        edit = false;
                        await UpdateTable();
                    }
                    else
                    {

                        informer.MessageQueue.Enqueue("Проблема в изменении данных.");
                    }
                }

                fbox.Clear();
                Telbox.Clear();
                adressbox.Clear();
                


            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Postgrid.Visibility = Visibility.Hidden;
            fbox.Clear();
            Telbox.Clear();
            adressbox.Clear();

        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(PostViewer,ka);
        }
    }
}
