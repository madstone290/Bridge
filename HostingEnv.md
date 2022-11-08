# 호스팅 환경

## Bridge.Api 개요
| 이름 | 내용| 비고|
|---|---|---|
| blue port| 21100 | |
| green port|22100 | |
| color endpoint | devops/color  | |
| 서버명(도메인)| placechart-api.duckdns.org | |
| nginx 로그 디렉토리 | /var/log/nginx/bridge-api | |
| blue system 서비스 | bridge-api-blue.service | |
| green system 서비스 | bridge-api-green.service | |


## Bridge.Web 개요
| 이름 | 내용| 비고|
|---|---|---|
| blue port| 21000 | |
| green port|22000 | |
| color endpoint | devops/color | |
| 서버명(도메인)| placechart.duckdns.org, placechart.com | |
| nginx 로그 디렉토리 | /var/log/nginx/bridge | |
| blue system 서비스 | bridge-web-blue.service | |
| green system 서비스 | bridge-web-green.service | |

