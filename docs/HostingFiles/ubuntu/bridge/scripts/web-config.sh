#!/bin/bash
PUBLISH_DIR=bridge/web/publish # publish directory
BLUE_DIR=bridge/web/blue # blue deployment directory
GREEN_DIR=bridge/web/green # green deployment directory

BLUE_NGINX_CONF=/etc/nginx/sites-available/bridge/bridge-web-blue.conf # blue nginx confi
GREEN_NGINX_CONF=/etc/nginx/sites-available/bridge/bridge-web-green.conf # green nginx config
UPSTREAM_NGINX_CONF=/etc/nginx/sites-enabled/bridge-web-upstream.conf # link of blue or green

BLUE_URL=localhost:21000 # blue server url
GREEN_URL=localhost:22000 # green server url
DEPLOY_COLOR_ROUTE=DevOps/Color # route of deployment color

BLUE_SERVICE=bridge-web-blue.service # blue service name
GREEN_SERVICE=bridge-web-green.service # green service name
