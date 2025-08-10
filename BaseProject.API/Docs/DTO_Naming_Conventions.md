# DTO Naming Conventions in C#

When defining Data Transfer Objects (DTOs) in C#, use **PascalCase** naming style with the suffix `Dto`. For clarity, differentiate between incoming request DTOs and outgoing response DTOs by including `Request` or `Response` in the class name.

---

## General Rule

- Use `Dto` suffix with PascalCase  
  Example: `UserDto`, `OrderDto`

---

## Incoming Request DTO

- **Naming pattern:**  
  `EntityRequestDto` or `ActionEntityRequestDto`

- **Purpose:** Represents data sent **to** your API or method.

- **Examples:**

  ```csharp
  public class CreateUserRequestDto
  {
      public string Name { get; set; }
      public string Email { get; set; }
  }

  public class UpdateOrderRequestDto
  {
      public int OrderId { get; set; }
      public string Status { get; set; }
  }
  ```

---

## Outgoing Response DTO

- **Naming pattern:**  
  `EntityResponseDto` or `DetailedEntityResponseDto`

- **Purpose:** Represents data sent **from** your API or method.

- **Examples:**

  ```csharp
  public class UserResponseDto
  {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
  }

  public class OrderDetailsResponseDto
  {
      public int OrderId { get; set; }
      public string Status { get; set; }
      public DateTime CreatedAt { get; set; }
  }
  ```

---

## Optional: Simple DTO

- For simple cases, when usage is clear, you can use a single `EntityDto` for both request and response.

- **Note:**  
  Not recommended for large projects or APIs with multiple DTO variations.

---

## Summary Table

| DTO Type        | Example Name                | Purpose                          |
|-----------------|-----------------------------|----------------------------------|
| Request DTO     | `CreateUserRequestDto`      | Data received by the API         |
| Response DTO    | `UserResponseDto`           | Data returned by the API         |
| Simple DTO      | `UserDto`                   | Shared request/response DTO      |