namespace event_api.Models;

public class Tapahtuma
{
    public int TapahtumaID { get; set; }
    public int HenkiloID { get; set; }
    public string Tyyppi { get; set; }
    public DateTime Aika { get; set; }
}

public class Henkilo
{
    public int HenkiloID { get; set; }
    public string Nimi { get; set; }
    public DateTime Syntymaaika { get; set; }
}