using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// Логика взаимодействия для EdIzmer.xaml
    /// </summary>
    public partial class EdIzmer : Window
    {
        Sqlcon sql;
        tabledata.EdinicaIzmerenia ediz;
        tabledata.EdinicaIzmerenia data;
        OtdelProdaj window;
        int ka = 1;
        Sklad window2;
        public EdIzmer(OtdelProdaj win,Sklad win2, Sqlcon sql)
        {
            
            InitializeComponent();
            this.sql = sql;
            window = win;
            window2 = win2;

            ediz = data;

        }

       
        async Task UpdateTable()
        {
            LoadingShow();
            string qy = SR.EDINICIIZMERENIA;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                EdIzmViewer.ItemsSource = d.DefaultView;
                EdIzmViewer.Columns[0].Visibility = Visibility.Hidden;

            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EdIzmgrid.Visibility = Visibility.Visible;
             //public static string EDINICIIZMERENIA = "SELECT [id] as [id], [Название единицы измерения] as [Название единицы измерения] FROM EdIzm";

    }
        

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
          
        }

        void LoadingShow()
        {
            if (LoadingGrid.Visibility == Visibility.Hidden)
            {
                LoadingGrid.Visibility = Visibility.Visible;
            }
            else { LoadingGrid.Visibility = Visibility.Hidden; }
        }
      

        private async void EdIzm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
           
            this.Close();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (EdIzmViewer.SelectedIndex != -1)
            {


                string id = (EdIzmViewer.SelectedItem as DataRowView).Row["id"].ToString();


                string table = "EdIzm";


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
            if (EdIzmViewer.SelectedItem != null)
            {
                edit = true;
                var row = (EdIzmViewer.SelectedItem as DataRowView);


                fbox.Text = row.Row["Название единицы измерения"].ToString();
               
                



                EdIzmgrid.Visibility = Visibility.Visible;
                //public static string SKLAD = "SELECT [id] as [id], [Название товара]  [Количество на складе]  [Единица измерения] [Категория]  [Стоимость(руб.)] FROM Sklad";

            }
        }

        private async void EdIzm_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (fbox.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO EdIzm ([Название единицы измерения]) VALUES ('" + fbox.Text + "')"))
                    {


                        await UpdateTable();

                       
                        EdIzmgrid.Visibility = Visibility.Hidden;
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

                    if (await sql.CommnadWithNonQuery("UPDATE EdIzm SET [Название единицы измерения] = '" + fbox.Text + "' where id = " + (EdIzmViewer.SelectedItem as DataRowView).Row["id"].ToString()))
                    {
                                              
                        EdIzmgrid.Visibility = Visibility.Hidden;
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
            EdIzmgrid.Visibility = Visibility.Hidden;
            fbox.Clear();
        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(EdIzmViewer,ka);
        }
    }
}
