using MySql.Data.MySqlClient;

public class DatabaseManager
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    public DatabaseManager()
    {
        // Conexión BBDD
        server = "34.175.85.243";
        database = "db_IPO";
        uid = "dbIPO";
        password = "XSbB&(zv}`gUm)o9";
        string connectionString;
        connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    public MySqlConnection Connection
    {
        get { return connection; }
    }

}