FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Business ./Fanex.CompanyMaxPayout.Business
COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service.csproj ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service.csproj

WORKDIR /src/Fanex.CompanyMaxPayout.Service

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n -r linux-x64

COPY ./Fanex.CompanyMaxPayout.Service/Fanex.CompanyMaxPayout.Service .

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-nap-mail-core

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.CompanyMaxPayout.Service.dll"]
