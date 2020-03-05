FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/YunDa.Web/YunDa.ISAS.Web.Host/YunDa.ISAS.Web.Host.csproj", "src/YunDa.Web/YunDa.ISAS.Web.Host/"]
RUN dotnet restore "src/YunDa.Web/YunDa.ISAS.Web.Host/YunDa.ISAS.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/YunDa.Web/YunDa.ISAS.Web.Host"
RUN dotnet build "YunDa.ISAS.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YunDa.ISAS.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YunDa.ISAS.Web.Host.dll"]