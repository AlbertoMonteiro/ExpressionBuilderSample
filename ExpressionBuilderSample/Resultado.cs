namespace ExpressionBuilderSample
{
    public class Resultado
    {
        public Resultado(string competencia, string cnpjEmpresaCliente, string razaoSocial, int valor)
        {
            Competencia = competencia;
            CnpjEmpresaCliente = cnpjEmpresaCliente;
            RazaoSocial = razaoSocial;
            Valor = valor;
        }

        public Resultado()
        {

        }

        public string Competencia { get; set; }
        public string CnpjEmpresaCliente { get; set; }
        public string RazaoSocial { get; set; }
        public int Valor { get; set; }
    }
}