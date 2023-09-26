FROM hub.nexdev.net/base/dotnet/core/sdk:7.0 AS builder
WORKDIR /src

COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Business ./Fanex.CompanyMaxPayout.Business
COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service.csproj ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.CompanyMaxPayout.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/rebuild-baseimage/dotnetcore/aspnet:7.0-nap-mail-core



# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

ENV TZ="America/La_Paz" 

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.CompanyMaxPayout.Service.dll"]
