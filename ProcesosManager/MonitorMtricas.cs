using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ProcesosManager
{
    /// <summary>
    /// Integrante 4: Monitor de Métricas.
    /// Responsable de mostrar el consumo de CPU y memoria de los procesos del sistema.
    /// </summary>
    public class MonitorMetricas
    {
        // Número de núcleos lógicos del equipo, necesario para calcular el % de CPU real.
        private static readonly int NucleosCPU = Environment.ProcessorCount;

        /// <summary>
        /// Punto de entrada del módulo. Program.cs (Integrante 1) llama a este método
        /// desde el menú principal, por ejemplo en la opción "Monitor de métricas".
        /// </summary>
        public static void MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("===== MONITOR DE MÉTRICAS =====");
                Console.WriteLine("1. Ver tabla de consumo (CPU / Memoria) - una sola vez");
                Console.WriteLine("2. Monitoreo en tiempo real (auto-refresh)");
                Console.WriteLine("3. Monitorear un proceso específico por PID");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarTablaMetricas();
                        Pausar();
                        break;
                    case "2":
                        MonitorearEnTiempoReal();
                        break;
                    case "3":
                        MonitorearProcesoPorPid();
                        break;
                    case "4":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        Pausar();
                        break;
                }
            }
        }

        /// <summary>
        /// Muestra una tabla estática (una sola lectura) con PID, Nombre, % CPU y Memoria (MB)
        /// de todos los procesos del sistema, ordenados por consumo de memoria.
        /// </summary>
        public static void MostrarTablaMetricas()
        {
            Console.Clear();
            Console.WriteLine("Calculando uso de CPU (esto toma ~1 segundo)...\n");

            var metricas = ObtenerMetricasDeTodosLosProcesos(intervaloMs: 500);

            Console.WriteLine($"{"PID",-8}{"Proceso",-30}{"CPU %",-10}{"Memoria (MB)",-15}");
            Console.WriteLine(new string('-', 63));

            foreach (var m in metricas.OrderByDescending(m => m.MemoriaMB))
            {
                Console.WriteLine($"{m.Pid,-8}{Truncar(m.Nombre, 28),-30}{m.CpuPorcentaje,-10:F1}{m.MemoriaMB,-15:F1}");
            }

            Console.WriteLine($"\nTotal de procesos: {metricas.Count}");
        }

        /// <summary>
        /// Refresca la tabla de métricas cada cierto intervalo hasta que el usuario presione una tecla.
        /// Similar a un mini "top" / Administrador de tareas.
        /// </summary>
        public static void MonitorearEnTiempoReal()
        {
            Console.Clear();
            Console.WriteLine("Monitoreo en tiempo real. Presione cualquier tecla para detener...\n");
            Thread.Sleep(1000);

            while (!Console.KeyAvailable)
            {
                var metricas = ObtenerMetricasDeTodosLosProcesos(intervaloMs: 500);

                Console.Clear();
                Console.WriteLine("===== MONITOREO EN TIEMPO REAL (Ctrl+C o tecla para salir) =====\n");
                Console.WriteLine($"{"PID",-8}{"Proceso",-30}{"CPU %",-10}{"Memoria (MB)",-15}");
                Console.WriteLine(new string('-', 63));

                foreach (var m in metricas.OrderByDescending(x => x.CpuPorcentaje).Take(15))
                {
                    Console.WriteLine($"{m.Pid,-8}{Truncar(m.Nombre, 28),-30}{m.CpuPorcentaje,-10:F1}{m.MemoriaMB,-15:F1}");
                }

                Console.WriteLine("\n(Mostrando los 15 procesos con mayor uso de CPU)");
            }

            Console.ReadKey(true); // consume la tecla que detuvo el bucle
        }

        /// <summary>
        /// Pide un PID por consola y muestra su consumo de CPU/Memoria de forma puntual.
        /// </summary>
        public static void MonitorearProcesoPorPid()
        {
            Console.Clear();
            Console.Write("Ingrese el PID del proceso a monitorear: ");
            if (!int.TryParse(Console.ReadLine(), out int pid))
            {
                Console.WriteLine("PID inválido.");
                Pausar();
                return;
            }

            try
            {
                using Process proceso = Process.GetProcessById(pid);
                Console.WriteLine($"\nMonitoreando '{proceso.ProcessName}' (PID {pid}). Calculando...");

                double cpu = ObtenerUsoCPU(proceso, intervaloMs: 1000);
                double memoriaMB = proceso.WorkingSet64 / (1024.0 * 1024.0);

                Console.WriteLine($"\nProceso : {proceso.ProcessName}");
                Console.WriteLine($"PID     : {proceso.Id}");
                Console.WriteLine($"CPU %   : {cpu:F1}");
                Console.WriteLine($"Memoria : {memoriaMB:F1} MB");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("No existe ningún proceso activo con ese PID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo obtener el acceso al proceso: {ex.Message}");
            }

            Pausar();
        }

        /// <summary>
        /// Calcula el porcentaje de uso de CPU de un proceso midiendo el tiempo de CPU
        /// consumido entre dos instantes separados por 'intervaloMs' milisegundos.
        /// </summary>
        public static double ObtenerUsoCPU(Process proceso, int intervaloMs = 500)
        {
            try
            {
                TimeSpan cpuInicial = proceso.TotalProcessorTime;
                DateTime tiempoInicial = DateTime.UtcNow;

                Thread.Sleep(intervaloMs);

                proceso.Refresh();
                TimeSpan cpuFinal = proceso.TotalProcessorTime;
                DateTime tiempoFinal = DateTime.UtcNow;

                double cpuUsadoMs = (cpuFinal - cpuInicial).TotalMilliseconds;
                double tiempoTranscurridoMs = (tiempoFinal - tiempoInicial).TotalMilliseconds * NucleosCPU;

                if (tiempoTranscurridoMs <= 0) return 0;

                double porcentaje = (cpuUsadoMs / tiempoTranscurridoMs) * 100.0;
                return Math.Max(0, Math.Min(100, porcentaje));
            }
            catch
            {
                // El proceso pudo haber terminado durante la medición, o no tenemos permisos.
                return 0;
            }
        }

        /// <summary>
        /// Recorre todos los procesos del sistema y calcula su % de CPU y memoria en MB.
        /// Pensado para ser reutilizado por ExportarLogs.cs (Integrante 5) si necesita
        /// guardar un snapshot de métricas en el historial.
        /// </summary>
        public static List<MetricaProceso> ObtenerMetricasDeTodosLosProcesos(int intervaloMs = 500)
        {
            Process[] procesos = Process.GetProcesses();
            var muestraInicial = new Dictionary<int, TimeSpan>();

            foreach (var p in procesos)
            {
                try { muestraInicial[p.Id] = p.TotalProcessorTime; }
                catch { /* proceso sin acceso o ya finalizado */ }
            }

            DateTime t0 = DateTime.UtcNow;
            Thread.Sleep(intervaloMs);
            DateTime t1 = DateTime.UtcNow;
            double msTranscurridos = (t1 - t0).TotalMilliseconds * NucleosCPU;

            var resultado = new List<MetricaProceso>();

            foreach (var p in procesos)
            {
                try
                {
                    p.Refresh();
                    double cpuPorcentaje = 0;

                    if (muestraInicial.TryGetValue(p.Id, out TimeSpan cpuInicial) && msTranscurridos > 0)
                    {
                        double cpuUsadoMs = (p.TotalProcessorTime - cpuInicial).TotalMilliseconds;
                        cpuPorcentaje = Math.Max(0, Math.Min(100, (cpuUsadoMs / msTranscurridos) * 100.0));
                    }

                    resultado.Add(new MetricaProceso
                    {
                        Pid = p.Id,
                        Nombre = p.ProcessName,
                        CpuPorcentaje = cpuPorcentaje,
                        MemoriaMB = p.WorkingSet64 / (1024.0 * 1024.0)
                    });
                }
                catch
                {
                    // Proceso finalizado o sin permisos de acceso (típico en procesos del sistema).
                }
                finally
                {
                    p.Dispose();
                }
            }

            return resultado;
        }

        private static string Truncar(string texto, int maxLargo)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            return texto.Length <= maxLargo ? texto : texto.Substring(0, maxLargo - 1) + "…";
        }

        private static void Pausar()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// Representa una métrica puntual de un proceso. Se expone públicamente para que
    /// otros módulos (por ejemplo ExportarLogs.cs) puedan reutilizar estos datos.
    /// </summary>
    public class MetricaProceso
    {
        public int Pid { get; set; }
        public string Nombre { get; set; }
        public double CpuPorcentaje { get; set; }
        public double MemoriaMB { get; set; }
    }
}
