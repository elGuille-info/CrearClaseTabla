# elGuille.Util.Developer.Data

```
'------------------------------------------------------------------------------
' Revisiones las aplicación y las de esta biblioteca de clases
'------------------------------------------------------------------------------

'<revision("1.1.0.12", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Solo estaba CInt en las conversiones de tipos de Visual Basic (para probar si iba).", _
'   Solucion:= "Añadir todas las conversiones de Visual Basic: CBool,CByte,CChar,CDate,CDbl,CDec,CInt,CLng,CObj,CSByte,CShort,CSng,CStr,CUInt,CULng,CUShort y poner las correspondientes en C#.", _
'   Comentarios:= "Añado estas conversiones, para facilitar la lectura quito Convert.: ToBoolean, ToByte, ToChar, ToDateTime, ToDouble, ToDecimal, ToInt32, ToInt64, (con CObj no haya nada que convertir), ToSByte, ToInt16, ToSingle, ToString, ToUInt32, ToUint64, ToUint16.")>


'<revision("1.1.0.11", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Las conversiones de tipos de Visual Basic tipo CInt(variable) en C# las deja igual.", _
'   Solucion:= "Añadir los tipos de las conversiones de Visual Basic (CInt, etc) para adecuarlas a C#: CInt(variable) debe ser Convert.ToInt32(variable).", _
'   Comentarios:= "")>

'<revision("1.1.0.10", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Quito los espacios alrededor de las primeras instrucciones y cambio el orden.", _
'   Solucion:= "Cambio de orden las instrucciones para que AndAlso esté antes de And, OrElse antes de Or, IsNot antes de Is, Nothing antes de Not.", _
'   Comentarios:= "En comprobarParam lo que hay que cambiar llega sin espacios (porque así se indica en el parámetro).")>

'<revision("1.1.0.9", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Espacios alrededor de ls primeras instrucciones.", _
'   Solucion:= "Añado espacios delante y detrás de |, &, ||, &&, !=, ==, null, !.", _
'   Comentarios:= "Por si esta falta de espacios le quita claridad en C# (de todas formas los espacios extras no afectan).")>


'<revision("1.1.0.8", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "Variable", _
'   Motivo:= "Esta asignación falla: Dim obj = LoQueSea, la convierte en Dim obj As = LoQueSea.", _
'   Solucion:= "No usar As si no se indica el tipo en un Dim.", _
'   Comentarios:= "Ahora se supone que las declaraciones implícitas funcionarán.")>


'<revision("1.1.0.7", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "tiposVB e tiposCS", _
'   Motivo:= "No se tiene en cuenta la declaración del tipo implícito en C#.", _
'   Solucion:= "Añado el tipo '' (cadena vacía) en Visual Basic y el correspondiente en C# es var.", _
'   Comentarios:= "De esta forma cuando no se indique el tipo de datos, se usar var (espero).")>


'<revision("1.1.0.6", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Espacios en Is y ==.", _
'   Solucion:= "Añado espacios delante y detrás de And, AndAlso, Or, OrElse, IsNot, Is, Nothing y Not.", _
'   Comentarios:= "Estaba definido ' Is' y se podía usar ' IsNot'.")>


'<revision("1.1.0.5", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "", _
'   Motivo:= "Al asignar Dim variable = LoQueSea en C# no pone var variable = LoQueSea;", _
'   Solucion:= "Ver la revisión 1.1.0.7", _
'   Comentarios:= "Al definir una variable implícita en C# no usa var (al menos en el caso que he probado en Crear).")>


'<revision("1.1.0.4", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB e instrCS", _
'   Motivo:= "Falta AndAlso y OrElse.", _
'   Solucion:= "Añado en instrVB: And, AndAlso, Or y OrElse, en instrCS: &, &&, | y ||.", _
'   Comentarios:= "Espero que esto soluciones los argumentos del IF cuando son compuestos como en: If DBNull.Value.Equals(obj) OrElse obj Is Nothing Then.")>


'<revision("1.1.0.3", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrCS", _
'   Motivo:= "IsNot e Is no siempre funciona en C# con is ! o is, debe usarse != y ==.", _
'   Solucion:= "Cambio is ! por != e is por ==.", _
'   Comentarios:= "No recuerdo qué versión se necesita para usar is, pero en mi experiencia con != y == siempre funciona.")>


'<revision("1.1.0.2", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "Tipo", _
'   Motivo:= "Al indicar una cadena vacía como tipo de datos se comprueba si está definido.", _
'   Solucion:= "Comprobar si es una cadena vacía para no hacer las comprobaciones.", _
'   Comentarios:= "Ver la revisión 1.1.0.7 que modifica esto. Se devuelve una cadena vacía si se cumple que es una cadena vacía antes de hacer la comprobación de si el tipo está definido.")>


'<revision("1.1.0.1", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "generarClase", _
'   Motivo:= "Si en Crear ExecuteScalar devuelve nulo, que no se produzca una excepción al asignar el nuevo ID.", _
'   Solucion:= "Compruebo si ExecuteScalar devuelve nulo al asignar el nuevo ID.", _
'   Comentarios:= "En este caso no se me ha dado este problema, pero mejor curarse en salud.")>


'<revision("1.1.0.0", _
'   FechaModificacion:= "01/Oct/2022", _
'   Tester:= "elGuille", _
'   FechaReporte:= "01/Oct/2022", _
'   Tipo:= "Revisión", _
'   Clases:= "Proyecto", _
'   Miembro:= "N.A.", _
'   Motivo:= "N.A.", _
'   Solucion:= "N.A.", _
'   Comentarios:= "Cambio de versión de .NET Framework a la 4.8.1.")>

```

