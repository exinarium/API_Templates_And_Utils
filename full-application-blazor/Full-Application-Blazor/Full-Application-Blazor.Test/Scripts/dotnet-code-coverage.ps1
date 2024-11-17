cd ../
Remove-Item -Recurse -Force ./TestResults/*
dotnet test --collect:"XPlat Code Coverage" --results-directory "./TestResults"
cd .\TestResults
Get-ChildItem -Recurse -Directory | ForEach-Object { reportgenerator -reports:"$($_.FullName)\coverage.cobertura.xml" -targetdir:. }
&"C:\Program Files\Mozilla Firefox\firefox.exe" @("${pwd}\index.html")
Read-Host -Prompt "Press Enter to exit"
