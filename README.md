# CrearClaseTabla
Generar clases a partir de una tabla de una base de datos de SQL Server o de Access
 
### Descripción:
Crear clases (en VB o C#) con el contenido de una tabla de una base de datos de Access o SQL Server

#### Introducción

Con esta utilidad vamos a poder generar una clase (tanto para VB .NET como para C#) que nos permitirá acceder a una tabla de una base de datos ya sea de Access como de SQL Server.

La idea de este código (o el porqué me compliqué la vida para hacerlo) es para poder tener una clase con cada uno de los campos de una tabla de una base de datos, de forma que la propia clase contenga todo el código necesario para conectar a la base de datos, así como poder crear nuevos registros (filas), eliminarlos, actualizarlos e incluso buscar en la tabla. También con la clase generada podemos acceder a cada uno de los campos mediante un índice, el cual puede ser numérico (al estilo de un array) o bien alfanumérico, es decir, podemos acceder al contenido de un campo (columna) indicando el nombre de dicha columna.

La clase generada también nos permitirá obtener un objeto DataTable con el contenido de los datos de dicha tabla, usando para ello, (si queremos), un filtro con el que poder seleccionar las filas que queremos obtener (usando la típica cadena SELECT).

<br>
<br>

## Enlaces originales en www.elguille.info.
 
El enlace original del código aquí mostrado:
 
[Generar clases para acceder a una tabla](https://www.elguille.info/net/adonet/crearclases/crearClases.asp)
 
Y el enlace original para acceder al código fuente y ejemplos:
 
[Utilidad para generar clases para acceder a una tabla](https://www.elguille.info/NEt/adonet/crearclases/crearclases_prog.htm)


### La captura de la primera versión (2004)

![Figura 1: La utilidad en ejecución (versión de 2004)](https://www.elguille.info/NEt/adonet/crearclases/crearClases01.png)


![Figura 2: La utilidad en ejecución (versión de 2022)](https://www.elguillemola.com/img/img2022/Screenshot_2022-10-01_140321.jpg)

## TODO:
Todo eso lo tengo que actualizar o bien crear una nueva entrada en el blog.
Ya está creada la entrada en el blog:
[Generar las clases de una tabla de SQL Server o Access (mdb)](https://www.elguillemola.com/generar-las-clases-de-una-tabla-de-sql-server-o-access-mdb/)

Sería interesante convertir el proyecto para .NET 6 (o 7) y también usando el código completamente en C#.
Actualmente está creado para usar con .NET Framework 4.8.1 y escrito enteramente en Visual Basic.

## Actualización

Ya está creado el proyecto para .NET 6.0 (Windows) y .NET MAUI:
[gsCrearClasesTablas](https://github.com/elGuille-info/gsCrearClasesTablas)



 