### Revisiones anteriores a 2002

#### Revisiones de 2021

```
'<revision("1.0.0.32", _
'   FechaModificacion:= "17/Abr/2021", _
'   Tester:= "elGuille", _
'   FechaReporte:= "17/Abr/2021", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "generarClase", _
'   Motivo:= "En nuevoNombreClase la variable o_N la pongo con el nombre completo de la clase en vez delprimer caracter.", _
'   Solucion:= "NO HACERLO, DEBE SER o_N porque el parámetro es o_NombreClase", _
'   Comentarios:= "Es por cuestión estética, aunque yo no uso nuevoClase (ni Borrar).")>
'

'<revision("1.0.0.31", _
'   FechaModificacion:= "17/Abr/2021", _
'   Tester:= "elGuille", _
'   FechaReporte:= "17/Abr/2021", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "generarClase", _
'   Motivo:= "Añadir una comprobación de si es nulo el objeto tran no usarlo.", _
'   Solucion:= "Cambiar el código.", _
'   Comentarios:= "Dio una vez error al no poder conectarse y no crearse el objeto connection.")>

```

#### Revisiones de 2020

```
'<revision("1.0.0.30", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "generarClase", _
'   Motivo:= "El If not con is nothing estaba mal asignado a la función If de ConvLang.", _
'   Solucion:= "Quitar el primer argumento (la variable).", _
'   Comentarios:= "Se repetía la comparación, ahora se queda en VB: If (Not con Is Nothing) y en C#: if( !(con is null) ).")>
'

'<revision("1.0.0.29", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "generarClase", _
'   Motivo:= "La variable CadenaConexion estaba en minúsculas.", _
'   Solucion:= "Que siempre esté en mayúsculas.", _
'   Comentarios:= "En C# daba error de que no estaba definida cadenaConexion.")>
'

'<revision("1.0.0.28", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "instrVB y instrCS", _
'   Motivo:= "Añado el Not de VB y el ! de C#.", _
'   Solucion:= "Para que se tengan en cuenta las comprobaciones de NOT lo que sea.", _
'   Comentarios:= "Aunque hay que tener en cuenta que por ejemplo Not algo Is Nothing en C# debería ser !(algo is null).")>
'

'<revision("1.0.0.27", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "If", _
'   Motivo:= "Añado comprobar parámetros en los 3 argumentos.", _
'   Solucion:= "Añado comprobarParam(comp) en el segundo argumento.", _
'   Comentarios:= "Es para convertir el Not de VB en ! en C#.")>
'

'<revision("1.0.0.16", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "Form1", _
'   Miembro:= "guardarCfg", _
'   Motivo:= "En la configuración guardar el password de SQL.", _
'   Solucion:= "Asigno el valor del password, antes era una cadena vacía.", _
'   Comentarios:= "Es un rollo tener que volver a escribirlo.")>
'

'<revision("1.0.0.15", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Modificaciones en la clase CrearClase y ConvLang.", _
'   Solucion:= "", _
'   Comentarios:= "Actualizar la aplicación al actualizar la biblioteca.")>
'

'<revision("1.0.0.26", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "Using", _
'   Motivo:= "En el código de C# da eror de que el formato no es correcto.", _
'   Solucion:= "Añadir dos llaves al final.", _
'   Comentarios:= "Daba error porque la cadena acaba con { y lo interpretaba como que seguía otro parámetro, hay que usar {{.")>
'

'<revision("1.0.0.25", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "GenerarClse", _
'   Motivo:= "Comproba si la conexión es nulo.", _
'   Solucion:= "Se añade una comprobación de si 'con' no es nulo.", _
'   Comentarios:= "Añadir esta comprobación por si se diera el caso de que la conexión no se abriera.")>
'

'<revision("1.0.0.24", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "GenerarClse", _
'   Motivo:= "ex2 As Exception se definía, pero no se usaba en el Try de tran.Rollback().", _
'   Solucion:= "Poner ex2 en el mensaje, además de quitarle el paréntesis inicial.", _
'   Comentarios:= "Al capturar la excepción de tran.Rollback() no se usaba la variable de la excepción y el mensaje debe empezar por ERROR para usarlo como comprobante de que hubo error.")>
'

'<revision("1.0.0.23", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "GenerarClse", _
'   Motivo:= "Daba error en BeginTransaction", _
'   Solucion:= "Ponerle los paréntesis.", _
'   Comentarios:= "Al cambia rel código a C# daba error por no tener los paréntesis de llamada a un método.")>
'

'<revision("1.0.0.22", _
'   FechaModificacion:= "21/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "21/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "GenerarClse", _
'   Motivo:= "Daba error en BeginTransaction", _
'   Solucion:= "Ponerle los paréntesis.", _
'   Comentarios:= "Al cambia rel código a C# daba error por no tener los paréntesis de llamada a un método.")>
'

'<revision("1.0.0.14", _
'   FechaModificacion:= "20/Dic/2020", _
'   Tester:= "elGuille", _
'   FechaReporte:= "20/Dic/2020", _
'   Tipo:= "Revisión", _
'   Clases:= "Form1", _
'   Miembro:= "Load", _
'   Motivo:= "No se veía la aplicación.", _
'   Solucion:= "Usar la posición guardad solo si Left es mayor de -1.", _
'   Comentarios:= "Después de haber usado un monitor externo, el valor de Left era negativo y no se mostraba bien sin ese monitor externo.")>

```

