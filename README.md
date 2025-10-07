# 🍰 Boleria API 

Processo completo de deploy API Boleria em Azure App Service, utilizando .NET 8 e Azure SQL Database. </br>
Inclui os scripts de criação da infraestrutura, publicação via publish.zip, configuração do ambiente e validação da aplicação.

#

## ⚙️ Pré-requisitos
- Conta Microsoft Azure.
- Azure CLI configurado (ou Cloud Shell no portal).
- .NET 8 SDK instalado localmente.
- Banco de dados PostgreSQL no Azure (será criado via script).

#

## 1. Clonar o Repositório
```bash
git clone https://github.com/ykxtais/boleria-devops.git
cd boleria-devops
```

## 2. Baixe as dependências para o SQL Server (.NET 8)
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 8.0.6
dotnet add package Microsoft.EntityFrameworkCore -v 8.0.6
```

## 3. Provisionando a Infraestrutura no Azure
Substitua <SUASENHA!123> por uma senha real:
```bash
#!/bin/bash

# Variáveis de ambiente
RG="rg-boleria"
LOC="brazilsouth"
ASPLAN="plan-boleria"
WEBAPP="boleria-api"
SQL_SERVER="sql-boleria"
SQL_ADMIN="sqladminuser"
SQL_PASSWORD="<SUASENHA!123>"
SQL_DB="boleria_db"
AI_NAME="ai-boleria"

# Criando grupo de recursos
az group create -n $RG -l $LOC

# Criando Application Insights
az monitor app-insights component create \
  --app $AI_NAME --location $LOC --resource-group $RG --application-type web

# Criando App Service Plan (.NET 8 Linux F1)
az appservice plan create -g $RG -n $ASPLAN --sku F1 --is-linux

# Criando Web App
az webapp create -g $RG -p $ASPLAN -n $WEBAPP --runtime "DOTNETCORE:8.0"

# Criando Azure SQL Server e Banco de Dados
az sql server create -g $RG -n $SQL_SERVER -l $LOC \
  -u $SQL_ADMIN -p $SQL_PASSWORD

az sql db create -g $RG -s $SQL_SERVER -n $SQL_DB --service-objective S0

# Criando regra de firewall para Azure Services 
az sql server firewall-rule create -g $RG -s $SQL_SERVER \
  -n AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Configurando string de conexão no Web App
CONN="Server=tcp:${SQL_SERVER}.database.windows.net,1433;Database=${SQL_DB};User ID=${SQL_ADMIN};Password=${SQL_PASSWORD};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
az webapp config connection-string set -g $RG -n $WEBAPP \
  --settings SqlServerDb="$CONN" --connection-string-type SQLAzure

# Configurando Application Insights no Web App
AI_CONN=$(az monitor app-insights component show -g $RG -a $AI_NAME --query connectionString -o tsv)
az webapp config appsettings set -g $RG -n $WEBAPP --settings \
  APPLICATIONINSIGHTS_CONNECTION_STRING="$AI_CONN" \
  ApplicationInsightsAgent_EXTENSION_VERSION="~3" \
  XDT_MicrosoftApplicationInsights_Mode="recommended"


# executa o DDL no banco SQL (pwsh)
pwsh -Command "Invoke-Sqlcmd -ConnectionString 'Server=tcp:${SQL_SERVER}.database.windows.net,1433;Database=${SQL_DB};User ID=${SQL_ADMIN};Password=${SQL_PASSWORD};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;' -InputFile ./ddl.sql"

echo "Pronto"
echo "webapp: https://${WEBAPP}.azurewebsites.net"
echo "swagger: https://${WEBAPP}.azurewebsites.net/swagger"
```

## 4. Compile, gere os artefatos de publicação
```bash
dotnet publish -c Release -o publish
cd publish
zip -r ../publish .
```

## 5. Realize o Deploy
```bash
az webapp deploy -g rg-boleria -n boleria-api --src-path ./publish.zip --type zip
```

## 6. Testando a API
- Acesse o Swagger em: ``` https://boleria-api.azurewebsites.net/swagger ``` </br>
- azurewebsites endpoints: ``` https://boleria-api.azurewebsites.net/api/bolo ``` </br> ``` https://elysia-api.azurewebsites.net/api/pedido ``` 

</br>

## 🖇 Rotas Disponíveis (via Swagger)

### Bolos

| Método | Rota                                | Descrição                           |
|--------|-------------------------------------|--------------------------------------|
| GET    | `/api/Bolo`                         | Lista todos os bolos                |
| GET    | `/api/Bolo/{id}`                    | Obtém um bolo por id                |
| POST   | `/api/Bolo`                         | Cria um bolo                         |
| PUT    | `/api/Bolo/{id}`                    | Atualiza um bolo                    |
| DELETE | `/api/Bolo/{id}`                    | Exclui um bolo                      |

### UsuarioController

| Método | Rota                                | Descrição                           |
|--------|-------------------------------------|--------------------------------------|
| GET    | `/api/Pedido   `                    | Lista todos os pedidos              |
| GET    | `/api/Pedido/{id}`                 | Obtém um pedido por id               |
| POST   | `/api/Pedido `                      | Cria um pedido                    |
| PUT    | `/api/Pedido/{id}`                 | Atualiza um pedido                |
| DELETE | `/api/Pedido/{id}`                 | Exclui um pedido                  |

</br>

## 🎂 Inserts (Swagger)

### POST Bolo — `/api/Bolo`

```json
{
  "nome": "Floresta negra",
  "sabor": "Chocolate com cereja",
  "preco": "25",
}
```

#

### POST Pedido — `/api/Pedido`
```json
{
  "boloId": "550e8400-exem-plo4-a716-446655440000",
  "nomeCliente": "Mike",
}
```
Substitua o **boloId** pelo id retornado em **POST Bolo — `/api/Bolo`**

#

# 🧁 Integrantes

➤ Iris Tavares Alves — 557728 </br>
➤ Taís Tavares Alves — 557553 
