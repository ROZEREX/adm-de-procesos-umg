using System;

namespace ProcesosManager
{
    // Helpers visuales compartidos por todos los módulos.
    public static class Ui
    {
        private const int Ancho = 58;

        public static void Titulo(string titulo, string? subtitulo = null)
        {
            titulo = Recortar(titulo, Ancho - 1);
            subtitulo = subtitulo == null ? null : Recortar(subtitulo, Ancho - 1);

            Limpiar();
            Console.WriteLine();
            Escribir("  ┌" + new string('─', Ancho) + "┐\n", ConsoleColor.DarkCyan);
            Escribir("  │ ", ConsoleColor.DarkCyan);
            Escribir(titulo.ToUpper().PadRight(Ancho - 1), ConsoleColor.Cyan);
            Escribir("│\n", ConsoleColor.DarkCyan);
            if (!string.IsNullOrEmpty(subtitulo))
            {
                Escribir("  │ ", ConsoleColor.DarkCyan);
                Escribir(subtitulo.PadRight(Ancho - 1), ConsoleColor.DarkGray);
                Escribir("│\n", ConsoleColor.DarkCyan);
            }
            Escribir("  └" + new string('─', Ancho) + "┘\n", ConsoleColor.DarkCyan);
            Console.WriteLine();
        }

        public static void Opcion(string tecla, string texto, string? detalle = null)
        {
            Escribir($"   {tecla} ", ConsoleColor.Yellow);
            Escribir(texto, ConsoleColor.White);
            if (!string.IsNullOrEmpty(detalle))
            {
                Escribir($"  {detalle}", ConsoleColor.DarkGray);
            }
            Console.WriteLine();
        }

        public static string? Pedir(string texto)
        {
            Console.WriteLine();
            Escribir("  › ", ConsoleColor.Cyan);
            Escribir(texto + ": ", ConsoleColor.Gray);
            return Console.ReadLine()?.Trim();
        }

        public static void Exito(string mensaje) => Etiqueta(" OK ", mensaje, ConsoleColor.DarkGreen, ConsoleColor.Green);
        public static void Error(string mensaje) => Etiqueta(" ERROR ", mensaje, ConsoleColor.DarkRed, ConsoleColor.Red);
        public static void Aviso(string mensaje) => Etiqueta(" AVISO ", mensaje, ConsoleColor.DarkYellow, ConsoleColor.Yellow);

        public static void Info(string mensaje)
        {
            Escribir($"  {mensaje}\n", ConsoleColor.DarkGray);
        }

        public static void Dato(string clave, string valor)
        {
            Escribir($"  {clave,-10}", ConsoleColor.DarkGray);
            Escribir($"{valor}\n", ConsoleColor.White);
        }

        public static void EncabezadoTabla(string columnas)
        {
            Escribir($"  {columnas}\n", ConsoleColor.Cyan);
            Escribir("  " + new string('─', columnas.Length) + "\n", ConsoleColor.DarkGray);
        }

        // Color según nivel de consumo: normal, medio o alto.
        public static ConsoleColor ColorNivel(double valor, double medio, double alto)
        {
            if (valor >= alto) return ConsoleColor.Red;
            if (valor >= medio) return ConsoleColor.Yellow;
            return ConsoleColor.Gray;
        }

        public static void Escribir(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(texto);
            Console.ResetColor();
        }

        public static void Pausar()
        {
            Console.WriteLine();
            Escribir("  Presione cualquier tecla para continuar...", ConsoleColor.DarkGray);
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
                else
                {
                    Console.ReadLine();
                }
            }
            catch { }
            Console.WriteLine();
        }

        public static void Limpiar()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch { }
        }

        private static string Recortar(string texto, int max)
        {
            return texto.Length <= max ? texto : texto.Substring(0, max - 1) + "…";
        }

        private static void Etiqueta(string etiqueta, string mensaje, ConsoleColor fondo, ConsoleColor texto)
        {
            Console.WriteLine();
            Console.Write("  ");
            Console.BackgroundColor = fondo;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(etiqueta);
            Console.ResetColor();
            Escribir($" {mensaje}\n", texto);
        }
    }
}
