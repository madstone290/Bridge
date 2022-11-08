#!/bin/bash
PUBLISH_DIR=bridge/api/publish # publish directory
BLUE_DIR=bridge/api/blue # blue deployment directory
GREEN_DIR=bridge/api/green # green deployment directory

BLUE_NGINX_CONF=/etc/nginx/sites-available/bridge/bridge-api-blue.conf # blue nginx confi
GREEN_NGINX_CONF=/etc/nginx/sites-available/bridge/bridge-api-green.conf # green nginx config
UPSTREAM_NGINX_CONF=/etc/nginx/sites-enabled/bridge-api-upstream.conf # link of blue or green

BLUE_URL=localhost:21100 # blue server url
GREEN_URL=localhost:22100 # green server url
DEPLOY_COLOR_ROUTE=DevOps/Color # route of deployment color

BLUE_SERVICE=bridge-api-blue.service # blue service name
GREEN_SERVICE=bridge-api-green.service # green service name
