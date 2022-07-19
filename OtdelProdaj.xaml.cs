using System;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Sql;
using MySql.Data.MySqlClient;
using Word = Microsoft.Office.Interop.Word;
using System.Data;



namespace Magazine
{
    /// <summary>
    /// Логика взаимодействия для OtdelProdaj.xaml
    /// </summary>
    public partial class OtdelProdaj : Window
    {
        string Login = "1";
        string password = "1";
        private readonly string TempFile = @"C:\Users\$Jugg$\Desktop\диплом\Договор.docx";
        public OtdelProdaj()
        {
            InitializeComponent();
        }



        tabledata.sklad sklad;
        tabledata.kategorii kategoria;
        tabledata.postavshik postavshik;
        PostTovaraOtProdaj win;
        Sklad win2;
        int ka = 5;
        
        


        public async Task UpdateInfo()
        {

          
            List<SelectedData> sklad_data = new List<SelectedData>();
          
          
            List<SelectedData> kategoria_data = new List<SelectedData>();
            List<SelectedData> kategoriadop_data = new List<SelectedData>();
            
            List<SelectedData> postavshik_data = new List<SelectedData>();
            List<SelectedData> postavshikdop_data = new List<SelectedData>();
           
        
            double maxPrice, minPrice;
           
            var pr = await sql.CommnadWithQuery("SELECT MAX([Код данного товара]) as [MAX(Код данного товара)], MIN([Код данного товара]) as [MIN(Код данного товара)] from ObshInforOtPr");
            if (pr.Select()[0][0].ToString() != "")
            {
                maxPrice = Convert.ToDouble(pr.Select()[0]["MAX(Код данного товара)"].ToString());
                minPrice = Convert.ToDouble(pr.Select()[0]["MIN(Код данного товара)"].ToString());
                if (maxPrice != minPrice)
                {

                    priceSlide.Minimum = minPrice;
                    priceSlide.LowerValue = minPrice;
                    priceSlide.Maximum = maxPrice;
                    priceSlide.UpperValue = maxPrice;
                }
                else
                {
                    priceSlide.Minimum = 0;
                    priceSlide.LowerValue = 0;
                    priceSlide.Maximum = maxPrice;
                    priceSlide.UpperValue = maxPrice;
                }

            }
            var tov = await sql.CommnadWithQuery("SELECT id, [Название товара] as [Название товара] from Sklad");
            var tov_a = tov.Select();
            for (int i = 0; i < tov_a.Length; i++)
            {
                sklad_data.Add(new SelectedData() { id = tov_a[i]["id"].ToString(), title = tov_a[i]["Название товара"].ToString() });
            }

               
            var kat = await sql.CommnadWithQuery("SELECT id, [Название категории] as [Название категории] from Kategorii");
            var kat_a = kat.Select();
            for (int i = 0; i < kat_a.Length; i++)
            {
                kategoria_data.Add(new SelectedData() { id = kat_a[i]["id"].ToString(), title = kat_a[i]["Название категории"].ToString() });
            }

            var katdop = await sql.CommnadWithQuery("SELECT id, [Название категории] as [Название категории] from Kategorii");
            var katdop_a = katdop.Select();
            for (int i = 0; i < katdop_a.Length; i++)
            {
                kategoriadop_data.Add(new SelectedData() { id = kat_a[i]["id"].ToString(), title = kat_a[i]["Название категории"].ToString() });
            }

            var post = await sql.CommnadWithQuery("SELECT id, [ФИО] as [ФИО] from Postavshik");
            var post_a = post.Select();
            for (int i = 0; i < post_a.Length; i++)
            {
                postavshik_data.Add(new SelectedData() { id = post_a[i]["id"].ToString(), title = post_a[i]["ФИО"].ToString() });
            }

            var postdop = await sql.CommnadWithQuery("SELECT id, [ФИО] as [ФИО] from [Postavshik]");
            var postdop_a = postdop.Select();
            for (int i = 0; i < postdop_a.Length; i++)
            {
                postavshikdop_data.Add(new SelectedData() { id = postdop_a[i]["id"].ToString(), title = postdop_a[i]["ФИО"].ToString() });
               
            }

            TovarBoxlist.ItemsSource = sklad_data;
            KatBoxList.ItemsSource = kategoria_data;
            PostBoxList.ItemsSource = postavshik_data;
            KategBoxlist.ItemsSource = kategoriadop_data;
            PostBoxlist.ItemsSource = postavshikdop_data;
            NazvBoxList.ItemsSource = sklad_data;
          



        }
        Sqlcon sql = new Sqlcon();

