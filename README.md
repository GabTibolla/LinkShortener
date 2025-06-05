# 🔗 LinkShortener

Encurtador de URLs simples feito com ASP.NET Core (.NET 8+)

## ⚙️ Como usar (passo a passo)

```bash
## 1. Clone o repositório
git clone https://github.com/seu-usuario/LinkShortener.git
cd LinkShortener
```

## 2. Edite a string de conexão do banco de dados em
```bash
-> LinkShortener.API/Config/config.json <-

# Exemplo de conteúdo:
# {
#   "ConnectionString": "Server=localhost;Database=LinkShortener;User Id=sa;Password=sua_senha;"
# }
```

## 🗄️ Banco de Dados

Para adicionar suporte a outros bancos de dados, basta **criar uma nova classe que herde de `LinkShortenerDB.cs`**, localizada no projeto `LinkShortener.DAO`.

Isso permite customizar a lógica de acesso ao banco sem alterar o restante da aplicação.
