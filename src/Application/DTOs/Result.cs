namespace Application.DTOs
{  
    public class Result<T> where T : class
    {
        public T Dados { get; set; }
        public string Mensagem { get; set; }
    }
}
