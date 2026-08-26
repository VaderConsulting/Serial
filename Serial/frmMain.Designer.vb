<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim ChartPen1 As ChartPen = New ChartPen
        Dim ChartPen2 As ChartPen = New ChartPen
        Dim ChartPen3 As ChartPen = New ChartPen
        Dim ChartPen4 As ChartPen = New ChartPen
        Dim ChartPen5 As ChartPen = New ChartPen
        Dim ChartPen6 As ChartPen = New ChartPen
        Dim ChartPen7 As ChartPen = New ChartPen
        Dim ChartPen8 As ChartPen = New ChartPen
        Dim ChartPen9 As ChartPen = New ChartPen
        Dim ChartPen10 As ChartPen = New ChartPen
        Dim ChartPen11 As ChartPen = New ChartPen
        Dim ChartPen12 As ChartPen = New ChartPen
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.lblX = New System.Windows.Forms.Label
        Me.lblY = New System.Windows.Forms.Label
        Me.lblZ = New System.Windows.Forms.Label
        Me.GaugeZ = New Gauge
        Me.GaugeY = New Gauge
        Me.GaugeX = New Gauge
        Me.PerfChartZ = New PerfChart
        Me.PerfChartY = New PerfChart
        Me.PerfChartX = New PerfChart
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        Me.SerialPort1.PortName = "COM11"
        '
        'lblX
        '
        Me.lblX.BackColor = System.Drawing.Color.Transparent
        Me.lblX.Location = New System.Drawing.Point(62, 161)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(99, 16)
        Me.lblX.TabIndex = 0
        Me.lblX.Text = "0"
        Me.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblY
        '
        Me.lblY.BackColor = System.Drawing.Color.Transparent
        Me.lblY.Location = New System.Drawing.Point(279, 161)
        Me.lblY.Name = "lblY"
        Me.lblY.Size = New System.Drawing.Size(99, 16)
        Me.lblY.TabIndex = 2
        Me.lblY.Text = "0"
        Me.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblZ
        '
        Me.lblZ.BackColor = System.Drawing.Color.Transparent
        Me.lblZ.Location = New System.Drawing.Point(495, 161)
        Me.lblZ.Name = "lblZ"
        Me.lblZ.Size = New System.Drawing.Size(99, 16)
        Me.lblZ.TabIndex = 4
        Me.lblZ.Text = "0"
        Me.lblZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GaugeZ
        '
        Me.GaugeZ.BaseArcColor = System.Drawing.Color.Gray
        Me.GaugeZ.BaseArcRadius = 80
        Me.GaugeZ.BaseArcStart = 135
        Me.GaugeZ.BaseArcSweep = 270
        Me.GaugeZ.BaseArcWidth = 2
        Me.GaugeZ.Cap_Idx = CType(1, Byte)
        Me.GaugeZ.CapColors = New System.Drawing.Color() {System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black}
        Me.GaugeZ.CapPosition = New System.Drawing.Point(10, 10)
        Me.GaugeZ.CapsPosition = New System.Drawing.Point() {New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10)}
        Me.GaugeZ.CapsText = New String() {"", "Z", "", "", ""}
        Me.GaugeZ.CapText = "Z"
        Me.GaugeZ.Center = New System.Drawing.Point(100, 100)
        Me.GaugeZ.Location = New System.Drawing.Point(445, 10)
        Me.GaugeZ.MaxValue = 250.0!
        Me.GaugeZ.MinValue = -250.0!
        Me.GaugeZ.Name = "GaugeZ"
        Me.GaugeZ.NeedleColor1 = Gauge.NeedleColorEnum.Gray
        Me.GaugeZ.NeedleColor2 = System.Drawing.Color.DimGray
        Me.GaugeZ.NeedleRadius = 80
        Me.GaugeZ.NeedleType = 0
        Me.GaugeZ.NeedleWidth = 2
        Me.GaugeZ.Range_Idx = CType(0, Byte)
        Me.GaugeZ.RangeColor = System.Drawing.Color.LightSkyBlue
        Me.GaugeZ.RangeEnabled = True
        Me.GaugeZ.RangeEndValue = 0.0!
        Me.GaugeZ.RangeInnerRadius = 70
        Me.GaugeZ.RangeOuterRadius = 80
        Me.GaugeZ.RangesColor = New System.Drawing.Color() {System.Drawing.Color.LightSkyBlue, System.Drawing.Color.DodgerBlue, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control}
        Me.GaugeZ.RangesEnabled = New Boolean() {True, True, False, False, False}
        Me.GaugeZ.RangesEndValue = New Single() {0.0!, 250.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeZ.RangesInnerRadius = New Integer() {70, 70, 70, 70, 70}
        Me.GaugeZ.RangesOuterRadius = New Integer() {80, 80, 80, 80, 80}
        Me.GaugeZ.RangesStartValue = New Single() {-250.0!, 0.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeZ.RangeStartValue = -250.0!
        Me.GaugeZ.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.GaugeZ.ScaleLinesInterInnerRadius = 73
        Me.GaugeZ.ScaleLinesInterOuterRadius = 80
        Me.GaugeZ.ScaleLinesInterWidth = 1
        Me.GaugeZ.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.GaugeZ.ScaleLinesMajorInnerRadius = 70
        Me.GaugeZ.ScaleLinesMajorOuterRadius = 80
        Me.GaugeZ.ScaleLinesMajorStepValue = 50.0!
        Me.GaugeZ.ScaleLinesMajorWidth = 2
        Me.GaugeZ.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.GaugeZ.ScaleLinesMinorInnerRadius = 75
        Me.GaugeZ.ScaleLinesMinorNumOf = 9
        Me.GaugeZ.ScaleLinesMinorOuterRadius = 80
        Me.GaugeZ.ScaleLinesMinorWidth = 1
        Me.GaugeZ.ScaleNumbersColor = System.Drawing.Color.Black
        Me.GaugeZ.ScaleNumbersFormat = Nothing
        Me.GaugeZ.ScaleNumbersRadius = 95
        Me.GaugeZ.ScaleNumbersRotation = 0
        Me.GaugeZ.ScaleNumbersStartScaleLine = 0
        Me.GaugeZ.ScaleNumbersStepScaleLines = 1
        Me.GaugeZ.Size = New System.Drawing.Size(210, 186)
        Me.GaugeZ.TabIndex = 3
        Me.GaugeZ.Text = "Gauge1"
        Me.GaugeZ.Value = 0.0!
        '
        'GaugeY
        '
        Me.GaugeY.BaseArcColor = System.Drawing.Color.Gray
        Me.GaugeY.BaseArcRadius = 80
        Me.GaugeY.BaseArcStart = 135
        Me.GaugeY.BaseArcSweep = 270
        Me.GaugeY.BaseArcWidth = 2
        Me.GaugeY.Cap_Idx = CType(1, Byte)
        Me.GaugeY.CapColors = New System.Drawing.Color() {System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black}
        Me.GaugeY.CapPosition = New System.Drawing.Point(10, 10)
        Me.GaugeY.CapsPosition = New System.Drawing.Point() {New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10)}
        Me.GaugeY.CapsText = New String() {"", "Y", "", "", ""}
        Me.GaugeY.CapText = "Y"
        Me.GaugeY.Center = New System.Drawing.Point(100, 100)
        Me.GaugeY.Location = New System.Drawing.Point(228, 10)
        Me.GaugeY.MaxValue = 250.0!
        Me.GaugeY.MinValue = -250.0!
        Me.GaugeY.Name = "GaugeY"
        Me.GaugeY.NeedleColor1 = Gauge.NeedleColorEnum.Gray
        Me.GaugeY.NeedleColor2 = System.Drawing.Color.DimGray
        Me.GaugeY.NeedleRadius = 80
        Me.GaugeY.NeedleType = 0
        Me.GaugeY.NeedleWidth = 2
        Me.GaugeY.Range_Idx = CType(0, Byte)
        Me.GaugeY.RangeColor = System.Drawing.Color.LightSkyBlue
        Me.GaugeY.RangeEnabled = True
        Me.GaugeY.RangeEndValue = 0.0!
        Me.GaugeY.RangeInnerRadius = 70
        Me.GaugeY.RangeOuterRadius = 80
        Me.GaugeY.RangesColor = New System.Drawing.Color() {System.Drawing.Color.LightSkyBlue, System.Drawing.Color.DodgerBlue, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control}
        Me.GaugeY.RangesEnabled = New Boolean() {True, True, False, False, False}
        Me.GaugeY.RangesEndValue = New Single() {0.0!, 250.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeY.RangesInnerRadius = New Integer() {70, 70, 70, 70, 70}
        Me.GaugeY.RangesOuterRadius = New Integer() {80, 80, 80, 80, 80}
        Me.GaugeY.RangesStartValue = New Single() {-250.0!, 0.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeY.RangeStartValue = -250.0!
        Me.GaugeY.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.GaugeY.ScaleLinesInterInnerRadius = 73
        Me.GaugeY.ScaleLinesInterOuterRadius = 80
        Me.GaugeY.ScaleLinesInterWidth = 1
        Me.GaugeY.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.GaugeY.ScaleLinesMajorInnerRadius = 70
        Me.GaugeY.ScaleLinesMajorOuterRadius = 80
        Me.GaugeY.ScaleLinesMajorStepValue = 50.0!
        Me.GaugeY.ScaleLinesMajorWidth = 2
        Me.GaugeY.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.GaugeY.ScaleLinesMinorInnerRadius = 75
        Me.GaugeY.ScaleLinesMinorNumOf = 9
        Me.GaugeY.ScaleLinesMinorOuterRadius = 80
        Me.GaugeY.ScaleLinesMinorWidth = 1
        Me.GaugeY.ScaleNumbersColor = System.Drawing.Color.Black
        Me.GaugeY.ScaleNumbersFormat = Nothing
        Me.GaugeY.ScaleNumbersRadius = 95
        Me.GaugeY.ScaleNumbersRotation = 0
        Me.GaugeY.ScaleNumbersStartScaleLine = 0
        Me.GaugeY.ScaleNumbersStepScaleLines = 1
        Me.GaugeY.Size = New System.Drawing.Size(210, 186)
        Me.GaugeY.TabIndex = 1
        Me.GaugeY.Text = "Gauge1"
        Me.GaugeY.Value = 0.0!
        '
        'GaugeX
        '
        Me.GaugeX.BaseArcColor = System.Drawing.Color.Gray
        Me.GaugeX.BaseArcRadius = 80
        Me.GaugeX.BaseArcStart = 135
        Me.GaugeX.BaseArcSweep = 270
        Me.GaugeX.BaseArcWidth = 2
        Me.GaugeX.Cap_Idx = CType(1, Byte)
        Me.GaugeX.CapColors = New System.Drawing.Color() {System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black}
        Me.GaugeX.CapPosition = New System.Drawing.Point(10, 10)
        Me.GaugeX.CapsPosition = New System.Drawing.Point() {New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10), New System.Drawing.Point(10, 10)}
        Me.GaugeX.CapsText = New String() {"", "X", "", "", ""}
        Me.GaugeX.CapText = "X"
        Me.GaugeX.Center = New System.Drawing.Point(100, 100)
        Me.GaugeX.Location = New System.Drawing.Point(12, 10)
        Me.GaugeX.MaxValue = 250.0!
        Me.GaugeX.MinValue = -250.0!
        Me.GaugeX.Name = "GaugeX"
        Me.GaugeX.NeedleColor1 = Gauge.NeedleColorEnum.Gray
        Me.GaugeX.NeedleColor2 = System.Drawing.Color.DimGray
        Me.GaugeX.NeedleRadius = 80
        Me.GaugeX.NeedleType = 0
        Me.GaugeX.NeedleWidth = 2
        Me.GaugeX.Range_Idx = CType(0, Byte)
        Me.GaugeX.RangeColor = System.Drawing.Color.LightSkyBlue
        Me.GaugeX.RangeEnabled = True
        Me.GaugeX.RangeEndValue = 0.0!
        Me.GaugeX.RangeInnerRadius = 70
        Me.GaugeX.RangeOuterRadius = 80
        Me.GaugeX.RangesColor = New System.Drawing.Color() {System.Drawing.Color.LightSkyBlue, System.Drawing.Color.DodgerBlue, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control}
        Me.GaugeX.RangesEnabled = New Boolean() {True, True, False, False, False}
        Me.GaugeX.RangesEndValue = New Single() {0.0!, 250.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeX.RangesInnerRadius = New Integer() {70, 70, 70, 70, 70}
        Me.GaugeX.RangesOuterRadius = New Integer() {80, 80, 80, 80, 80}
        Me.GaugeX.RangesStartValue = New Single() {-250.0!, 0.0!, 0.0!, 0.0!, 0.0!}
        Me.GaugeX.RangeStartValue = -250.0!
        Me.GaugeX.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.GaugeX.ScaleLinesInterInnerRadius = 73
        Me.GaugeX.ScaleLinesInterOuterRadius = 80
        Me.GaugeX.ScaleLinesInterWidth = 1
        Me.GaugeX.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.GaugeX.ScaleLinesMajorInnerRadius = 70
        Me.GaugeX.ScaleLinesMajorOuterRadius = 80
        Me.GaugeX.ScaleLinesMajorStepValue = 50.0!
        Me.GaugeX.ScaleLinesMajorWidth = 2
        Me.GaugeX.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.GaugeX.ScaleLinesMinorInnerRadius = 75
        Me.GaugeX.ScaleLinesMinorNumOf = 9
        Me.GaugeX.ScaleLinesMinorOuterRadius = 80
        Me.GaugeX.ScaleLinesMinorWidth = 1
        Me.GaugeX.ScaleNumbersColor = System.Drawing.Color.Black
        Me.GaugeX.ScaleNumbersFormat = Nothing
        Me.GaugeX.ScaleNumbersRadius = 95
        Me.GaugeX.ScaleNumbersRotation = 0
        Me.GaugeX.ScaleNumbersStartScaleLine = 0
        Me.GaugeX.ScaleNumbersStepScaleLines = 1
        Me.GaugeX.Size = New System.Drawing.Size(210, 186)
        Me.GaugeX.TabIndex = 0
        Me.GaugeX.Text = "Gauge1"
        Me.GaugeX.Value = 0.0!
        '
        'PerfChartZ
        '
        Me.PerfChartZ.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.PerfChartZ.Location = New System.Drawing.Point(445, 196)
        Me.PerfChartZ.Name = "PerfChartZ"
        Me.PerfChartZ.PerfChartStyle.AntiAliasing = True
        ChartPen1.Color = System.Drawing.Color.Tan
        ChartPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        ChartPen1.Width = 1.0!
        Me.PerfChartZ.PerfChartStyle.AvgLinePen = ChartPen1
        Me.PerfChartZ.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.Black
        Me.PerfChartZ.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.Black
        ChartPen2.Color = System.Drawing.Color.RoyalBlue
        ChartPen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen2.Width = 2.0!
        Me.PerfChartZ.PerfChartStyle.ChartLinePen = ChartPen2
        ChartPen3.Color = System.Drawing.Color.Green
        ChartPen3.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen3.Width = 1.0!
        Me.PerfChartZ.PerfChartStyle.HorizontalGridPen = ChartPen3
        Me.PerfChartZ.PerfChartStyle.ShowAverageLine = True
        Me.PerfChartZ.PerfChartStyle.ShowHorizontalGridLines = True
        Me.PerfChartZ.PerfChartStyle.ShowVerticalGridLines = True
        ChartPen4.Color = System.Drawing.Color.Green
        ChartPen4.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen4.Width = 1.0!
        Me.PerfChartZ.PerfChartStyle.VerticalGridPen = ChartPen4
        Me.PerfChartZ.ScaleMaximum = 250
        Me.PerfChartZ.ScaleMinimum = -250
        Me.PerfChartZ.ScaleMode = ScaleMode.Manual
        Me.PerfChartZ.Size = New System.Drawing.Size(210, 250)
        Me.PerfChartZ.TabIndex = 7
        Me.PerfChartZ.TimerInterval = 100
        Me.PerfChartZ.TimerMode = TimerMode.Disabled
        '
        'PerfChartY
        '
        Me.PerfChartY.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.PerfChartY.Location = New System.Drawing.Point(228, 196)
        Me.PerfChartY.Name = "PerfChartY"
        Me.PerfChartY.PerfChartStyle.AntiAliasing = True
        ChartPen5.Color = System.Drawing.Color.Tan
        ChartPen5.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        ChartPen5.Width = 1.0!
        Me.PerfChartY.PerfChartStyle.AvgLinePen = ChartPen5
        Me.PerfChartY.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.Black
        Me.PerfChartY.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.Black
        ChartPen6.Color = System.Drawing.Color.RoyalBlue
        ChartPen6.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen6.Width = 2.0!
        Me.PerfChartY.PerfChartStyle.ChartLinePen = ChartPen6
        ChartPen7.Color = System.Drawing.Color.Green
        ChartPen7.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen7.Width = 1.0!
        Me.PerfChartY.PerfChartStyle.HorizontalGridPen = ChartPen7
        Me.PerfChartY.PerfChartStyle.ShowAverageLine = True
        Me.PerfChartY.PerfChartStyle.ShowHorizontalGridLines = True
        Me.PerfChartY.PerfChartStyle.ShowVerticalGridLines = True
        ChartPen8.Color = System.Drawing.Color.Green
        ChartPen8.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen8.Width = 1.0!
        Me.PerfChartY.PerfChartStyle.VerticalGridPen = ChartPen8
        Me.PerfChartY.ScaleMaximum = 250
        Me.PerfChartY.ScaleMinimum = -250
        Me.PerfChartY.ScaleMode = ScaleMode.Manual
        Me.PerfChartY.Size = New System.Drawing.Size(210, 250)
        Me.PerfChartY.TabIndex = 6
        Me.PerfChartY.TimerInterval = 100
        Me.PerfChartY.TimerMode = TimerMode.Disabled
        '
        'PerfChartX
        '
        Me.PerfChartX.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.PerfChartX.Location = New System.Drawing.Point(12, 196)
        Me.PerfChartX.Name = "PerfChartX"
        Me.PerfChartX.PerfChartStyle.AntiAliasing = True
        ChartPen9.Color = System.Drawing.Color.Tan
        ChartPen9.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        ChartPen9.Width = 1.0!
        Me.PerfChartX.PerfChartStyle.AvgLinePen = ChartPen9
        Me.PerfChartX.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.Black
        Me.PerfChartX.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.Black
        ChartPen10.Color = System.Drawing.Color.RoyalBlue
        ChartPen10.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen10.Width = 2.0!
        Me.PerfChartX.PerfChartStyle.ChartLinePen = ChartPen10
        ChartPen11.Color = System.Drawing.Color.Green
        ChartPen11.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen11.Width = 1.0!
        Me.PerfChartX.PerfChartStyle.HorizontalGridPen = ChartPen11
        Me.PerfChartX.PerfChartStyle.ShowAverageLine = True
        Me.PerfChartX.PerfChartStyle.ShowHorizontalGridLines = True
        Me.PerfChartX.PerfChartStyle.ShowVerticalGridLines = True
        ChartPen12.Color = System.Drawing.Color.Green
        ChartPen12.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
        ChartPen12.Width = 1.0!
        Me.PerfChartX.PerfChartStyle.VerticalGridPen = ChartPen12
        Me.PerfChartX.ScaleMaximum = 250
        Me.PerfChartX.ScaleMinimum = -250
        Me.PerfChartX.ScaleMode = ScaleMode.Manual
        Me.PerfChartX.Size = New System.Drawing.Size(210, 250)
        Me.PerfChartX.TabIndex = 5
        Me.PerfChartX.TimerInterval = 100
        Me.PerfChartX.TimerMode = TimerMode.Disabled
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(665, 452)
        Me.Controls.Add(Me.PerfChartZ)
        Me.Controls.Add(Me.PerfChartY)
        Me.Controls.Add(Me.PerfChartX)
        Me.Controls.Add(Me.lblZ)
        Me.Controls.Add(Me.lblY)
        Me.Controls.Add(Me.lblX)
        Me.Controls.Add(Me.GaugeZ)
        Me.Controls.Add(Me.GaugeY)
        Me.Controls.Add(Me.GaugeX)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Acceleration"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents GaugeX As Gauge
    Friend WithEvents GaugeY As Gauge
    Friend WithEvents GaugeZ As Gauge
    Friend WithEvents lblX As System.Windows.Forms.Label
    Friend WithEvents lblY As System.Windows.Forms.Label
    Friend WithEvents lblZ As System.Windows.Forms.Label
    Friend WithEvents PerfChartX As PerfChart
    Friend WithEvents PerfChartY As PerfChart
    Friend WithEvents PerfChartZ As PerfChart

End Class
