#!/bin/bash

# CONST OVERRIDE
PUBLISH_DIR= # publish directory
BLUE_DIR= # blue deployment directory
GREEN_DIR= # green deployment directory

BLUE_NGINX_CONF= # blue nginx confi
GREEN_NGINX_CONF= # green nginx config
UPSTREAM_NGINX_CONF= # link of blue or green

BLUE_URL= # blue server url
GREEN_URL= # green server url
DEPLOY_COLOR_ROUTE= # route of deployment color

BLUE_SERVICE= # blue service name
GREEN_SERVICE= # green service name

if ! [[ -f $1 ]]
then
	echo >&2 "> $1 does not exist. Exit with 1."
	exit 1
fi

source $1 # first arg as shell file

# CONST NO OVERRIDE
HOME=/home/ubuntu/

BLUE_COLOR_URL=http://$BLUE_URL/$DEPLOY_COLOR_ROUTE
GREEN_COLOR_URL=http://$GREEN_URL/$DEPLOY_COLOR_ROUTE

HEALTH_CHECK_DELAY=10
SWITCH_DELAY=5

BLUE=blue
GREEN=green

TRUE=true
FALSE=false

# VALIDATE CONST
if [[ $PUBLISH_DIR != $HOME* ]]
then
	PUBLISH_DIR=$HOME$PUBLISH_DIR
fi

if [[ $BLUE_DIR != $HOME* ]]
then
	BLUE_DIR=$HOME$BLUE_DIR
fi

if [[ $GREEN_DIR != $HOME* ]]
then
	GREEN_DIR=$HOME$GREEN_DIR
fi

# VARIABLES
current_color=
current_service=
deploy_color=
deploy_dir=
deploy_url=
deploy_color_url=
deploy_service=
deploy_nginx_conf=


getCurrentColor(){
	echo >&2 ">> curl -s $BLUE_COLOR_URL"
	response="$(curl -s $BLUE_COLOR_URL)"
	echo >&2 ">> response: $response"

	if [[ $response == $BLUE ]]
	then
		current_color=$BLUE
		return
	fi

	echo >&2 ">> curl -s $GREEN_COLOR_URL"
	response="$(curl -s $GREEN_COLOR_URL)"
	echo >&2 ">> response: $response"

	if [[ $response == $GREEN ]]
	then
		current_color=$GREEN
		return
	fi
}

loadDeployEnvironment(){
	getCurrentColor
	
	if [[ $current_color == $GREEN ]]
	then
		current_service=$GREEN_SERVICE
		
		deploy_color=$BLUE
		deploy_dir=$BLUE_DIR
		deploy_url=http://$BLUE_URL
		deploy_color_url=http://$BLUE_URL/$DEPLOY_COLOR_ROUTE
		deploy_service=$BLUE_SERVICE
		deploy_nginx_conf=$BLUE_NGINX_CONF
	else
		current_service=$BLUE_SERVICE

		deploy_color=$GREEN
		deploy_dir=$GREEN_DIR
		deploy_url=http://$GREEN_URL
		deploy_color_url=http://$GREEN_URL/$DEPLOY_COLOR_ROUTE
		deploy_service=$GREEN_SERVICE
		deploy_nginx_conf=$GREEN_NGINX_CONF
	fi

	echo >&2 ">> current_color: $current_color"
	echo >&2 ">> deploy_color: $deploy_color"
	echo >&2 ">> current_service: $current_service"
	echo >&2 ">> publish_dir: $PUBLISH_DIR"
	echo >&2 ">> deploy_dir: $deploy_dir"
	echo >&2 ">> deploy_url: $deploy_url"
	echo >&2 ">> deploy_color_url: $deploy_color_url"
	echo >&2 ">> deploy_service: $deploy_service"
	echo >&2 ">> deploy_nginx_conf: $deploy_nginx_conf"
}

deploy(){
	if ! [[ "$(ls -A $PUBLISH_DIR)" ]]
	then 
		echo >&2 ">> Publish directory is empty"
		echo $FALSE
		return
	fi

	# validate path
	if [[ $deploy_dir != $HOME* ]]
	then 
		echo >&2 ">> $deploy_dir is invalid"
		echo $FALSE
		return
	fi

	if [[ "$(ls -A $deploy_dir)" ]]
	then
		echo >&2 "rm -r $deploy_dir/*"
		rm -r $deploy_dir/*
	fi

	# validate path
	if [[ $PUBLISH_DIR != $HOME* ]]
	then 
		echo >&2 ">> $PUBLISH_DIR is invalid"
		echo $FALSE
		return
	fi

	echo >&2 "mv $PUBLISH_DIR/* $deploy_dir"
	mv $PUBLISH_DIR/* $deploy_dir

	echo >&2 ">> sudo systemctl enable $deploy_service"
	sudo systemctl enable $deploy_service
	echo >&2 ">> sudo systemctl start $deploy_service"
	sudo systemctl start $deploy_service
	echo $TRUE
}

healthCheck(){	
	for retry_count in {1..3}
	do
		echo >&2 ">> curl -s $deploy_color_url"
		response="$(curl -s $deploy_color_url)"
		echo >&2 ">> response: $response"

		if [[ $response == $deploy_color ]]
		then
			echo $TRUE
			return
		else
			echo >&2 ">> $retry_count of 3 fail"
		fi

		if [[ $retry_count -eq 3 ]]
		then
			echo $FALSE
			return
		fi

		sleep 3
	done
}

switch(){
	echo >&2 ">> sudo ln -sf $deploy_nginx_conf $UPSTREAM_NGINX_CONF"
	sudo ln -sf $deploy_nginx_conf $UPSTREAM_NGINX_CONF

	echo >&2 ">> sudo systemctl reload nginx"
	sudo systemctl reload nginx
}

disableOldService(){
	echo >&2 ">> sudo systemctl stop $current_service"
	sudo systemctl stop $current_service
	
	echo >&2 ">> sudo systemctl disable $current_service"
	sudo systemctl disable $current_service
}

delay(){
	delay_second=$1
	for (( second = $delay_second; second > 0; second--))
	do
		echo >&2 ">> $second seconds remain"
		sleep 1
	done
}

loadDeployEnvironment

result="$(deploy)"
if [[ $result != $TRUE ]]
then
	echo >&2 "> Deployment is unsucessful"
	echo >&2 "> Exit with -1"
	exit -1
fi

echo >&2 "> Health check starts in $HEALTH_CHECK_DELAY"
delay $HEALTH_CHECK_DELAY

echo >&2 "> Start health check"
result=$(healthCheck)
if [[ $result != $TRUE ]]
then
	echo >&2 "> Deployed service is unhealthy"
	
	echo >&2 "> Stop $deploy_service"
	sudo systemctl disable --now $deploy_service

	echo >&2 "> Exit with -1"
	exit -1
fi

echo >&2 "> Deployed service is healthy"

echo >&2 "> Switch service in $SWITCH_DELAY"
delay $SWITCH_DELAY

echo >&2 "> Switch service"
switch

echo >&2 "> Disable old service"
disableOldService

echo >&2 "> Completed Successfully"
exit 0

