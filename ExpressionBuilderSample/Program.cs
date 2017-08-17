using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Remove
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string cnpj = "1232";

            var type = typeof(FakeGroupBy);
            var typeT = typeof(Totalizador);

            var parameter = Parameter(typeT, "c");

            var newExpression = New(type.GetConstructors()[0],
                new[] { Property(parameter, "Competencia"), Property(parameter, "CnpjEmpresaCliente") },
                type.GetProperty("Competencia"), type.GetProperty("CnpjEmpresaCliente"));
            var groupBy = Lambda<Func<Totalizador, FakeGroupBy>>(newExpression, new[] { parameter });


            var type1 = typeof(IGrouping<FakeGroupBy, Totalizador>);
            var parameter2 = Parameter(type1, "t");

            var metodoMax = GetMethod<IGrouping<FakeGroupBy, Totalizador>, string>(t => t.Max(r => r.RazaoSocial));
            var sumMethod = GetMethod<IGrouping<FakeGroupBy, Totalizador>, int>(t => t.Sum(r => r.QtdDocumentos));

            var type2 = typeof(Resultado);
            var constructorInfo = type2.GetConstructors()[0];
            var getRazaoSocial = Lambda<Func<Totalizador, string>>(Property(parameter, "RazaoSocial"), parameter);
            var getQtdDocumentos = Lambda<Func<Totalizador, int>>(Property(parameter, "QtdDocumentos"), parameter);
            var maxRazaoSocial = Call(null, metodoMax, parameter2, getRazaoSocial);
            var sumQtdDocumentos = Call(null, sumMethod, parameter2, getQtdDocumentos);
            var newExpression2 = New(constructorInfo,
                new Expression[]
                {
                    Property(Property(parameter2, "Key"), "Competencia"),
                    Property(Property(parameter2, "Key"), "CnpjEmpresaCliente"),
                    maxRazaoSocial,
                    sumQtdDocumentos
                },
                new[]
                {
                    type2.GetProperty("Competencia"),
                    type2.GetProperty("CnpjEmpresaCliente"),
                    type2.GetProperty("RazaoSocial"),
                    type2.GetProperty("Valor")
                }
            );

            var select = Lambda<Func<IGrouping<FakeGroupBy, Totalizador>, Resultado>>(newExpression2, parameter2);


            var queryable = AsQueryable()
                .Where(c => c.CnpjEmpresaContabil == cnpj)
                .GroupBy(groupBy)
                .Select(select);
            foreach (var VARIABLE in queryable)
            {
                Console.WriteLine($"{VARIABLE.Valor} - {VARIABLE.RazaoSocial} - {VARIABLE.CnpjEmpresaCliente} - {VARIABLE.Competencia}");
            }
        }

        private static MethodInfo GetMethod<T, TResult>(Expression<Func<T, TResult>> exp)
        {
            if (exp.Body is MethodCallExpression call)
            {
                return call.Method;
            }
            return null;
        }

        private static IQueryable<Totalizador> AsQueryable()
        {
            return Enumerable.Range(1, 2).Select(r => new Totalizador
            {
                CnpjEmpresaContabil = "1232",
                CnpjEmpresaCliente = "1232",
                Competencia = "2017-08",
                RazaoSocial = "Ola",
                QtdDocumentos = r * 8
            }).AsQueryable();
        }
    }

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
    class Totalizador
    {
        public string Estabelecimento { get; set; }
        public int QtdDocumentos { get; set; }
        public decimal? ValorDocumentos { get; set; }
        public string Especie { get; set; }
        public string Origem { get; set; }
        public string Operacao { get; set; }
        public string Uf { get; set; }
        public string EmpresaContabilBancoGuid { get; set; }
        public string CnpjEmpresaContabil { get; set; }
        public string Guid { get; set; }
        public string Competencia { get; set; }
        public string CnpjEmpresaCliente { get; set; }
        public string RazaoSocial { get; set; }
        public string Nome { get; set; }
    }
}
