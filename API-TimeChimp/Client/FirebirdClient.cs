namespace Api.Devion.Client;

public class FirebirdClientETS
{
    private readonly string _connectionString;

    public FirebirdClientETS(string server, string user, string password, string database)
    {
        //create the connectionstring
        _connectionString = $"Server={server};User={user};Password={password};Database={database}";
    }

    //select query
    public string selectQuery(string query, Dictionary<string, object> parameters)
    {
        //create connection
        FbConnection connection = new(_connectionString);

        //create list for the values
        List<Dictionary<string, object>> values = new();
        using FbCommand command = new(query, connection);

        foreach (KeyValuePair<string, object> record in parameters)
        {
            command.Parameters.AddWithValue(record.Key, record.Value);
        }

        //open the connection
        connection.Open();

        //execute the query
        using (FbDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                //add the values to the list
                Dictionary<string, object> value = new();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    value.Add(reader.GetName(i), reader.GetValue(i));
                }
                values.Add(value);
            }
        }

        //close the connection
        connection.Close();

        //convert the list to json
        string json = JsonTool.ConvertFrom(values);
        return json;
    }

    //select query without input files
    public string selectQuery(string query)
    {
        return selectQuery(query, new());
    }

    //execute query
    public void ExecuteQuery(string query, Dictionary<string, object?> parameters)
    {
        //create connection
        FbConnection connection = new(_connectionString);
        using FbCommand command = new(query, connection);

        foreach (KeyValuePair<string, object?> record in parameters)
        {
            command.Parameters.AddWithValue(record.Key, record.Value);
        }

        //open the connection
        connection.Open();

        //execute the query
        command.ExecuteNonQuery();

        //close the connection
        connection.Close();
    }
}