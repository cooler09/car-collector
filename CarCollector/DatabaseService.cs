using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarCollector.Models;

namespace CarCollector
{
    public class DatabaseService
    {
        private readonly SqlConnection _connection;

        public DatabaseService(SqlConnection connection)
        {
            _connection = connection;
        }
        public int ExecuteNonQuery(string sql)
        {
            SqlCommand command = new SqlCommand(sql, _connection);
            _connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            _connection.Close();
            return rowsAffected;
        }
        public int InsertWithId(string sql)
        {
            sql = sql + ";SELECT @@IDENTITY;";
            var command = new SqlCommand(sql, _connection);
            _connection.Open();
            int id = 0;
            var data = command.ExecuteScalar();
            if (data is int)
                id = Convert.ToInt32(data);
            _connection.Close();
            return id;
        }
        public List<Manufacturer> GetAllManufacturers()
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>();
            SqlDataReader reader = ExecuteReader("SELECT * FROM Manufacturers");
            if (reader.HasRows)
                while (reader.Read())
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                    manufacturers.Add(manufacturer);
                }
            _connection.Close();
            return manufacturers;
        }
        public List<Model> GetAllModels()
        {
            List<Model> models = new List<Model>();
            SqlDataReader reader = ExecuteReader("SELECT * FROM Models");
            if (reader.HasRows)
                while (reader.Read())
                {
                    Model model = new Model
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        ManufacturerId = Convert.ToInt32(reader["ManufacturerId"])
                    };
                    models.Add(model);
                }
            _connection.Close();
            return models;
        }
        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            SqlDataReader reader = ExecuteReader("SELECT * FROM Cars");
            if(reader.HasRows)
                while(reader.Read())
                {
                    Car car = new Car
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Series = reader["Series"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        ModelId = Convert.ToInt32(reader["ModelId"])
                    };
                    cars.Add(car);
                }
            _connection.Close();
            return cars;
        }
        public List<DetailedCar> GetDetailedList()
        {
            List<DetailedCar> cars = new List<DetailedCar>();
            SqlDataReader reader = ExecuteReader("SELECT ma.Name as Manufacturer, mo.Name as Model, c.Series, c.Year  FROM Cars as c inner join Models as mo on mo.Id = c.ModelId inner join Manufacturers as ma on ma.Id = mo.ManufacturerId");
            if (reader.HasRows)
                while (reader.Read())
                {
                    DetailedCar car = new DetailedCar
                    {
                        Manufacturer = reader["Manufacturer"].ToString(),
                        Series = reader["Series"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Model = reader["Model"].ToString()
                    };
                    cars.Add(car);
                }
            _connection.Close();
            return cars;
        }
        private SqlDataReader ExecuteReader(string sql)
        {
            var command = new SqlCommand(sql, _connection);
            _connection.Open();
            return command.ExecuteReader();
        }
    }
}
