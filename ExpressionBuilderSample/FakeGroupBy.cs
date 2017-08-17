namespace ExpressionBuilderSample
{
    class FakeGroupBy
    {
        public FakeGroupBy(string competencia, string cnpjEmpresaCliente)
        {
            Competencia = competencia;
            CnpjEmpresaCliente = cnpjEmpresaCliente;
        }

        public string Competencia { get; set; }
        public string CnpjEmpresaCliente { get; set; }
        public string Ano { get; set; }
    }
}