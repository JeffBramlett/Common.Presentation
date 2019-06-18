cd C:\Users\jeff_\Source\Repos\JeffBramlett\Common.Presentation\Common.Presentation\Common.Presentation
call nuget pack Common.Presentation.csproj -properties owners=jeffrey_bramlett -NoDefaultExcludes
cd bin\release
call nuget push Common.Presentation.1.0.2.nupkg oy2np2jza35zkbvjb6iar2eaintlylgele6xcgium2ge3i -source https://api.nuget.org/v3/index.json