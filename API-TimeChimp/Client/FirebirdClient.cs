namespace Api.Devion.Client;

public class FirebirdClientETS
{
    private readonly string _user = "WOUTER";
    private readonly string _password = "WOUTER";
    private readonly string _database = @"d:\ets\Dossier\MET\Winfact.fdb";
    private readonly string _server = "DEVIONSERVERETS";
    private readonly string _user_Devion = "STIJN";
    private readonly string _password_Devion = "STIJN";
    private readonly string _database_Devion = @"d:\ets\Dossier\DEV\Winfact.fdb";

    private string _connectionString;
    private string _connectionString_Devion;

    public FirebirdClientETS()
    {
        _connectionString = $"Server={_server};User={_user};Password={_password};Database={_database}";
        _connectionString_Devion = $"Server={_server};User={_user_Devion};Password={_password_Devion};Database={_database_Devion}";
    }

    public string selectQuery(string query)
    {
        FbConnection connection = new FbConnection(_connectionString_Devion);
        List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
        using (var command = new FbCommand(query, connection))
        {
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Dictionary<string, object> value = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        value.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    values.Add(value);
                }
            }
            connection.Close();
            string json = JsonTool.ConvertFrom(values);
            return json;

        }
    }

    public string insertQuery(string query)
    {
        FbConnection connection = new FbConnection(_connectionString_Devion);
        using (var command = new FbCommand(query, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "success";
        }
    }

    public string updateQuery(string query)
    {
        FbConnection connection = new FbConnection(_connectionString_Devion);
        using (var command = new FbCommand(query, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "success";
        }
    }

}