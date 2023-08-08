namespace Api.Devion.Client;

public class FirebirdClientETS
{
    private readonly string _connectionString;

    public FirebirdClientETS(String server, String user, String password, String database)
    {
        _connectionString = $"Server={server};User={user};Password={password};Database={database}";
    }

    public string selectQuery(string query)
    {
        FbConnection connection = new FbConnection(_connectionString);
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
        FbConnection connection = new FbConnection(_connectionString);
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
        FbConnection connection = new FbConnection(_connectionString);
        using (var command = new FbCommand(query, connection))
        {
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "success";
        }
    }

}