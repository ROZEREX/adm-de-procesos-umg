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

La autoría y aportes de cada integrante se pueden verificar en la tabla anterior y en el historial de commits de Git.

## Funciones

1. **Listar procesos** — PID, nombre y memoria de cada proceso activo.
2. **Gestionar procesos** — iniciar un programa por nombre o finalizar un proceso por PID (con confirmación).
3. **Monitor de métricas** — consumo de CPU y memoria: tabla única, tiempo real (top 15) o por PID.
4. **Exportar logs** — historial de acciones y métricas a un archivo.
5. **Salir**

## Requisitos

- **Linux** (Ubuntu, Debian, Fedora, WSL2 o cualquier distribución compatible) o **Windows 10/11**
- [.NET SDK](https://dotnet.microsoft.com/download) (.NET 10 o .NET 8 LTS)
- **Permisos de administrador / root (`sudo`)**: recomendado en Linux para poder consultar métricas y finalizar procesos de otros usuarios o del sistema.

## Ejecución en Linux

### 1. Instalar .NET SDK en Linux (Ubuntu / Debian / WSL)
```bash
sudo apt update
sudo apt install -y dotnet-sdk-8.0
# O si tu distribución tiene disponible .NET 10:
# sudo apt install -y dotnet-sdk-10.0
```

### 2. Clonar o acceder al proyecto
```bash
# Si descargas el repositorio:
git clone https://github.com/ROZEREX/adm-de-procesos-umg.git
cd adm-de-procesos-umg/ProcesosManager

# Si usas WSL (Ubuntu en Windows) y los archivos están en C:\:
# cd /mnt/c/MAMP/htdocs/procesos-umg/ProcesosManager
```

### 3. Compilar y ejecutar
```bash
dotnet build
dotnet run

# O con permisos de superusuario para gestionar procesos del sistema:
sudo dotnet run
```

## Ejecución en Windows

1. **Método rápido (un solo clic):**
   - Haz doble clic en el archivo `Ejecutar.bat` ubicado en la raíz del proyecto.
2. **Desde la terminal (PowerShell o CMD):**
   ```powershell
   cd ProcesosManager
   dotnet run
   ```
3. **Desde Visual Studio:**
   - Abre `ProcesosManager.slnx` en Visual Studio y presiona **F5**.

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
