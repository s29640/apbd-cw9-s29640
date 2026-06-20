# apbd-cw9-s29640
APBD Ćwiczenie 9

## inicjacja projektu

1. Utwórz repo na github https://github.com/s29640/apbd-cw9-s29640.git
2. Sklonuj repo
```bash
git clone https://github.com/s29640/apbd-cw9-s29640.git
cd apbpd-cw9-s29640
```
3. Utwórz projekt
```bash
dotnet new webapi --use-controllers -n UniversityTasksDbFirstApi
cd UniversityTasksDbFirstApi
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool update --global dotnet-ef
```
4. Utwórz rozwiązanie
```bash
cd ..
dotnet new sln -n APBD-cw9
dotnet sln add UniversityTasksDbFirstApi\UniversityTasksDbFirstApi.csproj
```
5. Utwórz bazę SqlServer
Uruchom sktrypt .database/zadanie_1_db_first_university_tasks_setup.sql
SQL server:localhost,1433; 
6. Zmodyfikuj appsettings.json
Dodaj:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=ApbdLecture9DbFirstTask;Trusted_Connection=True;TrustServerCertificate=True"
}
7. Wykonaj scafold
```bash
cd UniversityTasksDbFirstApi
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=ApbdLecture9DbFirstTask;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --context UniversityTasksDbContext --context-dir Data --output-dir Models --no-onconfiguring
```