# Event-api
Simple API demo, made with .NET, for adding new events for persons.

Requires .NET and PostgreSQL.

Log in to psql with `sudo -u postgres psql postgres`, update the password of user "postgres" with `ALTER USER postgres WITH PASSWORD 'new_password';`, and initialize the DB with the following tables:
```
CREATE TABLE Henkilo (
    HenkiloID SERIAL PRIMARY KEY,                 
    Nimi VARCHAR NOT NULL,
    Syntymaaika TIMESTAMP NOT NULL
);
```
```
CREATE TABLE Tapahtumat (
    TapahtumaID SERIAL PRIMARY KEY,
    HenkiloID INT NOT NULL,
    Tyyppi VARCHAR NOT NULL,
    Aika TIMESTAMP NOT NULL,
    CONSTRAINT fk_henkilo
        FOREIGN KEY (HenkiloID) REFERENCES Henkilo(HenkiloID)
);
```
And insert a few persons:
```
INSERT INTO Henkilo (Nimi, Syntymaaika) VALUES ('Teemu Teekkari', '1996-01-12');
```
```
INSERT INTO Henkilo (Nimi, Syntymaaika) VALUES ('Kalle Kylteri', '2000-06-11');
```

Update the `connectionString` in `Services/EventService.cs` and `Services/PersonService.cs` accordingly.

Run the project with `dotnet run`.

Some example HTTP requests can be found in Rest-folder