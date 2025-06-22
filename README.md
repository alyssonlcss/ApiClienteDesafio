# ApiClienteDesafio

## Sobre o Projeto

Este projeto é uma API RESTful desenvolvida como solução para um desafio técnico de cadastro de clientes, endereços e contatos. O objetivo principal é demonstrar boas práticas de arquitetura, validação, integração com serviços externos e uso de tecnologias modernas do ecossistema .NET.

A API permite o cadastro de clientes, onde o endereço é preenchido automaticamente a partir do CEP informado, consultando a API pública do ViaCEP. Todas as operações seguem padrões profissionais de DTO, mapeamento, validação e tratamento de erros.

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
