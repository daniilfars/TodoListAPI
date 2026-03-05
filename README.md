# 📋 TodoList API

**RESTful API** для управления задачами с поддержкой проектов, тегов и полноценной JWT-аутентификацией. Проект разработан на **ASP.NET Core 10** с использованием **Entity Framework Core** и **PostgreSQL**.

---

## 🚀 Технологии

- **.NET 10** (ASP.NET Core Web API)
- **Entity Framework Core** (ORM, Code First, Fluent API)
- **PostgreSQL** (база данных)
- **JWT** (аутентификация, Bearer токены)
- **BCrypt.Net-Next** (хеширование паролей)
- **Swagger / OpenAPI** (документация)

---

## ✨ Функциональность

### 🔐 Аутентификация и пользователи
- Регистрация нового пользователя (с хешированием пароля)
- Вход в систему (получение JWT-токена)
- CRUD для пользователей (с защитой: пользователь может редактировать/удалять только себя)

### 📁 Проекты
- Создание, просмотр, обновление, удаление проектов
- Привязка проектов к конкретному пользователю

### ✅ Задачи
- Создание, просмотр, обновление, удаление задач
- Привязка задач к проекту
- Добавление нескольких тегов к задаче
- Фильтрация задач по тегам

### 🏷️ Теги
- Создание, просмотр, обновление, удаление тегов
- Автоматическая проверка уникальности имени тега
- Защита от удаления тега, который используется в задачах

### 🔍 Дополнительно
- Валидация входных данных через DTO
- Обработка бизнес-исключений (дубликаты, несуществующие записи)
- Оптимизация запросов (`AsNoTracking()` для чтения)
- Асинхронные методы

---

## 📦 Установка и запуск

