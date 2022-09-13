# aspnet_crud

Este é um projeto simples de CRUD de pessoas em uma base de dados. Foi desenvolvido totalmente em ambiente Linux (Distro Ubuntu).

## Tecnologias utilizadas

As principais tecnologias utilizadas neste projeto foram:

* ASP.NET MVC
* ASP.NET Web API
* Bootstrap
* X.PagedList
* SQL Server (RDS Instance)

## Como funciona o projeto?

### Instalação

Primeiro é necessário verificar se está instalado a última versão do dotnet Runtime e do SDK na máquina (no momento em que este projeto foi criado, foi utilizado a versão 6.0).

Caso não tenha instalado, recomendo dar uma olhada na documentação feita pela própria Microsoft para realizar a instalação:

* [Instalação no Linux](https://docs.microsoft.com/pt-br/dotnet/core/install/linux);
* [Instalação no Windows](https://docs.microsoft.com/pt-br/dotnet/core/install/windows);

Assim que o Runtime e o SDK estiverem instalados, faça um clone do repositório em seu ambiente local.

O projeto ele é dividido em duas pastas principais: WebAPI e o WebMVC. Ambos são projetos separados com configurações próprias, mas que se comunicam através de rotas (como um microservice).

### Execução

* Entre no diretório do projeto através do terminal/prompt;
* Entre no diretório WebAPI e execute: `dotnet run`;
* Abra um novo terminal/prompt e acesse o diretório WebMVC e logo em seguida rode: `dotnet run`;
* Com o projeto MVC em execução, vá no seu navegador e informe a URL: `https://localhost:7173/`;
* Pronto! O projeto estará rodando por completo;

## Observações

O projeto WebAPI já vem incluso com uma documentação swagger para utilização da API. Para ver a documentação, certifique-se de que o projeto WebAPI esteja em execução. Logo em seguida acesse a URL: `https://localhost:7296/swagger`.

Para saber como está estruturado o banco de dados, deixei o arquivo com os comandos que executei para criação da database dentro do diretório `./.github/database.sql`.
