Shape is a one page (1 complete, 2nd Incomplete) ASP.NET MVC application demo on .Net Framework 6.
Which Saves the Sign up data into a MSSQL database using Dapper (a wrapper for Ado.Net).
Error logging is kept minimal at this stage and the logs are saved in Windows Event logs or in Console
(for developement purpose).

There are 2 Layout files i.e., _Layout and _PreLoginLayout. As the name suggests all pages prior logging in 
will have a different layout than the after login pages.
Database schema has to be imported from the Database/shape_database.sql file. After which you can remove Database 
folder from the source code.

The after Sign up page is only for the purpose of giving a thumbs up for the success of Registration process, which 
could have been a simple success message on the same page but anyways.

Following Softwares or standards are required to run this code on a machine

Microsoft Visual Studio 2022
Microsoft SQL Server Express 2019
.NET Framework 6
