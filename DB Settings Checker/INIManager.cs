using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DB_Settings_Checker
{
    public class INIManager
    {
        public INIManager(string aPath)     //конструктор, принимающий путь к INI-файлу
        {
            path = aPath;
        }
        public INIManager() : this("") { }

        //возвращает значение из INI-файла 
        public string GetPrivateString(string aSection, string aKey)
        {
            //для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            return buffer.ToString();
        }

        //пишем значение в INI-файл
        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            WritePrivateString(aSection, aKey, aValue, path);
        }

        //возвращает или устанавливает путь к INI файлу
        public string Path { get { return path; } set { path = value; } }

        //поля класса
        private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу

        //импорт функции GetPrivateProfileString (для чтения значений) 
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        //импорт функции WritePrivateProfileString (для записи значений) 
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

    }
}
