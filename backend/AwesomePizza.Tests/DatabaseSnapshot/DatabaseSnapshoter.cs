using AwesomePizzaDAL;

namespace AwesomePizza.Tests.DatabaseSnapshot;
public class DatabaseSnapshoter
{
    public AwesomePizzaContext Database { get; private set; }

    public DatabaseSnapshoter(AwesomePizzaContext database)
    {
        Database = database;
    }
}
