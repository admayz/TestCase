namespace TestCase.Models.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }

    public interface IDbMigrator
    {
        void Migrate();
    }
}
