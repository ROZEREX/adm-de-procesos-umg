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
            Console.Write("Ingrese el nombre del programa: ");

            string nombre = Console.ReadLine();

            try
            {
                Process.Start(nombre);
                ProcesosManager.ExportarLogs.Registrar("Proceso iniciado", nombre);

                Console.WriteLine();
                Console.WriteLine("Proceso iniciado correctamente.");
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("No se pudo iniciar el proceso.");
                Console.WriteLine("Verifique que el nombre del programa sea correcto.");
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

                    string respuesta = Console.ReadLine();

                    if (respuesta.ToUpper() == "S")
                    {
                        string nombreProceso = proceso.ProcessName;
                        proceso.Kill();
                        ProcesosManager.ExportarLogs.Registrar("Proceso finalizado", $"{nombreProceso} (PID {pid})");

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
                    Console.WriteLine("No existe un proceso con ese PID.");
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("No se pudo finalizar el proceso.");
                    Console.WriteLine("Es posible que no tenga permisos suficientes.");
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