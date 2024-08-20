# A82Moto
Backend para gestão de alugueis de moto

## Instruções para rodar o projeto
- Requisitos do ambiente
  - Ter o docker e docker-compose instalado no ambiente
  - Ter o git instalado para clonar o projeto
  - Ter o VS Code ou Visual Studio caso queira rodar o projeto local

## Rodando o projeto localmente
Clone o projeto para a estação de trabalho executando o comando abaixo.
```
git clone https://github.com/aislanmiranda/A82Moto.git
```
Com o clone do projeto da máquina, entre na pasta A82moto, e execute o arquivo docker-compose-debug para
subir o ambiente do mongodb e do rabbitmq.
```
docker-compose -f docker-compose-debug.yml up -d
```
Após executar o comando acima, valide se os serviços do mongodb e do rabbitmq estão rodando, executando o comando abaixo:
```
docker-compose ps
```
![Captura de Tela 2024-08-20 às 01 44 01](https://github.com/user-attachments/assets/78753d78-01ee-4784-bfd2-da9fb8bf4e6e)

Com os serviços rodando, rode o projeto backend conforme print abaixo.

![Captura de Tela 2024-08-20 às 01 47 43](https://github.com/user-attachments/assets/2c228663-3c8a-4c9a-b80c-4f80b5ff922c)

Após rodar o projeto backend, certifique-se que a documentação da api foi exibida.

![Captura de Tela 2024-08-20 às 01 57 30](https://github.com/user-attachments/assets/462ce05c-b36e-4c8c-b466-6de004560e5e)

## Rodando o projeto no docker
Já com o clone do projeto na máquina, entre na pasta A82moto, e execute o arquivo docker-compose para
subir o ambiente do mongodb, do rabbitmq e a aplicação backend.
> Garanta que o comando abaixo execute na local onde está o arquivo docker-compose.yml
```
docker-compose -f docker-compose --build up -d
```
![Captura de Tela 2024-08-20 às 02 07 53](https://github.com/user-attachments/assets/31f2b421-60cb-4e20-a94b-0f86e9bf0658)

Após executar o comando acima, valide se os serviços do mongodb, do rabbitmq e do backend estão rodando, executando o comando abaixo:
```
docker-compose ps
```
![Captura de Tela 2024-08-20 às 02 08 36](https://github.com/user-attachments/assets/13dd53c8-2f53-40a1-831c-0c18ec02f87f)

## Rodando aplicação que consome a fila
Com os projetos rodando localmente ou no docker, acesse o projeto RabbitConsumerConsole, clique com o botão direito e selecione a opção "Iniciar a depuração do projeto".

![Captura de Tela 2024-08-20 às 02 12 29](https://github.com/user-attachments/assets/4aa06c25-876a-4066-b471-29e6dfc4b77a)

## Curl das chamadas para api
> se estiver executando as chamadas apontando para o projeto backend que está no container, lembre-se de apontar a porta corretamente nas chamadas curl 8080.

> Consultar planos existentes. Ao rodar o projeto, os planos padrões são cadastrados através de uma função seed.
```
curl --location 'http://localhost:8080/api/plan' \
--header 'accept: */*'
```
> Cadastrar moto
```
curl --location 'http://localhost:8080/api/motorcycle' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
    "year": 2024,
    "model": "CG 125",
    "plate": "AAA-1234"
}'
```
> Consultar motos que não estão em uso
```
curl --location 'http://localhost:8080/api/motorcycle?inUse=false' \
--header 'accept: */*'
```
> Atualizar placa da moto
```
curl --location --request PATCH 'http://localhost:8080/api/motorcycle' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "id": "ID-MOTOCYCLE-AQUI",
  "plate": "AAA-1235"
}'
```
> Deletar moto
```
curl --location --request DELETE 'http://localhost:8080/api/motorcycle/ID-MOTOCYCLE-QUI' \
--header 'accept: */*'
```
> Cadastrar Entregador
```
curl --location 'http://localhost:8080/api/deliveryman' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "name": "Fulano de Tal",
  "cnpj": "00.000.000/0001-01",
  "birthDate": "1982-09-23",
  "numberCnh": 1002024,
  "typeCnh": "AB"
}'
```
> Consultar Entregadores
```
curl --location 'http://localhost:8080/api/deliveryman' \
--header 'accept: */*'
```
> Atualizar foto entregador
```
curl --location --request PATCH 'http://localhost:8080/api/deliveryman/photo' \
--header 'accept: */*' \
--form 'PhotoFile=@"LOCAL-DO-ARQUIVO/CNH_TESTE.png"' \
--form 'Id="ID-DELIVERYMAN-AQUI"'
```
> Consultar Orçamento informando plano e data prevista para entrega
```
curl --location 'http://localhost:8080/api/rent/budget' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "planId": "ID-PLAN-AQUI",
  "forecastDate": "2024-08-25"
}'
```
> Efetivar aluguel moto
```
curl --location 'http://localhost:8080/api/rent' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
  "deliveryManId": "ID-DELIVERYMAN",
  "motocycleId": "ID-MOTOCYCLE",
  "planId": "ID-PLAN",
  "forecastDate": "2024-08-25"
}'
```