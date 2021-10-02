'------------------------------------------------------------------------------
' Clase para crear trozos de código en VB y C#                      (07/Jul/04)
' los métodos también generan el código de VB
' Inicialmente se usará código de VB para crear el de C#
' al menos en lo que a los tipos de datos, tipos de métodos, etc.
'
' Nota: Ver las revisiones en Revisiones.txt
'
' ©Guillermo 'guille' Som, 2004, 2007, 2018, 2019
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On
Option Infer On
'
Imports System
Imports Microsoft.VisualBasic

Namespace elGuille.Util.Developer
    '
    Public Enum eLenguaje
        eVBNET
        eCS
        VisualBasic = eVBNET
        CSharp = eCS
    End Enum

    Public Class ConvLang
        Public Shared Lang As eLenguaje
        ' las comprobaciones se harán sin tener en cuenta mayúsculas/minúsculas
        Private Shared tiposVB() As String = {"short", "integer", "long", "string", "object", "double", "single", "date", "decimal", "boolean", "byte", "char"}
        Private Shared tiposCS() As String = {"short", "int", "long", "string", "object", "double", "float", "DateTime", "decimal", "bool", "byte", "char"}
        Private Shared modificadoresVB() As String = {"public", "private", "friend", "protected", "shared", "overrides", "overridable", "shadows"}
        Private Shared modificadoresCS() As String = {"public", "private", "internal", "protected", "static", "override", "virtual", "new"}
        Private Shared compVB() As String = {"=", "<>"}
        Private Shared compCS() As String = {"==", "!="}
        Private Shared instrVB() As String = {"IsNot ", " Is", " Nothing", "Not", ".Rows(0)", "New ", ", Me", " Me.", " Me(", " Me,", "(Me,", "True", "False", " & ", Chr(34) & " " & Chr(34) & "c"}
        Private Shared instrCS() As String = {"is !", " is", " null", "!", ".Rows[0]", "new ", ", this", " this.", " this(", " this,", "(this,", "true", "false", " + ", "' '"}

        ''' <summary>
        ''' El parámetro puede incluir más de uno (separado por espacio)
        ''' por ejemplo: Public Shared
        ''' </summary>
        Private Shared Function modificador(ByVal modif As String) As String
            ' el parámetro puede incluir más de uno (separado por espacio)
            ' por ejemplo: Public Shared
            If Lang = eLenguaje.eVBNET Then Return modif
            '
            Dim s1 As String = ""
            Dim i As Integer
            Dim a() As String = modif.Split(" "c)
            '
            For Each s As String In a
                If s <> "" Then
                    i = Array.IndexOf(modificadoresVB, s.ToLower())
                    If i > -1 Then
                        s1 &= " " & modificadoresCS(i)
                    Else
                        s1 &= " " & s
                    End If
                End If
            Next
            Return s1.TrimStart()
        End Function

        '----------------------------------------------------------------------
        ' Nuevos métodos agregados a la versión 2.0                 (30/Nov/18)
        '----------------------------------------------------------------------

        ''' <summary>
        ''' Poner comentarios XML
        ''' Se pondrán en una sola línea.
        ''' </summary>
        Public Shared Function DocumentacionXML(coment As String) As String
            If Lang = eLenguaje.eCS Then
                Return "///<summary>" & coment & "</summary>"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "'''<summary>" & coment & "</summary>"
            End If
        End Function

        ''' <summary>
        ''' Crea documentación XML con varias líneas
        ''' </summary>
        Public Shared Function DocumentacionXML(indentacion As String, coments As String()) As String
            Dim sb As New System.Text.StringBuilder
            Dim iniDocXML = ""

            If Lang = eLenguaje.eCS Then
                iniDocXML = "///"
            Else
                iniDocXML = "'''"
            End If
            sb.AppendFormat("{0}{1}<summary>{2}", indentacion, iniDocXML, vbCrLf)
            For i = 0 To coments.Length - 1
                sb.AppendFormat("{0}{1}{2}{3}", indentacion, iniDocXML, coments(i), vbCrLf)
            Next
            sb.AppendFormat("{0}{1}</summary>{2}", indentacion, iniDocXML, vbCrLf)

            Return sb.ToString
        End Function

        ''' <summary>
        ''' Poner un comentario
        ''' </summary>
        Public Shared Function Comentario() As String
            If Lang = eLenguaje.eCS Then
                Return "//"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "'"
            End If
        End Function
        ' esta sobrecarga la uso para indicar si se pone o no el comentario
        ' (para cuando las opciones son opcionales)
        Public Shared Function Comentario(ByVal poner As Boolean) As String
            If Lang = eLenguaje.eCS Then
                If poner Then
                    Return "//"
                Else
                    Return ""
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If poner Then
                    Return "'"
                Else
                    Return ""
                End If
            End If
        End Function
        Public Shared Function Comentario(ByVal coment As String) As String
            If Lang = eLenguaje.eCS Then
                Return "//" & coment
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "'" & coment
            End If
        End Function
        Public Shared Function [Imports](ByVal espacio As String) As String
            If Lang = eLenguaje.eCS Then
                Return "using " & espacio & ";"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Imports " & espacio
            End If
        End Function
        Public Shared Function [Class](ByVal modif As String, ByVal nombre As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} class {1}{{", modificador(modif), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Class {1}", modif, nombre)
            End If
        End Function
        Public Shared Function EndClass() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Class"
            End If
        End Function
        '
        Public Shared Function Constructor(ByVal nombreClase As String) As String
            Return Constructor("", nombreClase)
        End Function
        Public Shared Function Constructor(ByVal modif As String, ByVal nombreClase As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" Then modif = "Friend"
                Return String.Format("{0} {1}(){{", modificador(modif), nombreClase)
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return "Friend Sub New()"
                Else
                    Return String.Format("{0} Sub New()", modif)
                End If
            End If
        End Function
        Public Shared Function Constructor(ByVal modif As String, ByVal nombreClase As String, ByVal var As String, ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" Then modif = "Friend"
                Return String.Format("{0} {1}({2}){{", modificador(modif), nombreClase, Variable(var, elTipo))
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return "Friend Sub New()"
                Else
                    Return String.Format("{0} Sub New({1})", modif, Variable(var, elTipo))
                End If
            End If
        End Function
        '
        ''' <summary>
        ''' While con argumento
        ''' </summary>
        ''' <remarks>24/May/2019</remarks>
        Public Shared Function [While](var As String) As String
            If Lang = eLenguaje.eCS Then
                Return "while (" & var & ")"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "While " & var
            End If
        End Function

        ''' <summary>
        ''' While sin argumentos
        ''' </summary>
        ''' <remarks>24/May/2019</remarks>
        Public Shared Function [While]() As String
            If Lang = eLenguaje.eCS Then
                Return "while{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "While"
            End If
        End Function

        ''' <summary>
        ''' End While
        ''' </summary>
        ''' <remarks>24/May/2019</remarks>
        Public Shared Function [EndWhile]() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End While"
            End If
        End Function
        '
        Public Shared Function [Try]() As String
            If Lang = eLenguaje.eCS Then
                Return "try{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Try"
            End If
        End Function
        Public Shared Function EndTry() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Try"
            End If
        End Function
        Public Shared Function [Catch]() As String
            If Lang = eLenguaje.eCS Then
                Return "}catch{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Catch"
            End If
        End Function
        Public Shared Function [Catch](ByVal var As String, ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("}}catch({0}){{", Variable(var, elTipo))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Catch " & Variable(var, elTipo)
            End If
        End Function
        Public Shared Function [Finally]() As String
            If Lang = eLenguaje.eCS Then
                Return "finally{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Finally"
            End If
        End Function
        '
        Public Shared Function [Get]() As String
            If Lang = eLenguaje.eCS Then
                Return "get{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Get"
            End If
        End Function
        Public Shared Function EndGet() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Get"
            End If
        End Function
        Public Shared Function [Set](ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                Return "set{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Set(value As " & elTipo & ")"
            End If
        End Function
        Public Shared Function EndSet() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Set"
            End If
        End Function
        '
        Public Shared Function [Property](ByVal modif As String, ByVal elTipo As String, ByVal nombre As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} {2}{{", modificador(modif), Tipo(elTipo), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Property {1}() As {2}", modif, nombre, elTipo)
            End If
        End Function
        Public Shared Function [Property](ByVal modif As String, ByVal elTipo As String, ByVal index As String, ByVal tipoIndex As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} this[{2} {3}]{{", modificador(modif), Tipo(elTipo), Tipo(tipoIndex), index)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Default Property Item({1} As {2}) As {3}", modif, index, tipoIndex, elTipo)
            End If
        End Function
        Public Shared Function PropertyRead(ByVal modif As String, ByVal elTipo As String, ByVal nombre As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} {2}{{", modificador(modif), Tipo(elTipo), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} ReadOnly Property {1}() As {2}", modif, nombre, elTipo)
            End If
        End Function
        Public Shared Function PropertyRead(ByVal modif As String, ByVal elTipo As String, ByVal index As String, ByVal tipoIndex As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} this[{2} {3}]{{", modificador(modif), Tipo(elTipo), Tipo(tipoIndex), index)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Default ReadOnly Property Item({1} As {2}) As {3}", modif, index, tipoIndex, elTipo)
            End If
        End Function
        Public Shared Function PropertyWrite(ByVal modif As String, ByVal elTipo As String, ByVal nombre As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} {2}{{", modificador(modif), Tipo(elTipo), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} WriteOnly Property {1}() As {2}", modif, nombre, elTipo)
            End If
        End Function
        Public Shared Function PropertyWrite(ByVal modif As String, ByVal elTipo As String, ByVal index As String, ByVal tipoIndex As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} this[{2} {3}]{{", modificador(modif), Tipo(elTipo), Tipo(tipoIndex), index)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Default WriteOnly Property Item({1} As {2}) As {3}", modif, index, tipoIndex, elTipo)
            End If
        End Function
        Public Shared Function EndProperty() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Property"
            End If
        End Function
        Public Shared Function [Sub](ByVal modif As String, ByVal nombre As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} void {1}(){{", modificador(modif), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Sub {1}()", modif, nombre)
            End If
        End Function
        Public Shared Function [Sub](ByVal modif As String, ByVal nombre As String, _
                                     ByVal vNombre As String, ByVal vTipo As String, _
                                     ByVal ParamArray vars() As String) As String
            ' los parámetros opcionales (paramarray) se usará para indicar el nombre de la variable y el tipo separados por coma
            ' en caso de usar ByRef (ref en C#) indicarlo en el nombre de la variable: ... uno, String, ByRef dos, Integer
            Dim sb As New System.Text.StringBuilder
            '
            sb.Append(Variable(vNombre, vTipo))
            For i As Integer = 0 To vars.Length - 2 Step 2
                sb.AppendFormat(", {0}", Variable(vars(i), vars(i + 1)))
            Next
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} void {1}({2}){{", modificador(modif), nombre, sb.ToString)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Sub {1}({2})", modif, nombre, sb.ToString)
            End If
        End Function
        Public Shared Function EndSub() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Sub"
            End If
        End Function
        Public Shared Function [Function](ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            '
            If elTipo = "" OrElse elTipo.ToLower = "void" OrElse elTipo.ToLower = "sub" Then
                Return [Sub](modif, nombre)
            End If
            '
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} {2}(){{", modificador(modif), Tipo(elTipo), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Function {1}() As {2}", modif, nombre, elTipo)
            End If
        End Function
        Public Shared Function [Function](ByVal modif As String, ByVal nombre As String, ByVal elTipo As String, _
                                          ByVal vNombre As String, ByVal vTipo As String, _
                                          ByVal ParamArray vars() As String) As String
            '
            If elTipo = "" OrElse elTipo.ToLower = "void" OrElse elTipo.ToLower = "sub" Then
                Return [Sub](modif, nombre, vNombre, vTipo, vars)
            End If
            '
            Dim sb As New System.Text.StringBuilder
            '
            sb.Append(Variable(vNombre, vTipo))
            For i As Integer = 0 To vars.Length - 2 Step 2
                sb.AppendFormat(", {0}", Variable(vars(i), vars(i + 1)))
            Next
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1} {2}({3}){{", modificador(modif), Tipo(elTipo), nombre, sb.ToString)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} Function {1}({2}) As {3}", modif, nombre, sb.ToString, elTipo)
            End If
        End Function
        Public Shared Function EndFunction() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End Function"
            End If
        End Function
        '
        Public Shared Function [If](ByVal var As String, ByVal comp As String, ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                Dim i As Integer = Array.IndexOf(compVB, comp)
                If i > -1 Then comp = compCS(i)
                Return String.Format("if({0} {1} {2}){{", comprobarParam(var), comprobarParam(comp), comprobarParam(valor))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("If {0} {1} {2} Then", var, comp, valor)
            End If
        End Function
        Public Shared Function [ElseIf](ByVal var As String, ByVal comp As String, ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                Dim i As Integer = Array.IndexOf(compVB, comp)
                If i > -1 Then comp = compCS(i)
                Return String.Format("}}else if({0} {1} {2}){{", comprobarParam(var), comprobarParam(comp), comprobarParam(valor))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("ElseIf {0} {1} {2} Then", var, comp, valor)
            End If
        End Function
        Public Shared Function [Else]() As String
            If Lang = eLenguaje.eCS Then
                Return "}else{"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Else"
            End If
        End Function
        Public Shared Function [EndIf]() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End If"
            End If
        End Function
        '
        Public Shared Function [End](ByVal param As String) As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "End " & param
            End If
        End Function
        '
        Public Shared Function [ForEach](ByVal var As String, ByVal elTipo As String, ByVal donde As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("foreach({0} {1} in {2}){{", Tipo(elTipo), var, donde)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("For Each {0} As {1} In {2}", var, elTipo, donde)
            End If
        End Function
        Public Shared Function [For](ByVal var As String, ByVal ini As String, ByVal fin As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("for({0} = {1}; {0} <= {2}; {0}++){{", var, ini, fin)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("For {0} = {1} To {2}", var, ini, fin)
            End If
        End Function
        Public Shared Function [For](ByVal var As String, ByVal ini As String, ByVal fin As String, ByVal incr As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("for({0} = {1}; {0} <= {2}; {0} + {3}){{", var, ini, fin, incr)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("For {0} = {1} To {2} Step {3}", var, ini, fin, incr)
            End If
        End Function
        Public Shared Function [Next]() As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Next"
            End If
        End Function
        Public Shared Function [Next](ByVal param As String) As String
            If Lang = eLenguaje.eCS Then
                Return "}"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Next " & param
            End If
        End Function
        '
        Public Shared Function [Return](ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("return {0};", comprobarParam(valor))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Return " & valor
            End If
        End Function
        Public Shared Function [Exit](ByVal salirDe As String) As String
            If Lang = eLenguaje.eCS Then
                If "function sub property operator".IndexOf(salirDe.ToLower) > -1 Then
                    Return "return;"
                Else
                    Return "break;"
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                Return "Exit " & salirDe
            End If
        End Function

        ''' <summary>
        ''' La declaración de la instrucción Using usando una declaración New.
        ''' En VB: Using con As New SqlConnection(sCon)
        ''' En C#: using (SqlConnection con = new SqlConnection(sCon))
        ''' VB: Dim con As New SqlConnection(sCon)
        '''     Using con
        ''' C#: SqlConnection con = new SqlConnection(sCon)
        '''     using (con)
        ''' </summary>
        ''' <remarks>20/Mar/2019</remarks>
        Public Shared Function [Using](nombre As String, elTipo As String, param As String) As String
            If Lang = eLenguaje.CSharp Then
                Return String.Format("using ({0} {1} = new {0}({2})){{", Tipo(elTipo), nombre, comprobarParam(param))
            Else
                Return String.Format("Using {0} As New {1}({2})", nombre, elTipo, param)
            End If
        End Function

        ''' <summary>
        ''' La declaración de la instrucción Using usando una variable.
        ''' VB: Using con
        ''' C#: using (con)
        ''' </summary>
        ''' <remarks>20/Mar/2019</remarks>
        Public Shared Function [Using](nombre As String) As String
            If Lang = eLenguaje.CSharp Then
                Return String.Format("using ({0})", nombre)
            Else
                Return String.Format("Using {0}", nombre)
            End If
        End Function

        ''' <summary>
        ''' El cierre de la instrucción End Using.
        ''' </summary>
        ''' <remarks>20/Mar/2019</remarks>
        Public Shared Function EndUsing() As String
            If Lang = eLenguaje.CSharp Then
                Return "}"
            Else
                Return "End Using"
            End If
        End Function

        Public Shared Function Asigna(ByVal var As String, ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} = {1};", comprobarParam(var), comprobarParam(valor))
            Else 'If Lang = eLenguaje.eVBNET Then
                If valor.StartsWith(Chr(34)) = False Then
                    Return String.Format("{0} = {1}", var, valor).Replace("[", "(").Replace("]", ")")
                Else
                    Return String.Format("{0} = {1}", var, valor)
                End If
            End If
        End Function
        Public Shared Function AsignaNew(ByVal var As String, ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} = new {1};", comprobarParam(var), comprobarParam(valor))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} = New {1}", var, valor)
            End If
        End Function
        Public Shared Function AsignaNew(ByVal var As String, ByVal valor As String, ByVal param As String) As String
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} = new {1}({2});", comprobarParam(var), comprobarParam(valor), comprobarParam(param))
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} = New {1}({2})", var, valor, param)
            End If
        End Function
        '
        Public Shared Function Instruccion(ByVal cod As String) As String
            If Lang = eLenguaje.eCS Then
                Return comprobarParam(cod) & ";"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return cod
            End If
        End Function
        Public Shared Function FinInstruccion() As String
            If Lang = eLenguaje.eCS Then
                Return ";"
            Else 'If Lang = eLenguaje.eVBNET Then
                Return ""
            End If
        End Function
        '
        Public Shared Function Tipo(ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                Dim i As Integer = Array.IndexOf(tiposVB, elTipo.ToLower)
                If i > -1 Then
                    elTipo = tiposCS(i)
                End If
            End If
            Return elTipo
        End Function
        '
        Public Shared Function Variable(ByVal nombre As String, ByVal elTipo As String) As String
            ' El tipo de datos lo daremos en formato VB
            If Lang = eLenguaje.eCS Then
                Return String.Format("{0} {1}", Tipo(elTipo), nombre)
            Else 'If Lang = eLenguaje.eVBNET Then
                Return String.Format("{0} As {1}", nombre, elTipo)
            End If
        End Function
        Public Shared Function DeclaraVariable(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            Return Variable(modif, nombre, elTipo)
        End Function
        Public Shared Function Variable(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" OrElse modif.ToLower = "dim" Then
                    Return String.Format("{0} {1};", Tipo(elTipo), nombre)
                Else
                    Return String.Format("{0} {1} {2};", modificador(modif), Tipo(elTipo), nombre)
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return String.Format("{0} As {1}", nombre, elTipo)
                Else
                    Return String.Format("{0} {1} As {2}", modif, nombre, elTipo)
                End If
            End If
        End Function
        Public Shared Function DeclaraVariable(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String, ByVal valor As String) As String
            Return Variable(modif, nombre, elTipo, valor)
        End Function
        Public Shared Function Variable(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String, ByVal valor As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" OrElse modif.ToLower = "dim" Then
                    Return String.Format("{0} {1} = {2};", Tipo(elTipo), nombre, comprobarParam(valor))
                Else
                    Return String.Format("{0} {1} {2} = {3};", modificador(modif), Tipo(elTipo), nombre, comprobarParam(valor))
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return String.Format("{0} As {1} = {2}", nombre, elTipo, valor)
                Else
                    Return String.Format("{0} {1} As {2} = {3}", modif, nombre, elTipo, valor)
                End If
            End If
        End Function
        Public Shared Function DeclaraVariableNew(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            Return VariableNew(modif, nombre, elTipo)
        End Function
        Public Shared Function VariableNew(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" OrElse modif.ToLower = "dim" Then
                    If elTipo.IndexOf("(") > -1 Then
                        Return String.Format("{0} {1} = new {0};", Tipo(elTipo), nombre)
                    Else
                        Return String.Format("{0} {1} = new {0}();", Tipo(elTipo), nombre)
                    End If
                Else
                    If elTipo.IndexOf("(") > -1 Then
                        Return String.Format("{0} {1} {2} = new {1};", modificador(modif), Tipo(elTipo), nombre)
                    Else
                        Return String.Format("{0} {1} {2} = new {1}();", modificador(modif), Tipo(elTipo), nombre)
                    End If
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return String.Format("{0} As New {1}", nombre, elTipo)
                Else
                    Return String.Format("{0} {1} As New {2}", modif, nombre, elTipo)
                End If
            End If
        End Function
        Public Shared Function DeclaraVariableNewParam(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            Return VariableNewParam(modif, nombre, elTipo)
        End Function
        Public Shared Function VariableNewParam(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" OrElse modif.ToLower = "dim" Then
                    Return String.Format("{0} {1} = new {0};", Tipo(elTipo), nombre)
                Else
                    Return String.Format("{0} {1} {2} = new {1};", modificador(modif), Tipo(elTipo), nombre)
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return String.Format("{0} As New {1}", nombre, elTipo)
                Else
                    Return String.Format("{0} {1} As New {2}", modif, nombre, elTipo)
                End If
            End If
        End Function
        Public Shared Function DeclaraVariableNewParam(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String, ByVal param As String) As String
            Return VariableNewParam(modif, nombre, elTipo, param)
        End Function
        Public Shared Function VariableNewParam(ByVal modif As String, ByVal nombre As String, ByVal elTipo As String, ByVal param As String) As String
            If Lang = eLenguaje.eCS Then
                If modif = "" OrElse modif.ToLower = "dim" Then
                    Return String.Format("{0} {1} = new {0}({2});", Tipo(elTipo), nombre, comprobarParam(param))
                Else
                    Return String.Format("{0} {1} {2} = new {1}({3});", modificador(modif), Tipo(elTipo), nombre, comprobarParam(param))
                End If
            Else 'If Lang = eLenguaje.eVBNET Then
                If modif = "" Then
                    Return String.Format("{0} As New {1}({2})", nombre, elTipo, param)
                Else
                    Return String.Format("{0} {1} As New {2}({3})", modif, nombre, elTipo, param)
                End If
            End If
        End Function
        '
        ' comprobar si tiene New...
        Private Shared Function comprobarParam(ByVal var As String) As String
            ' por ejemplo, en un parámetro se puede indicar "New LosQueSea"
            If Lang = eLenguaje.eCS Then
                For i As Integer = 0 To instrVB.Length - 1
                    var = (" " & var).Replace(instrVB(i), instrCS(i)).Substring(1)
                Next
                '
                Return var ' (" " & var).Replace("New ", "new ").Replace(" Me.", " this.").Replace(" Me(", " this(").Replace(" Me,", " this,").Replace("(Me,", "(this,").Replace(" True", " true").Replace(" False", " false")
            Else
                Return var
            End If
        End Function
    End Class
End Namespace