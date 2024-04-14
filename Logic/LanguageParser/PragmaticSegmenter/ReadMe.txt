Code in this directory is copied outright from https://github.com/UglyToad/PragmaticSegmenterNet/tree/master/PragmaticSegmenterNet
But the newget package is not in .net core so I am putting it here

todo: confirm that none of the static classes in PragmaticSegmenter persist 
data that would be broken when hosted in Azure due to the way blazor hosts 
multiple app sessions in the same process 
(https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0)