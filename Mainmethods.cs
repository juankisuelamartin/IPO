using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

public class MainMethods
{
    private readonly DatabaseManager dbManager;

    public MainMethods()
    {
        dbManager = new DatabaseManager();
    }


}