#### Revisiones de 2019

```
'<revision("1.0.0.12", _
'   FechaModificacion:= "20/Mar/2019", _
'   Tester:= "elGuille", _
'   FechaReporte:= "20/Mar/2019", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "CadenaConexion se define Public.", _
'   Solucion:= "", _
'   Comentarios:= "Definida Public para poder asignar otro valor por si se usan diferentes bases de datos.")>
'

'<revision("1.0.0.11", _
'   FechaModificacion:= "20/Mar/2019", _
'   Tester:= "elGuille", _
'   FechaReporte:= "20/Mar/2019", _
'   Tipo:= "Revisión", _
'   Clases:= "ConvLang", _
'   Miembro:= "Using, End Using", _
'   Motivo:= "Añado la instrucción Using.", _
'   Solucion:= "", _
'   Comentarios:= "Añado la instrucción Using.")>
'

'<revision("1.0.0.10", _
'   FechaModificacion:= "20/Mar/2019", _
'   Tester:= "elGuille", _
'   FechaReporte:= "20/Mar/2019", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Cambio el método Borrar para que use Command en lugar de DataAdapter.", _
'   Solucion:= "", _
'   Comentarios:= "Cambio el método Borrar para que use Command en lugar de DataAdapter. Además la sobrecarga con parámetros indica lo que hay que usar en la cláusula WHERE.")>
'

'<revision("1.0.0.9", _
'   FechaModificacion:= "19/Mar/2019", _
'   Tester:= "elGuille", _
'   FechaReporte:= "19/Mar/2019", _
'   Tipo:= "Bug", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "En los comandos de AddWithValue se quedaba el comentario de TODO: Comprobar el tipo de datos a usar...", _
'   Solucion:= "", _
'   Comentarios:= "El comentario 'TODO: Comprobar el tipo de datos a usar...' solo se muestra cuando se usa el método Add no en AddWithValue.")>
'

'<revision("1.0.0.8", _
'   FechaModificacion:= "19/Mar/2019", _
'   Tester:= "elGuille", _
'   FechaReporte:= "19/Mar/2019", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Añado la opción de usar AddWithValue a los comandos de UPDATE, INSERT y DELETE.", _
'   Solucion:= "", _
'   Comentarios:= "Añado la opción de usar AddWithValue a los comandos de UPDATE, INSERT y DELETE para facilitar la asignación ya que con Add fallaba algunas veces.")>

```

