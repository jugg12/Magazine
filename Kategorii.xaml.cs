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
    /// Логика взаимодействия для Kategorii.xaml
    /// </summary>
    public partial class Kategorii : Window
    {

        Sqlcon sql;
        tabledata.kategorii kategorii;
        tabledata.kategorii data;
        OtdelProdaj window;
        int ka = 1;
        Sklad window2;
        public Kategorii(OtdelProdaj win,Sklad win2, Sqlcon sql)
        {
            InitializeComponent();

            this.sql = sql;
            window = win;
            window2 = win2;


            kategorii = data;
        }

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
            string qy = SR.KATEGORII;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                KatViewer.ItemsSource = d.DefaultView;
                KatViewer.Columns[0].Visibility = Visibility.Hidden;



            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        private async void kat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        private async void kat_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           
            kategoriigrid.Visibility = Visibility.Visible;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (KatViewer.SelectedIndex != -1)
            {


                string id = (KatViewer.SelectedItem as DataRowView).Row["id"].ToString();


                string table = "Kategorii";


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
            if (KatViewer.SelectedItem != null)
            {
                edit = true;
                var row = (KatViewer.SelectedItem as DataRowView);

                fbox.Text = row.Row["Название категории"].ToString();
                

                kategoriigrid.Visibility = Visibility.Visible;


            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            this.Close();
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (fbox.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO Kategorii ([Название категории]) VALUES ('" + fbox.Text + "')"))
                    {


                        await UpdateTable();
                        kategoriigrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {

                    if (await sql.CommnadWithNonQuery("UPDATE Kategorii SET [Название категории] = '" + fbox.Text + "' where id = " + (KatViewer.SelectedItem as DataRowView).Row["id"].ToString()))
                    {

                        // public static string KATEGORII = "SELECT [id] as [id], [Название категории] as [Название категории] FROM Kategorii";


                        kategoriigrid.Visibility = Visibility.Hidden;
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
              

            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            fbox.Clear();
            kategoriigrid.Visibility = Visibility.Hidden;
        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(KatViewer,ka);
        }
    }
}
