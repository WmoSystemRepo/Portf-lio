# Interceptors

Esta pasta contém interceptadores (`HttpInterceptor`) usados para interceptar as requisições HTTP da aplicação.

## AuthInterceptor

- **Arquivo:** `auth.interceptor.ts`
- **Responsável por:**
  - Adicionar o token de autenticação (`authToken`) no header de cada requisição HTTP.
  - Padrão do header:
    ```http
    Authorization: Bearer <token>
    ```

- **Fluxo:**
  1. O `LoginComponent` salva o token no `localStorage` com a chave `authToken`.
  2. O `AuthInterceptor` lê o `authToken` e adiciona automaticamente o header `Authorization` em todas as requisições HTTP.

- **Registro:**
  O interceptor é registrado globalmente no `AppModule`:
  
  ```typescript
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
