# InventoryApp - Sistema de Inventario y Ventas

## Integrante
- **Alan Abimael Matzir Agustín** - Carné: 2500363

## Descripción del Proyecto
Aplicación de escritorio desarrollada en C# con Windows Forms para gestión de inventario, clientes y ventas. Utiliza ADO.NET para conexión con base de datos MySQL y arquitectura por capas.

## ✅ Módulos Implementados

### 1. Gestión de Productos (ProductsInlineForm)
- ✅ CRUD completo con DataGridView
- ✅ Edición directa en la tabla
- ✅ Validaciones (nombre obligatorio, precio y stock >= 0)
- ✅ Mensajes de confirmación
- ✅ ID generado automáticamente

### 2. Gestión de Clientes (ClientsForm)
- ✅ CRUD completo
- ✅ Campos: ID, Nombre, Correo, Teléfono, Dirección
- ✅ Validación de formato de correo
- ✅ Validación de campos requeridos
- ✅ Repositorio con ADO.NET

### 3. Visualización de Ventas (SalesViewForm)
- ✅ Vista maestro-detalle
- ✅ Grid principal: ID, Cliente, Fecha, Total
- ✅ Grid secundario: Productos, Cantidad, Precio, Subtotal
- ✅ Filtros por cliente
- ✅ Filtros por rango de fechas
- ✅ Consultas SQL con JOIN
- ✅ Formato de datos (fechas y montos)

## Tecnologías Utilizadas
- **Lenguaje:** C# (.NET 8.0)
- **Framework:** Windows Forms
- **Base de Datos:** MySQL
- **Acceso a Datos:** ADO.NET
- **Patrón:** Arquitectura por capas (Domain, Infrastructure, Repositories, Services, WinForms)

## Estructura del Proyecto
```
InventoryApp/
├── Domain/              # Modelos (Client, Product, Sale, SaleDetail)
├── Infrastructure/      # Conexión a BD (DbConnectionFactory)
├── Repositories/        # Repositorios (ClientRepository, ProductRepository, SaleRepository)
├── Services/           # Lógica de negocio (SalesService)
└── WinForms/           # Formularios (MainForm, ProductsInlineForm, ClientsForm, SalesViewForm)
```

## Instalación y Ejecución

1. Clonar el repositorio
2. Configurar cadena de conexión en `DbConnectionFactory.cs`
3. Crear base de datos MySQL con las tablas necesarias
4. Abrir solución en Visual Studio
5. Ejecutar (F5)

## Base de Datos
Tablas requeridas:
- `producto` (id, nombre, precio, stock)
- `cliente` (id, nombre, email, telefono, direccion)
- `venta` (id, cliente_id, fecha, total)
- `detalle_venta` (id, venta_id, producto_id, cantidad, precio_unit, subtotal)

## Características Destacadas
- Interfaz profesional y amigable
- Validaciones completas
- Manejo de errores con try-catch
- Operaciones asíncronas (async/await)
- Transacciones para ventas
- Formato de moneda guatemalteca (Q)

## Notas
Proyecto desarrollado para la materia de Programación II  
Universidad San Pablo de Guatemala  
Docente: Ing. Jonatan Sandoval  
Fecha: Octubre 2025
