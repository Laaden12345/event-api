using event_api.Models;
using Npgsql;

namespace event_api.Services;

public static class EventService
{
    static List<Tapahtuma> Tapahtumat { get; }
    public static string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

    static EventService()
    {
        Tapahtumat = new List<Tapahtuma>{};
    }

    // Hakee tapahtuman id:n perusteella
    public static Tapahtuma Get(int id)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Tapahtumat WHERE TapahtumaID = @id;", conn);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return null;
            }
            reader.Read();
            // Ei jostain syystä onnistunut GetFieldValue<Tapahtuma>:n käyttö
            Tapahtuma tapahtuma = new Tapahtuma
            {
                TapahtumaID = reader.GetInt32(reader.GetOrdinal("TapahtumaID")),
                HenkiloID = reader.GetInt32(reader.GetOrdinal("HenkiloID")),
                Tyyppi = reader.GetString(reader.GetOrdinal("Tyyppi")),
                Aika = reader.GetDateTime(reader.GetOrdinal("Aika"))
            };

            conn.Close();
            return tapahtuma;
        }
    }

    // Hakee listan tapahtumista tyyppi- ja aikasuodattimien perusteella.
    public static List<Tapahtuma> Get(string type = null, DateTime? date = null)
    {
        Tapahtumat.Clear();
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command;
            // Tälle if-else-hässäkälle löytyisi varmaan joku simppelimpikin ratkaisu
            if (type == null && date == null)
            {
                command = new NpgsqlCommand("SELECT * FROM Tapahtumat;", conn);
            } else if (type == null)
            {
                string dateString = date.Value.ToString("yyyy-MM-dd") + "%";
                command = new NpgsqlCommand("SELECT * FROM Tapahtumat WHERE to_char(Aika, 'YYYY-MM-DD') like @date;", conn);
                command.Parameters.AddWithValue("@date", dateString);

            } else if (date == null)
            {
                command = new NpgsqlCommand("SELECT * FROM Tapahtumat WHERE Tyyppi ilike @type;", conn);
                command.Parameters.AddWithValue("@type", type);
            } else
            {
                command = new NpgsqlCommand("SELECT * FROM Tapahtumat WHERE Tyyppi ilike @type AND to_char(Aika, 'YYYY-MM-DD') like @date;", conn);
                string dateString = date.Value.ToString("yyyy-MM-dd") + "%";
                command.Parameters.AddWithValue("@date", dateString);
                command.Parameters.AddWithValue("@type", type);
            }
            
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Tapahtuma tapahtuma = new Tapahtuma{
                    TapahtumaID = reader.GetInt32(reader.GetOrdinal("TapahtumaID")),
                    HenkiloID = reader.GetInt32(reader.GetOrdinal("HenkiloID")),
                    Tyyppi = reader.GetString(reader.GetOrdinal("Tyyppi")),
                    Aika = reader.GetDateTime(reader.GetOrdinal("Aika"))
                };
                Tapahtumat.Add(tapahtuma);
            }
            conn.Close();
            return Tapahtumat;
        }
    }

    // Hakee listan tapahtumista henkilön id:llä
    public static List<Tapahtuma> GetByPerson(int personId)
    {
        Tapahtumat.Clear();
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Tapahtumat WHERE HenkiloID = @id;", conn);
            command.Parameters.AddWithValue("@id", personId);
            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return null;
            }
            while (reader.Read())
            {
                // Ei jostain syystä onnistunut GetFieldValue<Tapahtuma>:n käyttö
                Tapahtuma tapahtuma = new Tapahtuma
                {
                    TapahtumaID = reader.GetInt32(reader.GetOrdinal("TapahtumaID")),
                    HenkiloID = reader.GetInt32(reader.GetOrdinal("HenkiloID")),
                    Tyyppi = reader.GetString(reader.GetOrdinal("Tyyppi")),
                    Aika = reader.GetDateTime(reader.GetOrdinal("Aika"))
                };
                Tapahtumat.Add(tapahtuma);
            }
            conn.Close();
            return Tapahtumat;
        }
    }

    // Lisää uuden parametreissä annetun tapahtuman 
    public static void AddEvent(Tapahtuma tapahtuma)
    {
        if (tapahtuma is null)
        {
            return;
        }

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO Tapahtumat (HenkiloID, Tyyppi, Aika) VALUES (@henkiloID, @tyyppi, @aika);", conn);
            command.Parameters.AddWithValue("@henkiloID", tapahtuma.HenkiloID);
            command.Parameters.AddWithValue("@tyyppi", tapahtuma.Tyyppi);
            command.Parameters.AddWithValue("@aika", tapahtuma.Aika);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

    // Poistaa tapahtuman id:n perusteella
    public static void DeleteEvent(int id)
    {
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Tapahtumat WHERE TapahtumaID = @id;", conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
    
}