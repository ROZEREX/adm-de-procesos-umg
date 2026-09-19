using System;
using System.Diagnostics;

namespace ProcesoManager {
	public class ListarProcesos {
		public static void  MostrarProcesos() {
			Console.Clear();
			Console.WriteLine("=== LISTA DE PROCESOS ACTIVOS ===");
            		Console.WriteLine("{0,-6} | {1,-30} | {2,-15}", "PID", "Nombre del Proceso", "Memoria (MB)");
           		Console.WriteLine(new string('-', 60));

			Process[] procesos = Process.GetProcesses();

			foreach (var p in procesos) {
				try {
	                 	   double memoriaMb = p.WorkingSet64 / (1024 * 1024);
               			   Console.WriteLine("{0,-6} | {1,-30} | {2,-15:N2}", p.Id, p.ProcessName, memoriaMb);
				}
				catch (Exception){
                	   	   Console.WriteLine("{0,-6} | {1,-30} | {2,-15}", p.Id, p.ProcessName, "Acceso Denegado");
                		}
            		}
	
            		Console.WriteLine("\nPresiona una tecla para volver...");
			Console.ReadKey();
		}
	}
}
