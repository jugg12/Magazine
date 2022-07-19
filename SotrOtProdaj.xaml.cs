using System;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
    /// Логика взаимодействия для SotrOtProdaj.xaml
    /// </summary>
    public partial class SotrOtProdaj : Window
    {

        Sqlcon sql;
        tabledata.sotrudnik sotrudniki;
        tabledata.sotrudnik data;
        OtdelProdaj window;
        int ka = 8;
        PostTovaraOtProdaj window2;

        public SotrOtProdaj(OtdelProdaj win,PostTovaraOtProdaj win2, Sqlcon sql)
        {

            InitializeComponent();
            this.sql = sql;
            window = win;
            window2 = win2;


            sotrudniki = data;

        }
        List<SelectedData> dolj = new List<SelectedData>();
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
            string qy = SR.SOTRUDNIKI;


            var d = await sql.CommnadWithQuery(qy);
            if (d != null)
            {
                SotrViewer.ItemsSource = d.DefaultView;
                SotrViewer.Columns[0].Visibility = Visibility.Hidden;



            }
            else
            {
                informer.MessageQueue.Enqueue("Проблема с загрузкой таблицы");
            }
            LoadingShow();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await UpdateInfo();
            Sotrgrid.Visibility = Visibility.Visible;
        }

        private async void Sotr_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
        }

        public async Task UpdateInfo()
        {
            dolj = new List<SelectedData>();
            var a = await sql.CommnadWithQuery("SELECT id, [Название должности] FROM Doljnost");
            var b = a.Select();
            for (int i = 0; i < b.Length; i++)
            {
                dolj.Add(new SelectedData() { id = b[i]["id"].ToString(), title = b[i]["Название должности"].ToString() });
            }
            DoljBoxList.ItemsSource = dolj;
        }

        private async void cWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
            if (fbox.Text != "" & Ibox.Text != "" & Obox.Text != "" & Vbox.Text != "" && Stajbox.Text != "" && Nbox.Text != "" && Abox.Text != "" && DoljBoxList.Text != "")
            {
                if (!edit)
                {
                    if (await sql.CommnadWithNonQuery("INSERT INTO Sotrudniki ([Фамилия],[Имя],[Отчество],[Возраст],[Стаж работы на предприятии],[Рабочий номер телефона],[Адрес проживания],[Должность]) VALUES ('" + fbox.Text + "','" + Ibox.Text + "','" + Obox.Text + "','" + Vbox.Text + "','" + Stajbox.Text + "','" + Nbox.Text + "','" + Abox.Text + "','" + DoljBoxList.Text + "')"))
                    {


                        await UpdateTable();
                        DoljBoxList.SelectedIndex = -1;
                        Sotrgrid.Visibility = Visibility.Hidden;
                        informer.MessageQueue.Enqueue("Успешно добавленно.");

                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблема при добавлении.");
                    }
                }
                else
                {

                    if (await sql.CommnadWithNonQuery("UPDATE Sotrudniki SET [Фамилия] = '" + fbox.Text + "',[Имя] ='" + Ibox.Text + "' ,[Отчество] ='" + Obox.Text + "' ,[Возраст] ='"+Vbox.Text+"' ,[Стаж работы на предприятии] ='" + Stajbox.Text + "' , [Рабочий номер телефона] ='" + Nbox.Text + "' , [Адрес проживания] = '" + Abox.Text + "', [Должность] ='" + DoljBoxList.Text + "' where id = " + (SotrViewer.SelectedItem as DataRowView).Row["id"].ToString()))
                    {


                        DoljBoxList.SelectedIndex = -1;

                        Sotrgrid.Visibility = Visibility.Hidden;
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
                Ibox.Clear();
                Obox.Clear();
                Stajbox.Clear();
                Vbox.Clear();
                Abox.Clear();
                Nbox.Clear();


            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            fbox.Clear();
            Ibox.Clear();
            Obox.Clear();
            Stajbox.Clear();
            Vbox.Clear();
            Abox.Clear();
            Nbox.Clear();
            Sotrgrid.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (SotrViewer.SelectedItem != null)
            {
                edit = true;
                var row = (SotrViewer.SelectedItem as DataRowView);

                fbox.Text = row.Row["Фамилия"].ToString();
                Ibox.Text = row.Row["Имя"].ToString();
                Obox.Text = row.Row["Отчество"].ToString();
                Vbox.Text = row.Row["Возраст"].ToString();
                Stajbox.Text = row.Row["Стаж работы на предприятии"].ToString();
                Nbox.Text = row.Row["Рабочий номер телефона"].ToString();
                Abox.Text = row.Row["Адрес проживания"].ToString();
                DoljBoxList.Text = row.Row["Должность"].ToString();
                              
                Sotrgrid.Visibility = Visibility.Visible;


            }
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (SotrViewer.SelectedIndex != -1)
            {


                string id = (SotrViewer.SelectedItem as DataRowView).Row["id"].ToString();

                
                    string table="Sotrudniki";
              
              
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

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await UpdateTable();
            await UpdateInfo();
            this.Close();
        }

        private void Doljbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            Doljnost d = new Doljnost (this, sql);
            d.Show();
        }

        private void SotrViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(SotrViewer,ka);
        }
    }
    }


