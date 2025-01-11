# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar la solución y restaurar las dependencias
COPY autoagenda-back.sln .
COPY autoagenda-back/autoagenda-back.csproj ./autoagenda-back/
RUN dotnet restore autoagenda-back.sln

# Copiar el resto de los archivos y compilar
COPY . .
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar la aplicación compilada
COPY --from=build /app/out .

# Configurar ASP.NET Core para escuchar en el puerto 80
ENV ASPNETCORE_URLS=http://+:80

# Exponer el puerto 80
EXPOSE 80

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "autoagenda-back.dll"]
