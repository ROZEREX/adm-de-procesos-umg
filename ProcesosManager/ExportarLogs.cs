using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ProcesosManager
{
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

    public static class ExportarLogs
    {
        private const string NombreArchivo = "historial_procesos.json";

        private static readonly List<RegistroLog> historial = new List<RegistroLog>();

        private static readonly JsonSerializerOptions opcionesJson = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // Mantiene tildes y ñ legibles en el archivo en lugar de é, etc.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static void Registrar(string accion, string proceso)
        {
            historial.Add(new RegistroLog(accion, proceso));
        }

        public static void Gestionar()
        {
            bool salir = false;

            while (!salir)
            {
                Ui.Titulo("Historial y exportación", $"{historial.Count} registros en memoria");
                Ui.Opcion("1", "Mostrar historial");
                Ui.Opcion("2", "Exportar historial", NombreArchivo);
                Ui.Opcion("3", "Limpiar historial");
                Console.WriteLine();
                Ui.Opcion("0", "Volver al menú principal");

                string? opcion = Ui.Pedir("Seleccione una opción");

                switch (opcion)
                {
                    case "1":
                        MostrarHistorial();
                        Ui.Pausar();
                        break;

                    case "2":
                        ExportarArchivo();
                        Ui.Pausar();
                        break;

                    case "3":
                        LimpiarHistorial();
                        Ui.Pausar();
                        break;

                    case "0":
                    case "4":
                        salir = true;
                        break;

                    default:
                        Ui.Error("Opción no válida.");
                        Ui.Pausar();
                        break;
                }
            }
        }

        public static void MostrarHistorial()
        {
            Ui.Titulo("Historial de procesos");

            if (historial.Count == 0)
            {
                Ui.Aviso("No hay acciones registradas.");
                return;
            }

            Ui.EncabezadoTabla($"{"Fecha y hora",-21}{"Acción",-34}Proceso");

            foreach (RegistroLog registro in historial)
            {
                Ui.Escribir($"  {registro.FechaHora:dd/MM/yyyy HH:mm:ss}  ", ConsoleColor.DarkGray);
                Ui.Escribir($"{registro.Accion,-34}", ConsoleColor.White);
                Ui.Escribir($"{registro.Proceso}\n", ConsoleColor.Gray);
            }

            Console.WriteLine();
            Ui.Info($"Total de registros: {historial.Count}");
        }

        public static void ExportarArchivo()
        {
            try
            {
                var exportacion = new
                {
                    fechaExportacion = DateTime.Now,
                    equipo = Environment.MachineName,
                    totalRegistros = historial.Count,
                    registros = historial.Select(r => new
                    {
                        fechaHora = r.FechaHora,
                        accion = r.Accion,
                        proceso = r.Proceso
                    })
                };

                File.WriteAllText(NombreArchivo, JsonSerializer.Serialize(exportacion, opcionesJson));

                Ui.Exito($"Historial exportado ({historial.Count} registros).");
                Ui.Dato("Archivo:", Path.GetFullPath(NombreArchivo));
            }
            catch (Exception ex)
            {
                Ui.Error("No se pudo exportar el historial.");
                Ui.Dato("Detalle:", ex.Message);
            }
        }

        public static void LimpiarHistorial()
        {
            historial.Clear();
            Ui.Exito("Historial limpiado correctamente.");
        }

        public static List<RegistroLog> ObtenerHistorial()
        {
            return new List<RegistroLog>(historial);
        }
    }
}
