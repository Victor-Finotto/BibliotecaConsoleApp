using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Repositorios;

public class EmprestimoServico : IRepositorio<Emprestimo>
{
    private readonly EmprestimoRepositorio _emprestimoRepositorio;

    public EmprestimoServico(EmprestimoRepositorio emprestimoRepositorio)
    {
        _emprestimoRepositorio = emprestimoRepositorio;
    }

    public void Adicionar(Emprestimo emprestimo)
    {
        ValidarEmprestimoParaCadastro(emprestimo);
        _emprestimoRepositorio.Adicionar(emprestimo);
    }

    public void Remover(int id)
    {
        if (id <= 0)
            throw new Exception("ID INVÁLIDO. O ID DEVE SER MAIOR QUE ZERO.");

        _emprestimoRepositorio.Remover(id);
    }

    public void Atualizar(Emprestimo emprestimoAtualizado)
    {
        var emprestimoExistente = _emprestimoRepositorio.BuscarPorId(emprestimoAtualizado.Id);

        if (emprestimoExistente == null)
            throw new Exception("EMPRÉSTIMO NÃO ENCONTRADO.");

        if (emprestimoAtualizado.Id != emprestimoExistente.Id)
            throw new Exception("O ID DO EMPRÉSTIMO NÃO PODE SER ALTERADO.");

        ValidarEmprestimoParaCadastro(emprestimoAtualizado);

        emprestimoExistente.IdUsuario = emprestimoAtualizado.IdUsuario;
        emprestimoExistente.IdLivro = emprestimoAtualizado.IdLivro;
        emprestimoExistente.DataEmprestimo = emprestimoAtualizado.DataEmprestimo;
        emprestimoExistente.DataDevolucao = emprestimoAtualizado.DataDevolucao;
        emprestimoExistente.DataPrevistaDevolucao = emprestimoAtualizado.DataPrevistaDevolucao;
        emprestimoExistente.Devolvido = emprestimoAtualizado.Devolvido;

        _emprestimoRepositorio.Atualizar(emprestimoExistente);
    }

    public Emprestimo? BuscarPorId(int id)
    {
        var emprestimo = _emprestimoRepositorio.BuscarPorId(id);

        if (emprestimo == null)
            throw new Exception("EMPRÉSTIMO NÃO ENCONTRADO.");

        return emprestimo;
    }

    public List<Emprestimo> ListarTodos()
    {
        return _emprestimoRepositorio.ListarTodos();
    }

    private void ValidarEmprestimoParaCadastro(Emprestimo emprestimo)
    {
        if (emprestimo.Id <= 0)
            throw new Exception("ID INVÁLIDO. O ID DEVE SER MAIOR QUE ZERO.");

        if (emprestimo.IdUsuario <= 0)
            throw new Exception("USUÁRIO INVÁLIDO.");

        if (emprestimo.IdLivro <= 0)
            throw new Exception("LIVRO INVÁLIDO.");

        if (emprestimo.DataEmprestimo == default)
            throw new Exception("DATA DE EMPRÉSTIMO INVÁLIDA.");

        if (emprestimo.DataPrevistaDevolucao == default)
            throw new Exception("DATA PREVISTA DE DEVOLUÇÃO INVÁLIDA.");

        if (emprestimo.DataPrevistaDevolucao < emprestimo.DataEmprestimo)
            throw new Exception("A DATA PREVISTA DE DEVOLUÇÃO DEVE SER POSTERIOR À DATA DO EMPRÉSTIMO.");
    }
}
