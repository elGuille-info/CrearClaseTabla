'------------------------------------------------------------------------------
' Funciones de conversión a varios tipos                            (23/Jun/15)
' Principalmente pensados en los obtenidos de una base de datos
'
' Definida como clase con los miembros compartidos (es como un módulo)
'
' Primeras funciones creadas en 2011 para Solicitudes empleo
' 
' (c)Guillermo 'guille' Som, 2011, 2015, 2018
'------------------------------------------------------------------------------
Option Strict On
Option Infer On

Imports System


''' <summary>
''' Conversor de tipo usados para cuando se leen o guardan en una base de datos
''' Hay funciones para convertir los siguientes tipos:
''' Boolean, DateTime, Integer, Long
''' 
''' Yo suelo usarlos de esta forma:
''' TIPOData (antes ValorTIPO) para asignar de un valor leído de la base de datos al tipo
''' DataTIPO para asignar lo leído de la base de datos a una cadena
''' </summary>
Public Class ConversorTipos

    '--------------------------------------------------------------------------
    ' Las funciones DataTIPO
    '--------------------------------------------------------------------------

    ''' <summary>
    ''' Convierte un tipo Object en un valor Boolean (como cadena)
    ''' "0" para False, "1" para True
    ''' Ese objeto es el valor leído de la base de datos
    ''' Si el contenido es válido se devuelve el valor
    ''' si no, se devuelve un valor "0"
    ''' </summary>
    Public Shared Function DataBoolean(ByVal obj As Object) As String
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return "0"
        Else
            Return If(BooleanData(obj), "1", "0")
        End If
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un valor Decimal,
    ''' pero se devuelve como cadena.
    ''' Ese objeto es el valor leído de la base de datos
    ''' Si el contenido es válido se devuelve el valor
    ''' si no, se devuelve una cadena vacía.
    ''' Se quitan los ceros que haya después del signo decimal,
    ''' si no tiene decimales, no se muestran los ceros.
    ''' </summary>
    Public Shared Function DataDecimal(ByVal obj As Object) As String
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return ""
        Else
            ' Conversión extra para evitar "sustos"                 (06/Oct/11)
            Dim d As Decimal = 0
            Decimal.TryParse(obj.ToString, d)

            Return d.ToString.TrimEnd("0"c).TrimEnd({"."c, ","c})
        End If
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un valor Integer,
    ''' pero se devuelve como cadena.
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function DataInt32(ByVal obj As Object) As String
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return ""
        Else
            Dim d As Integer = 0
            Integer.TryParse(obj.ToString, d)

            Return d.ToString
        End If
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un valor Long,
    ''' pero se devuelve como cadena.
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function DataInt64(ByVal obj As Object) As String
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return ""
        Else
            Dim d As Long = 0
            Long.TryParse(obj.ToString, d)

            Return d.ToString
        End If
    End Function

    ''' <summary>
    ''' Convierte una fecha en cadena.<br/>
    ''' Si la fecha es 01/01/1900 se devuelve una cadena vacía
    ''' </summary>
    ''' <remarks>24/Jun/15</remarks>
    Public Shared Function DataDateTime(ByVal dt As DateTime) As String
        Return DataDateTime(dt, False)
    End Function

    ''' <summary>
    ''' Convierte una fecha en cadena.<br/>
    ''' Si usarMinValue es False, se devuelve una cadena vacía si la fecha es 1/1/1900
    ''' </summary>
    ''' <remarks>24/Jun/15</remarks>
    Public Shared Function DataDateTime(ByVal dt As DateTime, usarMinValue As Boolean) As String
        Dim s = dt.ToString("dd/MM/yyyy")
        If s = "01/01/1900" AndAlso usarMinValue = False Then s = ""

        Return s
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en una cadena.<br/>
    ''' Ese objeto es el valor leído de la base de datos.<br/>
    ''' Si la fecha es válida se devuelve en formato dd/MM/yyyy,
    ''' si no, se devuelve una cadena vacía.<br/>
    ''' Si la fecha es 01/01/1900 devuelve una cadena vacía.
    ''' </summary>
    Public Shared Function DataDateTime(ByVal obj As Object) As String
        Return DataDateTime(obj, False)
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en una cadena.<br/>
    ''' Ese objeto es el valor leído de la base de datos.<br/>
    ''' Si la fecha es válida se devuelve en formato dd/MM/yyyy,
    ''' si no, se devuelve una cadena vacía.<br/>
    ''' Si la fecha es 01/01/1900 devuelve una cadena vacía,
    ''' salvo si usarMinValue = True, en cuyo caso no se devuelve una cadena vacía
    ''' si no el valor 01/01/1900.
    ''' </summary>
    Public Shared Function DataDateTime(ByVal obj As Object, usarMinValue As Boolean) As String
        Dim sMin As String = ""
        If usarMinValue Then
            sMin = "01/01/1900"
        End If
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return sMin
        Else
            'Return CDate(obj).ToString("dd/MM/yyyy")
            ' por si es una cadena vacía                            (12/Oct/11)
            If TypeOf obj Is String AndAlso String.IsNullOrWhiteSpace(obj.ToString) Then
                Return sMin
            End If

            ' hacer las cosas bien                                  (04/Dic/18)
            Dim d = New Date(1900, 1, 1)
            Date.TryParse(obj.ToString, d)
            Dim s = CDate(d).ToString("dd/MM/yyyy")
            If s = "01/01/1900" AndAlso usarMinValue = False Then s = ""
            Return s
        End If

    End Function

    ''' <summary>
    ''' Convierte una cadena en un valor DateTime
    ''' para asignar los DateTimePicker a partir del valor del TextBox asociado.<br/>
    ''' Si la fecha no es válida se devuelve la fecha actual.<br/>
    ''' También se usa para asignar el campo en los parámetros del SqlCommand.
    ''' </summary>
    Public Shared Function DataDateTimePicker(ByVal str As String) As DateTime
        Dim d As DateTime = DateTime.Now

        If Not (String.IsNullOrWhiteSpace(str) OrElse str.Equals(DBNull.Value)) Then
            DateTime.TryParse(str, d)
            If d.Year < 1900 Then
                d = New DateTime(1900, 1, 1, 0, 0, 0)
            End If
        Else
            ' asignar el 01/01/1900 si es un valor en blanco        (07/Jul/15)
            d = New DateTime(1900, 1, 1, 0, 0, 0)
        End If

        Return d.Date
    End Function

    ''' <summary>
    ''' convierte una cadena en una fecha (DateTime.Date)
    ''' devuelve DBNull si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    Public Shared Function DataDateTimeDate(ByVal str As String) As Object
        Dim d As DateTime

        If DateTime.TryParse(str, d) Then
            Return d.Date
        Else
            Return DBNull.Value
        End If
    End Function

    '--------------------------------------------------------------------------
    ' Las funciones TIPOData
    '--------------------------------------------------------------------------

    ''' <summary>
    ''' Convierte un tipo Object en un valor Boolean
    ''' Ese objeto es el valor leído de la base de datos
    ''' Si el contenido es válido se devuelve el valor
    ''' si no, se devuelve un valor falso.
    ''' </summary>
    Public Shared Function BooleanData(ByVal obj As Object) As Boolean
        If obj Is Nothing OrElse obj.Equals(DBNull.Value) Then
            Return False
        Else
            Dim o = False
            Boolean.TryParse(obj.ToString, o)

            Return o
        End If
    End Function

    ''' <summary>
    ''' convierte una cadena en un valor Decimal
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    Public Shared Function DecimalData(ByVal str As String) As Decimal
        Dim d As Decimal = 0

        Decimal.TryParse(str, d)

        Return d
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un valor Decimal,
    ''' </summary>
    ''' <remarks>04/Dic/18</remarks>
    Public Shared Function DecimalData(ByVal obj As Object) As Decimal
        Return DecimalData(DataDecimal(obj))
    End Function

    ''' <summary>
    ''' convierte una cadena en un Double
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function DoubleData(ByVal str As String) As Double
        Dim i As Double = 0

        Double.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un Single
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function SingleData(ByVal str As String) As Single
        Dim i As Single = 0

        Single.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un Byte
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function ByteData(ByVal str As String) As Byte
        Dim i As Byte = 0

        Byte.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un SByte
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function SByteData(ByVal str As String) As SByte
        Dim i As SByte = 0

        SByte.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un Int16
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function Int16Data(ByVal str As String) As Int16
        Dim i As Int16 = 0

        Int16.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un Integer
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    Public Shared Function Int32Data(ByVal str As String) As Integer
        Dim i As Integer = 0

        Integer.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un Integer
    ''' </summary>
    ''' <remarks>04/Dic/18</remarks>
    Public Shared Function Int32Data(ByVal obj As Object) As Integer
        Return Int32Data(obj.ToString)
    End Function

    ''' <summary>
    ''' convierte una cadena en un Long (Int64)
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function Int64Data(ByVal str As String) As Int64
        Dim i As Int64 = 0

        Int64.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' Convierte un tipo Object en un Long
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function Int64Data(ByVal obj As Object) As Long
        Return Int64Data(obj.ToString)
    End Function

    ''' <summary>
    ''' convierte una cadena en un UInt16
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function UInt16Data(ByVal str As String) As UInt16
        Dim i As UInt16 = 0

        UInt16.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un UInt32 (UInteger)
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    Public Shared Function UInt32Data(ByVal str As String) As UInt32
        Dim i As UInt32 = 0

        UInt32.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un UInt64 (Ulong)
    ''' devuelve cero si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function UInt64Data(ByVal str As String) As UInt64
        Dim i As UInt64 = 0

        UInt64.TryParse(str, i)

        Return i
    End Function

    ''' <summary>
    ''' convierte una cadena en un Char
    ''' devuelve un valor Nothing (nulo) si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function CharData(str As String) As Char
        Dim c As Char = Nothing

        Char.TryParse(str, c)

        Return c
    End Function

    ''' <summary>
    ''' Convierte una cadena en una fecha (Date)
    ''' Si la fecha es incorrecta devuelve el 01/01/1900
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function DateTimeData(str As String) As Date
        Dim f = New Date(1900, 1, 1)

        Date.TryParse(str, f)

        Return f
    End Function

    ''' <summary>
    ''' Convierte una cadena en una fecha (Date)
    ''' Si la fecha es incorrecta devuelve el 01/01/1900
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function DateTimeData(obj As Object) As Date
        Return DateTimeData(obj.ToString)
    End Function

    ''' <summary>
    ''' convierte una cadena en un TimeSpan
    ''' devuelve {00:00:00} (nulo) si no es un valor adecuado (cadena vacía o nulo)
    ''' </summary>
    ''' <remarks>15/Dic/18</remarks>
    Public Shared Function TimeSpanData(str As String) As TimeSpan
        Dim c As TimeSpan = Nothing

        TimeSpan.TryParse(str, c)

        Return c
    End Function

End Class
