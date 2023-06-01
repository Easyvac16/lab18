using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace lab18
{
    internal class cs1
    {
        public static void task_1()
        {
            int[] array;
            Console.OutputEncoding = Encoding.Unicode;

            //Введення масиву
            array = ReadArrayFromConsole();
            Console.WriteLine("Масив був успішно введений.");
            
            
            //Фільтрація чисел
            Console.WriteLine("Виберіть фільтр:");
            Console.WriteLine("1. Видалити прості числа");
            Console.WriteLine("2. Видалити числа Фібоначчі");
            string filterOption = Console.ReadLine();
            array = ApplyFilter(array, filterOption);
            Console.WriteLine("Фільтрація була успішно застосована.");

            //Серелізація масиву
            SerializeArray(array);
            Console.WriteLine("Масив був успішно селерізований.");

            //Збереження селерізованого масиву у файл
            SaveArrayToFile(array);
            Console.WriteLine("Масив був успішно збережений у файл.");


            //Виведення серелізованого масиву на екран
            array = LoadSerializedArrayFromFile();
            
            Console.WriteLine("Масив був успішно завантажений з файлу.");
            Console.WriteLine(string.Join(",", array));


            Console.WriteLine();
            
        }
        static void SerializeArray(int[] array)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream("array.dat", FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, array);
            }
        }

        static int[] ReadArrayFromConsole()
        {
            Console.WriteLine("Введіть елементи масиву, розділені пробілом:");
            string input = Console.ReadLine();
            string[] numbers = input.Split(' ');

            int[] array = new int[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
                int number;
                if (int.TryParse(numbers[i], out number))
                {
                    array[i] = number;
                }
                else
                {
                    Console.WriteLine($"Невірний формат числа: {numbers[i]}. Ігноруємо.");
                }
            }

            return array;
        }

        static int[] ApplyFilter(int[] array, string option)
        {
            switch (option)
            {
                case "1":
                    return array.Where(number => !IsPrime(number)).ToArray();
                case "2":
                    return array.Where(number => !IsFibonacci(number)).ToArray();
                default:
                    Console.WriteLine("Невірний вибір фільтра. Повертаємо вихідний масив.");
                    return array;
            }
        }

        static bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        static bool IsFibonacci(int number)
        {
            if (number == 0 || number == 1)
                return true;

            int a = 0;
            int b = 1;
            int c = a + b;

            while (c <= number)
            {
                if (c == number)
                    return true;

                a = b;
                b = c;
                c = a + b;
            }

            return false;
        }

        static void SaveArrayToFile(int[] array)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("array.dat", FileMode.Create)))
            {
                writer.Write(array.Length);
                foreach (int number in array)
                {
                    writer.Write(number);
                }
            }
        
        }
        static int[] DeserializeArrayFromFile()
        {
            if (File.Exists("array.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream("array.dat", FileMode.Open, FileAccess.Read))
                {
                    return (int[])formatter.Deserialize(stream);
                }
            }
            else
            {
                Console.WriteLine("Файл array.dat не існує.");
                return null;
            }
        }

        static int[] LoadSerializedArrayFromFile()
        {
            if (File.Exists("array.dat"))
            {
                using (BinaryReader reader = new BinaryReader(File.Open("array.dat", FileMode.Open)))
                {
                    int length = reader.ReadInt32();
                    int[] array = new int[length];
                    for (int i = 0; i < length; i++)
                    {
                        array[i] = reader.ReadInt32();
                    }
                    return array;
                }
            }
            else
            {
                Console.WriteLine("Файл array.dat не існує.");
                return null;
            }
        }
    }

}