### Предварительные требования
- Установленный [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Установленный [PostgreSQL](https://www.postgresql.org/download/)
- (Опционально) [Git](https://git-scm.com/)

### Шаги

1. **Клонировать репозиторий**
   ```bash
   git clone https://github.com/ваш-логин/TodoList.git
   cd TodoList
   ```

2. **Настроить базу данных**  
   В файле `appsettings.Development.json` (или через `dotnet user-secrets`) укажите строку подключения:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=TodoList;Username=postgres;Password=ваш_пароль"
   }

3. **Настроить JWT**  
   В том же файле укажите параметры JWT:
   ```json
   "JwtSettings": {
     "SecretKey": "ваш-секретный-ключ-минимум-32-символа",
     "Issuer": "TodoListAPI",
     "Audience": "TodoListAPI",
     "ExpiryMinutes": 60
   }
   ```

4. **Применить миграции**
   ```bash
   dotnet ef database update
   ```

5. **Запустить приложение**
   ```bash
   dotnet run
   ```

6. **Открыть документацию Swagger**  
   Перейдите по адресу `https://localhost:5001/swagger` (порт может отличаться)

---

## 📖 Использование API

### 🔑 Аутентификация

| Метод | Эндпоинт | Описание |
|-------|----------|----------|
| POST | `/api/auth/register` | Регистрация нового пользователя |
| POST | `/api/auth/login` | Вход в систему (получение токена) |

**Пример регистрации:**
```json
POST /api/auth/register
{
  "userName": "alice",
  "email": "alice@example.com",
  "password": "SecurePass123"
}
```

**Ответ:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": 1,
    "name": "alice",
    "email": "alice@example.com",
    "createdAt": "2025-03-05T10:00:00Z"
  }
}
```

### 🔒 Защищённые эндпоинты

Для всех запросов (кроме регистрации и входа) требуется заголовок:
```
Authorization: Bearer {ваш_токен}
```

#### Проекты

| Метод | Эндпоинт | Описание |
|-------|----------|----------|
| GET | `/api/project` | Получить все проекты |
| GET | `/api/project/{id}` | Получить проект по ID |
| POST | `/api/project` | Создать новый проект |
| PUT | `/api/project/{id}` | Обновить проект |
| DELETE | `/api/project/{id}` | Удалить проект |

**Пример создания проекта:**
```json
POST /api/project
{
  "name": "Рабочие задачи",
  "description": "Проекты по работе",
  "userId": 1
}
```

#### Задачи

| Метод | Эндпоинт | Описание |
|-------|----------|----------|
| GET | `/api/task` | Получить все задачи |
| GET | `/api/task/{id}` | Получить задачу по ID |
| GET | `/api/task/bytag/{tagId}` | Получить задачи по тегу |
| POST | `/api/task` | Создать задачу |
| PUT | `/api/task/{id}` | Обновить задачу |
| DELETE | `/api/task/{id}` | Удалить задачу |

**Пример создания задачи с тегами:**
```json
POST /api/task
{
  "title": "Изучить EF Core",
  "description": "Прочитать документацию",
  "projectId": 1,
  "tagIds": [1, 2]
}
```

#### Теги

| Метод | Эндпоинт | Описание |
|-------|----------|----------|
| GET | `/api/tag` | Получить все теги |
| GET | `/api/tag/{id}` | Получить тег по ID |
| GET | `/api/tag/{tagId}/tasks` | Получить задачи по тегу |
| POST | `/api/tag` | Создать тег |
| PUT | `/api/tag/{id}` | Обновить тег |
| DELETE | `/api/tag/{id}` | Удалить тег |

#### Пользователи

| Метод | Эндпоинт | Описание |
|-------|----------|----------|
| GET | `/api/user` | Получить всех пользователей |
| GET | `/api/user/{id}` | Получить пользователя по ID |
| POST | `/api/user` | Создать пользователя (для администрирования) |
| PUT | `/api/user/{id}` | Обновить пользователя |
| DELETE | `/api/user/{id}` | Удалить пользователя |

⚠️ Пользователь может редактировать и удалять только свою учётную запись.

---

## 📁 Структура проекта

```
TodoList/
├── Controllers/               # Эндпоинты API
│   ├── AuthController.cs
│   ├── ProjectController.cs
│   ├── TaskController.cs
│   ├── TagController.cs
│   └── UserController.cs
├── Data/
│   └── AppDbContext.cs         # Контекст EF Core
├── Models/                      # Сущности БД
│   ├── User.cs
│   ├── Project.cs
│   ├── TaskItem.cs
│   ├── Tag.cs
│   └── DTOs/                    # Объекты передачи данных
│       ├── CreateDTOs/
│       ├── UpdateDTOs/
│       └── ResponseDTOs/
├── Services/                     # Бизнес-логика (интерфейсы и реализации)
│   ├── IAuthService.cs
│   ├── AuthService.cs
│   ├── ITokenService.cs
│   ├── TokenService.cs
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IProjectService.cs
│   ├── ProjectService.cs
│   ├── ITaskService.cs
│   ├── TaskService.cs
│   ├── ITagService.cs
│   └── TagService.cs
├── Configurations/
│   └── JwtSettings.cs            # Класс для настроек JWT
├── Migrations/                    # Миграции EF Core
├── Program.cs                     # Точка входа и DI
├── appsettings.json               # Конфигурация
└── TodoList.csproj                # Файл проекта
```

---

## 🧪 Тестирование

Для тестирования API можно использовать:
- **Postman** – скачать с [официального сайта](https://www.postman.com/)
- **Swagger** – встроенный UI по адресу `/swagger`
- **curl** – из командной строки

Пример тестирования через **curl**:
```bash
# Получить токен
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"alice@example.com","password":"SecurePass123"}'

# Создать проект (с токеном)
curl -X POST https://localhost:5001/api/project \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -H "Content-Type: application/json" \
  -d '{"name":"Тест","description":"Описание","userId":1}'
```

---

## 🤝 Вклад в проект

Если вы нашли ошибку или хотите предложить улучшение:
1. Создайте **Issue** в репозитории
2. Или отправьте **Pull Request** с описанием изменений

---

## 📄 Лицензия

Этот проект распространяется под лицензией MIT. Подробнее см. файл `LICENSE`.

---

**Автор:** Данииль  
**GitHub:** https://github.com/daniilfars/ 

---

⭐ Если проект оказался полезным, поставьте звезду на GitHub!
