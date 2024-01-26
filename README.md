Application for generation pdf from html.

Stack: .net 8, ASP.Net, Entity Framework, SQLite, Vue

Was created a Service for working with files. It has File repository for database access and use Hangfire for adding job tasks.
Hangfire run Conversion Job to process file and push pdf to database. After that user can see an updated status for that file and it will be possible to download.
Also at starting tha application it create recurring job with cron expression to process files what can be stop due server off.

Created Dockerfiles for .NET application and for Vue client. And docker-compose.yaml for creating docker containers.






