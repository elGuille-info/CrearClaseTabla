'------------------------------------------------------------------------------
' Utilidad para generar clases basadas en tablas de bases de datos  (07/Jul/04)
'
' Basada en crearClasesVB                                           (21/Mar/04)
'
' Para Visual Basic 2005 usando elGuille.Util.Developer.Data        (17/Abr/07)
'
' ©Guillermo 'guille' Som, 2004-2005, 2007, 2018-2019, 2021-2022
'------------------------------------------------------------------------------
' Revisiones:
'   0.0000  07/Jul/2004 Empiezo con los cambios para usar tablas SQL
'                       Un solo método para crear tanto código de VB como de C#
'                       que se apoya en la clase ConvLang
'           08/Jul/2004 Cambio bastante en el código generado
'           08/Jul/2004 Creo una clase para la conexión y generación del código
'           10/Jul/2004 Mejoras en la creación de la clase
'           13/Jul/2004 La misma aplicación para SQL y OleDb
'                       Las clases de creación están en una librería aparte
'   0.1000  14/Jul/2004 Publico la utilidad
'   0.1010  02/Nov/2004 Si el nombre de la clase contiene espacios,
'                       sustituirlos por un guión bajo  (David Sans)
'   0.1030  07/Feb/2005 Errores al borrar un registro y al cambiar de tabla
'   0.1040  15/Mar/2005 En el password ahora se muestra *
'
'   1.0000  17/Nov/2018 Añado a la clase CrearClase de la DLL DeveloperData:
'                       Para generar Option Infer On y Imports Microsoft.VisualBasic
'   1.0001  20/Nov/2018 Bug al añadir Imports Microsoft.VisualBasic
'                       (añadía dos veces Imports)
'   2.0000  30/Nov/2018 Utilizo el .NET 4.7.2
'
'   2.0001  15/Dic/2018 Utilizo ConversorTipos para convertir de cadena a un tipo
'                       Los tipos convertidos son:
'                       Int16, Int32, Int64, Single, Decimal, Double, Byte, SByte,
'                       UInt16, UInt32, UInt64, Boolean, DateTime, Char, TimeSpan
'   2.0002  19/Mar/2019 Añado la opción AddWithValue a los comandos...
'   2.0003  20/Mar/2019 El método Borrar se utiliza con Command en vez de DataAdapter
'                       En convLang añado Using/End Using
'   2.0004  22/Mar/2019 Algunos cambios menores en las clases aparte el de añadir
'                       la opción de poder usar ExecuteScalar en lugar de DataAdapter
'                       No quito el uso de DataAdapter en INSERT y UPDATE
'                       si no que se puede usar ExecuteScalar o un DataAdapter.
'
'   2.0016  17-abr-2021 Poder asignar rápidamente el uso de SQLExpress.
'           02-oct-2021 Seguramente lo cambié a .NET Framework 4.8
'
'   2.1.0   01-oct-2022 Cambio a .NET Framework 4.8.1 y cambios en CrearClase.
'   2.1.0.7 01-oct-2022 Añado un StatusStrip
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On
Option Infer On
'
Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
'
Imports elGuille.Util.Developer.Data
Imports elGuille.Util.Developer

