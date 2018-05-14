using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zacha\Documents\Projects\academypgh\car-collector\CarCollector\CarDatabase.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            DatabaseService dbService = new DatabaseService(connection);
            string input = "";
            while(input != "e")
            {
                Console.WriteLine("Welcome to your car collector app!");
                Console.WriteLine("Please select and action:");
                Console.WriteLine("[A]dd to Collection");
                Console.WriteLine("[V]iew Collection");
                Console.WriteLine("[E]xit");
                input = Console.ReadLine().ToLower();
                Console.Clear();
                switch(input[0])
                {
                    case 'a':
                        AddCollection(dbService);
                        break;
                    case 'v':
                        ViewCollection(dbService);
                        break;
                    case 'e':
                        break;
                    default:
                        Console.WriteLine("Input is invalid! Please try again");
                        break;
                }
            }
        }
        static void ViewCollection(DatabaseService dbService)
        {
            string input = "l";
            while (input != "e")
            {
                Console.WriteLine("What information would you like to view?");
                Console.WriteLine("[A] All");
                Console.WriteLine("[Ma] Manufacturers");
                Console.WriteLine("[Mo] Models");
                Console.WriteLine("[C] Cars");
                Console.WriteLine("[E] Exit");
                input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "ma":
                        var manufacturers = dbService.GetAllManufacturers();
                        if (manufacturers.Count > 0)
                            foreach (var manufacturer in manufacturers)
                            {
                                Console.WriteLine("Id: {0}, Name: {1}", manufacturer.Id, manufacturer.Name);
                            }
                        else
                            Console.WriteLine("There are not any car Manufacturers in your collection.");
                        break;
                    case "mo":
                        var models = dbService.GetAllModels();
                        if (models.Count > 0)
                            foreach (var model in models)
                            {
                                Console.WriteLine("Id: {0}, Name: {1}, ManufacturerId: {2}", model.Id, model.Name, model.ManufacturerId);
                            }
                        else
                            Console.WriteLine("There are not any car models in your collection.");
                        break;
                    case "c":
                        var cars = dbService.GetAllCars();
                        if (cars.Count > 0)
                            foreach (var car in cars)
                            {
                                Console.WriteLine("Id: {0}, Series: {1}, Year: {2}, ModelId: {3}", car.Id, car.Series, car.Year, car.ModelId);
                            }
                        else
                            Console.WriteLine("There are not any cars in your collection.");
                        break;
                    case "a":
                        var allCars = dbService.GetDetailedList();
                        if (allCars.Count > 0)
                            foreach (var car in allCars)
                            {
                                Console.WriteLine("{0} {1} {2} {3}", car.Manufacturer, car.Model, car.Series, car.Year);
                            }
                        else
                            Console.WriteLine("There are not any cars in your collection.");
                        break;
                    case "e":
                        break;
                    default:
                        Console.WriteLine("Input is invalid!");
                        break;
                }
            }
            Console.Clear();
        }
        static void AddCollection(DatabaseService dbService)
        {
            string input = "a"; //random letter to initialize
            while (input != "e")
            {
                Console.WriteLine("What would you like to add?");
                Console.WriteLine("[Ma] Manufacturers");
                Console.WriteLine("[Mo] Models");
                Console.WriteLine("[C] Cars");
                Console.WriteLine("[E] Exit");
                input = Console.ReadLine().ToLower();
                string sql;
                int id;
                switch (input)
                {
                    case "ma":
                        Console.WriteLine("Specify a Manufacturer name:");
                        string maName = Console.ReadLine();
                        sql = $"INSERT INTO Manufacturers VALUES('{maName}')";
                        id = dbService.InsertWithId(sql);
                        Console.WriteLine($"Manufacturer: {maName} inserted with id: {id}");
                        break;
                    case "mo":

                        Console.WriteLine("Specify a Model name:");
                        string moName = Console.ReadLine();
                        Console.WriteLine("Specify the manufacturerId of the model:");
                        string moMaId = Console.ReadLine();
                        sql = $"INSERT INTO Models VALUES('{moName}',{moMaId})";
                        id = dbService.InsertWithId(sql);
                        Console.WriteLine($"Model: {moName} inserted with id: {id}");
                        break;
                    case "c":

                        Console.WriteLine("Specify the car series:");
                        string cSeries = Console.ReadLine();
                        Console.WriteLine("Specify the cars year:");
                        string cYear = Console.ReadLine();
                        Console.WriteLine("Specify the cars modelId:");
                        string cModelId = Console.ReadLine();
                        sql = $"INSERT INTO Cars VALUES('{cSeries}',{cYear},{cModelId})";
                        id = dbService.InsertWithId(sql);
                        Console.WriteLine($"Car: {cSeries} inserted with id: {id}");
                        break;
                    case "e":
                        break;
                    default:
                        Console.WriteLine("Input is invalid!");
                        break;
                }
            }
            Console.Clear();
        }
    }
}
