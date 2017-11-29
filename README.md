# dotnet.doduo
it's a library to support event driven micro services to bind with message bus


# How configure your RabbitMQ with docker

`docker run -d --name rabbit -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest -p 8080:15672 -p 5672:5672 rabbitmq:3-management`