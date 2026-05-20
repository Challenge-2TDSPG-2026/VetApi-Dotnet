# 🐾 VetClinic API — FIAP Sprint 1

API RESTful para gestão de clínica veterinária desenvolvida com **ASP.NET Core 8**, **Oracle Database** e **Entity Framework Core**.

## Tecnologias

- ASP.NET Core 8 (Controllers)
- Entity Framework Core 8 + Oracle.EntityFrameworkCore
- Oracle Database XE / XEPDB1
- AutoMapper 12
- Swagger / OpenAPI (Swashbuckle)

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- Oracle Database (local ou via Docker)

### Oracle via Docker (recomendado)

```bash
docker run -d \
  --name oracle-xe \
  -p 1521:1521 \
  -e ORACLE_PASSWORD=fiap1234 \
  gvenzl/oracle-xe:21-slim
```

Aguarde ~60 segundos e conecte com:
- **User:** system
- **Password:** fiap1234
- **Host:** localhost:1521/XEPDB1

## Instalação e Execução

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/VetApi.git
cd VetApi
```

### 2. Configurar a connection string

Edite `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=system;Password=fiap1234;Data Source=localhost:1521/XEPDB1;"
  }
}
```

### 3. Aplicar migrations

```bash
dotnet ef database update
```

### 4. Executar

```bash
dotnet run
```

Acesse o **Swagger UI** em: `http://localhost:5000`

---

## Rotas da API

### Tutores `/api/tutores`

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/tutores` | Lista todos | 200 |
| GET | `/api/tutores/{id}` | Busca por ID | 200 / 404 |
| GET | `/api/tutores/email/{email}` | Busca por email | 200 / 404 |
| GET | `/api/tutores/ativos` | Lista ativos | 200 |
| GET | `/api/tutores/buscar?nome=x&ativo=true` | Busca avançada | 200 |
| GET | `/api/tutores/{id}/pets` | Pets do tutor | 200 / 404 |
| POST | `/api/tutores` | Cadastrar tutor | 201 / 400 |
| PUT | `/api/tutores/{id}` | Atualizar tutor | 204 / 400 / 404 |
| DELETE | `/api/tutores/{id}` | Remover tutor | 204 / 404 |

### Pets `/api/pets`

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/pets` | Lista todos | 200 |
| GET | `/api/pets/{id}` | Busca por ID | 200 / 404 |
| GET | `/api/pets/especie/{especie}` | Filtra por espécie | 200 |
| GET | `/api/pets/ativos` | Lista ativos | 200 |
| GET | `/api/pets/buscar?nome=x&especie=y&raca=z` | Busca avançada | 200 |
| GET | `/api/pets/{id}/jornada` | **Jornada completa do pet** | 200 / 404 |
| POST | `/api/pets` | Cadastrar pet | 201 / 400 / 404 |
| PUT | `/api/pets/{id}` | Atualizar pet | 204 / 400 / 404 |
| DELETE | `/api/pets/{id}` | Remover pet | 204 / 404 |

### Consultas `/api/consultas`

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/consultas` | Lista todas | 200 |
| GET | `/api/consultas/{id}` | Busca por ID | 200 / 404 |
| GET | `/api/consultas/status/{status}` | Filtra por status | 200 |
| GET | `/api/consultas/periodo?de=...&ate=...` | Filtra por período | 200 / 400 |
| GET | `/api/consultas/pet/{petId}` | Consultas de um pet | 200 / 404 |
| POST | `/api/consultas` | Agendar consulta | 201 / 400 / 404 |
| PUT | `/api/consultas/{id}` | Atualizar consulta | 204 / 400 / 404 |
| DELETE | `/api/consultas/{id}` | Remover consulta | 204 / 404 |

### Vacinações `/api/vacinacoes`

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/vacinacoes` | Lista todas | 200 |
| GET | `/api/vacinacoes/{id}` | Busca por ID | 200 / 404 |
| GET | `/api/vacinacoes/pet/{petId}` | Vacinas de um pet | 200 / 404 |
| GET | `/api/vacinacoes/proximas-doses?ate=...` | Próximas doses | 200 |
| POST | `/api/vacinacoes` | Registrar vacinação | 201 / 400 / 404 |
| DELETE | `/api/vacinacoes/{id}` | Remover vacinação | 204 / 404 |

