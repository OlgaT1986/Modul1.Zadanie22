using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul1.Zadanie22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер массива: ");
            int n = Convert.ToInt32(Console.ReadLine());
            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            // Task<int[]> параметризируем массив, ожидаем получить массив и нужно вернуть результат
            Task<int[]> task1 = new Task<int[]>(func1, n);  

            Action<Task<int[]>> action = new Action<Task<int[]>>(PrintArray);
            Task task2 = task1.ContinueWith(action);

            task1.Start();
            task2.Wait();
            Console.WriteLine();

            Action<Task<int[]>> func2 = new Action<Task<int[]>>(Summ);
            Task task3 = task1.ContinueWith(func2);

            Action<Task<int[]>> func3 = new Action<Task<int[]>>(Max);
            Task task4 = task1.ContinueWith(func3);

            task3.Wait();
            task4.Wait();

            Console.ReadKey();
        }
        // метод формирует массив, сразу преобразовываем метод чтобы он создавал делегат object 
        static int[] GetArray(object a) 
        {
            // получаем число путем приведения из object в int
            int n = (int)a;  
            int[] array = new int[n];
            // заполняем случайными числами массив
            Random random = new Random();  
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 500);
            }
            return array;
        }
        ////метод сортирует массив и сразу принимает Task  
        static int[] SortArray(Task<int[]> task)  
        {
            int[] array = task.Result;
            //Count возвращает кол-во эл-тов в массиве
            for (int i = 0; i < array.Count() - 1; i++) 
            {
                for (int j = i + 1; j < array.Count(); j++)
                {
                    // если элементы стоят не по возрастанию (когда предыдущий больше чем следующий)
                    if (array[i] > array[j]) 
                    {
                        //меняем их местами и используем переменную - m
                        int m = array[i]; 
                        array[i] = array[j];
                        array[j] = m;
                    }
                }
            }
            return array;
        }
        // метод будет выводить массив 
        static void PrintArray(Task<int[]> task)
        {
            int[] array = task.Result;
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"{array[i] } ");
            }
        }
        // метод вычисляет сумму чисел массива
        static void Summ(Task<int[]> task)
        {
            int m = 0;
            int[] array = task.Result;
            for (int j = 0; j < array.Count(); j++) m = m + array[j];
            Console.WriteLine("Сумма чисел в массиве = " + m);
        }
        // метод вычисляет максимальное число в массиве
        static void Max(Task<int[]> task)
        {
            int m;
            int[] array = task.Result;
            m = array.Max();
            Console.WriteLine("Максимальное число в массиве = " + m);
        }
    }
}

