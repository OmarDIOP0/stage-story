var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.StageStory>("stagestory");

builder.Build().Run();
