namespace BookStore.InventoryService.Data;

public class InventoryDatabase
{
    public IMongoDatabase Database { get; set; }
    public InventoryDatabase(string connectionString, string database)
    {
        var connection = new MongoClient(connectionString);
        Database = connection.GetDatabase(database);
    }

    public IMongoCollection<Category> Categories =>
        Database.GetCollection<Category>("Categories");

    public IMongoCollection<Book> Books =>
        Database.GetCollection<Book>("Books");
}