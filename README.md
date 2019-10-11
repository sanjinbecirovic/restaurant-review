# Summary

A sample web app that offers the following functionality:

Users are able to (depending on the user role):
    * Regular User: Can login/register, rate and leave a comment for a restaurant
    * Owner: Can login/register, create restaurants and reply comments about owned restaurants
    * Admin: Can lock owners, delete restaurants, comments, and reviews
Reviews have:
    * A 5 star based rate
    * Date of the visit
    * Comment
When a Regular User logs in he sees a Restaurant List ordered by Rate Average
When an Owner logs in he sees a Restaurant List only the ones owned by him, and the reviews pending to reply
Owners can reply the review once
Restaurants detailed view has:
    * The overall average rating
    * The highest rated review
    * The lowest rated review
    * Last reviews with rate, comment, and reply
Restaurant List can be filtered by Rating

The solution consists of:

* A client app implemented as a REACTJS SPA, with JWT token auth, mobx for state management, and ant design as UI library,
* A Net Core API with Swagger UI and API versioning out of the box.
* A data component for accessing the DB with EF Core and CRUD operations over db entities.

You can either use Visual Studio or Visual Studio Code by installing the latest [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core).

# Technology Stack

* [AutoMapper](http://docs.automapper.org/en/stable/)
* [API Versioning](https://github.com/microsoft/aspnet-api-versioning)
* [ASP.NET Core 2.2](https://docs.microsoft.com/en-us/aspnet/core)
* [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
* [Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
* [Entity Framework Core 2.2](https://docs.microsoft.com/en-us/ef/core/)
* [NLog](https://github.com/NLog/NLog/wiki)
* [Swagger](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger)

# Project Structure

* `.vscode` ➡ Configurations for Visual Studio Code.
  * `launch.json` ➡ Launch configurations, which use task definitons, to start the web API project.
  * `tasks.json` ➡ Task definitions for Visual Studio Code.
* `src` ➡ Contains projects for deployment/NuGet library.
  * `RestaurantReview.DataAccess` ➡ EF Core project containing DB model and migrations, and CRUD methods for the db entities.
  * `RestaurantReview.Web` ➡ ASP CORE Web API project with automatic versioning based on convention with Swagger UI.
* `.editorconfig` ➡ Configuration for consistent coding styles across various editors and IDEs. Some will need an addtional plugin, see <https://editorconfig.org/#download>.
* `.gitignore` ➡ Ignore temporary files, build results, and files generated by popular add-ons.
* `GitVersion.yml` ➡ Configuration for automatic versioning based on branching and merging information.
* `RestaurantReview.sln` ➡ SLN containing same structure as repository. All top-level files are added to `Solution Items`.
* `NuGet.Config` ➡ Configuration of NuGet server sources.
* `README.md` ➡ Description of repository/project.

# Logging / NLog

Errors are stored in a seperate log file ending with `.error.log` for easier lookup of issues.
For development purposes the logs will be stored in e.g. `<PATH TO REPO>\src\RestaurantReview.Web\bin\Debug\netcoreapp2.2\logs\`.
Archived files will always be stored in a sub-folder named `archive` with total of 10 files per file logger. A log file is archived when one of the following conditions is met:

* File size of 10MB has been reached.
  * Configuration is defined with parameter `archiveAboveSize` in **KB**
* On application startup, the log file will be rotated regardless of file size.
  * Parameter `archiveOldFileOnStartup` = `true`
  * This setting will always provide a clean/new log file of the current app process.
  * IIS recycle settings need to be updated to disable periodic restart time, reset interval and idle timeout. Otherwise `archiveOldFileOnStartup` should be set to `false`.

# Entity Framework Core

EF Core is available in different versions 2.x, 3.x... which lead to problems when used with different versions of Visual Studio. To have a constent behavior it is recommended to use [EF Core .NET CLI](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet) (dotnet command) instead of the [Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell).
Example of dotnet command: `dotnet ef database update InitialCreate`
Example of Package Manager Console (Visual Studio): `Update-Database -Migration InitialCreate`

## SQL Server Express

Make sure an instance of SQL Server Express is running at the machine, otherwise download and install it.
Use latest [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017#available-languages-ssms-182), english version is recommended.  Connect to `localhost\SQLEXPRESS`, and use Windows Authentication.