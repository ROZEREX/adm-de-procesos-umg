using System;
using System.Diagnostics;
using ProcesosManager;

namespace ProcesoManager {
	public class ListarProcesos {
		public static void MostrarProcesos() {
			Process[] procesos = Process.GetProcesses();

			Ui.Titulo("Procesos activos", $"{procesos.Length} procesos en ejecución");
			Ui.EncabezadoTabla(string.Format("{0,-8}{1,-32}{2,14}", "PID", "Nombre del proceso", "Memoria (MB)"));

			foreach (var p in procesos) {
				try {
					double memoriaMb = p.WorkingSet64 / (1024.0 * 1024.0);
					Ui.Escribir(string.Format("  {0,-8}", p.Id), ConsoleColor.DarkGray);
					Ui.Escribir(string.Format("{0,-32}", Recortar(p.ProcessName, 30)), ConsoleColor.Gray);
					Ui.Escribir(string.Format("{0,14:N2}\n", memoriaMb), Ui.ColorNivel(memoriaMb, 300, 1000));
				}
				catch (Exception) {
					Ui.Escribir(string.Format("  {0,-8}", p.Id), ConsoleColor.DarkGray);
					Ui.Escribir(string.Format("{0,-32}", Recortar(p.ProcessName, 30)), ConsoleColor.Gray);
					Ui.Escribir(string.Format("{0,14}\n", "sin acceso"), ConsoleColor.DarkGray);
				}
			}

			Console.WriteLine();
			Ui.Info("Amarillo: > 300 MB   Rojo: > 1 GB");
			Ui.Pausar();
		}

		private static string Recortar(string texto, int max) {
			return texto.Length <= max ? texto : texto.Substring(0, max - 1) + "…";
		}
	}
}
