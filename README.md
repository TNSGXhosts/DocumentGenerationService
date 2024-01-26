Application for generation pdf from html.

Stack: .net 8, ASP.Net, Entity Framework, SQLite, Vue

Was created a Service for working with files. It has File repository for database access and use Hangfire for adding job tasks.
Hangfire run Conversion Job to process file and push pdf to database. After that user can see an updated status for that file and it will be possible to download.
Also, when the application starts, a recurring job is created with a cron statement to process files that may have been left unprocessed due to the server being down.

Created Dockerfiles for .NET application and for Vue client. And docker-compose.yaml for creating docker containers.