#### Revisiones de 2018

```
'<revision("1.0.0.7", _
'   FechaModificacion:= "16/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "16/Dic/2018", _
'   Tipo:= "Bug", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Error en el método NuevoCLASE, el parámetro del tipo tenía el nom,bre o_CLASE y en el código no estaba el guión bajo", _
'   Solucion:= "", _
'   Comentarios:= "Error en el método NuevoCLASE, el parámetro del tipo tenía el nom,bre o_CLASE y en el código no estaba el guión bajo")>
'

'<revision("1.0.0.6", _
'   FechaModificacion:= "16/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "15/Dic/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Uso ConversorTipos.TIPOData en la propiedad Item", _
'   Solucion:= "", _
'   Comentarios:= "Uso ConversorTipos.TIPOData en la propiedad Item")>
'

'<revision("1.0.0.5", _
'   FechaModificacion:= "16/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "15/Dic/2018", _
'   Tipo:= "Bug", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Devolvía System. como nombre del tipo en las llamadas de la clase ConversorTipos", _
'   Solucion:= "", _
'   Comentarios:= "Devolvía System. como nombre del tipo en las llamadas de la clase ConversorTipos")>
'

'<revision("1.0.0.4", _
'   FechaModificacion:= "15/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "15/Dic/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Uso ConversorTipos.TIPOData para convertir los tipos de la base de datos y asignar a la clase", _
'   Solucion:= "", _
'   Comentarios:= "Uso ConversorTipos.TIPOData para convertir los tipos. Los tipos convertidos son: Int16, Int32, Int64, Single, Decimal, Double, Byte, SByte, UInt16, UInt32, UInt64, Boolean, DateTime, Char, TimeSpan")>
'

'<revision("1.0.0.3", _
'   FechaModificacion:= "14/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "14/Dic/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Al asignar valores negativos usando <tipo numérico>.Parse('0' & r('<campo>').ToString()) da error", _
'   Solucion:= "", _
'   Comentarios:= "Uso unos métodos de conversión definidos en una clase aparte que habrá que agregar al proyecto, las conversiones se harán así: = ConversorTipos.Valor<tipo numérico>(r('<campo>'))")>
'

'<revision("1.0.0.2", _
'   FechaModificacion:= "14/Dic/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "14/Dic/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Cambio los nombres privados de las clases para que empiecen por o_ en vez de o", _
'   Solucion:= "", _
'   Comentarios:= "Cambio los nombres privados de las clases para que empiecen por o_ en vez de o")>
'

'<revision("1.0.0.1", _
'   FechaModificacion:= "30/Nov/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "30/Nov/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "CrearClase", _
'   Miembro:= "", _
'   Motivo:= "Cambio la versión de .NET a la 4.7.2 y la nomclatura de los campos privados para que empiecen con m_", _
'   Solucion:= "", _
'   Comentarios:= "Revisión de la librería para usar con .NET 4.7.2 y cambio los nombres de los campos privados empiezan con m_ en vez de _")>
'

'<revision("1.0.0.0", _
'   FechaModificacion:= "17/Nov/2018", _
'   Tester:= "elGuille", _
'   FechaReporte:= "17/Nov/2018", _
'   Tipo:= "Revisión", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Nueva versión para Visual Basic 2017 de la librería con .NET 4.6.1.", _
'   Solucion:= "", _
'   Comentarios:= "Revisión de la librería para usar con Visual Studio 2017 y .NET 4.6.1")>
```