### Exames `/api/exames`

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/exames` | Lista todos | 200 |
| GET | `/api/exames/{id}` | Busca por ID | 200 / 404 |
| GET | `/api/exames/pet/{petId}` | Exames de um pet | 200 / 404 |
| GET | `/api/exames/tipo/{tipo}` | Filtra por tipo | 200 |
| POST | `/api/exames` | Registrar exame | 201 / 400 / 404 |
| PUT | `/api/exames/{id}` | Atualizar resultado | 204 / 400 / 404 |
| DELETE | `/api/exames/{id}` | Remover exame | 204 / 404 |

---

## Exemplos de Payload

### Criar Tutor
```json
{
  "nome": "Maria Souza",
  "email": "maria@email.com",
  "telefone": "11987654321",
  "cpf": "123.456.789-00",
  "endereco": "Rua das Flores, 100, São Paulo - SP"
}
```

### Criar Pet
```json
{
  "nome": "Rex",
  "especie": "Cachorro",
  "raca": "Golden Retriever",
  "sexo": "Macho",
  "dataNascimento": "2020-05-10",
  "peso": 12.5,
  "cor": "Dourado",
  "tutorId": 1
}
```

### Agendar Consulta
```json
{
  "petId": 1,
  "veterinario": "Dr. João Silva",
  "dataConsulta": "2025-06-15T10:00:00",
  "motivo": "Check-up anual e queda de pelo"
}
```

### Registrar Vacinação
```json
{
  "petId": 1,
  "vacina": "V10",
  "fabricante": "Zoetis",
  "lote": "LOT2024A",
  "dataAplicacao": "2025-05-19",
  "proximaDose": "2026-05-19",
  "veterinario": "Dra. Ana Costa"
}
```

### Registrar Exame
```json
{
  "petId": 1,
  "tipoExame": "Hemograma completo",
  "dataExame": "2025-05-19",
  "resultado": "Todos os índices dentro do esperado",
  "laboratorio": "LabVet SP",
  "veterinario": "Dr. Carlos Lima"
}
```

---

## Estrutura do Projeto

```
VetApi/
├── Controllers/
│   ├── TutoresController.cs
│   ├── PetsController.cs
│   ├── ConsultasController.cs
│   ├── VacinacoesController.cs
│   └── ExamesController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── TutorDtos.cs
│   ├── PetDtos.cs
│   ├── ConsultaDtos.cs
│   ├── JornadaDtos.cs        ← VacinacaoDto + ExameDto
│   └── JornadaPetDto.cs      ← Jornada completa
├── Mappings/
│   └── MappingProfile.cs
├── Migrations/
│   └── 20240101000000_InitialCreate.cs
├── Models/
│   ├── Tutor.cs
│   ├── Pet.cs
│   ├── Consulta.cs
│   ├── Vacinacao.cs
│   └── Exame.cs
├── appsettings.json
├── VetApi.csproj
└── Program.cs
```

## Comandos EF Core

```bash
dotnet ef migrations add NomeDaMigration
dotnet ef database update
dotnet ef migrations remove
```

---

## 🐳 Subindo tudo com Docker (recomendado para apresentação)

### Pré-requisito
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado

### 1 comando para rodar tudo

```bash
docker-compose up --build
```

Isso vai:
1. Baixar e iniciar o Oracle XE automaticamente
2. Fazer o build da API
3. Aplicar as migrations no banco
4. Subir a API

Acesse o **Swagger UI** em: **http://localhost:8080**

> ⚠️ Na **primeira vez**, o Oracle demora ~2 minutos para inicializar. A API vai aguardar automaticamente e aplicar as migrations assim que o banco estiver pronto.

### Parar tudo
```bash
docker-compose down
```

### Parar e apagar o banco (reset completo)
```bash
docker-compose down -v
```
