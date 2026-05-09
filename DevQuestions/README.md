# DevQuestions

Веб-API сервиса вопросов и ответов для разработчиков (формат Q&A в духе Stack Overflow). Реализован как модульный монолит на ASP.NET Core 9 с разделением на изолированные модули и слои.

## Стек

- **.NET 9**, ASP.NET Core (контроллеры + Minimal API)
- **PostgreSQL** — основное хранилище (EF Core + Dapper через `ISqlConnectionFactory`)
- **ElasticSearch** — полнотекстовый поиск
- **S3** — хранилище файлов (скриншоты к вопросам)
- **CSharpFunctionalExtensions** — `Result<T, Failure>` для бизнес-ошибок
- **FluentValidation** — валидация команд через декоратор
- **Scrutor** — авто-регистрация хендлеров и декорирование
- **OpenAPI / Swagger UI** — документация API
- **StyleCop.Analyzers** — статический анализ стиля

## Структура решения

```
src/
├── Core/
│   ├── Shared/                       # Общие примитивы: Envelope, Failure, Error, ICommand/IQuery
│   ├── Framework/                    # Веб-инфраструктура: IEndpoint, EndpointResult, ResponseExtensions
│   └── Infrastructure/
│       ├── Infrastructure.ElasticSearch/
│       └── Infrastructure.S3/
├── Questions/                        # Модуль вопросов (Domain/Application/Infrastructure/Presenters)
│   ├── Questions.Domain/
│   ├── Questions.Application/        # CQRS-хендлеры, декораторы, валидаторы
│   ├── Questions.Infrastructure.Postgres/
│   ├── Questions.Contracts/
│   └── Questions.Presenters/         # MVC-контроллеры
├── Tags/                             # Модуль тегов (Minimal API endpoints в стиле IEndpoint)
├── Comments/
├── Reports/
└── Web/
    └── Web/                          # Composition root: Program.cs, DI, middleware
```

## Архитектура

### Модули
Каждый бизнес-модуль (`Questions`, `Tags`, …) самодостаточен и раскладывается по классическим слоям:
- **Domain** — сущности и доменная логика
- **Application** — команды, запросы, хендлеры
- **Infrastructure** — реализация репозиториев, EF Core контексты
- **Contracts** — DTO и публичный API между модулями
- **Presenters** — HTTP-эндпоинты (контроллеры или Minimal API)

Межмодульное взаимодействие — через интерфейсы из `*.Contracts` (например, `ITagsContract`).

### CQRS
```csharp
public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    Task<Result<TResponse, Failure>> Handle(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<TResponse> Handle(TQuery query, CancellationToken ct);
}
```

Хендлеры регистрируются автоматически через Scrutor.

### Декораторы команд
Цепочка декораторов оборачивает каждый `ICommandHandler<,>` (порядок применения — снизу вверх):

1. `ValidationDecorator` — FluentValidation, при ошибке возвращает `Failure` без вызова handler'а
2. `TestDecorator`
3. `MetricsDecorator`

### Унифицированный ответ API

Все ответы оборачиваются в `Envelope` / `Envelope<T>`:
```json
{
  "result": { "...": "..." },
  "failureList": null,
  "timeGenerated": "2026-05-09T10:00:00Z"
}
```

При ошибке `result = null`, а в `failureList` лежат `Error[]` с `code`, `message`, `type`, `invalidField`.

### EndpointResult
`Framework.EndpointResults.EndpointResult<TValue>` — единая обёртка `IResult` поверх `Result<TValue, Failure>`:
- Успех → `200 OK` + `Envelope<TValue>`
- Ошибка → статус по `ErrorType` (`VALIDATION→400`, `NOT_FOUND→404`, `CONFLICT→409`, иначе `500`) + `Envelope` с массивом ошибок
- Реализует `IEndpointMetadataProvider` — типы ответов автоматически попадают в OpenAPI

```csharp
[HttpPost]
public async Task<EndpointResult<Guid>> Create(
    [FromServices] ICommandHandler<Guid, CreateQuestionCommand> handler,
    [FromBody] CreateQuestionDto request,
    CancellationToken ct)
{
    var result = await handler.Handle(new CreateQuestionCommand(request), ct);
    return new EndpointResult<Guid>(result);
}
```

### Failure / Error
`Error` создаётся через фабрики, отражающие тип ошибки:
```csharp
Error.Validation("question.title.empty", "Title is required", invalidField: "title");
Error.NotFound("question.not.found", "Question not found", id);
Error.Conflict("answer.already.solution", "Answer is already marked as solution");
Error.Failure("question.calc.failed", "Calculation failed");
```
`Failure` — коллекция `Error`, неявно конвертируется из одиночного `Error` или `Error[]`.

## Запуск

Запускающий проект — `src/Web/Web/Web.csproj`.

```bash
dotnet run --project src/Web/Web
```

После старта Swagger UI:
- HTTP: <http://localhost:5078/swagger/index.html>
- HTTPS: <https://localhost:7269/swagger/index.html>

Строки подключения к Postgres / ElasticSearch / S3 настраиваются в `appsettings.Development.json`.

## Полезные команды

```bash
# Сборка
dotnet build

# Запуск с hot reload
dotnet watch --project src/Web/Web

# Запуск конкретного проекта
dotnet build src/Questions/Questions.Application/Questions.Application.csproj
```

## Соглашения

- Глобальные настройки сборки — `Directory.Build.props` (TFM, nullable, StyleCop)
- Стилевые правила — `.editorconfig`
- Все публичные ошибки идут через `Failure`/`Error`, исключения — только для непредвиденных ситуаций (обрабатываются `ExceptionMiddleware`)
- Бизнес-валидация выполняется в декораторе на уровне команды, а не в контроллере