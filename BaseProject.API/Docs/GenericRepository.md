# GenericRepository<T> Documentation

## Overview

`GenericRepository<T>` is a generic repository class that supports **EF Core** and **Dapper** operations. It provides common CRUD functionality and additional helpers for pagination, projection, and existence checks.

**Type Parameter:**

- `T` – Entity type (must be a class)

**Dependencies:**

- `ApplicationDbContext` – EF Core DbContext
- `SqlDapperContext` – Dapper connection context

---

## Constructor

### `GenericRepository(ApplicationDbContext context, SqlDapperContext dapperContext)`

Initializes a new instance of the repository.

- `context` – EF Core DbContext
- `dapperContext` – Dapper connection context

---

## Dapper Methods

### `GetPaginatedAsync_Dapper<T, TResult>(string tableName, int pageIndex, int pageSize, Expression<Func<T, TResult>> selector, string? orderByColumn = "Id", bool ascending = true)`

Retrieves paginated data from a table using Dapper.

- **Parameters:**
  - `tableName` – Database table name
  - `pageIndex` – Page number (1-based)
  - `pageSize` – Number of records per page
  - `selector` – Projection selector
  - `orderByColumn` – Column name to order by (default: `Id`)
  - `ascending` – Sort direction (default: `true`)
- **Returns:** `PaginatedList<TResult>` – Paginated result

---

### `GetByIdAsync_Dapper(string tableName, object id)`

Retrieves a single entity by ID using Dapper.

- **Parameters:**
  - `tableName` – Database table name
  - `id` – Primary key
- **Returns:** `T` – The entity if found

---

### `GetByIdProjectedAsync_Dapper<TResult>(string tableName, object id, Expression<Func<T, TResult>> selector = null)`

Retrieves a projected entity by ID using Dapper.

- **Parameters:**
  - `tableName` – Database table name
  - `id` – Primary key
  - `selector` – Projection expression
- **Returns:** `TResult?` – Projected result or null

---

### `ExistsAsync_Dapper<T>(string tableName, string id)`

Checks if an entity exists in a table using Dapper.

- **Parameters:**
  - `tableName` – Database table name
  - `id` – Entity ID
- **Returns:** `bool` – True if exists

---

### Private Helpers (Dapper)

- `ExtractSelectedColumns<T, TResult>(Expression<Func<T, TResult>> selector)` – Extracts columns for projection.
- `BuildPaginatedListQuery(string tableName, List<string> selectedColumns, string orderByColumn, bool ascending)` – Builds a SQL query for paginated Dapper retrieval.
- `CreateParameters(int pageIndex, int pageSize)` – Creates Dapper parameters for pagination.
- `MapToPaginatedList<TResult>((TResult, int)[] result, int pageIndex, int pageSize)` – Maps raw query results to `PaginatedList`.

---

## EF Core Methods

### Create

#### `AddAsync(T entity)`

Adds a single entity asynchronously.

- **Parameters:** `entity` – Entity to add
- **Returns:** `Task`

#### `AddRangeAsync(IEnumerable<T> entities)`

Adds multiple entities asynchronously.

- **Parameters:** `entities` – Collection of entities
- **Returns:** `Task`

---

### Read

#### `ExistsAsync(Expression<Func<T, bool>> filter)`

Checks if any entity satisfies a filter.

- **Parameters:** `filter` – Filter expression
- **Returns:** `Task<bool>`

#### `ExistsAsync()`

Checks if any entity exists.

- **Returns:** `Task<bool>`

#### `CountAsync(Expression<Func<T, bool>> filter)`

Counts entities matching a filter.

- **Parameters:** `filter` – Filter expression
- **Returns:** `Task<int>`

#### `CountAsync()`

Counts all entities.

- **Returns:** `Task<int>`

#### `GetByIdAsync(object id)`

Gets an entity by primary key.

- **Parameters:** `id` – Entity ID
- **Returns:** `Task<T>`

#### `GetFirstOrDefaultAsync<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true, Expression<Func<T, TResult>>? selector = null, bool asNoTracking = true, bool ignoreQueryFilters = true)`

Retrieves the first entity matching a filter or projection.

- **Parameters:**
  - `filter` – Optional filter expression
  - `include` – Optional include expression
  - `orderBy` – Optional ordering expression
  - `ascending` – Sort direction
  - `selector` – Projection expression
  - `asNoTracking` – Track changes in EF (default: true)
  - `ignoreQueryFilters` – Ignore global query filters (default: true)
- **Returns:** `Task<TResult?>`

#### `GetPaginatedAsync<TResult>(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true, Expression<Func<T, TResult>> selector = null)`

Retrieves paginated data using EF Core.

- **Parameters:**
  - `pageIndex` – Page number
  - `pageSize` – Number of items per page
  - `filter` – Optional filter
  - `include` – Optional includes
  - `orderBy` – Optional ordering
  - `ascending` – Sort direction
  - `selector` – Projection
- **Returns:** `PaginatedList<TResult>`

---

### Update

#### `Update(T entity)`

Updates a single entity.

- **Parameters:** `entity` – Entity to update

#### `UpdateRange(IEnumerable<T> entities)`

Updates multiple entities.

- **Parameters:** `entities` – Collection of entities

---

### Delete

#### `Delete(T entity)`

Deletes a single entity.

- **Parameters:** `entity` – Entity to delete

#### `DeleteRange(IEnumerable<T> entities)`

Deletes multiple entities.

- **Parameters:** `entities` – Collection of entities

#### `Delete(object id)`

Deletes an entity by primary key.

- **Parameters:** `id` – Entity ID

---

### Notes

- Dapper is primarily used for **read operations**; inserts/updates/deletes are generally handled by EF Core.
- All EF Core read queries default to `AsNoTracking` to improve performance.
- Global query filters can be ignored optionally in EF Core queries.
