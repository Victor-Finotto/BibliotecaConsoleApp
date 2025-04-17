# 📚 BibliotecaConsoleApp

Sistema de gerenciamento de biblioteca desenvolvido em C# (.NET), com arquitetura em camadas e execução via Console Application. O projeto visa consolidar conhecimentos em Programação Orientada a Objetos, validação de dados e persistência com arquivos JSON.

---

## ✅ Funcionalidades Implementadas

- ✅ Cadastro, listagem, edição e remoção de **livros**
- ✅ Cadastro, listagem, edição e remoção de **usuários**
- ✅ Registro, atualização, listagem e remoção de **empréstimos**
- ✅ **Console interativo** com feedback e validações
- ✅ **Persistência de dados com arquivos JSON**

---

## 🧾 Funcionalidades Planejadas

- 🔄 Devolução de livros
- 🕘 Histórico de operações (log de ações)
- 🔐 Controle de autenticação e permissões

---

## 🛠 Tecnologias e Conceitos

- `C#` (.NET 9)
- Programação Orientada a Objetos (POO)
- Arquitetura em camadas:
  - Entidades
  - Serviços
  - Repositórios
  - Menus (Apresentação)
  - Interfaces
  - Utils
  - Sessão
- Validação de dados no console
- Persistência com arquivos JSON

---

## 📁 Estrutura de Pastas

```
/Entidades       -> Classes de domínio (Livro, Usuario, Emprestimo)
/Repositorios    -> Camada de acesso a dados (JSON)
/Servicos        -> Regras de negócio e validações
/Apresentacao    -> Menus e formulários de interação com o usuário via console
/Interfaces      -> Contratos genéricos para entidades e repositórios
/Utils           -> Métodos auxiliares (leitura, exibição, validações)
/Sessao          -> Controle de contexto da sessão do usuário
/Data            -> Arquivos de dados persistidos em JSON
Program.cs       -> Ponto de entrada da aplicação
```

---

## 📌 Status do Projeto

🚧 Em desenvolvimento

- 🔹 CRUD completo de livros, usuários e empréstimos ✅
- 🔹 Persistência com arquivos JSON em funcionamento ✅
- 🔜 Devoluções e histórico de atividades em breve

---

## 👨‍💻 Autor

**Victor Finotto**  
Desenvolvedor .NET  
[GitHub - @Victor-Finotto](https://github.com/Victor-Finotto)