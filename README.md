# Proposta

MottuSense é uma solução inteligente desenvolvida para a Mottu, focada no mapeamento automatizado do pátio e na gestão eficiente das motos da frota.
Utilizando sensores IoT e uma arquitetura em nuvem com APIs .o sistema permite monitoramento em tempo real, controle de entrada e saída dos veículos, e integração com um app mobile para operadores.
Combinando banco de dados relacional e não relacional, DevOps, testes de qualidade e tecnologias modernas de desenvolvimento mobile e backend, o MottuSense garante rastreabilidade, performance e escalabilidade — tudo alinhado com os pilares da Mottu: tecnologia, mobilidade e oportunidade.

🛵 Nome da Solução: MottuSense
🔤 Significado:
"Mottu" (nome da empresa)
"Sense" de sensorial, percepção, inteligência → representa a capacidade da solução de "sentir" e gerenciar o pátio de motos com IoT.

## Diferencial

- Monitoramento em tempo real das motos atraves dos sensores
- localização exata das motos


# MottuSense API

MottuSense API é um sistema de rastreamento de motos desenvolvido em C# com .NET 8. Ele permite gerenciar clientes, motos, pátios e sensores de localização. O sistema é ideal para empresas como a Mottu que precisam monitorar a localização de suas motos em tempo real, garantindo maior controle e segurança.


## Descrição do Projeto
Este projeto é um sistema de rastreamento de motos desenvolvido em C# com .NET 8. Ele permite gerenciar clientes, motos, pátios e sensores de localização.
### Principais Funcionalidades:
- Cadastro e gerenciamento de clientes.
- Registro de motos com informações detalhadas.
- Associação de motos a pátios e clientes.
- Monitoramento de localização das motos por meio de sensores.
### Observações 
- Recomendamos que ao abrir a aplicação no swagger, começe criando primeiro um patio, depois crie o cliente, depois crie a moto pois so é possivel criar uma moto se existe um cliente e patio cadastrado, por ultimo apos criar a moto crie sensores de localização e vincule a essa moto.
- Não é possivel criar motos com o mesmo numero de chassi, pois chassi é um valor unico.
- Não é possivel criar clientes com o mesmo email, pois assim como o chassi, o email é unico.
---

## Rotas da API

### **Clientes**
- **GET /api/clientes**  
  Retorna a lista de todos os clientes cadastrados.

- **GET /api/clientes/{id}**  
  Retorna os detalhes de um cliente específico.

- **POST /api/clientes**  
  Cadastra um novo cliente.  
  **Body:**

- **PUT /api/clientes/{id}**  
  Atualiza os dados de um cliente existente.  
  **Body:**  

- **DELETE /api/clientes/{id}**  
  Remove um cliente do sistema.

---

### **Motos**
- **GET /api/motos**  
  Retorna a lista de todas as motos cadastradas.

- **GET /api/motos/{id}**  
  Retorna os detalhes de uma moto específica.

- **POST /api/motos**  
  Cadastra uma nova moto.  
  **Body:**
  
- **PUT /api/motos/{id}**  
  Atualiza os dados de uma moto existente.  
  **Body:**

- **DELETE /api/motos/{id}**  
  Remove uma moto do sistema.

---

### **Pátios**
- **GET /api/patios**  
  Retorna a lista de todos os pátios cadastrados.

- **GET /api/patios/{id}**  
  Retorna os detalhes de um pátio específico.

- **POST /api/patios**  
  Cadastra um novo pátio.  
  **Body:**  

- **PUT /api/patios/{id}**  
  Atualiza os dados de um pátio existente.  
  **Body:**  

- **DELETE /api/patios/{id}**  
  Remove um pátio do sistema.

---

### **Sensores de Localização**
- **GET /api/sensores**  
  Retorna a lista de todas as localizações registradas.

- **GET /api/sensores/{id}**  
  Retorna os detalhes de uma localização específica.

- **POST /api/sensores**  
  Registra uma nova localização.  
  **Body:**

- **DELETE /api/sensores/{id}**  
  Remove um registro de localização.

---

## Instalação

### Pré-requisitos
- .NET 8 SDK instalado.
- Banco de dados configurado (ex.: SQL Server).
- Visual Studio 2022 ou outro editor compatível.

### Passos
1. Clone o repositório

2. Configure a string de conexão no arquivo `appsettings.json`

3. Execute as migrações para criar o banco de dados
   
4. Inicie o servidor

5. Acesse o swagger da API em: http://localhost:5043/swagger/index.html


  
  
