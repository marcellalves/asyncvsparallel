using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncVsParallel
{
    public class Modelo
    {
        public int Id { get; set; }
        public int Valor1 { get; set; }
        public int Valor2 { get; set; }
    }

    class Program
    {
        private static List<Modelo> _lista;
        private static Stopwatch _sw;

        static void Main(string[] args)
        {
            _lista = new List<Modelo>();
            _sw = new Stopwatch();

            for (var i = 0; i < 20; i++)
            {
                _lista.Add(new Modelo { Id = i });
            }

            AsyncContext.Run(() => MainAsync(args));
        }

        static async Task MainAsync(string[] args)
        {
            _sw.Start();
            //ForEachSimples();
            //_sw.Stop();
            //Console.WriteLine($"Tempo de execução do ForEachSimples: {FormatElapsedTime(_sw.Elapsed)}");

            //_sw.Restart();
            //await ForEachComAsync();
            //_sw.Stop();
            //Console.WriteLine($"Tempo de execução do ForEachComAsync: {FormatElapsedTime(_sw.Elapsed)}");

            //_sw.Restart();
            ForEachParallel();
            _sw.Stop();
            Console.WriteLine($"Tempo de execução do ForEachParallel: {FormatElapsedTime(_sw.Elapsed)}");

            //_sw.Restart();
            //TaskComWhenAll();
            //_sw.Stop();
            //Console.WriteLine($"Tempo de execução do TaskComWhenAll: {FormatElapsedTime(_sw.Elapsed)}");

            foreach (var item in _lista)
            {
                Console.WriteLine($"Id: {item.Id}, Valor1: {item.Valor1}, Valor2: {item.Valor2}");
            }

            Console.WriteLine("Execução terminada.");
            Console.ReadLine();
        }

        private static string FormatElapsedTime(TimeSpan ts)
        {
            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
        }

        private static void ForEachSimples()
        {
            foreach (var item in _lista)
            {
                item.Valor1 = RecuperarValor();
                item.Valor2 = RecuperarValor();
            }
        }

        private static async Task ForEachComAsync()
        {
            foreach (var item in _lista)
            {
                item.Valor1 = await OperacaoCustosaAsync();
                item.Valor2 = await OperacaoCustosaAsync();
            }
        }

        private static void ForEachParallel()
        {
            Parallel.ForEach(_lista, item =>
            {
                item.Valor1 = RecuperarValor();
                item.Valor2 = RecuperarValor();
            });
        }

        //private static async void TaskComWhenAll()
        //{
        //    var tasks = _lista.Select(i => OperacaoCustosaAsync(i)).ToArray();
        //    int[] valores = await Task.WhenAll(tasks);
        //}

        private static async Task OperacaoCustosaAsync(Modelo modelo)
        {
            modelo.Valor1 = await Task.Run(() => RecuperarValor());
            modelo.Valor2 = await Task.Run(() => RecuperarValor());
        }

        private static async Task<int> OperacaoCustosaAsync()
        {
            return await Task.Run(() => RecuperarValor());
        }

        private static int RecuperarValor()
        {
            var r = new Random();
            Thread.Sleep(r.Next(1000, 5000));
            return r.Next(0, 1000);
        }
    }
}
