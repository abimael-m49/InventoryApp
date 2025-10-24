using System;
using MySql.Data.MySqlClient;

namespace InventoryApp.Infrastructure
{
    public class DbConnectionFactory
    {
        private static DbConnectionFactory? _instance;

        private readonly string _connectionString;

        private DbConnectionFactory()
        {
            _connectionString =
                "Server=localhost;" +
                "Port=3306;" +
                "Database=inventario_db;" +
                "Uid=root;" +
                "Pwd=Abimael.m49;";
        }

        public static DbConnectionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbConnectionFactory();
                }
                return _instance;
            }
        }

        public MySqlConnection CreateOpen()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}