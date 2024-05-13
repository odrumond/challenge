build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project call-center-events run
start:
	dotnet run --project call-center-events
run-postgres-migration:
	dotnet ef database update --project Data/Data --startup-project call-center-events --context PostgresCallCenterDbContext