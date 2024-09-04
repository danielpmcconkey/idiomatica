# Idiomatica

## Instructions for new developers

1. Install VisualStudio with .NET core 8 
    1. This solution requires a number of packages to run. I'm not sure if they'll install auto-magically or not. We can figure that out together
2. Install (or confirm an existing installation of) a local MS SQL Server for dev and test purposes.
    1. I'm currently using this version Microsoft SQL Server 2022 (RTM-GDR) (KB5040936) - 16.0.1121.4 (X64)   Jul  2 2024 00:22:34 
    2. This can be found by querying `select @@VERSION;` in SQL Server Management Studio
    3. The version shouldn't really matter for development or testing. We're not doing anything exotic here. More or less base TSQL everywhere
4. Create a dev database
    1. Open the file ./Idiomatica/Queries/__FreshDbInstall.sql using SQL Server Management Studio
    2. Execute the script
        1. This will create your dev database
        2. You should also see some warnings relating to the max cluster length for some of the AspNetUser* tables. These table create scripts were originally created by Microsoft so I haven't tried "fixing" them. It's not been a problem yet
    3. Confirm no actual errors. If you do get errors, attempt to troubleshoot or ask for help
    4. Verify that you see a bunch of tables (7 at time of writing) that are named `dbo.*` and many tables (25 at time of writing) that are named `Idioma.*`
5. Your default user name is `testDev@testDev.com` and your default PW is `lmno12#45P`
6. At this point, you can run the app. 
    1. Open the solution in Visual Studio
    2. Try to build the entire solution. If it doesn't, there's likely a missing package
    3. If it does, go ahead and play it (F5)
    4. If nothing happens, confirm that `IdiomaticaWeb` is your start-up project and try again
    5. You should be able to log-in, check your book list, and start reading the default book, Rapunzel

