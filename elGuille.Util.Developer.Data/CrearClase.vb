'------------------------------------------------------------------------------
' Clase genérica para crear una clase                               (13/Jul/04)
' Esta clase se usará como base de las de SQL y OleDb
'
' Nota: Ver las revisiones en Revisiones.txt
'
' ©Guillermo 'guille' Som, 2004, 2005, 2007, 2018-2020
'------------------------------------------------------------------------------
Option Explicit On
Option Strict On
Option Infer On
'
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb

Imports elGuille.Util.Developer

Namespace elGuille.Util.Developer.Data
    Public Class CrearClase
        '
        Protected Shared mDataTable As New DataTable
        Protected Shared cadenaConexion As String
        Protected Shared nombreTabla As String = "Tabla1"
        '
        Public Shared Conectado As Boolean
        '
        ' Campos para usar desde las clases derivadas
        ' para usar con SQL
        Private Shared dataSource As String
        Private Shared initialCatalog As String
        Private Shared userId As String
        Private Shared usarSeguridadSQL As Boolean
        '
        ' para usar con OleDb (Access)
        Private Shared baseDeDatos As String
        Private Shared provider As String
        '
        ' para ambos tipos de bases de datos
        Private Shared usarCommandBuilder As Boolean = True
        Private Shared dbPrefix As String = "Sql"
        Private Shared password As String
        Private Shared esSQL As Boolean
        Private Shared lang As eLenguaje
        Private Shared nombreClase As String
        Private Shared cadenaSelect As String

        ' Si se usa ExecuteScalar / NonQuery en vez de DataAdapter

        ''' <summary>
        ''' Si se usa DataAdapter o ExecuteScalar ExecuteNonQuery
        ''' </summary>
        ''' <remarks>07/Abr/19 13.17
        ''' Ya lo tenía con UsarExecuteScalar del 23/Mar/19
        ''' </remarks>
        Public Shared Property UsarDataAdapter As Boolean = True

        ' Si se utiliza .Parameters.Add o .Parameters.AddWithValue  (19/Mar/19)

        ''' <summary>
        ''' Si se utiliza .Parameters.Add o .Parameters.AddWithValue.
        ''' True  usará .Parameters.AddWithValue
        ''' False usará .Parameters.Add
        ''' Valor predeterminado = True
        ''' </summary>
        ''' <remarks>19/Mar/2019</remarks>
        Public Shared Property UsarAddWithValue As Boolean = True

        ''' <summary>
        ''' Si se usa Overrides (override en C#) en los métodos
        ''' Actualizar, Crear y Borrar.
        ''' </summary>
        ''' <remarks>25/Mar/2019</remarks>
        Public Shared Property UsarOverrides As Boolean = True

        '''' <summary>
        '''' Si se usa Command con ExecuteScalar en lugar de DataAdapter
        '''' en los comandos UPDATE e INSERT.
        '''' Si se usa ExecuteScalar no se tendrá en cuenta UsarStringBuilder.
        '''' </summary>
        '''' <remarks>23/Mar/2019</remarks>
        'Public Shared Property UsarExecuteScalar As Boolean = False

        '
        ' estos métodos sólo se usarán desde las clases derivadas
        Protected Shared Function GenerarClaseOleDb(ByVal lang As eLenguaje, _
                                                    ByVal usarCommandBuilder As Boolean, _
                                                    ByVal nombreClase As String, _
                                                    ByVal baseDeDatos As String, _
                                                    ByVal cadenaSelect As String, _
                                                    ByVal password As String, _
                                                    ByVal provider As String) As String
            esSQL = False
            If provider = "" Then
                provider = "Microsoft.Jet.OLEDB.4.0"
            End If
            CrearClase.lang = lang
            CrearClase.usarCommandBuilder = usarCommandBuilder
            CrearClase.baseDeDatos = baseDeDatos
            CrearClase.nombreClase = nombreClase
            CrearClase.cadenaSelect = cadenaSelect
            CrearClase.password = password
            CrearClase.provider = provider
            dbPrefix = "OleDb"
            '------------------------------------------------------------------
            ' Esto es lo que hace que no funcione                   (08/Jun/05)
            '------------------------------------------------------------------
            ' crear una nueva instancia del dataTable               (07/Feb/05)
            'mDataTable = New DataTable
            '------------------------------------------------------------------
            '
            Return generarClase()
        End Function
        '
        Protected Shared Function GenerarClaseSQL(ByVal lang As eLenguaje, _
                                                  ByVal usarCommandBuilder As Boolean, _
                                                  ByVal nombreClase As String, _
                                                  ByVal dataSource As String, _
                                                  ByVal initialCatalog As String, _
                                                  ByVal cadenaSelect As String, _
                                                  ByVal userId As String, _
                                                  ByVal password As String, _
                                                  ByVal usarSeguridadSQL As Boolean) As String
            esSQL = True
            CrearClase.lang = lang
            CrearClase.usarCommandBuilder = usarCommandBuilder
            CrearClase.nombreClase = nombreClase
            CrearClase.dataSource = dataSource
            CrearClase.initialCatalog = initialCatalog
            CrearClase.cadenaSelect = cadenaSelect
            CrearClase.userId = userId
            CrearClase.password = password
            CrearClase.usarSeguridadSQL = usarSeguridadSQL
            dbPrefix = "Sql"
            '------------------------------------------------------------------
            ' Esto es lo que hace que no funcione                   (08/Jun/05)
            '------------------------------------------------------------------
            ' crear una nueva instancia del dataTable               (07/Feb/05)
            'mDataTable = New DataTable
            '------------------------------------------------------------------
            '
            Return generarClase()
        End Function
        '
        Private Shared Function generarClase() As String
            ' generar la clase a partir de la tabla seleccionada,
            ' las columnas son los campos a usar
            '
            Dim sb As New System.Text.StringBuilder
            Dim s As String
            Dim sb1 As System.Text.StringBuilder
            Dim sb2 As System.Text.StringBuilder
            Dim sb3 As System.Text.StringBuilder
            Dim campoIDnombre As String = "ID"
            Dim campoIDtipo As String = "System.Int32"
            Dim campos As New Hashtable
            Dim novalidos As String = " -ºª!|@#$%&/()=?¿*+^'¡-<>,.;:{}[]Çç€\" & Chr(34) & vbTab
            '
            ' buscar el campo autoincremental de la tabla           (12/Jul/04)
            ' también se buscará si es Unique
            For Each col As DataColumn In mDataTable.Columns
                ' comprobar si tiene caracteres no válidos          (14/Jul/04)
                ' en caso de que sea así, sustituirlos por un guión bajo
                Dim i As Integer
                s = col.ColumnName
                Do
                    i = s.IndexOfAny(novalidos.ToCharArray)
                    If i > -1 Then
                        If i = s.Length - 1 Then
                            s = s.Substring(0, i) & "_"
                        ElseIf i > 0 Then
                            s = s.Substring(0, i) & "_" & s.Substring(i + 1)
                        Else
                            s = "_" & s.Substring(i + 1)
                        End If
                    End If
                Loop While i > -1
                campos.Add(col.ColumnName, s)
                '
                ' No siempre el predeterminado es AutoIncrement
                If col.AutoIncrement OrElse col.Unique Then
                    campoIDnombre = s 'col.ColumnName
                    campoIDtipo = col.DataType.ToString
                    'Exit For
                End If
            Next
            '
            '
            ConvLang.Lang = lang
            sb.AppendFormat("{0}{1}", ConvLang.Comentario("------------------------------------------------------------------------------"), vbCrLf)
            If esSQL Then
                sb.AppendFormat("{0} Clase {1} generada automáticamente con CrearClaseSQL{2}", ConvLang.Comentario(), nombreClase, vbCrLf)
                sb.AppendFormat("{0} de la tabla '{1}' de la base '{2}'{3}", ConvLang.Comentario(), nombreTabla, initialCatalog, vbCrLf)
            Else
                sb.AppendFormat("{0} Clase {1} generada automáticamente con CrearClaseOleDb{2}", ConvLang.Comentario(), nombreClase, vbCrLf)
                sb.AppendFormat("{0} de la tabla '{1}' de la base '{2}'{3}", ConvLang.Comentario(), nombreTabla, baseDeDatos, vbCrLf)
            End If
            ' Al mostrar MMM/ se muestra mar./ para el mes de marzo (22/Mar/19)
            sb.AppendFormat("{0} Fecha: {1}{2}", ConvLang.Comentario(), DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss").Replace("./", "/"), vbCrLf)
            sb.AppendFormat("{0}{1}", ConvLang.Comentario(), vbCrLf)
            ' Cambio 'guille' por (elGuille)                        (22/Mar/19)
            If DateTime.Now.Year > 2021 Then
                sb.AppendFormat("{0}{1}", ConvLang.Comentario(String.Format(" ©Guillermo (elGuille) Som, 2004-{0}", DateTime.Now.Year)), vbCrLf)
            Else
                sb.AppendFormat("{0}{1}", ConvLang.Comentario(" ©Guillermo (elGuille) Som, 2004-2021"), vbCrLf)
            End If
            sb.AppendFormat("{0}{1}", ConvLang.Comentario("------------------------------------------------------------------------------"), vbCrLf)
            '
            If lang = eLenguaje.eVBNET Then
                sb.AppendFormat("Option Strict On{0}", vbCrLf)
                ' Añado Option Infer On                             (17/Nov/18)
                sb.AppendFormat("Option Infer On{0}", vbCrLf)
                sb.AppendFormat("{0}{1}", ConvLang.Comentario(), vbCrLf)
                ' Añado Imports Microsoft.VisualBasic               (17/Nov/18)
                ' No hay que indicar Imports en la cadena           (20/Nov/18)
                sb.AppendFormat("{0}{1}", ConvLang.Imports("Microsoft.VisualBasic"), vbCrLf)
            End If
            ' las importaciones de espacios de nombres
            sb.AppendFormat("{0}{1}", ConvLang.Imports("System"), vbCrLf)
            sb.AppendFormat("{0}{1}", ConvLang.Imports("System.Data"), vbCrLf)
            If esSQL Then
                sb.AppendFormat("{0}{1}", ConvLang.Imports("System.Data.SqlClient"), vbCrLf)
            Else
                sb.AppendFormat("{0}{1}", ConvLang.Imports("System.Data.OleDb"), vbCrLf)
            End If
            sb.AppendFormat("{0}{1}", ConvLang.Comentario(), vbCrLf)
            '
            ' declaración clase
            sb.AppendFormat("{0}{1}", ConvLang.Class("Public", nombreClase), vbCrLf)
            '
            '------------------------------------------------------------------
            ' los campos privados para usar con las propiedades
            '------------------------------------------------------------------
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Las variables privadas"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" TODO: Revisar los tipos de los campos"), vbCrLf)
            For Each col As DataColumn In mDataTable.Columns
                ' Los nombres de los campos privados empiezan con m_ (30/Nov/18)
                sb.AppendFormat("    {0}{1}", ConvLang.Variable("Private", "m_" & campos(col.ColumnName).ToString, col.DataType.ToString), vbCrLf)
            Next
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(""), vbCrLf)
            '
            ' ajustarAncho: método privado para ajustar los caracteres de los campos de tipo String
            'sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Este método se usará para ajustar los anchos de las propiedades"), vbCrLf)
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {" Este método se usará para ajustar los anchos de las propiedades"}))
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Private", "ajustarAncho", "String", "cadena", "String", "ancho", "Integer"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.VariableNewParam("Dim", "sb", "System.Text.StringBuilder", "New String("" ""c, ancho)"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" devolver la cadena quitando los espacios en blanco"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" esto asegura que no se devolverá un tamaño mayor ni espacios ""extras"""), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Return("(cadena & sb.ToString()).Substring(0, ancho).Trim()"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            ' Propiedades públicas de instancia
            '
            '------------------------------------------------------------------
            ' propiedades públicas mapeadas a cada columna de la tabla
            '------------------------------------------------------------------
            ' Añadir líneas de separación para facilitar la lectura (30/Nov/18)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("------------------------------------------------------------------"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Las propiedades públicas"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" TODO: Revisar los tipos de las propiedades"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("------------------------------------------------------------------"), vbCrLf)
            For Each col As DataColumn In mDataTable.Columns
                If col.DataType.ToString = "System.Byte[]" Then
                    sb.AppendFormat("    {0}{1}", ConvLang.Property("Public", "System.Byte()", campos(col.ColumnName).ToString), vbCrLf)
                Else
                    ' Si se usa Overrides, y es autoincrement,      (13/Abr/19)
                    ' añadirle el Overrides
                    If UsarOverrides AndAlso col.AutoIncrement = True Then
                        sb.AppendFormat("    {0}{1}", ConvLang.Property("Public Overrides", col.DataType.ToString, campos(col.ColumnName).ToString), vbCrLf)
                    Else
                        sb.AppendFormat("    {0}{1}", ConvLang.Property("Public", col.DataType.ToString, campos(col.ColumnName).ToString), vbCrLf)
                    End If

                End If
                sb.AppendFormat("        {0}{1}", ConvLang.Get(), vbCrLf)
                If col.DataType.ToString <> "System.String" Then
                    sb.AppendFormat("            {0}{1}", ConvLang.Return(" m_" & campos(col.ColumnName).ToString), vbCrLf)
                Else
                    If col.MaxLength > 255 Then
                        sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" Seguramente sería mejor sin ajustar el ancho..."), vbCrLf)
                        sb.AppendFormat("            {0}{1}", ConvLang.Comentario(ConvLang.Return(String.Format("ajustarAncho(m_{0},{1})", campos(col.ColumnName).ToString, col.MaxLength))), vbCrLf)
                        sb.AppendFormat("            {0}{1}", ConvLang.Return(" m_" & campos(col.ColumnName).ToString), vbCrLf)
                    Else
                        sb.AppendFormat("            {0}{1}", ConvLang.Return(String.Format("ajustarAncho(m_{0},{1})", campos(col.ColumnName).ToString, col.MaxLength)), vbCrLf)
                    End If
                End If
                sb.AppendFormat("        {0}{1}", ConvLang.EndGet(), vbCrLf)
                If col.DataType.ToString = "System.Byte[]" Then
                    sb.AppendFormat("        {0}{1}", ConvLang.Set("System.Byte()"), vbCrLf)
                Else
                    sb.AppendFormat("        {0}{1}", ConvLang.Set(col.DataType.ToString), vbCrLf)
                End If
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("m_" & campos(col.ColumnName).ToString, "value"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndSet(), vbCrLf)
                sb.AppendFormat("    {0}{1}", ConvLang.EndProperty(), vbCrLf)
            Next
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' Item: propiedad predeterminada (indizador)
            '       permite acceder a los campos mediante un índice numérico
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {" Propiedad predeterminada (indizador) Permite acceder mediante un índice numérico"}))
            sb.AppendFormat("    {0}{1}", ConvLang.Property("Public", "String", "index", "Integer"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Devuelve el contenido del campo indicado en index"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" (el índice corresponde con la columna de la tabla)"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Get(), vbCrLf)
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                If i = 0 Then
                    sb.AppendFormat("            {0}{1}", ConvLang.If("index", "=", "0"), vbCrLf)
                Else
                    sb.AppendFormat("            {0}{1}", ConvLang.ElseIf("index", "=", i.ToString), vbCrLf)
                End If
                If col.DataType.ToString = "System.Byte[]" Then
                    ' TODO: convertir el array de bytes en una cadena...
                    sb.AppendFormat("                {0}{1}", ConvLang.Return("<Binario largo>"), vbCrLf)
                Else
                    sb.AppendFormat("                {0}{1}", ConvLang.Return("Me." & campos(col.ColumnName).ToString & ".ToString()"), vbCrLf)
                End If
            Next
            sb.AppendFormat("            {0}{1}", ConvLang.EndIf(), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" Para que no de error el compilador de C# y"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" se devuelva el valor <NULO> en caso de que no exista ese campo."), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Return(Chr(34) & "<NULO>" & Chr(34)), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndGet(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Set("String"), vbCrLf)
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                If i = 0 Then
                    sb.AppendFormat("            {0}{1}", ConvLang.If("index", "=", "0"), vbCrLf)
                Else
                    sb.AppendFormat("            {0}{1}", ConvLang.ElseIf("index", "=", i.ToString), vbCrLf)
                End If
                '
                Select Case col.DataType.ToString
                    Case "System.String"
                        sb.AppendFormat("                {0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, "value"), vbCrLf)
                    Case "System.Int16", "System.Int32", "System.Int64",
                         "System.Single", "System.Decimal", "System.Double",
                         "System.Byte", "System.SByte", "System.UInt16", "System.UInt32", "System.UInt64",
                         "System.Boolean", "System.DateTime", "System.Char", "System.TimeSpan"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("Me.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("ConversorTipos.{1}Data(value)", col.ColumnName, col.DataType.ToString().Replace("System.", ""))), vbCrLf)
                    Case "System.Byte[]"
                        sb.AppendFormat("                {0} Es un Binario largo (array de Byte){1}", ConvLang.Comentario(), vbCrLf)
                        sb.AppendFormat("                {0} y por tanto no se le puede asignar el contenido de una cadena...{1}", ConvLang.Comentario(), vbCrLf)
                    Case Else
                        sb.AppendFormat("                {0}{1}", ConvLang.Comentario(" TODO: Comprobar la conversión a realizar"), vbCrLf)
                        sb.AppendFormat("                {0}{1}", ConvLang.Comentario(String.Format("       con el tipo {0}", col.DataType.ToString)), vbCrLf)
                        sb.AppendFormat("                {2}{0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, "value"), vbCrLf, ConvLang.Comentario())
                        sb.AppendFormat("                {0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, col.DataType.ToString & ".Parse(value)"), vbCrLf)
                End Select
            Next
            sb.AppendFormat("            {0}{1}", ConvLang.EndIf(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndSet(), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndProperty(), vbCrLf)
            '
            '------------------------------------------------------------------
            ' La propiedad Item usando el nombre de la columna      (11/Jul/04)
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {" Propiedad predeterminada (indizador) Permite acceder mediante el nombre de una columna"}))
            sb.AppendFormat("    {0}{1}", ConvLang.Property("Public", "String", "index", "String"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Devuelve el contenido del campo indicado en index"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" (el índice corresponde al nombre de la columna)"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Get(), vbCrLf)
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                If i = 0 Then
                    sb.AppendFormat("            {0}{1}", ConvLang.If("index", "=", Chr(34) & campos(col.ColumnName).ToString & Chr(34)), vbCrLf)
                Else
                    sb.AppendFormat("            {0}{1}", ConvLang.ElseIf("index", "=", Chr(34) & campos(col.ColumnName).ToString & Chr(34)), vbCrLf)
                End If
                If col.DataType.ToString = "System.Byte[]" Then
                    ' TODO: convertir el array de bytes en una cadena...
                    sb.AppendFormat("                {0}{1}", ConvLang.Return("<Binario largo>"), vbCrLf)
                Else
                    sb.AppendFormat("                {0}{1}", ConvLang.Return("Me." & campos(col.ColumnName).ToString & ".ToString()"), vbCrLf)
                End If
            Next
            sb.AppendFormat("            {0}{1}", ConvLang.EndIf(), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" Para que no de error el compilador de C# y"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" se devuelva el valor <NULO> en caso de que no exista ese campo."), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Return(Chr(34) & "<NULO>" & Chr(34)), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndGet(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Set("String"), vbCrLf)
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                If i = 0 Then
                    sb.AppendFormat("            {0}{1}", ConvLang.If("index", "=", Chr(34) & campos(col.ColumnName).ToString & Chr(34)), vbCrLf)
                Else
                    sb.AppendFormat("            {0}{1}", ConvLang.ElseIf("index", "=", Chr(34) & campos(col.ColumnName).ToString & Chr(34)), vbCrLf)
                End If
                '
                Select Case col.DataType.ToString
                    Case "System.String"
                        sb.AppendFormat("                {0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, "value"), vbCrLf)
                    Case "System.Int16", "System.Int32", "System.Int64",
                         "System.Single", "System.Decimal", "System.Double",
                         "System.Byte", "System.SByte", "System.UInt16", "System.UInt32", "System.UInt64",
                         "System.Boolean", "System.DateTime", "System.Char", "System.TimeSpan"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("Me.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("ConversorTipos.{1}Data(value)", col.ColumnName, col.DataType.ToString().Replace("System.", ""))), vbCrLf)
                    Case "System.Byte[]"
                        sb.AppendFormat("                {0} Es un Binario largo (array de Byte){1}", ConvLang.Comentario(), vbCrLf)
                        sb.AppendFormat("                {0} y por tanto no se le puede asignar el contenido de una cadena...{1}", ConvLang.Comentario(), vbCrLf)
                    Case Else
                        sb.AppendFormat("                {0}{1}", ConvLang.Comentario(" TODO: Comprobar la conversión a realizar"), vbCrLf)
                        sb.AppendFormat("                {0}{1}", ConvLang.Comentario(String.Format("       con el tipo {0}", col.DataType.ToString)), vbCrLf)
                        sb.AppendFormat("                {2}{0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, "value"), vbCrLf, ConvLang.Comentario())
                        sb.AppendFormat("                {0}{1}", ConvLang.Asigna("Me." & campos(col.ColumnName).ToString, col.DataType.ToString & ".Parse(value)"), vbCrLf)
                End Select
            Next
            sb.AppendFormat("            {0}{1}", ConvLang.EndIf(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndSet(), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndProperty(), vbCrLf)

            sb.AppendLine()
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("-------------------------------------------------------------------------"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Campos y métodos compartidos (estáticos) para gestionar la base de datos"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("-------------------------------------------------------------------------"), vbCrLf)
            sb.AppendLine()

            '------------------------------------------------------------------
            ' la cadena de conexión
            '------------------------------------------------------------------
            sb.AppendFormat("{0}{1}", ConvLang.DocumentacionXML("    ",
                                                               {" La cadena de conexión a la base de datos.",
                                                                " Definida Public para poder asignar otro valor",
                                                                " por si se usan diferentes bases de datos."}), vbCrLf)
            sb1 = New System.Text.StringBuilder
            If lang = eLenguaje.eCS Then
                sb1.Append("@")
            End If
            If esSQL Then
                sb1.AppendFormat("""Data Source={0};", dataSource)
                sb1.AppendFormat(" Initial Catalog={0};", initialCatalog)
                If usarSeguridadSQL Then
                    sb1.AppendFormat(" user id={0}; password={1}", userId, password)
                Else
                    sb1.Append(" Integrated Security=yes;")
                End If
            Else
                If password <> "" Then
                    sb1.AppendFormat("{0}Provider={1}; Data Source={2}; Jet OLEDB:Database Password={3}", Chr(34), provider, baseDeDatos, password)
                Else
                    sb1.AppendFormat("{0}Provider={1}; Data Source={2}", Chr(34), provider, baseDeDatos)
                End If
            End If
            sb1.Append(Chr(34))
            ' Añado Property a CadenaConexion y CadenaSelect        (13/Abr/19)
            ' para que sea más fácil saber las referencias que tienen.
            sb.AppendFormat("    {0}{1}", ConvLang.DeclaraVariable("Public Shared Property", "CadenaConexion", "String", sb1.ToString), vbCrLf)
            '
            '------------------------------------------------------------------
            ' la cadena de selección (campo público)
            '------------------------------------------------------------------
            sb.AppendFormat("    {0}{1}", ConvLang.DocumentacionXML(" La cadena de selección"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.DeclaraVariable("Public Shared Property", "CadenaSelect", "String", Chr(34) & cadenaSelect & Chr(34)), vbCrLf)
            sb.AppendLine()

            '------------------------------------------------------------------
            ' constructores
            ' Uno sin parámetros y otro que recibe la cadena de conexión
            '------------------------------------------------------------------
            sb.AppendFormat("    {0}{1}", ConvLang.Constructor("Public", nombreClase), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndSub(), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Constructor("Public", nombreClase, "conex", "String"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Asigna("CadenaConexion", "conex"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndSub(), vbCrLf)

            sb.AppendLine()
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("-----------------------------------------"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Métodos compartidos (estáticos) privados"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("-----------------------------------------"), vbCrLf)
            sb.AppendLine()

            '------------------------------------------------------------------
            ' row2<nombreClase>: asigna una fila de la tabla a un objeto del tipo de la clase
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {" Asigna una fila de la tabla a un objeto " & nombreClase}))
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "Row2Tipo", nombreClase, "r", "DataRow"), vbCrLf)
            sb.AppendFormat("        {2} asigna a un objeto {0} los datos del dataRow indicado{1}", nombreClase, vbCrLf, ConvLang.Comentario())
            sb.AppendFormat("        {0}{1}", ConvLang.VariableNew("Dim", "o_" & nombreClase, nombreClase), vbCrLf)
            sb.AppendLine()
            For Each col As DataColumn In mDataTable.Columns
                Select Case col.DataType.ToString
                    Case "System.String"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""].ToString()", col.ColumnName)), vbCrLf)
                    Case "System.Int16", "System.Int32", "System.Int64",
                         "System.Single", "System.Decimal", "System.Double",
                         "System.Byte", "System.SByte", "System.UInt16", "System.UInt32", "System.UInt64",
                         "System.Boolean", "System.DateTime", "System.Char", "System.TimeSpan"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("ConversorTipos.{1}Data(r[""{0}""].ToString())", col.ColumnName, col.DataType.ToString().Replace("System.", ""))), vbCrLf)
                    Case "System.Byte[]"
                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""]", col.ColumnName)), vbCrLf, ConvLang.Comentario())
                    Case Else
                        ' El resto de tipos
                        sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" TODO: Comprobar la conversión a realizar"), vbCrLf)
                        sb.AppendFormat("        {0}{1}", ConvLang.Comentario(String.Format("       con el tipo {0}", col.DataType.ToString)), vbCrLf)
                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""]", col.ColumnName)), vbCrLf, ConvLang.Comentario())
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("{1}.Parse(r[""{0}""].ToString())", col.ColumnName, col.DataType.ToString)), vbCrLf)
                End Select
            Next
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("o_" & nombreClase), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' <nombreClase>2Row: asigna un objeto de la clase a la fila indicada
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {String.Format(" Asigna un objeto {0} a la fila indicada", nombreClase)}))
            sb.AppendFormat("    {0}{1}", ConvLang.Sub("Private Shared", String.Format("{0}2Row", nombreClase), "o_" & nombreClase, nombreClase, "r", "DataRow"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(String.Format(" asigna un objeto {0} al dataRow indicado", nombreClase)), vbCrLf)
            For Each col As DataColumn In mDataTable.Columns
                ' Si es AutoIncrement no asignarle un valor         (10/Jul/04)
                ' si es Unique y no AutoIncrement se debe asignar   (13/Jul/04)
                If col.AutoIncrement Then 'OrElse col.Unique Then
                    sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" TODO: Comprueba si esta asignación debe hacerse"), vbCrLf)
                    sb.AppendFormat("        {0}{1}", ConvLang.Comentario("       pero mejor lo dejas comentado ya que es un campo autoincremental o único"), vbCrLf)
                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna(String.Format("r[""{0}""]", col.ColumnName), String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString)), vbCrLf, ConvLang.Comentario())
                Else
                    sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("r[""{0}""]", col.ColumnName), String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString)), vbCrLf)
                End If
            Next
            sb.AppendFormat("    {0}{1}", ConvLang.EndSub(), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' nuevo<nombreClase>: crea una nueva fila y la asigna a un objeto de la clase
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {String.Format(" Crea una nueva fila y la asigna a un objeto {0}", nombreClase)}))
            sb.AppendFormat("    {0}{1}", ConvLang.Sub("Private Shared", "nuevo" & nombreClase, "dt", "DataTable", "o_" & nombreClase, nombreClase), vbCrLf)
            sb.AppendFormat("        {2} Crear un nuevo {0}{1}", nombreClase, vbCrLf, ConvLang.Comentario())
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "dr", "DataRow", "dt.NewRow()"), vbCrLf)
            '------------------------------------------------------------------
            ' En lugar de "o" & nombreClase.Substring(0, 1),        (02/Nov/04)
            ' usar "o_"  & nombreClase.Substring(0, 1)
            ' ya que si la clase empieza por R,
            ' se creará una variable llamada oR, que no es válida.
            '   Gracias a David Sans por la indicación.
            '------------------------------------------------------------------
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "o_" & nombreClase.Substring(0, 1), nombreClase, "Row2Tipo" & "(dr)"), vbCrLf)
            sb.AppendLine()
            For Each col As DataColumn In mDataTable.Columns
                sb.AppendFormat("        o_{0}.{1} = o_{2}.{1}{3}{4}", nombreClase.Substring(0, 1), campos(col.ColumnName).ToString, nombreClase, ConvLang.FinInstruccion(), vbCrLf)
            Next
            sb.AppendLine()
            sb.AppendFormat("        {0}2Row(o_{1}, dr){2}{3}", nombreClase, nombreClase.Substring(0, 1), ConvLang.FinInstruccion(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Instruccion("dt.Rows.Add(dr)"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndSub(), vbCrLf)
            '
            ' Métodos públicos compartidos (estáticos)
            '
            sb.AppendLine()
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("------------------"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario(" Métodos públicos"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Comentario("------------------"), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' Tabla: devuelve una tabla con los datos indicados en la cadena de selección
            ' hay dos sobrecargas: una sin parámetros y
            ' otra en la que se indica la cadena de selección a usar
            '------------------------------------------------------------------
            sb.AppendFormat("{0}{1}", ConvLang.DocumentacionXML("    ", {" Devuelve una tabla con los datos indicados en la cadena de selección"}), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "Tabla", "DataTable"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Return("Tabla(CadenaSelect)"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "Tabla", "DataTable", "sel", "String"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Comentario(String.Format(" devuelve una tabla con los datos de la tabla {0}", nombreTabla)), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Variable("Dim", "da", dbPrefix & "DataAdapter"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.VariableNewParam("Dim", "dt", "DataTable", String.Format("{0}{1}{0}", Chr(34), nombreClase)), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Try(), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.AsignaNew("da", dbPrefix & "DataAdapter(sel, CadenaConexion)"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("da.Fill(dt)"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.Catch(), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Return("Nothing"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndTry(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("dt"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' Buscar                                                (10/Jul/04)
            '------------------------------------------------------------------
            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ", {" Busca en la tabla los datos indicados en el parámetro.",
                                                                      " El parámetro contendrá lo que se usará después del WHERE.",
                                                                      " Si no se encuentra lo buscado, se devuelve un valor nulo."}))
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "Buscar", nombreClase, "sWhere", "String"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "o_" & nombreClase, nombreClase, "Nothing"), vbCrLf)
            'Dim sel As String = "SELECT * FROM Clientes WHERE " & sWhere
            sb.AppendFormat("        {0}{1}", ConvLang.Variable("Dim", "sel", "String", String.Format("{0}SELECT * FROM {1} WHERE {0} & sWhere", Chr(34), nombreTabla)), vbCrLf)
            'Using con As New SqlConnection(CadenaConexion)
            sb.AppendFormat("        {0}{1}", ConvLang.Using("con", "SqlConnection", "CadenaConexion"), vbCrLf)
            'Dim cmd As New SqlCommand()
            sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariableNew("Dim", "cmd", "SqlCommand"), vbCrLf)
            'Dim reader As SqlDataReader
            sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "reader", "SqlDataReader"), vbCrLf)
            'cmd.CommandType = CommandType.Text
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandType", "CommandType.Text"), vbCrLf)
            'cmd.Connection = con
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.Connection", "con"), vbCrLf)
            'cmd.CommandText = sel
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandText", "sel"), vbCrLf)
            'Try
            sb.AppendFormat("            {0}{1}", ConvLang.Try(), vbCrLf)
            'con.Open()
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Open()"), vbCrLf)
            'reader = cmd.ExecuteReader()
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("reader", "cmd.ExecuteReader()"), vbCrLf)
            'While reader.Read
            sb.AppendFormat("                {0}{1}", ConvLang.While("reader.Read()"), vbCrLf)
            sb.AppendFormat("                    {0}{1}", ConvLang.Asigna("o_" & nombreClase, "Reader2Tipo(reader)"), vbCrLf)
            'If o_Clientes.ID > 0 Then
            sb.AppendFormat("                    {0}{1}", ConvLang.If($"o_{nombreClase}.ID", ">", "0"), vbCrLf)
            'Exit While
            sb.AppendFormat("                        {0}{1}", ConvLang.Exit("While"), vbCrLf)
            sb.AppendFormat("                    {0}{1}", ConvLang.EndIf(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.EndWhile(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("reader.Close()"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Close()"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Return("Nothing"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.EndTry(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndUsing(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("o_" & nombreClase), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            ' Métodos públicos de instancia
            '
            '------------------------------------------------------------------
            ' Actualizar: Actualiza los datos en la tabla usando la instancia actual
            '------------------------------------------------------------------

            '
            ' Cambio la actualización e inserción por comandos,     (22/Mar/19)
            ' sin usar el dataAdapter
            '

            sb.AppendFormat("{0}", ConvLang.DocumentacionXML("    ",
                                                             {" Actualizar: Actualiza los datos en la tabla usando la instancia actual",
                                                              " Si la instancia no hace referencia a un registro existente, se creará uno nuevo",
                                                              " Para comprobar si el objeto en memoria coincide con uno existente,",
                                                              " se comprueba si el " & campoIDnombre & " existe en la tabla.",
                                                              " TODO: Si en lugar de " & campoIDnombre & " usas otro campo, indicalo en la cadena SELECT",
                                                              " También puedes usar la sobrecarga en la que se indica la cadena SELECT a usar"}))
            If UsarOverrides Then
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Overrides", "Actualizar", "String"), vbCrLf)
            Else
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public", "Actualizar", "String"), vbCrLf)
            End If

            sb.AppendFormat("        {0} TODO: Poner aquí la selección a realizar para acceder a este registro{1}", ConvLang.Comentario(), vbCrLf)
            sb.AppendFormat("        {0}       yo uso el {2} que es el identificador único de cada registro{1}", ConvLang.Comentario(), vbCrLf, campoIDnombre)
            If campoIDtipo.IndexOf("String") > -1 Then
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "sel", "String", String.Format("{0}SELECT * FROM {1} WHERE {2} = '{0} & Me.{2} & {0}'{0}", Chr(34), nombreTabla, campoIDnombre)), vbCrLf)
            Else
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "sel", "String", String.Format("{0}SELECT * FROM {1} WHERE {2} = {0} & Me.{2}.ToString()", Chr(34), nombreTabla, campoIDnombre)), vbCrLf)
            End If
            sb.AppendFormat("        {0}{1}", ConvLang.Return("Actualizar(sel)"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {" Actualiza los datos de la instancia actual.",
                                                      " En caso de error, devolverá la cadena de error empezando por ERROR:."}))
            If UsarDataAdapter Then
                sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                      {" Si la instancia no hace referencia a un registro existente, se creará uno nuevo."}))
            Else
                sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                      {" Usando ExecuteNonQuery si la instancia no hace referencia a un registro existente, NO se creará uno nuevo."}))
            End If
            If UsarOverrides Then
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Overrides", "Actualizar", "String", "sel", "String"), vbCrLf)
            Else
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public", "Actualizar", "String", "sel", "String"), vbCrLf)
            End If
            sb.AppendFormat("        {1} Actualiza los datos indicados{0}", vbCrLf, ConvLang.Comentario())
            sb.AppendFormat("        {1} El parámetro, que es una cadena de selección, indicará el criterio de actualización{0}", vbCrLf, ConvLang.Comentario())
            sb.AppendLine()
            ' El código para usar Command o DataAdapter             (07/Abr/19)
            If UsarDataAdapter Then
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "cnn", dbPrefix & "Connection"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "da", dbPrefix & "DataAdapter"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariableNewParam("Dim", "dt", "DataTable", String.Format("{0}{1}{0}", Chr(34), nombreClase)), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.AsignaNew("cnn", dbPrefix & "Connection", "CadenaConexion"), vbCrLf)
                sb.AppendFormat("        {0}{1}{2}", ConvLang.Comentario(), ConvLang.AsignaNew("da", dbPrefix & "DataAdapter", "CadenaSelect" & ", cnn"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.AsignaNew("da", dbPrefix & "DataAdapter", "sel, cnn"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Asigna("da.MissingSchemaAction", "MissingSchemaAction.AddWithKey"), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("-------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Esta no es la más óptima, pero funcionará"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("-------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {2}{0}{1}", ConvLang.DeclaraVariableNewParam("Dim", "cb", dbPrefix & "CommandBuilder", "da"), vbCrLf, ConvLang.Comentario(Not usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna("da.UpdateCommand", "cb.GetUpdateCommand()"), vbCrLf, ConvLang.Comentario(Not usarCommandBuilder))
                sb.AppendLine()
                '
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("--------------------------------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Esta está más optimizada pero debes comprobar que funciona bien..."), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("--------------------------------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {2}{0}{1}", ConvLang.DeclaraVariable("Dim", "sCommand", "String"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Comentario(), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{1} El comando UPDATE{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{1} TODO: Comprobar cual es el campo de índice principal (sin duplicados){0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {3}{1}       Yo compruebo que sea un campo llamado {2}, pero en tu caso puede ser otro{0}", vbCrLf, ConvLang.Comentario(), campoIDnombre, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {3}{1}       Ese campo, (en mi caso {2}) será el que hay que poner al final junto al WHERE.{0}", vbCrLf, ConvLang.Comentario(), campoIDnombre, ConvLang.Comentario(usarCommandBuilder))
            Else
                ' Dim msg = ""
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "msg", "String"), vbCrLf)
                sb.AppendLine()
                ' Using con As New SqlConnection(CadenaConexion)
                sb.AppendFormat("        {0}{1}", ConvLang.Using("con", "SqlConnection", "CadenaConexion"), vbCrLf)
                '     Dim tran As SqlTransaction = Nothing
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "tran", "SqlTransaction", "Nothing"), vbCrLf)
                '     Try
                sb.AppendFormat("            {0}{1}", ConvLang.Try(), vbCrLf)
                '         con.Open()
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("con.Open()"), vbCrLf)
                '         tran = con.BeginTransaction()
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("tran", "con.BeginTransaction()"), vbCrLf)
                '         Dim cmd As New SqlCommand()
                sb.AppendLine()
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariableNew("Dim", "cmd", "SqlCommand"), vbCrLf)
                '         cmd.CommandType = CommandType.StoredProcedure
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandType", "CommandType.StoredProcedure")), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandType", "CommandType.Text"), vbCrLf)
                '         cmd.Connection = con
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.Connection", "con"), vbCrLf)
                '         cmd.CommandText = "ActualizarCliente"
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandText", $"""Actualizar{nombreTabla}""")), vbCrLf, ChrW(34))
                sb.AppendLine()
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "sCommand", "String"), vbCrLf)
            End If
            '
            sb1 = New System.Text.StringBuilder
            sb2 = New System.Text.StringBuilder
            '
            sb1.AppendFormat("{0}UPDATE {1} SET ", Chr(34), nombreTabla)
            '
            s = ""
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                ' si el campo tiene caracteres no válidos           (14/Jul/04)
                ' ponerlo entre corchetes
                s = col.ColumnName
                If campos(col.ColumnName).ToString <> s Then
                    s = "[" & col.ColumnName & "]"
                End If
                If campos(col.ColumnName).ToString <> campoIDnombre Then
                    If esSQL Then
                        sb2.AppendFormat("{0} = @{0}, ", s)
                    Else
                        sb2.AppendFormat("{0} = ?, ", s)
                    End If
                End If
            Next
            s = sb2.ToString.TrimEnd
            If s.EndsWith(",") Then
                sb1.AppendFormat("{0} ", s.Substring(0, s.Length - 1))
            Else
                sb1.AppendFormat("{0} ", s)
            End If
            If esSQL Then
                sb1.AppendFormat(" WHERE ({1} = @{1}){0}", Chr(34), campoIDnombre)
            Else
                sb1.AppendFormat(" WHERE ({1} = ?){0}", Chr(34), campoIDnombre)
            End If
            '
            If UsarDataAdapter Then
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna("sCommand", sb1.ToString), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.AsignaNew("da.UpdateCommand", dbPrefix & "Command", "sCommand, cnn"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                If esSQL Then
                    For Each col As DataColumn In mDataTable.Columns
                        Select Case col.DataType.ToString
                            Case "System.String"
                                If col.MaxLength > 255 Then
                                    sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", col.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                    If UsarAddWithValue Then
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, 0, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NText, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NText, {1}, {2}{0}{2}", col.ColumnName, 0, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                                Else
                                    If UsarAddWithValue Then
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NVarChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                                End If
                            Case Else
                                If UsarAddWithValue Then
                                    s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34), tipoSQL(col.DataType.ToString))
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{1}@{0}{1}, {2}, 0, {1}{0}{1}", col.ColumnName, Chr(34), tipoSQL(col.DataType.ToString))
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                        End Select
                    Next
                Else
                    Dim j As Integer = mDataTable.Columns.Count
                    Dim k As Integer
                    Dim sp(j) As String
                    For k = 0 To j
                        sp(k) = "p" & (k + 1).ToString
                    Next
                    k = 0
                    Dim sp1 As String
                    Dim colID As DataColumn = Nothing
                    For Each col As DataColumn In mDataTable.Columns
                        If campos(col.ColumnName).ToString = campoIDnombre Then
                            colID = col
                        Else
                            sp1 = sp(k)
                            k += 1
                            Select Case col.DataType.ToString
                                Case "System.String"
                                    If col.MaxLength > 255 Then
                                        sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", col.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                        If UsarAddWithValue Then
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, 0, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        Else
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, 0, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        End If
                                    Else
                                        If UsarAddWithValue Then
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        Else
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        End If
                                    End If
                                Case Else
                                    If UsarAddWithValue Then
                                        s = String.Format("{1}@{3}{1}, {0}", col.ColumnName, Chr(34), tipoOleDb(col.DataType.ToString), sp1)
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{1}@{3}{1}, {2}, 0, {1}{0}{1}", col.ColumnName, Chr(34), tipoOleDb(col.DataType.ToString), sp1)
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                            End Select
                        End If
                    Next
                    sp1 = sp(j - 1)
                    Select Case colID.DataType.ToString
                        Case "System.String"
                            If colID.MaxLength > 255 Then
                                sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", colID.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                If UsarAddWithValue Then
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, 0, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, 0, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                            Else
                                If UsarAddWithValue Then
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                            End If
                        Case Else
                            If UsarAddWithValue Then
                                s = String.Format("{1}@{3}{1}, {0}", colID.ColumnName, Chr(34), tipoOleDb(colID.DataType.ToString), sp1)
                                sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                            Else
                                sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                s = String.Format("{1}@{3}{1}, {2}, 0, {1}{0}{1}", colID.ColumnName, Chr(34), tipoOleDb(colID.DataType.ToString), sp1)
                                sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                            End If
                    End Select
                    s = String.Format("{0}@{2}{0}, {1}, 0, {0}{0}", Chr(34), "OleDbType.Integer", sp(j))
                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                End If
                '
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Try(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("da.Fill(dt)"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""ERROR: "" & ex.Message"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                '
                sb.AppendFormat("        {0}{1}", ConvLang.If("dt.Rows.Count", "=", "0"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" crear uno nuevo"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("Crear()"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Else(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion(nombreClase & "2Row(Me, dt.Rows(0))"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndIf(), vbCrLf)
                '
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Try(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("da.Update(dt)"), vbCrLf)
                sb.AppendFormat("            dt.AcceptChanges(){0}{1}", ConvLang.FinInstruccion(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""Actualizado correctamente"""), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""ERROR: "" & ex.Message"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndTry(), vbCrLf)

            Else
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("sCommand", sb1.ToString), vbCrLf)
                ' cmd.CommandText = sCommand                        (26/Abr/19)
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandText", "sCommand"), vbCrLf)
                sb.AppendLine()
                ' No usar With ya que C# no lo soporta... (ni lo tengo definido :-P )
                ' Nota: solo para SQL Server y AddWithValue
                '         cmd.Parameters.AddWithValue("@ID", ID)
                For Each col As DataColumn In mDataTable.Columns
                    Select Case col.DataType.ToString
                        Case "System.String"
                            s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34))
                            sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("cmd.Parameters.AddWithValue(" & s & ")"), vbCrLf)
                        Case Else
                            s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34))
                            sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("cmd.Parameters.AddWithValue(" & s & ")"), vbCrLf)
                    End Select
                Next
                sb.AppendLine()
                '         cmd.Transaction = tran
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.Transaction", "tran"), vbCrLf)
                '         cmd.ExecuteNonQuery()
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("cmd.ExecuteNonQuery()"), vbCrLf)
                sb.AppendLine()
                '         ' Si llega aquí es que todo fue bien,
                '         ' por tanto, llamamos al método Commit
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" Si llega aquí es que todo fue bien,"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" por tanto, llamamos al método Commit."), vbCrLf)
                '         tran.Commit()
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("tran.Commit()"), vbCrLf)
                sb.AppendLine()
                '         msg = "Se ha actualizado el Cliente correctamente."
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("msg", """Se ha actualizado un " & nombreClase & " correctamente."""), vbCrLf)
                sb.AppendLine()
                '     Catch ex As Exception
                sb.AppendFormat("            {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                '         msg = $"ERROR: {ex.Message}"
                sb.AppendFormat("              {0}{1}", ConvLang.Asigna("msg", "$""ERROR: {ex.Message}"""), vbCrLf)
                '         ' Si hay error, deshacemos lo que se haya hecho
                sb.AppendFormat("              {0}{1}", ConvLang.Comentario(" Si hay error, deshacemos lo que se haya hecho."), vbCrLf)
                '         Try
                sb.AppendFormat("              {0}{1}", ConvLang.Try(), vbCrLf)
                ' Añadir comprobación de nulo en el objeto tran     (17-abr-21)
                '   If tran IsNot Nothing Then
                sb.AppendFormat("                  {0}{1}", ConvLang.If("tran", "IsNot", "Nothing"), vbCrLf)
                '             tran.Rollback()
                sb.AppendFormat("                        {0}{1}", ConvLang.Instruccion("tran.Rollback()"), vbCrLf)
                ' End If
                sb.AppendFormat("                  {0}{1}", ConvLang.EndIf, vbCrLf)
                '         Catch ex2 As Exception
                sb.AppendFormat("              {0}{1}", ConvLang.Catch("ex2", "Exception"), vbCrLf)
                '             msg &= $" (ERROR RollBack: {ex.Message})"
                sb.AppendFormat("               {0}{1}", ConvLang.Asigna("msg", "$""ERROR RollBack: {ex2.Message}"""), vbCrLf)
                '         End Try
                sb.AppendFormat("              {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("            {0}{1}", ConvLang.Finally, vbCrLf)
                ' If Not (con is nothing) then
                sb.AppendFormat("              {0}{1}", ConvLang.If("", "Not", "(con Is Nothing)"), vbCrLf)
                '     con.Close()
                sb.AppendFormat("                  {0}{1}", ConvLang.Instruccion("con.Close()"), vbCrLf)
                ' End If
                sb.AppendFormat("              {0}{1}", ConvLang.EndIf, vbCrLf)
                '     End Try
                sb.AppendFormat("            {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                ' End Using
                sb.AppendFormat("            {0}{1}", ConvLang.EndUsing(), vbCrLf)
                sb.AppendLine()
                ' Return msg
                sb.AppendFormat("            {0}{1}", ConvLang.Return("msg"), vbCrLf)
            End If
            '
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' Crear: Crea un nuevo registro usando el contenido de la instancia
            '------------------------------------------------------------------
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {" Crear un nuevo registro",
                                                      " En caso de error, devolverá la cadena de error empezando por ERROR:."}))
            If UsarOverrides Then
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Overrides", "Crear", "String"), vbCrLf)
            Else
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public", "Crear", "String"), vbCrLf)
            End If
            If UsarDataAdapter Then
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "cnn", dbPrefix & "Connection"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "da", dbPrefix & "DataAdapter"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariableNewParam("Dim", "dt", "DataTable", String.Format("{0}{1}{0}", Chr(34), nombreClase)), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.AsignaNew("cnn", dbPrefix & "Connection", "CadenaConexion"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.AsignaNew("da", dbPrefix & "DataAdapter", "CadenaSelect" & ", cnn"), vbCrLf)
                sb.AppendFormat("        {0}{1}{2}", ConvLang.Comentario(), ConvLang.AsignaNew("da", dbPrefix & "DataAdapter", "CadenaSelect" & ", CadenaConexion"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Asigna("da.MissingSchemaAction", "MissingSchemaAction.AddWithKey"), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("-------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Esta no es la más óptima, pero funcionará"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("-------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {2}{0}{1}", ConvLang.DeclaraVariableNewParam("Dim", "cb", dbPrefix & "CommandBuilder", "da"), vbCrLf, ConvLang.Comentario(Not usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna("da.InsertCommand", "cb.GetInsertCommand()"), vbCrLf, ConvLang.Comentario(Not usarCommandBuilder))
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("--------------------------------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" Esta está más optimizada pero debes comprobar que funciona bien..."), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Comentario("--------------------------------------------------------------------"), vbCrLf)
                sb.AppendFormat("        {2}{0}{1}", ConvLang.DeclaraVariable("Dim", "sCommand", "String"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Comentario(), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0} El comando INSERT{1}", ConvLang.Comentario(), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0} TODO: No incluir el campo de clave primaria incremental{1}", ConvLang.Comentario(), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {3}{0}       Yo compruebo que sea un campo llamado {2}, pero en tu caso puede ser otro{1}", ConvLang.Comentario(), vbCrLf, campoIDnombre, ConvLang.Comentario(usarCommandBuilder))
            Else
                ' Dim msg = ""
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "msg", "String"), vbCrLf)
                sb.AppendLine()
                ' Using con As New SqlConnection(CadenaConexion)
                sb.AppendFormat("        {0}{1}", ConvLang.Using("con", "SqlConnection", "CadenaConexion"), vbCrLf)
                '     Dim tran As SqlTransaction = Nothing
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "tran", "SqlTransaction", "Nothing"), vbCrLf)
                sb.AppendLine()
                '     Try
                sb.AppendFormat("            {0}{1}", ConvLang.Try(), vbCrLf)
                '         con.Open()
                sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Open()"), vbCrLf)
                '         tran = con.BeginTransaction()
                sb.AppendFormat("                {0}{1}", ConvLang.Asigna("tran", "con.BeginTransaction()"), vbCrLf)
                sb.AppendLine()
                '         Dim cmd As New SqlCommand()
                sb.AppendFormat("                {0}{1}", ConvLang.DeclaraVariableNew("Dim", "cmd", "SqlCommand"), vbCrLf)
                '         cmd.CommandType = CommandType.StoredProcedure
                sb.AppendFormat("                {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandType", "CommandType.StoredProcedure")), vbCrLf)
                sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.CommandType", "CommandType.Text"), vbCrLf)
                '         cmd.Connection = con
                sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.Connection", "con"), vbCrLf)
                '         cmd.CommandText = "CrearCliente"
                sb.AppendFormat("                {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandText", $"""Crear{nombreTabla}""")), vbCrLf, ChrW(34))
                sb.AppendLine()
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "sCommand", "String"), vbCrLf)
            End If
            '
            sb1 = New System.Text.StringBuilder
            sb2 = New System.Text.StringBuilder
            sb3 = New System.Text.StringBuilder
            sb1.AppendFormat("{0}INSERT INTO {1} (", Chr(34), nombreTabla)
            '
            For i As Integer = 0 To mDataTable.Columns.Count - 1
                Dim col As DataColumn = mDataTable.Columns(i)
                s = col.ColumnName
                If campos(col.ColumnName).ToString <> s Then
                    s = "[" & col.ColumnName & "]"
                End If
                ' si no es AutoIncrement debe estar en los parámetros
                If col.AutoIncrement = False Then 'If col.ColumnName <> campoIDnombre Then
                    sb2.AppendFormat("{0}, ", s)
                    If esSQL Then
                        sb3.AppendFormat("@{0}, ", s)
                    Else
                        sb3.Append("?, ")
                    End If
                End If
            Next
            s = sb2.ToString.TrimEnd
            If s.EndsWith(",") Then
                sb1.AppendFormat("{0}", s.Substring(0, s.Length - 1))
            Else
                sb1.AppendFormat("{0}", s)
            End If
            sb1.Append(") ")
            '
            s = sb3.ToString.TrimEnd
            If s.EndsWith(",") Then
                sb1.AppendFormat(" VALUES({0}) ", s.Substring(0, s.Length - 1))
            Else
                sb1.AppendFormat(" VALUES({0}) ", s)
            End If
            '
            If UsarDataAdapter = False Then
                ' Añadirle SELECT @@Identity al comando INSERT
                sb1.Append("SELECT @@Identity")
            End If
            sb1.AppendFormat("{0}", Chr(34))
            '
            If UsarDataAdapter Then
                sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna("sCommand", sb1.ToString), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                sb.AppendFormat("        {2}{0}{1}", ConvLang.AsignaNew("da.InsertCommand", dbPrefix & "Command", "sCommand, cnn"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                '
                If esSQL Then
                    For Each col As DataColumn In mDataTable.Columns
                        Select Case col.DataType.ToString
                            Case "System.String"
                                If col.MaxLength > 255 Then
                                    sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", col.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                    If UsarAddWithValue Then
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, 0, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NText, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NText, {1}, {2}{0}{2}", col.ColumnName, 0, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                                Else
                                    If UsarAddWithValue Then
                                        s = String.Format("{2}@{0}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        s = String.Format("{2}@{0}{2}, SqlDbType.NVarChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34))
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                                End If
                            Case Else
                                If UsarAddWithValue Then
                                    s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34), tipoSQL(col.DataType.ToString))
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{1}@{0}{1}, {2}, 0, {1}{0}{1}", col.ColumnName, Chr(34), tipoSQL(col.DataType.ToString))
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                        End Select
                    Next
                Else
                    Dim j As Integer = mDataTable.Columns.Count
                    Dim k As Integer
                    Dim sp(j) As String
                    For k = 0 To j
                        sp(k) = "p" & (k + 1).ToString
                    Next
                    k = 0
                    Dim sp1 As String
                    Dim colID As DataColumn = Nothing
                    For Each col As DataColumn In mDataTable.Columns
                        If campos(col.ColumnName).ToString = campoIDnombre Then
                            colID = col
                        Else
                            sp1 = sp(k)
                            k += 1
                            Select Case col.DataType.ToString
                                Case "System.String"
                                    If col.MaxLength > 255 Then
                                        sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", col.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                        If UsarAddWithValue Then
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, 0, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        Else
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.UpdateCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, 0, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        End If
                                    Else
                                        If UsarAddWithValue Then
                                            s = String.Format("{2}@{3}{2}, {0}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        Else
                                            s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", col.ColumnName, col.MaxLength, Chr(34), sp1)
                                            sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                        End If
                                    End If
                                Case Else
                                    If UsarAddWithValue Then
                                        s = String.Format("{1}@{3}{1}, {0}", col.ColumnName, Chr(34), tipoOleDb(col.DataType.ToString), sp1)
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    Else
                                        sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                        s = String.Format("{1}@{3}{1}, {2}, 0, {1}{0}{1}", col.ColumnName, Chr(34), tipoOleDb(col.DataType.ToString), sp1)
                                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    End If
                            End Select
                        End If
                    Next
                    sp1 = sp(j - 1)
                    Select Case colID.DataType.ToString
                        Case "System.String"
                            If colID.MaxLength > 255 Then
                                sb.AppendFormat("        {3}{2} TODO: Este campo seguramente es MEMO y el valor debería ser cero en lugar de {0}{1}", colID.MaxLength, vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                If UsarAddWithValue Then
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, 0, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {3}{0}{1}{2}", ConvLang.Comentario(), ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, 0, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                            Else
                                If UsarAddWithValue Then
                                    s = String.Format("{2}@{3}{2}, {0}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                Else
                                    s = String.Format("{2}@{3}{2}, OleDbType.VarWChar, {1}, {2}{0}{2}", colID.ColumnName, colID.MaxLength, Chr(34), sp1)
                                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                                End If
                            End If
                        Case Else
                            If UsarAddWithValue Then
                                s = String.Format("{1}@{3}{1}, {0}", colID.ColumnName, Chr(34), tipoOleDb(colID.DataType.ToString), sp1)
                                sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.AddWithValue(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                            Else
                                sb.AppendFormat("        {2}{1} TODO: Comprobar el tipo de datos a usar...{0}", vbCrLf, ConvLang.Comentario(), ConvLang.Comentario(usarCommandBuilder))
                                s = String.Format("{1}@{3}{1}, {2}, 0, {1}{0}{1}", colID.ColumnName, Chr(34), tipoOleDb(colID.DataType.ToString), sp1)
                                sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                            End If
                    End Select
                    s = String.Format("{0}@{2}{0}, {1}, 0, {0}{0}", Chr(34), "OleDbType.Integer", sp(j))
                    sb.AppendFormat("        {2}{0}{1}", ConvLang.Instruccion("da.InsertCommand.Parameters.Add(" & s & ")"), vbCrLf, ConvLang.Comentario(usarCommandBuilder))
                End If
                '
                sb.AppendLine()
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Try(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("da.Fill(dt)"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""ERROR: "" & ex.Message"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Instruccion("nuevo" & nombreClase & "(dt, Me)"), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("        {0}{1}", ConvLang.Try(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("da.Update(dt)"), vbCrLf)
                sb.AppendFormat("            dt.AcceptChanges(){0}{1}", ConvLang.FinInstruccion(), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""Se ha creado un nuevo " & nombreClase & Chr(34)), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Return("""ERROR: "" & ex.Message"), vbCrLf)
                sb.AppendFormat("        {0}{1}", ConvLang.EndTry(), vbCrLf)
            Else
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("sCommand", sb1.ToString), vbCrLf)
                ' cmd.CommandText = sCommand                        (26/Abr/19)
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandText", "sCommand"), vbCrLf)
                sb.AppendLine()
                ' No usar With ya que C# no lo soporta... (ni lo tengo definido :-P )
                ' Nota: solo para SQL Server y AddWithValue
                '         cmd.Parameters.AddWithValue("@ID", ID)
                For Each col As DataColumn In mDataTable.Columns
                    If col.AutoIncrement = False Then
                        Select Case col.DataType.ToString
                            Case "System.String"
                                s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34))
                                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("cmd.Parameters.AddWithValue(" & s & ")"), vbCrLf)
                            Case Else
                                s = String.Format("{1}@{0}{1}, {0}", col.ColumnName, Chr(34))
                                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("cmd.Parameters.AddWithValue(" & s & ")"), vbCrLf)
                        End Select
                    End If
                Next
                sb.AppendLine()
                '         cmd.Transaction = tran
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.Transaction", "tran"), vbCrLf)
                sb.AppendLine()
                '         Dim id As Integer = CInt(cmd.ExecuteScalar())
                sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "id", "Integer", "CInt(cmd.ExecuteScalar())"), vbCrLf)
                '         Me.ID = id
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("Me.ID", "id"), vbCrLf)
                sb.AppendLine()
                '         ' Si llega aquí es que todo fue bien,
                '         ' por tanto, llamamos al método Commit
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" Si llega aquí es que todo fue bien,"), vbCrLf)
                sb.AppendFormat("            {0}{1}", ConvLang.Comentario(" por tanto, llamamos al método Commit."), vbCrLf)
                '         tran.Commit()
                sb.AppendFormat("            {0}{1}", ConvLang.Instruccion("tran.Commit()"), vbCrLf)
                sb.AppendLine()
                '         msg = "Se ha actualizado el Cliente correctamente."
                sb.AppendFormat("            {0}{1}", ConvLang.Asigna("msg", """Se ha creado un " & nombreClase & " correctamente."""), vbCrLf)
                sb.AppendLine()
                '     Catch ex As Exception
                sb.AppendFormat("            {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
                '         msg = $"ERROR: {ex.Message}"
                sb.AppendFormat("              {0}{1}", ConvLang.Asigna("msg", "$""ERROR: {ex.Message}"""), vbCrLf)
                '         Try
                sb.AppendFormat("              {0}{1}", ConvLang.Try(), vbCrLf)
                '         ' Si hay error, deshacemos lo que se haya hecho
                sb.AppendFormat("                {0}{1}", ConvLang.Comentario(" Si hay error, deshacemos lo que se haya hecho."), vbCrLf)
                ' Añadir comprobación de nulo en el objeto tran     (17-abr-21)
                '   If tran IsNot Nothing Then
                sb.AppendFormat("                  {0}{1}", ConvLang.If("tran", "IsNot", "Nothing"), vbCrLf)
                '             tran.Rollback()
                sb.AppendFormat("                        {0}{1}", ConvLang.Instruccion("tran.Rollback()"), vbCrLf)
                ' End If
                sb.AppendFormat("                  {0}{1}", ConvLang.EndIf, vbCrLf)
                '         Catch ex2 As Exception
                sb.AppendFormat("              {0}{1}", ConvLang.Catch("ex2", "Exception"), vbCrLf)
                '             msg &= $" (ERROR RollBack: {ex.Message})"
                sb.AppendFormat("                {0}{1}", ConvLang.Asigna("msg", "$""ERROR RollBack: {ex2.Message})"""), vbCrLf)
                '         End Try
                sb.AppendFormat("              {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                sb.AppendFormat("            {0}{1}", ConvLang.Finally, vbCrLf)
                ' If Not (con is nothing) then
                sb.AppendFormat("              {0}{1}", ConvLang.If("", "Not", "(con Is Nothing)"), vbCrLf)
                '     con.Close()
                sb.AppendFormat("                  {0}{1}", ConvLang.Instruccion("con.Close()"), vbCrLf)
                ' End If
                sb.AppendFormat("              {0}{1}", ConvLang.EndIf, vbCrLf)
                '     End Try
                sb.AppendFormat("            {0}{1}", ConvLang.EndTry(), vbCrLf)
                sb.AppendLine()
                ' End Using
                sb.AppendFormat("            {0}{1}", ConvLang.EndUsing(), vbCrLf)
                sb.AppendLine()
                ' Return msg
                sb.AppendFormat("            {0}{1}", ConvLang.Return("msg"), vbCrLf)
            End If
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            '------------------------------------------------------------------
            ' Borrar: Borra el registro con el mismo ID que tenga la clase
            '         En caso de que quieras usar otro criterio para comprobar cual es el registro actual
            '         cambia la comparación
            '------------------------------------------------------------------
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {" Borrar el registro con el mismo ID que tenga la clase.",
                                                      " NOTA: En caso de que quieras usar otro criterio",
                                                      " para comprobar cuál es el registro actual, cambia la comparación."}))
            If UsarOverrides Then
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Overrides", "Borrar", "String"), vbCrLf)
            Else
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public", "Borrar", "String"), vbCrLf)
            End If
            sb.AppendFormat("        {0} TODO: Poner aquí la selección a realizar para acceder a este registro{1}", ConvLang.Comentario(), vbCrLf)
            sb.AppendFormat("        {0}       yo uso el {2} que es el identificador único de cada registro{1}", ConvLang.Comentario(), vbCrLf, campoIDnombre)
            If campoIDtipo.IndexOf("String") > -1 Then
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "where", "String", String.Format("{0}{2} = '{0} & Me.{2} & {0}'{0}", Chr(34), nombreTabla, campoIDnombre)), vbCrLf)
            Else
                sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "where", "String", String.Format("{0}{2} = {0} & Me.{2}.ToString()", Chr(34), nombreTabla, campoIDnombre)), vbCrLf)
            End If
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("Borrar(where)"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {" Borrar el registro o los registros indicados en la cadena WHERE.",
                                                      " La cadena indicada se usará después de la cláusula WHERE de TSQL.",
                                                      " Ejemplo, si la cadena es: Nombre = 'Pepe' AND Telefono = '666777999'",
                                                      " Ejecutará: WHERE Nombre = 'Pepe' AND Telefono = '666777999'",
                                                      " Y borrará todos los registros de esta tabla que coincidan."}))
            If UsarOverrides Then
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Overrides", "Borrar", "String", "where", "String"), vbCrLf)
            Else
                sb.AppendFormat("    {0}{1}", ConvLang.Function("Public", "Borrar", "String", "where", "String"), vbCrLf)
            End If
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "msg", "String", Chr(34) & Chr(34)), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "sCon", "String", "CadenaConexion"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariable("Dim", "sel", "String", String.Format("{1}DELETE FROM {0} WHERE {1} & where", nombreClase, Chr(34))), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Using("con", "SqlConnection", "sCon"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "tran", "SqlTransaction", "Nothing"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("            {0}{1}", ConvLang.Try(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.DeclaraVariableNew("Dim", "cmd", "SqlCommand"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandType", "CommandType.StoredProcedure")), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.CommandType", "CommandType.Text"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.Connection", "con"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Comentario(ConvLang.Asigna("cmd.CommandText", $"""Borrar{nombreTabla}""")), vbCrLf, ChrW(34))
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.CommandText", "sel"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Open()"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("tran", "con.BeginTransaction()"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("cmd.Transaction", "tran"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("cmd.ExecuteNonQuery()"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("                {0}{1}", ConvLang.Comentario(" Si llega aquí es que todo fue bien,"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Comentario(" por tanto, llamamos al método Commit."), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("tran.Commit()"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("msg", """Eliminado correctamente los registros con : "" & where & ""."""), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("            {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("msg", """ERROR al eliminar los registros con : "" & where & ""."" & ex.Message"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("              {0}{1}", ConvLang.Try(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("tran.Rollback()"), vbCrLf)
            sb.AppendFormat("              {0}{1}", ConvLang.Catch("ex2", "Exception"), vbCrLf)
            sb.AppendFormat("               {0}{1}", ConvLang.Asigna("msg", "$""ERROR RollBack: {ex2.Message})"""), vbCrLf)
            sb.AppendFormat("              {0}{1}", ConvLang.EndTry(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("            {0}{1}", ConvLang.Finally, vbCrLf)
            ' If Not (con is nothing) then
            sb.AppendFormat("              {0}{1}", ConvLang.If("", "Not", "(con Is Nothing)"), vbCrLf)
            '     con.Close()
            sb.AppendFormat("                  {0}{1}", ConvLang.Instruccion("con.Close()"), vbCrLf)
            ' End If
            sb.AppendFormat("              {0}{1}", ConvLang.EndIf, vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.EndTry(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.EndUsing(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("msg"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            ' TablaCol y Reader2Tipo                                (24/May/19)
            '
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {$" Devuelve una colección de {nombreClase} según la cadena select."}))
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "TablaCol", $"List(Of {nombreClase})", "sel", "String"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.DeclaraVariableNew("Dim", "col", $"List(Of {nombreClase})"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Using("con", "SqlConnection", "CadenaConexion"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariableNew("Dim", "cmd", "SqlCommand"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.DeclaraVariable("Dim", "reader", "SqlDataReader"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandType", "CommandType.Text"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.Connection", "con"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Asigna("cmd.CommandText", "sel"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Try(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Open()"), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("                {0}{1}", ConvLang.Asigna("reader", "cmd.ExecuteReader()"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.While("reader.Read()"), vbCrLf)
            sb.AppendFormat("                    {0}{1}", ConvLang.DeclaraVariable("Dim", "r", nombreClase, "Reader2Tipo(reader)"), vbCrLf)
            sb.AppendFormat("                    {0}{1}", ConvLang.Instruccion("col.Add(r)"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.EndWhile(), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("reader.Close()"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Instruccion("con.Close()"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.Catch("ex", "Exception"), vbCrLf)
            sb.AppendFormat("                {0}{1}", ConvLang.Return("col"), vbCrLf)
            sb.AppendFormat("            {0}{1}", ConvLang.EndTry(), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.EndUsing(), vbCrLf)
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("col"), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            sb.AppendFormat(ConvLang.DocumentacionXML("    ",
                                                     {" Asigna los datos desde un SqlDataReader."}))
            sb.AppendFormat("    {0}{1}", ConvLang.Function("Public Shared", "Reader2Tipo", nombreClase, "r", "SqlDataReader"), vbCrLf)
            sb.AppendFormat("        {0}{1}", ConvLang.VariableNew("Dim", "o_" & nombreClase, nombreClase), vbCrLf)
            sb.AppendLine()
            For Each col As DataColumn In mDataTable.Columns
                Select Case col.DataType.ToString
                    Case "System.String"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""].ToString()", col.ColumnName)), vbCrLf)
                    Case "System.Int16", "System.Int32", "System.Int64",
                         "System.Single", "System.Decimal", "System.Double",
                         "System.Byte", "System.SByte", "System.UInt16", "System.UInt32", "System.UInt64",
                         "System.Boolean", "System.DateTime", "System.Char", "System.TimeSpan"
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("ConversorTipos.{1}Data(r[""{0}""].ToString())", col.ColumnName, col.DataType.ToString().Replace("System.", ""))), vbCrLf)
                    Case "System.Byte[]"
                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""]", col.ColumnName)), vbCrLf, ConvLang.Comentario())
                    Case Else
                        ' El resto de tipos
                        sb.AppendFormat("        {0}{1}", ConvLang.Comentario(" TODO: Comprobar la conversión a realizar"), vbCrLf)
                        sb.AppendFormat("        {0}{1}", ConvLang.Comentario(String.Format("       con el tipo {0}", col.DataType.ToString)), vbCrLf)
                        sb.AppendFormat("        {2}{0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("r[""{0}""]", col.ColumnName)), vbCrLf, ConvLang.Comentario())
                        sb.AppendFormat("        {0}{1}", ConvLang.Asigna(String.Format("o_{0}.{1}", nombreClase, campos(col.ColumnName).ToString), String.Format("{1}.Parse(r[""{0}""].ToString())", col.ColumnName, col.DataType.ToString)), vbCrLf)
                End Select
            Next
            sb.AppendLine()
            sb.AppendFormat("        {0}{1}", ConvLang.Return("o_" & nombreClase), vbCrLf)
            sb.AppendFormat("    {0}{1}", ConvLang.EndFunction(), vbCrLf)
            sb.AppendLine()
            '
            sb.AppendFormat("{0}{1}", ConvLang.EndClass(), vbCrLf)
            '
            Return sb.ToString
        End Function
        '
        Private Shared Function tipoSQL(ByVal elTipo As String) As String
            Dim aCTS() As String = {"System.Boolean", "System.Int16", "System.Int32", "System.Int64", _
                                    "System.Decimal", "System.Single", "System.Double", "System.Byte", _
                                    "System.DateTime", "System.Guid", "System.Object"}
            Dim aSQL() As String = {"Bit", "SmallInt", "Int", "BigInt", _
                                    "Decimal", "Real", "Float", "TinyInt", _
                                    "DateTime", "UniqueIdentifier", "Variant"}
            Dim i As Integer = Array.IndexOf(aCTS, elTipo)
            If i > -1 Then
                Return "SqlDbType." & aSQL(i)
            End If
            Return "SqlDbType.Int"
        End Function
        '
        Private Shared Function tipoOleDb(ByVal elTipo As String) As String
            Dim aCTS() As String = {"System.Byte[]", "System.Boolean", "System.Int16", "System.Int32", "System.Int64", _
                                    "System.Decimal", "System.Single", "System.Double", "System.Byte", _
                                    "System.DateTime", "System.Guid", "System.Object", "System.String"}
            Dim aOle() As String = {"LongVarBinary", "Boolean", "SmallInt", "Integer", "BigInt", _
                                    "Decimal", "Single", "Double", "UnsignedTinyInt", _
                                    "Date", "Guid", "Variant", "VarWChar"}
            Dim i As Integer = Array.IndexOf(aCTS, elTipo)
            If i > -1 Then
                Return "OleDbType." & aOle(i)
            End If
            Return "OleDbType.Integer"
        End Function
    End Class
End Namespace