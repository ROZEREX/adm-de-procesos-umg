# Administrador de Procesos

Aplicación de consola en **C# / .NET 10** para listar, iniciar, finalizar y monitorear los procesos del sistema (Linux y Windows).
Proyecto del curso — Tarea 11.4.2 *Software Administración de Procesos*, Universidad Mariano Gálvez de Guatemala.

## Integrantes y aportes

| No. | Integrante | GitHub | Módulo | Archivo(s) |
|----|-----------|--------|--------|-----------|
| 1 | Andres Barrios (Coordinador) | [rozerex](https://github.com/rozerex) | Menú principal e integración | `Program.cs`, `ProcesosManager.csproj`, `ProcesosManager.slnx`, `.gitignore` |
| 2 | Sebastian Galvez | [SebsRodriguez12](https://github.com/SebsRodriguez12) | Listar procesos | `ListarProcesos.cs` |
| 3 | David Tuyuc | [DavidTuyuc](https://github.com/DavidTuyuc) | Gestionar procesos (iniciar / matar) | `GestionarProcesos.cs` |
| 4 | Jaime Reyes | [jaimereyes1705-ux](https://github.com/jaimereyes1705-ux) | Monitor de métricas (CPU / memoria) | `MonitorMtricas.cs` |
| 5 | Sebastian Chavarria | [SebastianCH22](https://github.com/SebastianCH22) | Exportar logs (historial / archivo) y manual de usuario | `ExportarLogs.cs` |

Cada archivo `.cs` indica en su encabezado el integrante responsable. La autoría también se puede verificar en el historial de commits.

## Funciones

1. **Listar procesos** — PID, nombre y memoria de cada proceso activo.
2. **Gestionar procesos** — iniciar un programa por nombre o finalizar un proceso por PID (con confirmación).
3. **Monitor de métricas** — consumo de CPU y memoria: tabla única, tiempo real (top 15) o por PID.
4. **Exportar logs** — historial de acciones y métricas a un archivo.
5. **Salir**

## Requisitos

- Linux (probado) o Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Recomendado: ejecutar como Administrador para ver y finalizar procesos del sistema.

## Ejecución

```bash
git clone https://github.com/ROZEREX/adm-de-procesos-umg.git
cd adm-de-procesos-umg/ProcesosManager
dotnet run
```

O abrir `ProcesosManager.slnx` en Visual Studio y presionar **F5**.

## Estructura

```
ProcesosManager.slnx
ProcesosManager/
├── Program.cs             # Menú principal (Integrante 1)
├── ListarProcesos.cs      # Opción 1 (Integrante 2)
├── GestionarProcesos.cs   # Opción 2 (Integrante 3)
├── MonitorMtricas.cs      # Opción 3 (Integrante 4)
└── ExportarLogs.cs        # Opción 4 (Integrante 5)
```
