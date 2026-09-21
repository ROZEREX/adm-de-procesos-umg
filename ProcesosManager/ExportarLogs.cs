using System;
using System.Collections.Generic;
using System.IO;

namespace ProcesosManager
{
    /// <summary>
    /// Representa una acción realizada dentro del administrador de procesos.
    /// </summary>
    public class RegistroLog
    {
        public DateTime FechaHora { get; set; }
        public string Accion { get; set; }
        public string Proceso { get; set; }

        public RegistroLog(string accion, string proceso)
        {
            FechaHora = DateTime.Now;
            Accion = accion;
            Proceso = proceso;
        }

        public override string ToString()
        {
            return $"[{FechaHora:dd/MM/yyyy HH:mm:ss}] {Accion} - Proceso: {Proceso}";
        }
    }

    /// <summary>
    /// Integrante 5: Historial y exportación de registros.
    /// </summary>
    public static class ExportarLogs
    {
        private static readonly List<RegistroLog> historial = new List<RegistroLog>();

        /// <summary>
        /// Registra una acción realizada sobre un proceso.
        /// </summary>
        public static void Registrar(string accion, string proceso)
        {
            historial.Add(new RegistroLog(accion, proceso));
        }

        /// <summary>
        /// Menú principal del módulo de Exportar Logs.
        /// </summary>
        public static void Gestionar()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();

                Console.WriteLine("==================================================");
                Console.WriteLine("              EXPORTAR LOGS");
                Console.WriteLine("==================================================");
                Console.WriteLine(" 1. Mostrar historial");
                Console.WriteLine(" 2. Exportar historial a archivo");
                Console.WriteLine(" 3. Limpiar historial");
                Console.WriteLine(" 4. Volver al menú principal");
                Console.WriteLine("──────────────────────────────────────────────────");
                Console.Write(" Seleccione una opción: ");

                string? opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        MostrarHistorial();
                        Pausar();
                        break;

                    case "2":
                        ExportarArchivo();
                        Pausar();
                        break;

                    case "3":
                        LimpiarHistorial();
                        Pausar();
                        break;

                    case "4":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("\nOpción no válida.");
                        Pausar();
                        break;
                }
            }
        }

        /// <summary>
        /// Muestra todos los registros almacenados.
        /// </summary>
        public static void MostrarHistorial()
        {
            Console.Clear();

            Console.WriteLine("==================================================");
            Console.WriteLine("              HISTORIAL DE PROCESOS");
            Console.WriteLine("==================================================");
            Console.WriteLine();

            if (historial.Count == 0)
            {
                Console.WriteLine("No hay acciones registradas.");
                return;
            }

            foreach (RegistroLog registro in historial)
            {
                Console.WriteLine(registro);
            }

            Console.WriteLine();
            Console.WriteLine($"Total de registros: {historial.Count}");
        }

        /// <summary>
        /// Exporta el historial a un archivo de texto.
        /// </summary>
        public static void ExportarArchivo()
        {
            string nombreArchivo = "historial_procesos.txt";

            try
            {
                using (StreamWriter escritor = new StreamWriter(nombreArchivo))
                {
                    escritor.WriteLine("==================================================");
                    escritor.WriteLine("              HISTORIAL DE PROCESOS");
                    escritor.WriteLine("==================================================");
                    escritor.WriteLine();
                    escritor.WriteLine(
                        $"Fecha de exportación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                    );
                    escritor.WriteLine();

                    if (historial.Count == 0)
                    {
                        escritor.WriteLine("No hay acciones registradas.");
                    }
                    else
                    {
                        foreach (RegistroLog registro in historial)
                        {
                            escritor.WriteLine(registro);
                        }

                        escritor.WriteLine();
                        escritor.WriteLine(
                            $"Total de registros: {historial.Count}"
                        );
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Historial exportado correctamente.");
                Console.WriteLine($"Archivo: {nombreArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error al exportar el historial.");
                Console.WriteLine($"Detalle: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina todos los registros almacenados.
        /// </summary>
        public static void LimpiarHistorial()
        {
            historial.Clear();

            Console.WriteLine();
            Console.WriteLine("Historial limpiado correctamente.");
        }

        /// <summary>
        /// Devuelve una copia del historial actual.
        /// </summary>
        public static List<RegistroLog> ObtenerHistorial()
        {
            return new List<RegistroLog>(historial);
        }

        /// <summary>
        /// Pausa la ejecución hasta que el usuario presione una tecla.
        /// </summary>
        private static void Pausar()
        {
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");

            try
            {
                Console.ReadKey(true);
            }
            catch
            {
                Console.ReadLine();
            }
        }
    }
}