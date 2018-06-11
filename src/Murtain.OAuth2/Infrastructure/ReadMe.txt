add-migration 01 -Context ApplicationDbContext
update-database -Context ApplicationDbContext

add-migration 01 -Context PersistedGrantDbContext
update-database -Context PersistedGrantDbContext


add-migration 01 -Context ConfigurationDbContext
update-database -Context ConfigurationDbContext

