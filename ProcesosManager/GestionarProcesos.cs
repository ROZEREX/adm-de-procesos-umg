using System;
using System.Diagnostics;

namespace ProcesoManager
{
    public class GestionarProcesos
    {
        public static void Gestionar()
        {
            int opcion;

            do
            {
                Console.Clear();

                Console.WriteLine("=== GESTIONAR PROCESOS ===");
                Console.WriteLine("1. Iniciar proceso");
                Console.WriteLine("2. Matar proceso");
                Console.WriteLine("3. Volver");
                Console.WriteLine("===========================");
                Console.Write("Seleccione una opcion: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                switch (opcion)
                {
                    case 1:
                        IniciarProceso();
                        break;

                    case 2:
                        MatarProceso();
                        break;

                    case 3:
                        break;

                    default:
                        Console.WriteLine("Opcion no valida.");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 3);
        }

        public static void IniciarProceso()
        {
            Console.Clear();

            Console.WriteLine("=== INICIAR PROCESO ===");
            Console.Write("Ingrese el nombre o ruta del programa (ej: notepad, calc, chrome): ");

            string nombre = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío.");
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
                    
                    // Si tienes el método de Logs disponible:
                    // ProcesosManager.ExportarLogs.Registrar("Proceso iniciado", nombre);

                    Console.WriteLine();
                    Console.WriteLine("Proceso iniciado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("No se pudo iniciar el proceso.");
                    Console.WriteLine($"Detalle del error: {ex.Message}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Presiona una tecla para volver...");
            Console.ReadKey();
        }

        public static void MatarProceso()
        {
            Console.Clear();

            Console.WriteLine("=== MATAR PROCESO ===");
            Console.Write("Ingrese el PID del proceso: ");

            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                try
                {
                    Process proceso = Process.GetProcessById(pid);

                    Console.WriteLine();
                    Console.WriteLine("Proceso encontrado: " + proceso.ProcessName);
                    Console.Write("¿Desea finalizarlo? (S/N): ");

                    string respuesta = Console.ReadLine()?.Trim();

                    if (respuesta?.Equals("S", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        string nombreProceso = proceso.ProcessName;

                        // Se utiliza 'entireProcessTree: true' para matar el proceso y sus subprocesos
                        proceso.Kill(entireProcessTree: true);

                        // Si tienes el método de Logs disponible:
                        // ProcesosManager.ExportarLogs.Registrar("Proceso finalizado", $"{nombreProceso} (PID {pid})");

                        Console.WriteLine();
                        Console.WriteLine("Proceso finalizado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Operacion cancelada.");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine();
                    Console.WriteLine("No existe un proceso activo con ese PID.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("No se pudo finalizar el proceso.");
                    Console.WriteLine($"Detalle del error: {ex.Message}");
                    Console.WriteLine("Asegúrese de ejecutar la aplicación como Administrador.");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("El PID ingresado no es valido.");
            }

            Console.WriteLine();
            Console.WriteLine("Presiona una tecla para volver...");
            Console.ReadKey();
        }
    }
}