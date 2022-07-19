using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine
{
    
    public static class SR
    {
       
        public static string POSTAVSHIKI = "SELECT id as [Индекс поставщика], [ФИО] as [ФИО], [Номер телефона] as [Номер телефона], [Адрес] as [Адрес] FROM Postavshik";
        public static string SOTRUDNIKI = "SELECT id, [Фамилия] as [Фамилия], [Имя] as [Имя], [Отчество] as [Отчество], [Возраст] as [Возраст], [Стаж работы на предприятии] as [Стаж работы на предприятии], [Рабочий номер телефона] as [Рабочий номер телефона], [Адрес проживания] as [Адрес проживания], [Должность] as [Должность] FROM Sotrudniki";
        public static string SKLAD = "SELECT id as [Код товара] , [Название товара] as [Название товара], [Количество на складе] as [Количество на складе], [Единица измерения] as [Единица измерения], [Категория] as [Категория], [Стоимость(руб.)] as [Стоимость(руб.)] FROM Sklad ";
        public static string KATEGORII = "SELECT [id] as [id], [Название категории] as [Название категории] FROM Kategorii";
        public static string EDINICIIZMERENIA = "SELECT [id] as [id], [Название единицы измерения] as [Название единицы измерения] FROM EdIzm";
        public static string DOLJNOST = "SELECT [id] as [id], [Название должности] as [Название должности] FROM Doljnost";
        public static string ObshInforOtPr = "SELECT [obshid] as [id], [Наименование товара] as [Название товара], [Категория товара] as [Категория], [Поставщик] as [Поставщик], [Код данного товара] as [Код товара], [Дата поставки товара] as [Дата поставки] FROM ObshInforOtPr";
        public static string POSTAVKATOVARA = "SELECT id , [Индекс поставщика] as [Индекс поставщика], [Код товара] as [Код товара], [Стоимость поставки] as [Стоимость поставки], [Количество] as [Количество], [Ответственный за поставку] as [Ответственный за поставку], [Дата поставки] as [Дата поставки] FROM PostavkiTovara";

    }
}
 