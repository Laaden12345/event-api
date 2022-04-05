using Npgsql;
using event_api.Models;

namespace event_api.Services;

public static class PersonService
{
    public static string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

    static PersonService()
    {
    }

    public static Henkilo Get(int id)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Henkilo WHERE HenkiloID = @id;", conn);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return null;
            }
            reader.Read();
            // Ei jostain syystä onnistunut castaus suoraan Person-tyyppiin
            Henkilo henkilo = new Henkilo
            {
                HenkiloID = reader.GetInt32(reader.GetOrdinal("HenkiloID")),
                Nimi = reader.GetString(reader.GetOrdinal("Nimi")),
                Syntymaaika = reader.GetDateTime(reader.GetOrdinal("Syntymaaika"))
            };

            conn.Close();
            return henkilo;
        }
    }
    
}