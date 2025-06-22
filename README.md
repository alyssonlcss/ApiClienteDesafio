# ApiClienteDesafio

## Sobre o Projeto

Este projeto é uma API RESTful desenvolvida como solução para um desafio técnico de cadastro de clientes, endereços e contatos. O objetivo principal é demonstrar boas práticas de arquitetura, validação, integração com serviços externos e uso de tecnologias modernas do ecossistema .NET.

A API permite o cadastro de clientes, onde o endereço é preenchido automaticamente a partir do CEP informado, consultando a API pública do ViaCEP. Todas as operações seguem padrões profissionais de DTO, mapeamento, validação e tratamento de erros.

---

## Testes - Postman Collection

Para testar os endpoints da API, uma coleção completa do Postman é fornecida no arquivo `ApiClienteDesafioRequestsTests.postman_collection.json` na raiz do projeto. Siga os passos abaixo para importá-la e utilizá-la:

1. **Abra o Postman**
   - Baixe e instale o Postman se ainda não o tiver: https://www.postman.com/downloads/

2. **Importe a Coleção**
   - Clique em `Import` no canto superior esquerdo do Postman.
   - Selecione o arquivo `ApiClienteDesafioRequestsTests.postman_collection.json` do diretório do seu projeto.
   - A coleção chamada `ApiClienteDesafio` aparecerá no seu espaço de trabalho.

3. **Defina a Variável de Ambiente**
   - A coleção utiliza a variável `baseUrl` (padrão: `http://localhost:5199`).
   - Você pode definir ou editar essa variável no Postman clicando no olhar rápido da `Environment` (ícone de olho) no canto superior direito, em seguida, `Edit` ou `Add` um novo ambiente com a variável:
     - `baseUrl` = `http://localhost:5199` (ou a URL da sua API em execução)
   - Selecione este ambiente antes de executar as requisições.

4. **Execute as Requisições**
   - Expanda as pastas da coleção (`clients`, `addresses`, `contacts`) para ver todas as requisições disponíveis (GET, POST, PUT, DELETE).
   - Cada requisição contém exemplos de payloads e comentários para te guiar.
   - Ajuste os corpos das requisições conforme necessário e clique em `Send` para testar os endpoints.

5. **Verifique as Respostas**
   - Respostas bem-sucedidas e de erro são padronizadas em inglês.
   - Você pode ver exemplos de respostas na coleção ou na aba de resposta do Postman após enviar uma requisição.

> **Dica:**
> - A coleção abrange todas as operações principais: criar, atualizar, obter e excluir para clientes, endereços e contatos.
> - Restrições exclusivas (email, telefone) e campos obrigatórios são aplicados conforme as regras do desafio.
> - Para mais detalhes sobre os formatos de solicitação/resposta, veja os corpos de exemplo em cada solicitação ou consulte a documentação do Swagger em `/swagger`.

---

## Tecnologias Utilizadas

- **.NET 8.0**
- **Entity Framework Core 8**
- **AutoMapper**
- **Swashbuckle/Swagger** (documentação)
- **SQL Server** (padrão, mas pode ser adaptado)

---

## Como rodar o projeto

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd ApiClienteDesafio/ApiClienteDesafio
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Crie a migration inicial e o banco de dados:**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

5. **Acesse a documentação Swagger:**
   Abra o navegador em: [http://localhost:5199/swagger](http://localhost:5199/swagger)

---

## Regras do Desafio

- **a) Consulta de CEP:**
  - Ao receber o CEP do cliente, a API consulta o ViaCEP e preenche automaticamente os dados de endereço no banco de dados.

- **b) API Restful:**
  - Todos os endpoints seguem o padrão REST, com verbos HTTP adequados e respostas padronizadas.

- **c) Uso de DTO:**
  - Todas as operações de entrada e saída utilizam Data Transfer Objects (DTOs), garantindo segurança e clareza na comunicação.

- **d) Uso de Mapper:**
  - O AutoMapper é utilizado para mapear entre Models e DTOs, centralizando e padronizando as conversões.

- **e) Uso de Entity Framework:**
  - O acesso ao banco de dados é feito via Entity Framework Core, com migrations e contexto fortemente tipado.

---

## Padrões e Boas Práticas

- **Validação centralizada:**
  - Todas as validações de dados são feitas via DataAnnotations, validações customizadas e services, garantindo robustez e mensagens claras.

- **Tratamento de erros padronizado:**
  - Todos os endpoints retornam mensagens de erro amigáveis e status HTTP apropriados.

- **Separação de responsabilidades:**
  - Controllers, Services, Validators, DTOs e Models bem separados, facilitando manutenção e testes.

- **Uso de interfaces para services:**
  - Facilita testes unitários e desacoplamento.

- **Documentação automática:**
  - Swagger disponível para explorar e testar todos os endpoints.

- **Versionamento e extensibilidade:**
  - Estrutura pronta para versionamento e expansão futura.

---

## Observação sobre a modelagem

Como o desafio não especificava a cardinalidade entre Cliente, Endereço e Contato, foi assumido o relacionamento 1:1 (um cliente possui um endereço e um contato), tanto no modelo de dados quanto na API.
