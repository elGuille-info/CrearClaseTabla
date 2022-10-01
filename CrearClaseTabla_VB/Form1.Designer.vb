<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms requiere el siguiente procedimiento
    'Puede modificarse utilizando el Diseñador de Windows Forms. 
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.cboTablas = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnMostrarTablas = New System.Windows.Forms.Button()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.btnGenerarClase = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtClase = New System.Windows.Forms.TextBox()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSelect = New System.Windows.Forms.TextBox()
        Me.grbAccess = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAccessPassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtProvider = New System.Windows.Forms.TextBox()
        Me.btnExaminar = New System.Windows.Forms.Button()
        Me.txtNombreBase = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.grbSQL = New System.Windows.Forms.GroupBox()
        Me.chkUsarSQLEXpress = New System.Windows.Forms.CheckBox()
        Me.chkSeguridadSQL = New System.Windows.Forms.CheckBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUserId = New System.Windows.Forms.TextBox()
        Me.txtInitialCatalog = New System.Windows.Forms.TextBox()
        Me.txtDataSource = New System.Windows.Forms.TextBox()
        Me.labelPassw = New System.Windows.Forms.Label()
        Me.labelUser = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.optCS = New System.Windows.Forms.RadioButton()
        Me.optVB = New System.Windows.Forms.RadioButton()
        Me.optAccess = New System.Windows.Forms.RadioButton()
        Me.optSQL = New System.Windows.Forms.RadioButton()
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkUsarCommandBuilder = New System.Windows.Forms.CheckBox()
        Me.chkUsarAddWithValue = New System.Windows.Forms.CheckBox()
        Me.chkUsarDataAdapter = New System.Windows.Forms.CheckBox()
        Me.chkUsarOverrides = New System.Windows.Forms.CheckBox()
        Me.grbOpciones = New System.Windows.Forms.GroupBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LabelInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.grbAccess.SuspendLayout()
        Me.grbSQL.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.grbOpciones.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnGuardar.Location = New System.Drawing.Point(579, 680)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(2)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(99, 22)
        Me.btnGuardar.TabIndex = 13
        Me.btnGuardar.Text = "&Guardar"
        Me.toolTip1.SetToolTip(Me.btnGuardar, " Guardar la clase generada (se guardará con el nombre de la tabla y la extensión " &
        "del lenguaje) ")
        '
        'cboTablas
        '
        Me.cboTablas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTablas.Location = New System.Drawing.Point(128, 262)
        Me.cboTablas.Margin = New System.Windows.Forms.Padding(2)
        Me.cboTablas.Name = "cboTablas"
        Me.cboTablas.Size = New System.Drawing.Size(202, 21)
        Me.cboTablas.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(17, 262)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 19)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Tablas:"
        Me.toolTip1.SetToolTip(Me.Label3, " Lista de las tablas de la base de datos ")
        '
        'btnMostrarTablas
        '
        Me.btnMostrarTablas.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnMostrarTablas.Location = New System.Drawing.Point(341, 262)
        Me.btnMostrarTablas.Margin = New System.Windows.Forms.Padding(2)
        Me.btnMostrarTablas.Name = "btnMostrarTablas"
        Me.btnMostrarTablas.Size = New System.Drawing.Size(115, 22)
        Me.btnMostrarTablas.TabIndex = 6
        Me.btnMostrarTablas.Text = "Mostrar &tablas"
        Me.toolTip1.SetToolTip(Me.btnMostrarTablas, " Mostrar las tablas que contiene la base de datos indicada ")
        '
        'txtCodigo
        '
        Me.txtCodigo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCodigo.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(11, 425)
        Me.txtCodigo.Margin = New System.Windows.Forms.Padding(2)
        Me.txtCodigo.Multiline = True
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtCodigo.Size = New System.Drawing.Size(550, 302)
        Me.txtCodigo.TabIndex = 12
        Me.txtCodigo.Text = "TextBox1"
        Me.txtCodigo.WordWrap = False
        '
        'btnGenerarClase
        '
        Me.btnGenerarClase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarClase.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnGenerarClase.Location = New System.Drawing.Point(256, 79)
        Me.btnGenerarClase.Margin = New System.Windows.Forms.Padding(2)
        Me.btnGenerarClase.Name = "btnGenerarClase"
        Me.btnGenerarClase.Size = New System.Drawing.Size(103, 22)
        Me.btnGenerarClase.TabIndex = 5
        Me.btnGenerarClase.Text = "Generar &Clase"
        Me.toolTip1.SetToolTip(Me.btnGenerarClase, " Generar el código de la clase en el lenguaje indicado ")
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(17, 319)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 19)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Clase:"
        Me.toolTip1.SetToolTip(Me.Label5, " Nombre de la clase a generar (por defecto tendrá el mismo nombre que la tabla) ")
        '
        'txtClase
        '
        Me.txtClase.Location = New System.Drawing.Point(128, 319)
        Me.txtClase.Margin = New System.Windows.Forms.Padding(2)
        Me.txtClase.Name = "txtClase"
        Me.txtClase.Size = New System.Drawing.Size(176, 20)
        Me.txtClase.TabIndex = 10
        Me.txtClase.Text = "TextBox1"
        '
        'btnSalir
        '
        Me.btnSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSalir.Location = New System.Drawing.Point(579, 706)
        Me.btnSalir.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(99, 21)
        Me.btnSalir.TabIndex = 14
        Me.btnSalir.Text = "Salir"
        Me.toolTip1.SetToolTip(Me.btnSalir, " Terminar la aplicación ")
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(17, 292)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(107, 19)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Cadena select:"
        Me.toolTip1.SetToolTip(Me.Label7, " Cadena de selección de los elementos a usar por la clase (SELECT * FROM <tabla> " &
        "WHERE <condición> ORDER BY <campo>) ")
        '
        'txtSelect
        '
        Me.txtSelect.Location = New System.Drawing.Point(128, 292)
        Me.txtSelect.Margin = New System.Windows.Forms.Padding(2)
        Me.txtSelect.Name = "txtSelect"
        Me.txtSelect.Size = New System.Drawing.Size(330, 20)
        Me.txtSelect.TabIndex = 8
        Me.txtSelect.Text = "Select * From"
        '
        'grbAccess
        '
        Me.grbAccess.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbAccess.Controls.Add(Me.Label4)
        Me.grbAccess.Controls.Add(Me.txtAccessPassword)
        Me.grbAccess.Controls.Add(Me.Label6)
        Me.grbAccess.Controls.Add(Me.txtProvider)
        Me.grbAccess.Controls.Add(Me.btnExaminar)
        Me.grbAccess.Controls.Add(Me.txtNombreBase)
        Me.grbAccess.Controls.Add(Me.Label8)
        Me.grbAccess.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.grbAccess.Location = New System.Drawing.Point(13, 8)
        Me.grbAccess.Margin = New System.Windows.Forms.Padding(2)
        Me.grbAccess.Name = "grbAccess"
        Me.grbAccess.Padding = New System.Windows.Forms.Padding(2)
        Me.grbAccess.Size = New System.Drawing.Size(660, 103)
        Me.grbAccess.TabIndex = 0
        Me.grbAccess.TabStop = False
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(13, 72)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(107, 19)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Password:"
        Me.toolTip1.SetToolTip(Me.Label4, " Si la base está protegida con contraseña, escíbela aquí ")
        '
        'txtAccessPassword
        '
        Me.txtAccessPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAccessPassword.Location = New System.Drawing.Point(124, 72)
        Me.txtAccessPassword.Margin = New System.Windows.Forms.Padding(2)
        Me.txtAccessPassword.Name = "txtAccessPassword"
        Me.txtAccessPassword.Size = New System.Drawing.Size(434, 20)
        Me.txtAccessPassword.TabIndex = 6
        Me.txtAccessPassword.Text = "TextBox1"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(13, 49)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 19)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Provider:"
        Me.toolTip1.SetToolTip(Me.Label6, " El proveedor de la base de datos (normalmente: Provider: (Microsoft.Jet.OLEDB.4." &
        "0) ")
        '
        'txtProvider
        '
        Me.txtProvider.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProvider.Location = New System.Drawing.Point(124, 49)
        Me.txtProvider.Margin = New System.Windows.Forms.Padding(2)
        Me.txtProvider.Name = "txtProvider"
        Me.txtProvider.Size = New System.Drawing.Size(434, 20)
        Me.txtProvider.TabIndex = 4
        Me.txtProvider.Text = "TextBox2"
        '
        'btnExaminar
        '
        Me.btnExaminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExaminar.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnExaminar.Location = New System.Drawing.Point(566, 23)
        Me.btnExaminar.Margin = New System.Windows.Forms.Padding(2)
        Me.btnExaminar.Name = "btnExaminar"
        Me.btnExaminar.Size = New System.Drawing.Size(80, 22)
        Me.btnExaminar.TabIndex = 2
        Me.btnExaminar.Text = "&Examinar..."
        Me.toolTip1.SetToolTip(Me.btnExaminar, " Seleccionar la base de datos ")
        '
        'txtNombreBase
        '
        Me.txtNombreBase.AllowDrop = True
        Me.txtNombreBase.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreBase.Location = New System.Drawing.Point(124, 23)
        Me.txtNombreBase.Margin = New System.Windows.Forms.Padding(2)
        Me.txtNombreBase.Name = "txtNombreBase"
        Me.txtNombreBase.Size = New System.Drawing.Size(434, 20)
        Me.txtNombreBase.TabIndex = 1
        Me.txtNombreBase.Text = "txtNombreBase"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(13, 23)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 19)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Base de datos:"
        Me.toolTip1.SetToolTip(Me.Label8, " Base de datos ")
        '
        'grbSQL
        '
        Me.grbSQL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbSQL.Controls.Add(Me.chkUsarSQLEXpress)
        Me.grbSQL.Controls.Add(Me.chkSeguridadSQL)
        Me.grbSQL.Controls.Add(Me.txtPassword)
        Me.grbSQL.Controls.Add(Me.txtUserId)
        Me.grbSQL.Controls.Add(Me.txtInitialCatalog)
        Me.grbSQL.Controls.Add(Me.txtDataSource)
        Me.grbSQL.Controls.Add(Me.labelPassw)
        Me.grbSQL.Controls.Add(Me.labelUser)
        Me.grbSQL.Controls.Add(Me.Label2)
        Me.grbSQL.Controls.Add(Me.Label1)
        Me.grbSQL.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.grbSQL.Location = New System.Drawing.Point(13, 114)
        Me.grbSQL.Margin = New System.Windows.Forms.Padding(2)
        Me.grbSQL.Name = "grbSQL"
        Me.grbSQL.Padding = New System.Windows.Forms.Padding(2)
        Me.grbSQL.Size = New System.Drawing.Size(660, 137)
        Me.grbSQL.TabIndex = 2
        Me.grbSQL.TabStop = False
        '
        'chkUsarSQLEXpress
        '
        Me.chkUsarSQLEXpress.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUsarSQLEXpress.Location = New System.Drawing.Point(530, -2)
        Me.chkUsarSQLEXpress.Margin = New System.Windows.Forms.Padding(2)
        Me.chkUsarSQLEXpress.Name = "chkUsarSQLEXpress"
        Me.chkUsarSQLEXpress.Size = New System.Drawing.Size(126, 19)
        Me.chkUsarSQLEXpress.TabIndex = 9
        Me.chkUsarSQLEXpress.Text = "Usar SQLEXPRESS"
        Me.toolTip1.SetToolTip(Me.chkUsarSQLEXpress, "Marca esta casilla para usar '.\SQLEXPRESS' en el Data source y 'Seguridad Integr" &
        "ada'")
        '
        'chkSeguridadSQL
        '
        Me.chkSeguridadSQL.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkSeguridadSQL.Location = New System.Drawing.Point(13, 76)
        Me.chkSeguridadSQL.Margin = New System.Windows.Forms.Padding(2)
        Me.chkSeguridadSQL.Name = "chkSeguridadSQL"
        Me.chkSeguridadSQL.Size = New System.Drawing.Size(427, 19)
        Me.chkSeguridadSQL.TabIndex = 4
        Me.chkSeguridadSQL.Text = "Usar seguridad de SQL, sino la seguridad integrada (Integrated Security) "
        Me.toolTip1.SetToolTip(Me.chkSeguridadSQL, " Marca esta opción para indicar el usuario y el password de la base de datos sino" &
        " se usará la autenticación de Windows ")
        '
        'txtPassword
        '
        Me.txtPassword.Enabled = False
        Me.txtPassword.Location = New System.Drawing.Point(316, 103)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(2)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(112, 20)
        Me.txtPassword.TabIndex = 8
        '
        'txtUserId
        '
        Me.txtUserId.Enabled = False
        Me.txtUserId.Location = New System.Drawing.Point(111, 103)
        Me.txtUserId.Margin = New System.Windows.Forms.Padding(2)
        Me.txtUserId.Name = "txtUserId"
        Me.txtUserId.Size = New System.Drawing.Size(116, 20)
        Me.txtUserId.TabIndex = 6
        '
        'txtInitialCatalog
        '
        Me.txtInitialCatalog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInitialCatalog.Location = New System.Drawing.Point(124, 49)
        Me.txtInitialCatalog.Margin = New System.Windows.Forms.Padding(2)
        Me.txtInitialCatalog.Name = "txtInitialCatalog"
        Me.txtInitialCatalog.Size = New System.Drawing.Size(434, 20)
        Me.txtInitialCatalog.TabIndex = 3
        Me.txtInitialCatalog.Text = "Downloads"
        '
        'txtDataSource
        '
        Me.txtDataSource.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataSource.Location = New System.Drawing.Point(124, 23)
        Me.txtDataSource.Margin = New System.Windows.Forms.Padding(2)
        Me.txtDataSource.Name = "txtDataSource"
        Me.txtDataSource.Size = New System.Drawing.Size(434, 20)
        Me.txtDataSource.TabIndex = 1
        Me.txtDataSource.Text = "(local)\NETSDK"
        '
        'labelPassw
        '
        Me.labelPassw.Enabled = False
        Me.labelPassw.Location = New System.Drawing.Point(235, 107)
        Me.labelPassw.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelPassw.Name = "labelPassw"
        Me.labelPassw.Size = New System.Drawing.Size(77, 19)
        Me.labelPassw.TabIndex = 7
        Me.labelPassw.Text = "Password:"
        Me.toolTip1.SetToolTip(Me.labelPassw, " La contraseña para la base de SQL Server ")
        '
        'labelUser
        '
        Me.labelUser.Enabled = False
        Me.labelUser.Location = New System.Drawing.Point(30, 107)
        Me.labelUser.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelUser.Name = "labelUser"
        Me.labelUser.Size = New System.Drawing.Size(77, 19)
        Me.labelUser.TabIndex = 5
        Me.labelUser.Text = "User id:"
        Me.toolTip1.SetToolTip(Me.labelUser, " El ID de usuario ")
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(13, 49)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Initial catalog:"
        Me.toolTip1.SetToolTip(Me.Label2, " La base de datos de SQL Server ")
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(13, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Data source:"
        Me.toolTip1.SetToolTip(Me.Label1, " El servidor de SQL a utilizar ")
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.optCS)
        Me.Panel1.Controls.Add(Me.optVB)
        Me.Panel1.Location = New System.Drawing.Point(256, 52)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(103, 23)
        Me.Panel1.TabIndex = 4
        '
        'optCS
        '
        Me.optCS.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optCS.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.optCS.Location = New System.Drawing.Point(55, 2)
        Me.optCS.Margin = New System.Windows.Forms.Padding(2)
        Me.optCS.Name = "optCS"
        Me.optCS.Size = New System.Drawing.Size(40, 19)
        Me.optCS.TabIndex = 1
        Me.optCS.Text = "&C#"
        Me.toolTip1.SetToolTip(Me.optCS, " Para generar el código para C# ")
        '
        'optVB
        '
        Me.optVB.Checked = True
        Me.optVB.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.optVB.Location = New System.Drawing.Point(6, 2)
        Me.optVB.Margin = New System.Windows.Forms.Padding(2)
        Me.optVB.Name = "optVB"
        Me.optVB.Size = New System.Drawing.Size(40, 19)
        Me.optVB.TabIndex = 0
        Me.optVB.TabStop = True
        Me.optVB.Text = "&VB"
        Me.toolTip1.SetToolTip(Me.optVB, " Para generar el código para VB .NET ")
        '
        'optAccess
        '
        Me.optAccess.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.optAccess.Location = New System.Drawing.Point(25, 4)
        Me.optAccess.Margin = New System.Windows.Forms.Padding(2)
        Me.optAccess.Name = "optAccess"
        Me.optAccess.Size = New System.Drawing.Size(197, 23)
        Me.optAccess.TabIndex = 1
        Me.optAccess.Text = "Usar base de datos de &Access"
        Me.toolTip1.SetToolTip(Me.optAccess, " Para usar una base de Access (OleDb) " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Se usará el DataAdapter)")
        '
        'optSQL
        '
        Me.optSQL.Checked = True
        Me.optSQL.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.optSQL.Location = New System.Drawing.Point(25, 110)
        Me.optSQL.Margin = New System.Windows.Forms.Padding(2)
        Me.optSQL.Name = "optSQL"
        Me.optSQL.Size = New System.Drawing.Size(222, 23)
        Me.optSQL.TabIndex = 3
        Me.optSQL.TabStop = True
        Me.optSQL.Text = "Usar base de datos de &SQL Server"
        Me.toolTip1.SetToolTip(Me.optSQL, " Para usar una base de SQL Server " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Se recomienda no usar el DataAdapter)")
        '
        'chkUsarCommandBuilder
        '
        Me.chkUsarCommandBuilder.Checked = True
        Me.chkUsarCommandBuilder.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUsarCommandBuilder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUsarCommandBuilder.Location = New System.Drawing.Point(24, 33)
        Me.chkUsarCommandBuilder.Margin = New System.Windows.Forms.Padding(2)
        Me.chkUsarCommandBuilder.Name = "chkUsarCommandBuilder"
        Me.chkUsarCommandBuilder.Size = New System.Drawing.Size(133, 19)
        Me.chkUsarCommandBuilder.TabIndex = 1
        Me.chkUsarCommandBuilder.Text = "usar CommandBuilder"
        Me.toolTip1.SetToolTip(Me.chkUsarCommandBuilder, " Si se debe usar CommandBuilder para generar el código UPDATE, INSERT o DELETE ")
        '
        'chkUsarAddWithValue
        '
        Me.chkUsarAddWithValue.Checked = True
        Me.chkUsarAddWithValue.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUsarAddWithValue.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUsarAddWithValue.Location = New System.Drawing.Point(6, 56)
        Me.chkUsarAddWithValue.Margin = New System.Windows.Forms.Padding(2)
        Me.chkUsarAddWithValue.Name = "chkUsarAddWithValue"
        Me.chkUsarAddWithValue.Size = New System.Drawing.Size(126, 19)
        Me.chkUsarAddWithValue.TabIndex = 2
        Me.chkUsarAddWithValue.Text = "usar AddWithValue"
        Me.toolTip1.SetToolTip(Me.chkUsarAddWithValue, " Si se utiliza AddWithValue o Add  para añadir los valores a los parámetros del c" &
        "omando.")
        '
        'chkUsarDataAdapter
        '
        Me.chkUsarDataAdapter.AutoSize = True
        Me.chkUsarDataAdapter.Checked = True
        Me.chkUsarDataAdapter.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUsarDataAdapter.Location = New System.Drawing.Point(6, 12)
        Me.chkUsarDataAdapter.Margin = New System.Windows.Forms.Padding(2)
        Me.chkUsarDataAdapter.Name = "chkUsarDataAdapter"
        Me.chkUsarDataAdapter.Size = New System.Drawing.Size(239, 17)
        Me.chkUsarDataAdapter.TabIndex = 0
        Me.chkUsarDataAdapter.Text = "usar DataAdpater en lugar de cmd.Execute..."
        Me.toolTip1.SetToolTip(Me.chkUsarDataAdapter, "Marca esta opción si quieres usar DataAdapter en vez de ExecuteNonQuery en UPDATE" &
        " y DELETE o ExecuteScalar en INSERT." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Al usar cmd.Execute en SQL se usarán tran" &
        "sacciones)")
        Me.chkUsarDataAdapter.UseVisualStyleBackColor = False
        '
        'chkUsarOverrides
        '
        Me.chkUsarOverrides.Checked = True
        Me.chkUsarOverrides.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUsarOverrides.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUsarOverrides.Location = New System.Drawing.Point(6, 79)
        Me.chkUsarOverrides.Margin = New System.Windows.Forms.Padding(2)
        Me.chkUsarOverrides.Name = "chkUsarOverrides"
        Me.chkUsarOverrides.Size = New System.Drawing.Size(126, 19)
        Me.chkUsarOverrides.TabIndex = 3
        Me.chkUsarOverrides.Text = "usar Overrides"
        Me.toolTip1.SetToolTip(Me.chkUsarOverrides, "Usar Overrides los métodos Actualizar, Crear y Borrar.")
        '
        'grbOpciones
        '
        Me.grbOpciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbOpciones.Controls.Add(Me.chkUsarCommandBuilder)
        Me.grbOpciones.Controls.Add(Me.chkUsarOverrides)
        Me.grbOpciones.Controls.Add(Me.chkUsarAddWithValue)
        Me.grbOpciones.Controls.Add(Me.chkUsarDataAdapter)
        Me.grbOpciones.Controls.Add(Me.Panel1)
        Me.grbOpciones.Controls.Add(Me.btnGenerarClase)
        Me.grbOpciones.Location = New System.Drawing.Point(309, 314)
        Me.grbOpciones.Name = "grbOpciones"
        Me.grbOpciones.Size = New System.Drawing.Size(364, 106)
        Me.grbOpciones.TabIndex = 11
        Me.grbOpciones.TabStop = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LabelInfo, Me.LabelVersion})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 732)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(689, 24)
        Me.StatusStrip1.TabIndex = 15
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LabelInfo
        '
        Me.LabelInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(558, 19)
        Me.LabelInfo.Spring = True
        Me.LabelInfo.Text = "©Guillermo Som (elGuille), 2004-2007, 2018-2022"
        Me.LabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelVersion
        '
        Me.LabelVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.LabelVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(85, 19)
        Me.LabelVersion.Text = "v2.1.0 (2.1.0.6)"
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(689, 756)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.grbOpciones)
        Me.Controls.Add(Me.optSQL)
        Me.Controls.Add(Me.txtSelect)
        Me.Controls.Add(Me.txtCodigo)
        Me.Controls.Add(Me.txtClase)
        Me.Controls.Add(Me.optAccess)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.cboTablas)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnMostrarTablas)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.grbAccess)
        Me.Controls.Add(Me.grbSQL)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MinimumSize = New System.Drawing.Size(594, 555)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Crear clases de una base de datos Access o SQL Server"
        Me.grbAccess.ResumeLayout(False)
        Me.grbAccess.PerformLayout()
        Me.grbSQL.ResumeLayout(False)
        Me.grbSQL.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.grbOpciones.ResumeLayout(False)
        Me.grbOpciones.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents btnGuardar As System.Windows.Forms.Button
    Private WithEvents cboTablas As System.Windows.Forms.ComboBox
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents txtCodigo As System.Windows.Forms.TextBox
    Private WithEvents btnGenerarClase As System.Windows.Forms.Button
    Private WithEvents Label5 As System.Windows.Forms.Label
    Private WithEvents txtClase As System.Windows.Forms.TextBox
    Private WithEvents btnSalir As System.Windows.Forms.Button
    Private WithEvents Label7 As System.Windows.Forms.Label
    Private WithEvents txtSelect As System.Windows.Forms.TextBox
    Private WithEvents btnMostrarTablas As System.Windows.Forms.Button
    Private WithEvents Label4 As System.Windows.Forms.Label
    Private WithEvents Label6 As System.Windows.Forms.Label
    Private WithEvents btnExaminar As System.Windows.Forms.Button
    Private WithEvents txtNombreBase As System.Windows.Forms.TextBox
    Private WithEvents Label8 As System.Windows.Forms.Label
    Private WithEvents Panel1 As System.Windows.Forms.Panel
    Private WithEvents optCS As System.Windows.Forms.RadioButton
    Private WithEvents optVB As System.Windows.Forms.RadioButton
    Private WithEvents chkSeguridadSQL As System.Windows.Forms.CheckBox
    Private WithEvents txtPassword As System.Windows.Forms.TextBox
    Private WithEvents txtUserId As System.Windows.Forms.TextBox
    Private WithEvents txtInitialCatalog As System.Windows.Forms.TextBox
    Private WithEvents txtDataSource As System.Windows.Forms.TextBox
    Private WithEvents labelPassw As System.Windows.Forms.Label
    Private WithEvents labelUser As System.Windows.Forms.Label
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents optAccess As System.Windows.Forms.RadioButton
    Private WithEvents optSQL As System.Windows.Forms.RadioButton
    Private WithEvents grbAccess As System.Windows.Forms.GroupBox
    Private WithEvents grbSQL As System.Windows.Forms.GroupBox
    Private WithEvents txtAccessPassword As System.Windows.Forms.TextBox
    Private WithEvents txtProvider As System.Windows.Forms.TextBox
    Private WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents chkUsarCommandBuilder As System.Windows.Forms.CheckBox
    Private WithEvents chkUsarAddWithValue As CheckBox
    Private WithEvents chkUsarDataAdapter As CheckBox
    Private WithEvents chkUsarOverrides As CheckBox
    Friend WithEvents grbOpciones As GroupBox
    Private WithEvents chkUsarSQLEXpress As CheckBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Private WithEvents LabelInfo As ToolStripStatusLabel
    Friend WithEvents LabelVersion As ToolStripStatusLabel
End Class
