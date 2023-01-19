
#!/bin/bash

# CONST NO OVERRIDE
DOCKER_IMAGE=madstone290/bridge-api:latest
DOCKER_NETWORK=nginx_group
COLOR_ENDPOINT=devops/color
BLUE_IP=172.20.0.11
GREEN_IP=172.20.0.12
NGINX_CONTAINER=nginx
BLUE_CONTAINER=bridge-api-blue
GREEN_CONTAINER=bridge-api-green
BLUE_URL=http://$BLUE_IP/$COLOR_ENDPOINT
GREEN_URL=http://$GREEN_IP/$COLOR_ENDPOINT
WORK_DIR=/home/ubuntu/docker/bridge/
BLUE_CONFIG=${WORK_DIR}bridge-api-blue.conf
GREEN_CONFIG=${WORK_DIR}bridge-api-green.conf

HEALTH_CHECK_DELAY=10
SWITCH_DELAY=5

BLUE=blue
GREEN=green

TRUE=true
FALSE=false

# VARIABLES
deploy_color=$1

getDeployColor(){
	echo >&2 ">> curl -s $BLUE_URL"
	response="$(curl -s $BLUE_URL)"
	echo >&2 ">> response: $response"

	if [[ $response == $BLUE ]]
	then
		deploy_color=$GREEN
		return
	fi

	echo >&2 ">> curl -s $GREEN_URL"
	response="$(curl -s $GREEN_URL)"
	echo >&2 ">> response: $response"

	if [[ $response == $GREEN ]]
	then
		deploy_color=$BLUE
		return
	fi
}

deployBlue(){
	echo >&2 ">> docker pull $DOCKER_IMAGE"
	docker pull $DOCKER_IMAGE
	echo >&2 ">> docker run -d --name $BLUE_CONTAINER --network $DOCKER_NETWORK --ip $BLUE_IP $DOCKER_IMAGE COLOR=$BLUE"
	docker run -d --name $BLUE_CONTAINER --network $DOCKER_NETWORK --ip $BLUE_IP $DOCKER_IMAGE COLOR=$BLUE
}

deployGreen(){
	echo >&2 ">> docker pull $DOCKER_IMAGE"
	docker pull $DOCKER_IMAGE
	echo >&2 ">> docker run -d --name $GREEN_CONTAINER --network $DOCKER_NETWORK --ip $GREEN_IP $DOCKER_IMAGE COLOR=$GREEN"
	docker run -d --name $GREEN_CONTAINER --network $DOCKER_NETWORK --ip $GREEN_IP $DOCKER_IMAGE COLOR=$GREEN
}


deploy(){
	getDeployColor

	if [[ $deploy_color == $BLUE ]]
	then
		deployBlue
		return
	else
		deployGreen
		return
	fi
}

healthCheck(){
	if [[ $deploy_color == $BLUE ]]
	then
		deploy_url=$BLUE_URL
	else
		deploy_url=$GREEN_URL
	fi

	for retry_count in {1..3}
	do
		echo >&2 ">> curl -s $deploy_url"
		response="$(curl -s $deploy_url)"
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
	if [[ $deploy_color == $BLUE ]]
	then
		config_path=$BLUE_CONFIG
	else
		config_path=$GREEN_CONFIG
	fi

	echo >&2 ">> docker cp $config_path $NGINX_CONTAINER:/etc/nginx/conf.d/bridge-api.conf"
	docker cp $config_path $NGINX_CONTAINER:/etc/nginx/conf.d/bridge-api.conf

	echo >&2 ">> docker exec $NGINX_CONTAINER nginx -s reload"
	docker exec $NGINX_CONTAINER nginx -s reload
}

deletePrevContainer(){
	if [[ $deploy_color == $BLUE ]]
	then
		prev_container=$GREEN_CONTAINER
	else
		prev_container=$BLUE_CONTAINER
	fi

	echo >&2 ">> docker rm $prev_container -f"
	docker rm $prev_container -f
}

delay(){
	delay_second=$1
	for (( second = $delay_second; second > 0; second--))
	do
		echo >&2 ">> $second seconds remain"
		sleep 1
	done
}

echo >&2 "> Startring deploy"
deploy

echo >&2 "> Health check starts in $HEALTH_CHECK_DELAY"
delay $HEALTH_CHECK_DELAY

echo >&2 "> Start health check"
result=$(healthCheck)
if [[ $result != $TRUE ]]
then
	echo >&2 "> Deployed service is unhealthy"
	
	echo >&2 "> Exit with -1"
	exit -1
fi

echo >&2 "> Deployed service is healthy"

echo >&2 "> Switch service in $SWITCH_DELAY"
delay $SWITCH_DELAY

echo >&2 "> Switch container"
switch

echo >&2 "> Delete previous container"
deletePrevContainer

echo >&2 "> Completed Successfully"
exit 0
