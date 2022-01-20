FROM mcr.microsoft.com/dotnet/sdk:2.1
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]

ENV ASPNETCORE_ENVIRONMENT=Docker

RUN dotnet build
EXPOSE 80/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh