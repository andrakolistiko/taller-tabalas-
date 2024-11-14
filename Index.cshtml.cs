using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace taller.Pages
{
    public class IndexModel : PageModel
    {
        public List<PersonaInfo> PersonasList { get; set; } = new List<PersonaInfo>();
        public List<MascotaInfo> MascotasList { get; set; } = new List<MascotaInfo>();

        private string connectionString = "Server=.;Database=taller001;Trusted_Connection=True;TrustServerCertificate=True;";

        public void OnGet()
        {
            try
            {
                // Obtener personas y mascotas en un solo bloque de código
                PersonasList = GetDataFromDatabase<PersonaInfo>("SELECT * FROM persona", MapPersonaData);
                MascotasList = GetDataFromDatabase<MascotaInfo>("SELECT * FROM mascota", MapMascotaData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Método genérico para obtener datos de la base de datos
        private List<T> GetDataFromDatabase<T>(string sqlQuery, Func<SqlDataReader, T> mapData)
        {
            List<T> resultList = new List<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(mapData(reader));  // Mapea el dato utilizando la función proporcionada
                        }
                    }
                }
            }

            return resultList;
        }

        // Método para mapear los datos de persona
        private PersonaInfo MapPersonaData(SqlDataReader reader)
        {
            return new PersonaInfo
            {
                idPersona = reader.GetInt32(0),
                nombre = reader.GetString(1),
                apellido = reader.GetString(2),
                correo = reader.GetString(3)
            };
        }

        // Método para mapear los datos de mascota
        private MascotaInfo MapMascotaData(SqlDataReader reader)
        {
            return new MascotaInfo
            {
                idMascota = reader.GetInt32(0),
                nombre = reader.GetString(1),
                especie = reader.GetString(2),
                color = reader.GetString(3)
            };
        }

        // Clases de datos
        public class PersonaInfo
        {
            public int idPersona { get; set; }
            public string nombre { get; set; } = string.Empty;
            public string apellido { get; set; } = string.Empty;
            public string correo { get; set; } = string.Empty;
        }

        public class MascotaInfo
        {
            public int idMascota { get; set; }
            public string nombre { get; set; } = string.Empty;
            public string especie { get; set; } = string.Empty;
            public string color { get; set; } = string.Empty;
        }
    }
}
