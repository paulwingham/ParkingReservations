using System.Data.SQLite;

namespace Paul.ParkingReservations.Infrastructure.SqlLite;

public interface ISqliteConnectionProvider
{
    SQLiteConnection GetSqliteConnection();

    void Dispose();
}