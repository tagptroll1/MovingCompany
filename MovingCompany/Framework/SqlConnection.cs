using Npgsql;

namespace MovingCompany.Framework;

/// <summary>
/// Used to abstract the sql adapter
/// </summary>
/// 
public static class SqlConnectionFactory
{
	public static NpgsqlConnection GetConnection(string cs)
	{
        var connection = new NpgsqlConnection(cs);

        return connection;
	}
}
