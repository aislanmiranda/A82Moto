﻿# Etapa 1: Imagem de Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Definir diretório de trabalho
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]

# Restaurar dependências
RUN dotnet restore "Api/Api.csproj"

# Copiar todo o código-fonte para a imagem de build
COPY . .

# Publicar a aplicação para uma pasta /app/release
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/release

# Etapa 2: Imagem de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Definir diretório de trabalho na imagem de runtime
WORKDIR /app
RUN mkdir -p /app/Uploads/
RUN chmod -R 755 /app/Uploads/

# Copiar os binários da aplicação da imagem de build
COPY --from=build /app/release .

# Expor a porta da aplicação
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]