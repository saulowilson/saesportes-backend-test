# Teste backend SA Esportes

Olá, este repositório consiste em um projeto de teste de backend para SA Esportes. Nele há uma api feita em .NET Core WebAPI e um front em Angular para consumir a api. 

## Tecnologias presentes no projeto:
* .NET Core WebAPI
* SignalR
* RabbitMQ
* Angular


# Executando o projeto
O projeto está todo configurado para rodar no docker. Para rodar, execute o seguinte comando na pasta raíz onde o docker-compose.yml se encontra:

``docker compose up``

O projeto encontra-se disponível em 

``http://localhost:4200/produtor``
``http://localhost:4200/consumidor``