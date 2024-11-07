namespace Application.DTOs
{
    public class Result
    {
        public string Mensagem { get; set; }
    }
    public class Result<T> : Result where T : class
    {
        public T Dados { get; set; }
    }
}
