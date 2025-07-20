using System.Data.SQLite;

namespace Paul.ParkingReservations.Infrastructure.SqlLite;

public sealed class SqliteConnectionProvider : ISqliteConnectionProvider, IDisposable
{
    private readonly SQLiteConnection _sqliteConnection;
    private readonly SQLiteTransaction _sqLiteTransaction;

    public SqliteConnectionProvider(string connectionString)
    {
        _sqliteConnection = new SQLiteConnection(connectionString);
        _sqliteConnection.Open();
        _sqLiteTransaction = _sqliteConnection.BeginTransaction();
    }

    public SQLiteConnection GetSqliteConnection()
    {
        return _sqliteConnection;
    }

    public void Dispose()
    {
        _sqLiteTransaction.Commit();
        _sqliteConnection.Close();
    }
}