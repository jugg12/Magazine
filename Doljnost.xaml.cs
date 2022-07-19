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
    /// Логика взаимодействия для Doljnost.xaml
    /// </summary>
    public partial class Doljnost : Window
    {

        Sqlcon sql;
        tabledata.Doljnost dolj;
        tabledata.Doljnost data;
        int ka = 1;
        public SotrOtProdaj window;
        public Doljnost(SotrOtProdaj win, Sqlcon sql)
        {
            InitializeComponent();
            this.sql = sql;
            window = win;


            dolj = data;
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
            string qy = SR.DOLJNOST;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                DoljViewer.ItemsSource = d.DefaultView;
                DoljViewer.Columns[0].Visibility = Visibility.Hidden;



            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        async Task UpdateInfo()
        {

        }

            private async void Dolj_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.IsEnabled = true;

            await window.UpdateInfo();

        }
        bool edit = false;

        private async void Dolj_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();

            //public static string DOLJNOST = "SELECT [id] as [id], [Название должности] as [Название должности] FROM Doljnost";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            doljgrid.Visibility = Visibility.Visible;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DoljViewer.SelectedIndex != -1)
            {


                string id = (DoljViewer.SelectedItem as DataRowView).Row["id"].ToString();


                string table = "Doljnost";


                if (await sql.Delete("id", id, table))
                {
                    await UpdateTable();
                    await UpdateInfo();
                    informer.MessageQueue.Enqueue("Удалено!");

                }
                else
                {
                    informer.MessageQueue.Enqueue("Проблемма при удалении.");
                }


            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();

            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (DoljViewer.SelectedItem != null)
            {
                edit = true;
                var row = (DoljViewer.SelectedItem as DataRowView);

                Dbox.Text = row.Row["Название должности"].ToString();


                doljgrid.Visibility = Visibility.Visible;


            }
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Dbox.Text != "" )
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO Doljnost ([Название должности]) VALUES ('" + Dbox.Text + "')"))
                    {


                        await UpdateTable();
                        await UpdateInfo();

                        doljgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {

                    if (await sql.CommnadWithNonQuery("UPDATE Doljnost SET [Название должности] = '" + Dbox.Text + "' where id = " + (DoljViewer.SelectedItem as DataRowView).Row["id"].ToString()))
                    {


                       

                        doljgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно изменено.");
                        edit = false;
                        await UpdateTable();
                        await UpdateInfo();
                    }
                    else
                    {

                        informer.MessageQueue.Enqueue("Проблема в изменении данных.");
                    }
                }

                Dbox.Clear();
                


            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Dbox.Clear();
            doljgrid.Visibility = Visibility.Hidden;
        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(DoljViewer,ka);
        }
    }
}
