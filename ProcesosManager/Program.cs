using System;
using ProcesoManager;

namespace ProcesosManager
{
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

            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch { }

            bool salir = false;

            while (!salir)
            {
                Ui.Titulo("Administrador de Procesos", $"{Environment.MachineName} · {DateTime.Now:dd/MM/yyyy HH:mm}");
                Ui.Opcion("1", "Listar procesos");
                Ui.Opcion("2", "Gestionar procesos", "iniciar / finalizar");
                Ui.Opcion("3", "Monitor de métricas", "CPU / memoria");
                Ui.Opcion("4", "Historial y exportación", "JSON");
                Console.WriteLine();
                Ui.Opcion("0", "Salir");

                string? opcion = Ui.Pedir("Seleccione una opción");

                switch (opcion)
                {
                    case "1":
                        ExportarLogs.Registrar("Consulta de lista de procesos", "-");
                        EjecutarListarProcesos();
                        break;
                    case "2":
                        EjecutarGestionarProcesos();
                        break;
                    case "3":
                        ExportarLogs.Registrar("Consulta de monitor de métricas", "-");
                        EjecutarMonitorMetricas();
                        break;
                    case "4":
                        EjecutarExportarLogs();
                        break;
                    case "0":
                    case "5":
                        salir = true;
                        Ui.Info("Saliendo del programa...");
                        break;
                    default:
                        Ui.Error("Opción no válida. Ingrese un número del 0 al 4.");
                        Ui.Pausar();
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
                Ui.Error($"Error al listar procesos: {ex.Message}");
                Ui.Pausar();
            }
        }

        private static void EjecutarGestionarProcesos()
        {
            try
            {
                // Integrante 3: GestionarProcesos.cs (Matar / Iniciar)
                GestionarProcesos.Gestionar();
            }
            catch (Exception ex)
            {
                Ui.Error($"Error al gestionar procesos: {ex.Message}");
                Ui.Pausar();
            }
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
                Ui.Error($"Error en monitor de métricas: {ex.Message}");
                Ui.Pausar();
            }
        }

        private static void EjecutarExportarLogs()
        {
            try
            {
                // Integrante 5: ExportarLogs.cs (Historial / Exportar a archivo)
                ExportarLogs.Gestionar();
            }
            catch (Exception ex)
            {
                Ui.Error($"Error al exportar logs: {ex.Message}");
                Ui.Pausar();
            }
        }
    }
}
