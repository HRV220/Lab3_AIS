using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MyLibrary;

class Client
{
    private const int Port = 8080;
    private static UdpClient udpClient;

    public static async Task Main(string[] args)
    {
        udpClient = new UdpClient();
        string command = "";
        string num_str = "";

        while (true)
        {
            Console.Clear();
            Console.WriteLine(
                   "1. Вывод всех записей на экран.\n" +
                   "2. Вывод записи по номеру.\n" +
                   "3. Удаление записи.\n" +
                   "4. Добавление записи.\n" +
                   "Для выхода из приложения ESC.");

            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    {
                        Console.Clear();
                        command = "get_all_data";
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        Console.Clear();
                        Console.WriteLine("Введите номер строки для вывода");
                        num_str = Console.ReadLine();
                        command = $"get_str:{num_str}";
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        Console.Clear();
                        Console.WriteLine("Введите номер строки для удаления");
                        num_str = Console.ReadLine();
                        command = $"del_str:{num_str}";
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        Console.Clear();
                        command = $"add_str:{AddRecordToFile()}";
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        Console.Clear();
                        command = "destroy";
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        break;
                    }
            }
            byte[] requestBytes = Encoding.UTF8.GetBytes(command);
            await udpClient.SendAsync(requestBytes, requestBytes.Length, "127.0.0.1", Port);

            var receivedResult = await udpClient.ReceiveAsync();
            string response = Encoding.UTF8.GetString(receivedResult.Buffer);
            if (key.Key == ConsoleKey.D1)
            {
                Console.Clear();
                try
                {
                    string sep = new string('-', 161);
                    Console.WriteLine(sep);
                    Console.WriteLine($"| {"№",-5} | {"Производитель",-15} | {"Модель",-10} | {"Цвет",-10} | {"VIN",-20} | {"Цена",-15} | {"Год выпуска",-15} | {"Километраж",-15} | {"Машина продана",-15} | {"Аварии",-10} |");
                    Console.WriteLine(sep);
                    string[] strings = response.Split('\n');
                    for (int i = 0; i < (strings.Length - 1); i++)
                    {
                        CarAuction car = CarAuction.Parse(strings[i]);
                        Console.WriteLine($"| {car.Id,-5} | {car.Maker,-15} | {car.Model,-10} | {car.Color,-10} | {car.VIN,-20} | {car.Price,-15} | {car.YearProd,-15} | {car.Mileage,-15} | {car.IsSold,-15} | {car.HasAccidents,-10} |");
                        Console.WriteLine(sep);
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(response);
                }
                Console.ReadKey();
            }
            else if (key.Key == ConsoleKey.D2)
            {
                Console.Clear();
                try
                {
                    string sep = new string('-', 161);
                    Console.WriteLine(sep);
                    Console.WriteLine($"| {"№",-5} | {"Производитель",-15} | {"Модель",-10} | {"Цвет",-10} | {"VIN",-20} | {"Цена",-15} | {"Год выпуска",-15} | {"Километраж",-15} | {"Машина продана",-15} | {"Аварии",-10} |");
                    Console.WriteLine(sep);
                    CarAuction car = CarAuction.Parse(response);
                    Console.WriteLine($"| {car.Id,-5} | {car.Maker,-15} | {car.Model,-10} | {car.Color,-10} | {car.VIN,-20} | {car.Price,-15} | {car.YearProd,-15} | {car.Mileage,-15} | {car.IsSold,-15} | {car.HasAccidents,-10} |");
                    Console.WriteLine(sep);
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(response);
                }
                Console.ReadKey();
            }
            else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.D4)
            {
                Console.Clear();
                Console.WriteLine(response);
                Console.ReadKey();
            }
        }
    }
    public static string AddRecordToFile()
    {
        CarAuction car = new CarAuction();
        while (car.Maker == "" || car.Maker == null)
        {
            Console.Clear();
            Console.Write("Введите название марки производителя автомобиля: ");
            car.Maker = Console.ReadLine();
        }
        while (car.Model == "" || car.Model == null)
        {
            Console.Clear();
            Console.Write("Введите название модели автомобиля: ");
            car.Model = Console.ReadLine();
        }
        while (car.Color == "" || car.Color == null)
        {
            Console.Clear();
            Console.Write("Введите цвет автомобиля: ");
            car.Color = Console.ReadLine();
        }
        while (car.VIN == "" || car.VIN == null)
        {
            Console.Clear();
            Console.Write("Введите VIN автомобиля: ");
            car.VIN = Console.ReadLine();
        }
        int intVar;
        while (car.Price <= 0 || car.Price == null)
        {
            Console.Clear();
            Console.Write("Введите цену автомобиля: ");
            if (int.TryParse(Console.ReadLine(), out intVar))
            {
                car.Price = intVar;
            }
            else
            {
                car.Price = -1;
            }
        }
        while (car.YearProd <= 0 || car.YearProd == null || car.YearProd >= DateTime.Now.Year)
        {
            Console.Clear();
            Console.Write("Введите год выпуска автомобиля: ");
            if (int.TryParse(Console.ReadLine(), out intVar))
            {
                car.YearProd = intVar;
            }
            else
            {
                car.YearProd = -1;
            }
        }
        while (car.Mileage <= 0 || car.Mileage == null)
        {
            Console.Clear();
            Console.Write("Введите пробег автомобиля: ");
            if (int.TryParse(Console.ReadLine(), out intVar))
            {
                car.Mileage = intVar;
            }
            else
            {
                car.Mileage = -1;
            }
        }
        ConsoleKeyInfo key; // Объявляем переменную key вне цикла do

        do
        {
            Console.Clear();
            Console.WriteLine("Укажите была ли машина в авариях\n1. Да\n2. Нет");
            key = Console.ReadKey(); // Присваиваем значение переменной key

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    car.HasAccidents = true;
                    break;
                case ConsoleKey.D2:
                    car.HasAccidents = false;
                    break;
            }

        } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2);
        return car.ToString();
    }
}