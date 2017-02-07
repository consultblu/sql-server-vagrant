using System;
using System.Data.SqlClient;

class Example
{
    static void Main(string[] args)
    {
        Console.WriteLine("SQL Server Version:");
        Console.WriteLine(SqlExecuteScalar(@"Server=.\SQLEXPRESS; Database=master; Integrated Security=true", "select @@version"));

        Console.WriteLine("SQL Server User Name (integrated Windows authentication credentials) with .NET SqlClient:");
        Console.WriteLine(SqlExecuteScalar(@"Server=.\SQLEXPRESS; Database=master; Integrated Security=true", "select suser_name()"));

        string tcpIpConnectionString = string.Format(
            "Server={0},1433; User ID=alice.doe; Password=HeyH0Password; Database=master",
            Environment.GetEnvironmentVariable("COMPUTERNAME"));

        Console.WriteLine("SQL Server User Name (alice.doe; username/password credentials; TCP/IP connection) with .NET SqlClient:");
        Console.WriteLine(SqlExecuteScalar(tcpIpConnectionString, "select suser_name()"));

        Console.WriteLine("Is this SQL Server connection encrypted? (alice.doe; username/password credentials; Encrypted TCP/IP connection):");
        Console.WriteLine(SqlExecuteScalar(tcpIpConnectionString + "; Encrypt=true", "select encrypt_option from sys.dm_exec_connections where session_id=@@SPID"));
    }

    private static object SqlExecuteScalar(string connectionString, string sql)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return command.ExecuteScalar();
            }
        }
    }
}
