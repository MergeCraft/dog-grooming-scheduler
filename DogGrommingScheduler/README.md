# Dog Grooming Scheduler

Este repositorio contiene una aplicación de ejemplo con proyectos de backend, lógica de negocio, acceso a datos y un cliente Blazor WebAssembly.

## Gestión de versiones de paquetes (Central Package Management)

Este repositorio utiliza "Central Package Management" para controlar las versiones de paquetes NuGet de forma centralizada.

Qué significa
- Las versiones de paquetes se declaran en `Directory.Packages.props` usando elementos `PackageVersion`.
- Los archivos de proyecto (`*.csproj`) deben declarar las dependencias con `PackageReference Include="..."` sin el atributo `Version`.

Por qué lo hacemos
- Mantener versiones consistentes entre proyectos del repositorio.
- Facilitar actualizaciones en un solo lugar.
- Reducir la duplicación de versiones en múltiples `.csproj`.

Cómo añadir o actualizar un paquete
1. Abrir `Directory.Packages.props` y añadir o actualizar la línea:
   - `<PackageVersion Include="Paquete.Nombre" Version="X.Y.Z" />`
2. En el `.csproj` del proyecto que use el paquete, añadir:
   - `<PackageReference Include="Paquete.Nombre" />`
3. Restaurar paquetes y construir la solución.

Nota sobre herramientas (Visual Studio / dotnet CLI)
- Visual Studio puede intentar añadir la versión en el `.csproj` cuando agregas paquetes desde el diálogo de NuGet. Si la gestión central está habilitada (ver `Directory.Packages.props`), mueve la versión a `Directory.Packages.props` o usa la CLI para editar ambos archivos.
- Para agregar con la CLI sin versión en el `.csproj`:
  - `dotnet add <ruta-al-proyecto> package Paquete.Nombre --version X.Y.Z` (esto añadirá la versión en el `.csproj`) — después mueve la versión a `Directory.Packages.props` y elimina el atributo `Version` del `PackageReference`.

Buenas prácticas
- Mantén `Directory.Packages.props` actualizado cuando agregues paquetes.
- Revisa PRs para asegurarte de que no se introducen `Version` en los `.csproj` si la gestión central está activada.
- Documenta cualquier excepción (por ejemplo, paquetes con versiones flotantes) en este README.

Si prefieres no usar la gestión centralizada, quita o cambia `ManagePackageVersionsCentrally` en `Directory.Packages.props` y gestiona versiones directamente en cada `.csproj`.