#### Revisiones de 2007

```
'<revision("0.0.0.1", _
'   FechaModificacion:= "17/Abr/2007", _
'   Tester:= "elGuille", _
'   FechaReporte:= "17/Abr/2007", _
'   Tipo:= "Revisión", _
'   Clases:= "", _
'   Miembro:= "", _
'   Motivo:= "Nueva versión para Visual Basic 2005 de la librería.", _
'   Solucion:= "", _
'   Comentarios:= "Nuevo nombre de la librería: elGuille.Util.Developer.Data " & _
'				  "para publicarla en www.CodePlex.com/CrearClaseTabla")>
```

### Revisiones anteriores a 2007

#### Revisiones de 2005

```
'<revision("1.0.1000.5", _
'   FechaModificacion:= "08/Jun/2005", _
'   Tester:= "elGuille", _
'   FechaReporte:= "08/Jun/2005", _
'   Tipo:= "Bug", _
'   Clase:= "CrearClase", _
'   Miembros:= "GenerarClaseOleDb; GenerarClaseSQL", _
'   Motivo:= "Cuando se llama por segunda vez, da error en Fill porque el DataTable tiene los datos anteriores.", _
'   Solucion:= "Crear una nueva instancia de mDataTable al crear la clase, pero NO en CrearClase.")>
'

'<revision("1.0.1000.4", _
'   FechaModificacion:= "07/Feb/2005", _
'   Tester:= "elGuille", _
'   FechaReporte:= "07/Feb/2005", _
'   Tipo:= "Bug", _
'   Clase:= "CrearClase", _
'   Miembros:= "GenerarClaseOleDb; GenerarClaseSQL", _
'   Motivo:= "Cuando se llama por segunda vez, da error en Fill porque el DataTable tiene los datos anteriores.", _
'   Solucion:= "Crear una nueva instancia de mDataTable al crear la clase.")>
'

'<revision("1.0.1000.3", _
'   FechaModificacion:= "07/Feb/2005", _
'   Tester:= "elGuille", _
'   FechaReporte:= "07/Feb/2005", _
'   Tipo:= "Bug", _
'   Clase:= "CrearClase", _
'   Miembro:= "Borrar(sel)", _
'   Motivo:= "El segundo parámetro en SQL (@p2) da error al usarlo con la clase authors.", _
'   Solucion:= "Comentar la instrucción y recomendar que se compruebe con otras bases de SQL Server.")>
```

#### Revisiones de 2004

```
'<revision("1.0.1000.2", _
'   FechaModificacion:= "02/Nov/2004", _
'   Tester:= "David Sans", _
'   FechaReporte:= "02/Nov/2004", _
'   Tipo:= "Bug", _
'   Clase:= "CrearClase", _
'   Miembro:= "nuevo<nombreClase>", _
'   Motivo:= "Bug si la clase empieza por R, se crearía una variable llamada oR.", _
'   Solucion:= "Cambiar el nombre de la variable por o_<PrimeraLetraClase>.")>
'

'<revision("1.0.1000.1", _
'   FechaModificacion:= "02/Nov/2004", _
'   Tester:= "David Sans", _
'   FechaReporte:= "02/Nov/2004", _
'   Tipo:= "Bug", _
'   Clases:= "CrearClaseOleDb; CrearClaseSQL", _
'   Miembro:= "GenerarClase", _
'   Motivo:= "Si el nombre de la clase tiene espacios, no se puede generar la clase.", _
'   Solucion:= "Sustituir los espacios por guiones bajos.", _
'   Comentarios:= "Esta comprobación se debería hacer en la utilidad de crear las clases.")>
'
```
