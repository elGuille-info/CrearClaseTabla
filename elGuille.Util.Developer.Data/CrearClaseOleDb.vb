'------------------------------------------------------------------------------
' Clase para crear una clase a partir de una tabla de Access        (12/Jul/04)
'
' Basado en el código de    crearClaseVB                            (21/Mar/04)
'                           CrearClaseSQL                           (07/Jul/04)
'
' Todos los métodos son estáticos (compartidos) para usarlos sin crear una instancia
'
' Nota: Ver las revisiones en Revisiones.txt
'
' ©Guillermo 'guille' Som, 2004, 2005, 2007
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On 
'
Imports System
Imports Microsoft.VisualBasic
'
Imports System.Data
Imports System.Data.OleDb

Imports elGuille.Util.Developer

Namespace elGuille.Util.Developer.Data
    Public Class CrearClaseOleDb
        Inherits CrearClase
        '
        '
        Public Shared Function Conectar(ByVal baseDeDatos As String) As String
            Return Conectar(baseDeDatos, "", "", "")
        End Function
        Public Shared Function Conectar(ByVal baseDeDatos As String, _
                                        ByVal cadenaSelect As String) As String
            Return Conectar(baseDeDatos, cadenaSelect, "", "")
        End Function
        Public Shared Function Conectar(ByVal baseDeDatos As String, _
                                        ByVal cadenaSelect As String, _
                                        ByVal provider As String) As String
            Return Conectar(baseDeDatos, cadenaSelect, provider, "")
        End Function
        Public Shared Function Conectar(ByVal baseDeDatos As String, _
                                        ByVal cadenaSelect As String, _
                                        ByVal provider As String, _
                                        ByVal password As String) As String
            ' si se produce algún error, se devuelve una cadena empezando por ERROR
            '
            Conectado = False
            '
            If provider = "" Then
                provider = "Microsoft.Jet.OLEDB.4.0"
            ElseIf provider.IndexOf("Provider=") > -1 Then
                Dim i As Integer = provider.IndexOf("=")
                provider = provider.Substring(i)
            End If
            '
            If password <> "" Then
                cadenaConexion = "Provider=" & provider & "; Data Source=" & baseDeDatos & "; Jet OLEDB:Database Password=" & password
            Else
                cadenaConexion = "Provider=" & provider & "; Data Source=" & baseDeDatos
            End If
            '
            If cadenaSelect = "" Then
                Conectado = True
                Return ""
            End If
            '
            Dim dbDataAdapter As New OleDbDataAdapter(cadenaSelect, cadenaConexion)
            '
            dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
            '
            ' Limpiar el contenido de mDataTable                    (08/Jun/05)
            mDataTable = New DataTable
            '
            Try
                dbDataAdapter.Fill(mDataTable)
                System.Threading.Thread.Sleep(100)
                Conectado = True
            Catch ex As OleDbException
                If ex.Message.ToLower().IndexOf("not a valid password") > -1 Then
                    Return "ERROR en fill, el password indicado no es correcto (o no se ha indicado): " & ex.Message & " - " & ex.GetType().Name
                Else
                    Return "ERROR en fill:" & vbCrLf & ex.Message & " - " & ex.GetType().Name
                End If
            Catch ex As Exception
                Return "ERROR en fill:" & vbCrLf & ex.Message & " - " & ex.GetType().Name
            End Try
            '
            Return ""
        End Function
        '
        Public Shared Function NombresTablas() As String()
            Dim nomTablas() As String = Nothing
            Dim dt As DataTable
            Dim restrictions() As Object = {Nothing, Nothing, Nothing, "TABLE"}
            Dim dbConnection As New OleDbConnection(cadenaConexion)
            '
            Try
                dbConnection.Open()
            Catch ex As Exception
                ReDim nomTablas(0)
                nomTablas(0) = "ERROR: " & ex.Message
                Conectado = False
                Return nomTablas
            End Try
            '
            dt = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions)
            Dim i As Integer = dt.Rows.Count - 1
            If i > -1 Then
                ReDim nomTablas(i)
                For i = 0 To dt.Rows.Count - 1
                    'nomTablas(i) = dt.Rows(i)("TABLE_NAME").ToString()
                    nomTablas(i) = dt.Rows(i).Item("TABLE_NAME").ToString()
                Next
            End If
            ' 
            Return nomTablas
        End Function
        '
        Public Shared Function GenerarClase(ByVal lang As eLenguaje, _
                                            ByVal usarCommandBuilder As Boolean, _
                                            ByVal nombreClase As String, _
                                            ByVal nomTabla As String, _
                                            ByVal baseDeDatos As String, _
                                            ByVal cadenaSelect As String, _
                                            ByVal password As String, _
                                            ByVal provider As String) As String
            Dim s As String
            '
            nombreTabla = nomTabla
            If nombreTabla = "" OrElse nombreClase = "" Then
                Return "ERROR, no se ha indicado el nombre de la tabla o de la clase."
            End If
            s = Conectar(baseDeDatos, cadenaSelect, provider, password)
            If Conectado = False OrElse s <> "" Then
                Return s
            End If
            '
            ' Comprobar si el nombre de la clase tiene espacios     (02/Nov/04)
            ' de ser así, cambiarlo por un guión bajo.
            ' Bug reportado por David Sans
            nombreClase = nombreClase.Replace(" ", "_")
            '
            Return CrearClase.GenerarClaseOleDb(lang, usarCommandBuilder, nombreClase, baseDeDatos, cadenaSelect, password, provider)
        End Function
        Public Shared Function GenerarClase(ByVal lang As eLenguaje, _
                                            ByVal usarCommandBuilder As Boolean, _
                                            ByVal nombreClase As String, _
                                            ByVal nomTabla As String, _
                                            ByVal baseDeDatos As String, _
                                            ByVal cadenaSelect As String) As String
            '
            Return GenerarClase(lang, usarCommandBuilder, nombreClase, nomTabla, baseDeDatos, cadenaSelect, "", "")
        End Function
        Public Shared Function GenerarClase(ByVal lang As eLenguaje, _
                                            ByVal usarCommandBuilder As Boolean, _
                                            ByVal nombreClase As String, _
                                            ByVal nomTabla As String, _
                                            ByVal baseDeDatos As String, _
                                            ByVal cadenaSelect As String, _
                                            ByVal provider As String) As String
            '
            Return GenerarClase(lang, usarCommandBuilder, nombreClase, nomTabla, baseDeDatos, cadenaSelect, "", provider)
        End Function
    End Class
End Namespace