Public Class Form1

    ''' <summary>
    ''' Devuelve el valor de FileVersion usando FileVersionInfo.
    ''' </summary>
    ''' <returns>El valor de FileVersion.</returns>
    ''' <remarks>01/Oct/22</remarks>
    Private Function VersionDLL() As String
        Dim s As String

        Try
            Dim ensamblado = GetType(Form1).Assembly
            'Dim ensamblado = System.Reflection.Assembly.GetExecutingAssembly()
            Dim fvi = FileVersionInfo.GetVersionInfo(ensamblado.Location)
            ' FileDescription en realidad muestra (o eso parece) lo mismo de ProductName
            s = fvi.FileVersion

        Catch ex As Exception
            s = "2.1.0.7"
        End Try

        Return s
    End Function

    Private ValoresAntSQLExpress As (usarSQL As Boolean, segIntegrada As Boolean, dataSource As String, initialCatalog As String)

    Private inicializando As Boolean = True

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ts As TimeSpan = Nothing

        ' centrar el formulario horizontalmente
        Me.Left = (Screen.PrimaryScreen.Bounds.Width - Me.Width) \ 2

        ' Solo asignar los valores guardados si no es un valor negativo (20/Dic/20)
        If My.Settings.Left <> -1 Then
            If My.Settings.Left > -1 Then
                Me.Left = My.Settings.Left
                Me.Top = My.Settings.Top
            End If
        End If
        ' Añdir información al status. (01/oct/22 11.55)
        Dim sCopyR = "©Guillermo Som (elGuille), 2004-2007, 2018-"
        Dim elAño = 2022
        If Date.Today.Year > 2022 Then
            elAño = Date.Today.Year
        End If
        LabelInfo.Text = $"  {sCopyR}{elAño}  "
        ' Usando My.Application.Info.Version.ToString(3) devuelve solo las 3 primeras cifras.
        LabelVersion.Text = $"  {My.Application.Info.ProductName} - v{My.Application.Info.Version.ToString(3)} ({VersionDLL()})  "

        txtSelect.Text = ""
        txtCodigo.Text = ""
        txtClase.Text = ""
        '
        txtDataSource.Text = My.Settings.SQLDataSource
        txtInitialCatalog.Text = My.Settings.SQLInitialCatalog
        chkSeguridadSQL.Checked = My.Settings.SQLSeguridad
        txtUserId.Text = My.Settings.SQLUserId
        txtPassword.Text = My.Settings.SQLPassword
        '
        txtNombreBase.Text = My.Settings.OleDbBaseDeDatos
        txtProvider.Text = My.Settings.OleDbProvider
        txtAccessPassword.Text = My.Settings.OleDbPassword
        '
        If My.Settings.UsarSQL Then
            optAccess.Checked = False
            optSQL.Checked = True
        Else
            optAccess.Checked = True
            optSQL.Checked = False
        End If
        grbSQL.Enabled = optSQL.Checked
        grbAccess.Enabled = optAccess.Checked
        '
        If My.Settings.Lenguaje.ToLower() = "vb" Then
            optVB.Checked = True
        Else
            optCS.Checked = True
        End If

        ' Guardar los valores iniciales, por si se usa chkUsarSQLEXpress (17/Abr/21)
        ValoresAntSQLExpress = (optSQL.Checked, chkSeguridadSQL.Checked, txtDataSource.Text, txtInitialCatalog.Text)

        inicializando = False

        chkUsarCommandBuilder.Checked = My.Settings.UsarCommandBuilder
        chkUsarAddWithValue.Checked = My.Settings.UsarAddWithValue
        chkUsarDataAdapter.Checked = My.Settings.UsarExecuteScalar
        chkUsarOverrides.Checked = My.Settings.usarOverrides
        '
        btnGenerarClase.Enabled = False
        Panel1.Enabled = False
        btnGuardar.Enabled = False
    End Sub
    '
    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        guardarCfg()
    End Sub
    '
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Close()
    End Sub
    '
    Private Sub cboTablas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTablas.SelectedIndexChanged
        txtSelect.Text = "SELECT * FROM " & cboTablas.Text
        Dim i As Integer = cboTablas.Text.IndexOf(".")
        ' Si la tabla contiene espacios,                            (02/Nov/04)
        ' sustituirlos por guiones bajos.
        ' Bug reportado por David Sans
        If i > -1 Then
            txtClase.Text = cboTablas.Text.Substring(i + 1).Replace(" ", "_")
        Else
            txtClase.Text = cboTablas.Text.Replace(" ", "_")
        End If
    End Sub
    '
    Private Sub btnMostrarTablas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMostrarTablas.Click
        guardarCfg()

        ' No tener en cuenta la cadena select para mostrar las tablas
        txtSelect.Text = ""
        If optSQL.Checked Then
            CrearClaseSQL.Conectar(txtDataSource.Text, txtInitialCatalog.Text, txtSelect.Text, txtUserId.Text, txtPassword.Text, chkSeguridadSQL.Checked)
        Else
            CrearClaseOleDb.Conectar(Me.txtNombreBase.Text, txtSelect.Text, txtProvider.Text, txtAccessPassword.Text)
        End If
        '
        btnGenerarClase.Enabled = CrearClase.Conectado
        Panel1.Enabled = CrearClase.Conectado
        btnGuardar.Enabled = CrearClase.Conectado
        '
        If CrearClase.Conectado = False Then Return
        '
        Dim nomTablas() As String
        If optSQL.Checked Then
            nomTablas = CrearClaseSQL.NombresTablas()
        Else
            nomTablas = CrearClaseOleDb.NombresTablas()
        End If
        '
        If (nomTablas Is Nothing) OrElse nomTablas(0).StartsWith("ERROR") Then
            btnGenerarClase.Enabled = False
            Panel1.Enabled = False
            btnGuardar.Enabled = False
            'Return
        End If
        '
        cboTablas.Items.Clear()
        If Not nomTablas Is Nothing Then
            For i As Integer = 0 To nomTablas.Length - 1
                cboTablas.Items.Add(nomTablas(i))
            Next
        End If
        If cboTablas.Items.Count > 0 Then
            cboTablas.SelectedIndex = 0
        End If
    End Sub
    '
    Private Sub btnGenerarClase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarClase.Click
        ' generar la clase a partir de la tabla seleccionada
        If txtSelect.Text = "" Then
            MessageBox.Show("Debes especificar la cadena de selección de datos",
                            "Generar clase",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtSelect.Focus()
            Return
        End If
        '
        txtCodigo.Text = ""
        guardarCfg()
        '
        ' Si la tabla contiene espacios,                            (02/Nov/04)
        ' sustituirlos por guiones bajos.
        ' Bug reportado por David Sans
        txtClase.Text = txtClase.Text.Replace(" ", "_")

        ' Lo asigno en la clase base ya que son métodos compartidos (22/Mar/19)
        CrearClase.UsarDataAdapter = chkUsarDataAdapter.Checked
        If chkUsarDataAdapter.Checked = False Then
            'chkUsarCommandBuilder.Checked = False
            chkUsarCommandBuilder.Enabled = False
        End If
        CrearClase.UsarAddWithValue = chkUsarAddWithValue.Checked
        CrearClase.UsarOverrides = chkUsarOverrides.Checked

        ' Si no está habilitado es que no se utiliza                (07/Abr/19)
        ' ya que solo se usa con DataAdapter
        Dim usarCB As Boolean = chkUsarCommandBuilder.Checked
        If chkUsarCommandBuilder.Enabled = False Then
            'chkUsarCommandBuilder.Checked = False
            usarCB = False
        End If
        If optVB.Checked Then
            If optSQL.Checked Then
                txtCodigo.Text = CrearClaseSQL.GenerarClase(eLenguaje.eVBNET, usarCB,
                                                            txtClase.Text, cboTablas.Text, txtDataSource.Text,
                                                            txtInitialCatalog.Text, txtSelect.Text, txtUserId.Text,
                                                            txtPassword.Text, chkSeguridadSQL.Checked)
            Else
                txtCodigo.Text = CrearClaseOleDb.GenerarClase(eLenguaje.eVBNET, usarCB,
                                                              txtClase.Text, cboTablas.Text, txtNombreBase.Text,
                                                              txtSelect.Text, txtAccessPassword.Text, txtProvider.Text)
            End If
        Else
            If optSQL.Checked Then
                txtCodigo.Text = CrearClaseSQL.GenerarClase(eLenguaje.eCS, usarCB,
                                                            txtClase.Text, cboTablas.Text, txtDataSource.Text,
                                                            txtInitialCatalog.Text, txtSelect.Text, txtUserId.Text,
                                                            txtPassword.Text, chkSeguridadSQL.Checked)
            Else
                txtCodigo.Text = CrearClaseOleDb.GenerarClase(eLenguaje.eCS, usarCB,
                                                              txtClase.Text, cboTablas.Text, txtNombreBase.Text,
                                                              txtSelect.Text, txtAccessPassword.Text, txtProvider.Text)
            End If
        End If
    End Sub
    '
    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        ' guarda la clase en un fichero .vb o .cs
        Dim sFic As String = Application.StartupPath & "\" & txtClase.Text
        If optVB.Checked Then
            sFic &= ".vb"
        Else
            sFic &= ".cs"
        End If
        Dim sw As New System.IO.StreamWriter(sFic, False, System.Text.Encoding.Default)
        sw.Write(txtCodigo.Text)
        sw.Close()
    End Sub
    '
    Private Sub chkSeguridadSQL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeguridadSQL.CheckedChanged
        Dim b As Boolean = chkSeguridadSQL.Checked
        labelUser.Enabled = b
        labelPassw.Enabled = b
        txtUserId.Enabled = b
        txtPassword.Enabled = b
    End Sub
    '
    Private Sub optAccess_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAccess.CheckedChanged
        If inicializando Then Return
        grbAccess.Enabled = optAccess.Checked
        ' Con Access usar DataAdapter
        If optAccess.Checked Then
            chkUsarDataAdapter.Checked = True
        End If
    End Sub
    Private Sub optSQL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSQL.CheckedChanged
        If inicializando Then Return
        grbSQL.Enabled = optSQL.Checked
        ' Al cambiar a SQL seleccionar automáticamente Execute... es decir, desmarcar DataAdapter
        If optSQL.Checked Then
            chkUsarDataAdapter.Checked = False
        End If
    End Sub
    '
    Private Sub txtNombreBase_DragOver(
                    ByVal sender As Object,
                    ByVal e As System.Windows.Forms.DragEventArgs) _
                    Handles txtNombreBase.DragOver, MyBase.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub
    '
    Private Sub txtNombreBase_DragDrop(
                    ByVal sender As Object,
                    ByVal e As System.Windows.Forms.DragEventArgs) _
                    Handles txtNombreBase.DragDrop, MyBase.DragDrop
        Dim archivos() As String
        '
        If e.Data.GetDataPresent("FileDrop") Then
            archivos = CType(e.Data.GetData("FileDrop"), String())
            txtNombreBase.Text = archivos(0)
            txtNombreBase.SelectionStart = txtNombreBase.Text.Length
        End If
    End Sub
    '
    Private Sub btnExaminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExaminar.Click
        With New OpenFileDialog
            .Title = "Seleccionar base de datos"
            .Filter = "Bases de Access (*.mdb)|*.mdb"
            .FileName = txtNombreBase.Text
            If .ShowDialog = DialogResult.OK Then
                txtNombreBase.Text = .FileName
            End If
        End With
    End Sub
    '
    Private Sub guardarCfg()
        My.Settings.SQLDataSource = txtDataSource.Text
        My.Settings.SQLInitialCatalog = txtInitialCatalog.Text
        My.Settings.SQLSeguridad = chkSeguridadSQL.Checked
        My.Settings.SQLUserId = txtUserId.Text
        ' guardar siempre una cadena vacía en el password
        ' le indico que la guarde                                   (21/Dic/20)
        My.Settings.SQLPassword = txtPassword.Text

        My.Settings.OleDbBaseDeDatos = txtNombreBase.Text
        My.Settings.OleDbProvider = txtProvider.Text
        My.Settings.OleDbPassword = "" 'txtAccessPassword.Text
        '
        My.Settings.UsarSQL = optSQL.Checked
        '
        If optVB.Checked Then
            My.Settings.Lenguaje = "VB"
        Else
            My.Settings.Lenguaje = "C#"
        End If
        '
        My.Settings.UsarCommandBuilder = chkUsarCommandBuilder.Checked
        My.Settings.UsarAddWithValue = chkUsarAddWithValue.Checked
        My.Settings.UsarExecuteScalar = chkUsarDataAdapter.Checked
        My.Settings.usarOverrides = chkUsarOverrides.Checked
        '
        If WindowState = FormWindowState.Normal Then
            My.Settings.Left = Me.Left
            My.Settings.Top = Me.Top
        Else
            My.Settings.Left = Me.RestoreBounds.Left
            My.Settings.Top = Me.RestoreBounds.Top
        End If
        '
        My.Settings.CopyrightAutor = "Guillermo Som (elGuille)"
        My.Settings.CopyrightVersion = My.Application.Info.Version.ToString
    End Sub

    Private Sub chkUsarDataAdapter_CheckedChanged(sender As Object, e As EventArgs) Handles chkUsarDataAdapter.CheckedChanged
        If inicializando Then Return
        chkUsarCommandBuilder.Enabled = chkUsarDataAdapter.Checked
    End Sub

    Private Sub chkUsarSQLEXpress_CheckedChanged(sender As Object, e As EventArgs) Handles chkUsarSQLEXpress.CheckedChanged
        If inicializando Then Return
        If chkUsarSQLEXpress.Checked Then
            ValoresAntSQLExpress = (optSQL.Checked, chkSeguridadSQL.Checked, txtDataSource.Text, txtInitialCatalog.Text)
            optSQL.Checked = True
            chkSeguridadSQL.Checked = False
            txtDataSource.Text = ".\SQLEXPRESS"
        Else
            optSQL.Checked = ValoresAntSQLExpress.usarSQL
            chkSeguridadSQL.Checked = ValoresAntSQLExpress.segIntegrada
            txtDataSource.Text = ValoresAntSQLExpress.dataSource
            txtInitialCatalog.Text = ValoresAntSQLExpress.initialCatalog
        End If
    End Sub

End Class
