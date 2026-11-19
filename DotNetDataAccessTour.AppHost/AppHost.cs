var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithDataVolume();

var database = sql.AddDatabase("DefaultConnection", "DotNetDataAccessTour");

var seq = builder.AddSeq("seq", port: 5342)
    .WithEndpoint(targetPort: 80, port: 5342, scheme: "http", name: "http", isProxied: false)
    .WithEndpoint(targetPort: 5341, port: 5341, scheme: "http", name: "ingestion", isProxied: false)
    .WithDataVolume();

var webDataDemo = builder.AddProject<Projects.WebDataDemo>("webdatademo")
    .WithReference(database)
    .WithReference(seq)
    .WithEnvironment("SEQ_INGESTION_URI", "http://localhost:5341")
    .WaitFor(database)
    .WaitFor(seq);

var webDataDemoMigrations = webDataDemo
    .AddEFMigrations("webdatademo-migrations", "WebDataDemo.Data.AppDbContext")
    .RunDatabaseUpdateOnStart();

webDataDemo.WaitForCompletion(webDataDemoMigrations);

builder.Build().Run();
