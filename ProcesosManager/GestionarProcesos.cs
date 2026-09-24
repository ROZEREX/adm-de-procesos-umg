using System;
using System.Diagnostics;
using ProcesosManager;

namespace ProcesoManager
{
    public class GestionarProcesos
    {
        public static void Gestionar()
        {
            bool salir = false;

            while (!salir)
            {
                Ui.Titulo("Gestionar procesos");
                Ui.Opcion("1", "Iniciar proceso");
                Ui.Opcion("2", "Matar proceso", "por PID");
                Console.WriteLine();
                Ui.Opcion("0", "Volver");

                string? opcion = Ui.Pedir("Seleccione una opción");

                switch (opcion)
                {
                    case "1":
                        IniciarProceso();
                        break;

                    case "2":
                        MatarProceso();
                        break;

                    case "0":
                    case "3":
                        salir = true;
                        break;

                    default:
                        Ui.Error("Opción no válida.");
                        Ui.Pausar();
                        break;
                }
            }
        }

        public static void IniciarProceso()
        {
            Ui.Titulo("Iniciar proceso", "Nombre o ruta del programa (ej: notepad, calc, chrome)");

            string? nombre = Ui.Pedir("Programa");

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Ui.Aviso("El nombre no puede estar vacío.");
            }
            else
            {
                try
                {
                    // Configurar ProcessStartInfo para usar el Shell del sistema operativo
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = nombre,
                        UseShellExecute = true // Permite abrir programas usando el Shell (ej. 'notepad', rutas locales o URLs)
                    };

                    Process.Start(psi);

                    ExportarLogs.Registrar("Proceso iniciado", nombre);

                    Ui.Exito($"Proceso '{nombre}' iniciado correctamente.");
                }
                catch (Exception ex)
                {
                    Ui.Error("No se pudo iniciar el proceso.");
                    Ui.Dato("Detalle:", ex.Message);
                }
            }

            Ui.Pausar();
        }

        public static void MatarProceso()
        {
            Ui.Titulo("Matar proceso");

            if (int.TryParse(Ui.Pedir("PID del proceso"), out int pid))
            {
                try
                {
                    Process proceso = Process.GetProcessById(pid);

                    Console.WriteLine();
                    Ui.Dato("Proceso:", proceso.ProcessName);
                    Ui.Dato("PID:", pid.ToString());

                    string? respuesta = Ui.Pedir("¿Desea finalizarlo? (S/N)");

                    if (respuesta?.Equals("S", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        string nombreProceso = proceso.ProcessName;

                        // Se utiliza 'entireProcessTree: true' para matar el proceso y sus subprocesos
                        proceso.Kill(entireProcessTree: true);

                        ExportarLogs.Registrar("Proceso finalizado", $"{nombreProceso} (PID {pid})");

                        Ui.Exito("Proceso finalizado correctamente.");
                    }
                    else
                    {
                        Ui.Aviso("Operación cancelada.");
                    }
                }
                catch (ArgumentException)
                {
                    Ui.Error("No existe un proceso activo con ese PID.");
                }
                catch (Exception ex)
                {
                    Ui.Error("No se pudo finalizar el proceso.");
                    Ui.Dato("Detalle:", ex.Message);
                    Ui.Info("Asegúrese de ejecutar la aplicación como Administrador.");
                }
            }
            else
            {
                Ui.Error("El PID ingresado no es válido.");
            }

            Ui.Pausar();
        }
    }
}