        async Task UpdateTable()
        {

            LoadingShow();
            try
            {

                var d = await sql.CommnadWithQuery(SR.ObshInforOtPr);
                GridView.ItemsSource = d.DefaultView;
                GridView.Columns[0].Visibility = Visibility.Hidden;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadingShow();
        }

        void LoadingShow()
        {
            if (LoadingGrid.Visibility == Visibility.Hidden)
            {
                LoadingGrid.Visibility = Visibility.Visible;
            }
            else { LoadingGrid.Visibility = Visibility.Hidden; }
        }
        public class SelectedData
        {
            public string id { get; set; }
            public string title { get; set; }
        }



        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnclr_Click(object sender, RoutedEventArgs e)
        {
            priceSlide.LowerValue = priceSlide.Minimum;
            priceSlide.UpperValue = priceSlide.Maximum;
            TovarBoxlist.SelectedIndex = -1;
            KategBoxlist.SelectedIndex = -1;
            PostBoxlist.SelectedIndex = -1;
            datePickerPost.Text = "";

        }

        private async void btndob_Click(object sender, RoutedEventArgs e)
        {
            await UpdateInfo();
            global_grid.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            global_grid.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            printGrid.Visibility = Visibility.Hidden;
        }

        private void btnsotr_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            
            SotrOtProdaj m = new SotrOtProdaj(this,win,sql);
            m.Show();
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            PostOtdProdaj m = new PostOtdProdaj(this,win,sql);
            m.Show();
        }

        private void btnPostTovara_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            PostTovaraOtProdaj m = new PostTovaraOtProdaj(this,sql);
            m.Show();
        }

