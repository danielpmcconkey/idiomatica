# Idiomatica

## Getting your build environment up and running

1. Install (or confirm an existing installation of) VisualStudio with .NET core 8 
    1. This solution requires a number of packages to run. I'm not sure if they'll install auto-magically or not. We can figure that out together
2. Install (or confirm an existing installation of) a local MS SQL Server for dev and test purposes.
    1. I'm currently using this version Microsoft SQL Server 2022 (RTM-GDR) (KB5040936) - 16.0.1121.4 (X64)   Jul  2 2024 00:22:34 
    2. This can be found by querying `select @@VERSION;` in SQL Server Management Studio
    3. The version shouldn't really matter for development or testing. We're not doing anything exotic here. More or less base TSQL everywhere
3. Create a dev database
    1. Open the file `./Idiomatica/Queries/__FreshDbInstall.sql` using SQL Server Management Studio
    2. Execute the script
        1. This will create your dev database
        2. You should also see some warnings relating to the max cluster length for some of the AspNetUser* tables. These table create scripts were originally created by Microsoft so I haven't tried "fixing" them. It's not been a problem yet
    3. Confirm no actual errors. If you do get errors, attempt to troubleshoot or ask for help
    4. Verify that you see a bunch of tables (7 at time of writing) that are named `dbo.*` and many tables (25 at time of writing) that are named `Idioma.*`
4. Your default user name is `testDev@testDev.com` and your default PW is `lmno12#45P`
5. At this point, you can run the app. 
    1. Open the solution in Visual Studio
    2. Try to build the entire solution. If it doesn't, there's likely a missing package
    3. If it does, go ahead and play it (F5)
    4. If nothing happens, confirm that `IdiomaticaWeb` is your start-up project and try again
    5. You should be able to log-in, check your book list, and start reading the default book, Rapunzel
    6. Feel free to create your own user log-in if you'd like or use the initial test user
6. Create the test database
    1. Go back to the database generation script from above (`./Idiomatica/Queries/__FreshDbInstall.sql`)
    2. Do a find + reploce on the eniter file.
        1. Find `Idiomatica_dev`
        2. Replace with `Idiomatica_test`
    3. Don't save the file or you'll accidentally commit the change you just made
7. Run the automated tests
    1. Open up the solution in VisualStudio
    2. Open up the Test Explorer and run all of the "LogicTests"
    3. Confirm everything passes
    4. You should do this step everytime you plan to merge one of your commits into main.

## Learning the application structure

### Projects in the solution

This application is built on an N-Tier pattern, which segregates the application responsibilities into separate tiers, from core data at the bottom, to user interface at the top. That's the order we'll go in...

1. *Model* 
    1. This is the core data layer. All data types used in the application are defined here, but there should be very little business logic in this layer
    2. DAL folder. DAL stands for Data Access Layer. This is the interface to the data defined in the model classes.
        1. IdiomaticaContext.cs this is the Entity Framework definition for how model objects relate to one another. All data interaction in this applicaiton does so through an instantiation of the IdiomaticaContext class
        2. DataCache_\* files. The DataCache is one gigantic class spread out across multiple files. This is where the create/read/update/delete methods for access data are housed. Generally, these functions should only be accessed by the API classes. They're also not well named :).
    3. Available\* files. These are all enums, defining look-up data. They generally correspond to an int field in the database
2. *Logic*
    1. Herein resides all of the business logic for the application. These are classes and methods that the front-end UI relies upon to consistently and reliably interact with data
    2. 