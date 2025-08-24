dotnet ef migrations add UpdateEntitiesNames --startup-project ../BaseProject.API --project ../BaseProject.Infrastructure

dotnet ef database update

dotnet ef migrations remove --project ../BaseProject.Infrastructure --startup-project ../BaseProject.API