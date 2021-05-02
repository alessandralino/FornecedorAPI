# FornecedorAPI
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/makes-people-smile.svg)](https://devioapi.azurewebsites.net/swagger/index.html)

Esta aplicação devolve dados de Fornecedor para sistemas de controle de Produto. 

## Features

O projeto pode ser usado como modelo para construção de WebAPI REST com ASP.NET Core de modo forma prática.
Atualmente as funciolidades envolvem operações de: 

- Autenticação e permissão de acesso de usuário via JWT (Jason Web Token); 
- CRUD de Fornecedor e Produtos;
- Upload de Imagem (com a opção de utilizar imagens relativamente grandes). *

 Na versão atual suprimi o bloqueio à aplicação, deixando o acesso ao Swagger público via Cloud.
 Você pode acessar a versão atual e as anteriores [aqui](https://devioapi.azurewebsites.net/swagger/index.html).

## Começando

Para executar a aplicação, será necessário instalar:

- [.NET Core 2.2_](https://dotnet.microsoft.com/download/dotnet/2.2)
- [_Visual Studio 2019_](https://visualstudio.microsoft.com/pt-br/downloads/)
- [_Elmah_](https://elmah.io/features/asp-net-core/)
- [Healthy Checks com _Xabaril_](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)
- [_AutoMapper_](https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection)

## Desenvolvimento

Para iniciar o desenvolvimento, é necessário clonar o projeto do GitHub num diretório de sua preferência:

```shell
cd "diretorio de sua preferencia"
git clone https://github.com/AlessandraLino/FornecedorAPI.git
```

## Build

Para construir o projeto utilizar [Comandos de CLI](https://docs.microsoft.com/pt-br/dotnet/core/tools/)

```shell
dotnet build [<PROJECT>|<SOLUTION>] [-c|--configuration <CONFIGURATION>]
    [-f|--framework <FRAMEWORK>] [--force] [--interactive] [--no-dependencies]
    [--no-incremental] [--no-restore] [--nologo] [-o|--output <OUTPUT_DIRECTORY>]
    [-r|--runtime <RUNTIME_IDENTIFIER>] [--source <SOURCE>]
    [-v|--verbosity <LEVEL>] [--version-suffix <VERSION_SUFFIX>]

dotnet build -h|--help

```

O comando dotnet build compila o projeto e suas dependências em um conjunto de binários. Os binários incluem o código do projeto em arquivos de IL (linguagem intermediária) com uma extensão . dll . 
Dependendo do tipo de projeto e das configurações, outros arquivos podem ser incluídos.

## Deploy e Publicação

O arquivo appsettings.Development e Staging configuram publicação nesses respectivos ambientes. Já em Produção os dados foram configurados na plataforma Azure. 
Para não expor dados desse ambiente neste repositório foi utilizado o _Management User Secrets_. 


### Teste via Swagger

- Utilize o endpoint "/api/v1/nova-conta" para registrar um usuário; 
- Insira o usuário e senha no body do endpoint "/api/v1/entrar";
- Obtenha o conteúdo de "accessToken" da resposta;
- Insira-o em Autorize (canto superior direito). 

Verifique também o token obtido em [jwt.io](https://jwt.io/)

_Métodos que não requerem permissão via Claims na versão 1.0:_
- [Listar Fornecedores](/api/v1/Fornecedores)



