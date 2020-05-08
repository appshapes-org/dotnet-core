namespace AppShapes.Core.Testing.Infrastructure
{
    public class MockMigrator : NullMigrator
    {
        public override void Migrate(string targetMigration = null)
        {
            ++MigrateCalled;
            base.Migrate(targetMigration);
        }

        public int MigrateCalled { get; private set; }
    }
}