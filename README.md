use https://dillinger.io/ if you need a markdown viewer

# Idiomatica

## Getting your build environment up and running

1. Pre-requisites
    1. Install (or confirm an existing installation of) VisualStudio with .NET core 8 
        1. This solution requires a number of packages to run. I'm not sure if they'll install auto-magically or not. We can figure that out together
    1. Install (or confirm an existing installation of) a local MS SQL Server for dev and test purposes.
        1. I'm currently using this version Microsoft SQL Server 2022 (RTM-GDR) (KB5040936) - 16.0.1121.4 (X64)   Jul  2 2024 00:22:34 
        1. This can be found by querying `select @@VERSION;` in SQL Server Management Studio
        1. The version shouldn't really matter for development or testing. We're not doing anything exotic here. More or less base TSQL everywhere
    1. Open up the solution in VisualStudio and rebuild the entire solution
        1. If there are any errors, stop here and either troubleshoot yourself or ask for help
1. Create the dev database
    1. Set your start-up project in VisualStudio to IdiomaticaWeb
    1. Check your database connection strings
        1. Open the Program.cs file in the IdiomaticaWeb project
        1. Under the section of code that begins `if (builder.Environment.IsDevelopment())`...
        1. Confirm that you have uncommented the line that looks for the `AZURE_SQL_CONNECTIONSTRING_DEV` config value
        1. Confirm that you have commented out the line that retrieves the `AZURE_SQL_CONNECTIONSTRING_TEST` config
        1. Confirm that you have commented out the line at the end of that block of code just under the comment that reads "use the below connection to run against prod DB..."
        1. Confirm that you are in Debug mode in Visual Studio
    1. Build the database
        1. Using the Package Manager Console...
        1. Set the "Default project:" pulldown (in the console window) to "TestDataPopulator"
        1. Type `Update-Database` into the PM command prompt and hit enter
        1. You should see not output with red text
1. Create the testt database
    1. In your Program.cs file, uncomment the line that retrieves the test database connection string
    1. Again, run the `Update-Database` command in the Package Manager console
    1. Re-comment the test connection string line and save the file
1. Go over to SQL Server Management Studio (SSMS) and confirm that you have 2 shiny new database
1. Populate dev and test data
    1. Change your VisualStudio start-up project to the TestDataPopulator
    1. Make sure you are in Debug mode
    1. Press the "play" button (or hit F5)
    1. If you see the message that a fatal error has occurred, troubleshoot or ask for help
    1. Change your VisualStudio start-up project to the IdiomaticaWeb project.
        1. If you forget this step, you're gonna have to delete your data and start over
1. Run the automated tests
    1. In VisualStudio, open the Test Explorer
    2. Run all of the "LogicTests"
    1. Confirm everything passes
    1. You should do this step everytime you plan to merge one of your commits into main.
1. Run the app
    1. Ensure that the start-up project is set to IdiomaticaWeb
    1. Press "play" (F5)
    1. The test user you just created is `testDev@testDev.com` and its default PW is `lmno12#45P`
    1. You should be able to create your own user, if you'd like (though the UI is a bit jank, still)



## Learning the application

This is a TBD section of this document. Some of the design of this applicaiton is documented in in the IdiomaticaArchimate.archimate file in the top directory of the project. You need to install Archi to view it.