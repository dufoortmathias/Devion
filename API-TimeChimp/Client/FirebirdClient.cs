namespace Api.Devion.Client;

public class FirebirdClientETS
{
    private readonly string _connectionString;

    public FirebirdClientETS(String server, String user, String password, String database)
    {
        //create the connectionstring
        _connectionString = $"Server={server};User={user};Password={password};Database={database}";
    }

    //select query
    public string selectQuery(string query)
    {
        //create connection
        FbConnection connection = new FbConnection(_connectionString);

        //create list for the values
        List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
        using (var command = new FbCommand(query, connection))
        {
            //open the connection
            connection.Open();

            //execute the query
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //add the values to the list
                    Dictionary<string, object> value = new Dictionary<string, object>();
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
    }

    //insert query
    public void insertQuery(string query)
    {
        //create connection
        FbConnection connection = new FbConnection(_connectionString);
        using (var command = new FbCommand(query, connection))
        {
            //open the connection
            connection.Open();

            //execute the insert query
            command.ExecuteNonQuery();

            //close the connection
            connection.Close();
        }
    }

    //update query
    public void updateQuery(string query)
    {
        //create connection
        FbConnection connection = new FbConnection(_connectionString);
        using (var command = new FbCommand(query, connection))
        {
            //open the connection
            connection.Open();

            //execute the insert query
            command.ExecuteNonQuery();

            //close the connection
            connection.Close();
        }
    }

}