using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ProcesosManager
{
    public class MonitorMetricas
    {
        // Número de núcleos lógicos del equipo, necesario para calcular el % de CPU real.
        private static readonly int NucleosCPU = Environment.ProcessorCount;

        public static void MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Ui.Titulo("Monitor de métricas", $"{NucleosCPU} núcleos lógicos");
                Ui.Opcion("1", "Tabla de consumo", "CPU / memoria, una sola vez");
                Ui.Opcion("2", "Monitoreo en tiempo real", "auto-refresh");
                Ui.Opcion("3", "Monitorear un proceso", "por PID");
                Console.WriteLine();
                Ui.Opcion("0", "Volver al menú principal");

                string? opcion = Ui.Pedir("Seleccione una opción");

                switch (opcion)
                {
                    case "1":
                        MostrarTablaMetricas();
                        Ui.Pausar();
                        break;
                    case "2":
                        MonitorearEnTiempoReal();
                        break;
                    case "3":
                        MonitorearProcesoPorPid();
                        break;
                    case "0":
                    case "4":
                        salir = true;
                        break;
                    default:
                        Ui.Error("Opción inválida.");
                        Ui.Pausar();
                        break;
                }
            }
        }

        public static void MostrarTablaMetricas()
        {
            Ui.Titulo("Consumo de procesos");
            Ui.Info("Calculando uso de CPU (esto toma ~1 segundo)...");

            var metricas = ObtenerMetricasDeTodosLosProcesos(intervaloMs: 500);

            Ui.Titulo("Consumo de procesos", $"{metricas.Count} procesos · ordenado por memoria");
            ImprimirTabla(metricas.OrderByDescending(m => m.MemoriaMB));
        }

        public static void MonitorearEnTiempoReal()
        {
            Ui.Titulo("Monitoreo en tiempo real");
            Ui.Info("Presione cualquier tecla para detener...");
            Thread.Sleep(1000);

            while (!Console.KeyAvailable)
            {
                var metricas = ObtenerMetricasDeTodosLosProcesos(intervaloMs: 500);

                Ui.Titulo("Monitoreo en tiempo real", $"Top 15 por CPU · {DateTime.Now:HH:mm:ss} · tecla para salir");
                ImprimirTabla(metricas.OrderByDescending(x => x.CpuPorcentaje).Take(15));
            }

            Console.ReadKey(true); // consume la tecla que detuvo el bucle
        }

        public static void MonitorearProcesoPorPid()
        {
            Ui.Titulo("Monitorear proceso");

            if (!int.TryParse(Ui.Pedir("PID del proceso a monitorear"), out int pid))
            {
                Ui.Error("PID inválido.");
                Ui.Pausar();
                return;
            }

            try
            {
                using Process proceso = Process.GetProcessById(pid);
                Console.WriteLine();
                Ui.Info($"Monitoreando '{proceso.ProcessName}' (PID {pid}). Calculando...");

                double cpu = ObtenerUsoCPU(proceso, intervaloMs: 1000);
                double memoriaMB = proceso.WorkingSet64 / (1024.0 * 1024.0);

                Console.WriteLine();
                Ui.Dato("Proceso:", proceso.ProcessName);
                Ui.Dato("PID:", proceso.Id.ToString());
                Ui.Escribir($"  {"CPU:",-10}", ConsoleColor.DarkGray);
                Ui.Escribir($"{cpu:F1} %\n", Ui.ColorNivel(cpu, 10, 50));
                Ui.Escribir($"  {"Memoria:",-10}", ConsoleColor.DarkGray);
                Ui.Escribir($"{memoriaMB:F1} MB\n", Ui.ColorNivel(memoriaMB, 300, 1000));
            }
            catch (ArgumentException)
            {
                Ui.Error("No existe ningún proceso activo con ese PID.");
            }
            catch (Exception ex)
            {
                Ui.Error($"No se pudo obtener el acceso al proceso: {ex.Message}");
            }

            Ui.Pausar();
        }

        private static void ImprimirTabla(IEnumerable<MetricaProceso> metricas)
        {
            Ui.EncabezadoTabla($"{"PID",-8}{"Proceso",-30}{"CPU %",8}{"Memoria (MB)",15}");

            foreach (var m in metricas)
            {
                Ui.Escribir($"  {m.Pid,-8}", ConsoleColor.DarkGray);
                Ui.Escribir($"{Truncar(m.Nombre, 28),-30}", ConsoleColor.Gray);
                Ui.Escribir($"{m.CpuPorcentaje,8:F1}", Ui.ColorNivel(m.CpuPorcentaje, 10, 50));
                Ui.Escribir($"{m.MemoriaMB,15:F1}\n", Ui.ColorNivel(m.MemoriaMB, 300, 1000));
            }

            Console.WriteLine();
            Ui.Info("Amarillo: consumo medio   Rojo: consumo alto");
        }

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
    }

    public class MetricaProceso
    {
        public int Pid { get; set; }
        public string Nombre { get; set; } = "";
        public double CpuPorcentaje { get; set; }
        public double MemoriaMB { get; set; }
    }
}
