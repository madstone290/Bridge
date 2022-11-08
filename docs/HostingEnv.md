# 호스팅 환경

## Bridge.Api 개요
| 이름 | 내용| 비고|
|---|---|---|
| blue port| 21100 | |
| green port|22100 | |
| color endpoint | devops/color  | |
| 서버명(도메인)| placechart-api.duckdns.org | |
| nginx 로그 디렉토리 | /var/log/nginx/bridge-api | |
| blue system 서비스 | bridge-api-blue.service | /etc/systemd/system/bridge-api-blue.service |
| green system 서비스 | bridge-api-green.service | /etc/systemd/system/bridge-api-green.service |


## Bridge.Web 개요
| 이름 | 내용| 비고|
|---|---|---|
| blue port| 21000 | |
| green port|22000 | |
| color endpoint | devops/color | |
| 서버명(도메인)| placechart.duckdns.org, placechart.com | |
| nginx 로그 디렉토리 | /var/log/nginx/bridge | |



## /etc/systemd/system/bridge-api-blue.service
```bash
[Unit]
Description=Bridge api

[Service]
WorkingDirectory=/home/ubuntu/bridge/api/blue
ExecStart=/usr/bin/dotnet Api.dll Color=blue
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=bridge api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:21100
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

## /etc/systemd/system/bridge-api-green.service
```bash
[Unit]
Description=Bridge api

[Service]
WorkingDirectory=/home/ubuntu/bridge/api/green
ExecStart=/usr/bin/dotnet Api.dll Color=green
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=bridge api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:22100
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

## /etc/systemd/system/bridge-api-green.service
```
[Unit]
Description=Bridge web

[Service]
WorkingDirectory=/home/ubuntu/bridge/web/green
ExecStart=/usr/bin/dotnet WebApp.dll Color=green
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=Bridge web
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:22000
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target

```