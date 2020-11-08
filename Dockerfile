FROM mcr.microsoft.com/dotnet/core/sdk:3.1
COPY . /code
WORKDIR /code
RUN dotnet build --configuration Release
ENTRYPOINT ["dotnet", "run", "--project", "ItHappend.RestAPI"]