dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o /scripts
rm /scripts/*.pdb