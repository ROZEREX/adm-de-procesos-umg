using System;
using ProcesoManager;

namespace ProcesosManager
{
    /// <summary>
    /// Integrante 1: Setup inicial + Menú principal (Program.cs)
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Title = "Administrador de Procesos";
                }
            }
            catch { }

            bool salir = false;

            while (!salir)
            {
                SafeClear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================================");
                Console.WriteLine("             ADMINISTRADOR DE PROCESOS            ");
                Console.WriteLine("==================================================");
                Console.ResetColor();
                Console.WriteLine(" 1. Listar Procesos");
                Console.WriteLine(" 2. Gestionar Procesos (Matar / Iniciar)");
                Console.WriteLine(" 3. Monitor de Métricas (Consumo CPU / Memoria)");
                Console.WriteLine(" 4. Exportar Logs (Historial / Exportar a archivo)");
                Console.WriteLine(" 5. Salir");
                Console.WriteLine("──────────────────────────────────────────────────");
                Console.Write(" Seleccione una opción: ");

                string? opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        EjecutarListarProcesos();
                        break;
                    case "2":
                        EjecutarGestionarProcesos();
                        break;
                    case "3":
                        EjecutarMonitorMetricas();
                        break;
                    case "4":
                        EjecutarExportarLogs();
                        break;
                    case "5":
                        salir = true;
                        Console.WriteLine("\nSaliendo del programa...");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOpción no válida. Ingrese un número del 1 al 5.");
                        Console.ResetColor();
                        Pausar();
                        break;
                }
            }
        }

        private static void EjecutarListarProcesos()
        {
            try
            {
                // Integrante 2: ListarProcesos.cs
                ListarProcesos.MostrarProcesos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al listar procesos: {ex.Message}");
                Pausar();
            }
        }

        private static void EjecutarGestionarProcesos()
        {
            // Integrante 3: GestionarProcesos.cs (Matar / Iniciar)
            Console.WriteLine("\n[Módulo: Gestionar Procesos - Integrante 3 en desarrollo]");
            Pausar();
        }

        private static void EjecutarMonitorMetricas()
        {
            try
            {
                // Integrante 4: MonitorMetricas.cs
                MonitorMetricas.MostrarMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError en monitor de métricas: {ex.Message}");
                Pausar();
            }
        }

        private static void EjecutarExportarLogs()
        {
            // Integrante 5: ExportarLogs.cs (Historial / Exportar a archivo)
            Console.WriteLine("\n[Módulo: Exportar Logs - Integrante 5 en desarrollo]");
            Pausar();
        }

        private static void SafeClear()
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

        private static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
            }
            catch { }
        }
    }
}