        private void btnSklad_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Sklad m = new Sklad(this,win, sql);
            m.Show();
        }

        private async void btnud_Click(object sender, RoutedEventArgs e)
        {
            if (GridView.SelectedItem != null)
            {
                
                    var item = (GridView.SelectedItem as DataRowView);
                    if (await sql.Delete("obshid", item[0].ToString(), "ObshInforOtPr"))
                    {

                        await UpdateInfo();
                        await UpdateTable();
                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Проблемма при удалении выбранной позиции :(");
                    }


                }


            }
         
            
        

        private async void GridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridView.SelectedItem != null)
            {
                await ShowInfo((GridView.SelectedItem as DataRowView)[0].ToString());
            }
        }

        public async Task ShowInfo(string id)
        {

           
            //var info_s = await sql.CommnadWithQuery("SELECT [Название товара] , [ФИО] , [Количество на складе] , [Номер телефона], [Единица измерения] , [Адрес] , [Категория] , [Стоимость(руб.)] , [Индекс поставщика], [Код товара] , [Стоимость поставки] , [Количество] , [Ответственный за поставку], [Дата поставки] , [Фамилия] , [Имя] , [Отчество] , [Возраст] , [Стаж работы на предприятии], [Рабочий номер телефона], [Адрес проживания] , [Должность] from [Sklad],[Postavshik],[PostavkiTovara],[Sotrudniki] ");
            var info_s = await sql.CommnadWithQuery("SELECT [Название товара] , [ФИО] , [Количество на складе] , [Номер телефона], [Единица измерения] , [Адрес] , [Категория] , [Стоимость(руб.)], [Индекс поставщика], [Код товара] , [Стоимость поставки] , [Количество] , [Ответственный за поставку], [Дата поставки] , [Фамилия] , [Имя] , [Отчество] , [Возраст] , [Стаж работы на предприятии], [Рабочий номер телефона], [Адрес проживания] , [Должность] from [ObshInforOtPr] JOIN [Sklad] on [Наименование товара] = [Название товара] JOIN [Postavshik] on [Поставщик] = [ФИО] JOIN [PostavkiTovara] on [Дата поставки товара] = [Дата поставки] AND [Код данного товара] = [Код товара] JOIN [Sotrudniki] on [Ответственный за поставку] = [Фамилия]+' '+[Имя]+' '+[Отчество] where [obshid] = " + id);

            var info = info_s.Select()[0];
            naimtovar.Content = info["Название товара"].ToString();
            kolvosklad.Content = info["Количество на складе"].ToString();
            edizm.Content = info["Единица измерения"].ToString();           
            kateg.Content = info["Категория"].ToString();
            stoim.Content = info["Стоимость(руб.)"].ToString();

            Fio.Content = info["ФИО"].ToString();
            Phone.Content = info["Номер телефона"].ToString();
            adress.Content = info["Адрес"].ToString();
          
            IndexPost.Content = info["Индекс поставщика"].ToString();
            KodTovara.Content = info["Код товара"].ToString();
            StoimPost.Content = info["Стоимость поставки"].ToString();
            Kolvo.Content = info["Количество"].ToString();
            OtvZaPost.Content = info["Ответственный за поставку"].ToString();
            DataPost.Content = info["Дата поставки"].ToString();

            FamSotr.Content = info["Фамилия"].ToString();
            NameSotr.Content = info["Имя"].ToString();
            OtchSotr.Content = info["Отчество"].ToString();
            VozrSotr.Content = info["Возраст"].ToString();
            StajSotr.Content = info["Стаж работы на предприятии"].ToString();
            NomerSotr.Content = info["Рабочий номер телефона"].ToString();
            AdresSotr.Content = info["Адрес проживания"].ToString();
            DoljSotr.Content = info["Должность"].ToString();
          




            printGrid.Visibility = Visibility.Visible;

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {



            await UpdateTable();
            await UpdateInfo();
        }

        private void TovarBoxlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Nazvbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Sklad s = new Sklad(this,win,sql);
            s.Show();
        }

        private void Postbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            
            PostOtdProdaj p = new PostOtdProdaj(this,win,sql);
            p.Show();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (NazvBoxList.Text != "" & KatBoxList.Text != "" & PostBoxList.Text != "" & StoimBox.Text != "" & beginDate.Text != "")
            {
                if (await sql.CommnadWithNonQuery("INSERT INTO ObshInforOtPr ([Наименование товара],[Категория товара],[Поставщик],[Код данного товара],[Дата поставки товара]) values ('" + NazvBoxList.Text + "','" + KatBoxList.Text + "','" + PostBoxList.Text + "','" + StoimBox.Text + "','" + beginDate.Text + "')"))
                {


                    await UpdateTable();
                    global_grid.Visibility = Visibility.Hidden;
                    informer.MessageQueue.Enqueue("Успешно добавленно!");
                    StoimBox.Text = "";
                    beginDate.Text = "";
                 }


                else
                {
                    informer.MessageQueue.Enqueue("Сбой при добавлении!");
                }
                }
            
            else
            {
                informer.MessageQueue.Enqueue("Необходимо заполнить все поля!");

            }
        }

        private async void btnfind_Click(object sender, RoutedEventArgs e)
        {
            LoadingShow();

            string query = "SELECT obshid,[Наименование товара] as [Название товара], [Категория товара] as [Категория], [Поставщик] as [Поставщик], [Код данного товара] as [Код товара], [Дата поставки товара] as [Дата поставки] from [ObshInforOtPr] as [ObshInforOtPr] WHERE [Код данного товара] BETWEEN '" + Convert.ToInt32(priceSlide.LowerValue) + "' AND '" + Convert.ToInt32(priceSlide.UpperValue)+"'";
            if (TovarBoxlist.Text != "")
            {


                query += " AND [Наименование товара] = '" + (TovarBoxlist.Text)+"'";

                

            }

            if (KategBoxlist.Text != "")
            {


                query += " AND [Категория товара] = '" + (KategBoxlist.Text)+"'";

               

            }

            if (PostBoxlist.Text != "")
            {


                query += " AND [Поставщик] = '" + (PostBoxlist.Text)+"'";

               

            }


            if (datePickerPost.Text != "")
            {
                query += " AND [Дата поставки товара] = '" + datePickerPost.Text + "'";
            }
           
          
            var d = await sql.CommnadWithQuery(query);
            GridView.ItemsSource = d.DefaultView;
            GridView.Columns[0].Visibility = Visibility.Hidden;
            LoadingShow();
        }

        private async void KatBoxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

        }

        private void Katerbtn_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Kategorii k = new Kategorii(this,win2,sql);
            k.Show();
        }

        private void avtor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (login.Text != "" & pass.Password != "")
                {
                    if (login.Text == Login & pass.Password == password)
                    {
                        Auth.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        informer.MessageQueue.Enqueue("Неверный логин или пароль!");
                    }
                }

                else
                {
                    informer.MessageQueue.Enqueue("Не все поля заполнены!");
                }

            }
            catch (Exception ex)
            {
                informer.MessageQueue.Enqueue("Проблемы с авторизацией. Обратитесь в тех.поддержку");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            
        }

        private void btnviv_Click(object sender, RoutedEventArgs e)
        {
            PrintExcel.Print(GridView,ka);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var Date = Convert.ToString(DateTime.Now);
            //sklad tovar
            var naimtov = Convert.ToString(naimtovar.Content);
            var kolvo= Convert.ToString(kolvosklad.Content);
            var ediz= Convert.ToString(edizm.Content);
            var kategorii= Convert.ToString(kateg.Content);
            var stoimost= Convert.ToString(stoim.Content);
            //postavshk
            var FIO = Convert.ToString(Fio.Content);
            var Nomet = Convert.ToString(Phone.Content);
            var Adress = Convert.ToString(adress.Content);
            //postavkatovara
            var Index = Convert.ToString(IndexPost.Content);
            var Kod = Convert.ToString(KodTovara.Content);
            var StoimPos = Convert.ToString(StoimPost.Content);
            var KolvoTov = Convert.ToString(Kolvo.Content);
            var Otv = Convert.ToString(OtvZaPost.Content);
            var DataPostavki =Convert.ToString(DataPost.Content);
            //sotrudnik
            var FamS  = Convert.ToString(FamSotr.Content);
            var Name = Convert.ToString(NameSotr.Content);
            var Otch = Convert.ToString(OtchSotr.Content);
            var Vozr = Convert.ToString(VozrSotr.Content);
            var Staj = Convert.ToString(StajSotr.Content);
            var Numb  = Convert.ToString(NomerSotr.Content);
            var Adres = Convert.ToString(AdresSotr.Content);
            var Dolj = Convert.ToString(DoljSotr.Content);

            // Перевод в ворд
            var wordapp = new Word.Application();   
            wordapp.Visible = false;
            try
            {
                var wordDoc = wordapp.Documents.Open(TempFile);

                ReplaceWord("{FamS}", FamS, wordDoc);
                ReplaceWord("{FamS2}", FamS, wordDoc);
                ReplaceWord("{Name}", Name, wordDoc);
                ReplaceWord("{Name2}", Name, wordDoc);
                ReplaceWord("{Otch}", Otch, wordDoc);
                ReplaceWord("{Otch2}", Otch, wordDoc);
                ReplaceWord("{Adres}", Adres, wordDoc);
                ReplaceWord("{Dolj}", Dolj, wordDoc);
                ReplaceWord("{Staj}", Staj, wordDoc);
                ReplaceWord("{DataPostavki}", DataPostavki, wordDoc);
                ReplaceWord("{DataPostavki2}", DataPostavki, wordDoc);
                ReplaceWord("{naimtov}", naimtov, wordDoc);
                ReplaceWord("{naimtov2}", naimtov, wordDoc);
                ReplaceWord("{KolvoTov}", KolvoTov, wordDoc);
                ReplaceWord("{KolvoTov2}", KolvoTov, wordDoc);
                ReplaceWord("{StoimPos}", StoimPos, wordDoc);
                ReplaceWord("{StoimPos2}", StoimPos, wordDoc);
                ReplaceWord("{Index}", Index, wordDoc);
                ReplaceWord("{Nomet}", Nomet, wordDoc);
                ReplaceWord("{Nomet2}", Nomet, wordDoc);
                ReplaceWord("{Adress}", Adress, wordDoc);
                ReplaceWord("{Numb}", Numb, wordDoc);
                ReplaceWord("{FIO}", FIO, wordDoc);
                ReplaceWord("{FIO2}", FIO, wordDoc);
                ReplaceWord("{FIO3}", FIO, wordDoc);
                ReplaceWord("{Otvza}", Otv, wordDoc);
                ReplaceWord("{Kod}", Kod, wordDoc);
                ReplaceWord("{Date}", Date, wordDoc);

                wordDoc.SaveAs(@"C:\Users\$Jugg$\Desktop\диплом\ДоговорПередел.docx");
               
                wordapp.Visible = true;
            }
            catch {
                MessageBox.Show("Произошла ошибка");
            }
        }
        private void ReplaceWord(string stub, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText:stub,ReplaceWith:text);
        }
    }
}
