FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/UpBlazor.Web/UpBlazor.Web.csproj", "UpBlazor.Web/"]
RUN dotnet restore "src/UpBlazor.Web/UpBlazor.Web.csproj"
COPY . .
WORKDIR "/src/UpBlazor.Web"
RUN dotnet build "UpBlazor.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UpBlazor.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UpBlazor.Web.dll"]
