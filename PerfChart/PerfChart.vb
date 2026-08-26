Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

'http://www.codeproject.com/KB/miscctrl/SimplePerfChart.aspx

''' <summary>
''' Scale mode for value aspect ratio
''' </summary>
Public Enum ScaleMode
    ''' <summary>
    ''' Absolute Scale Mode: Values from 0 to 100 are accepted and displayed
    ''' </summary>
    Absolute
    ''' <summary>
    ''' Relative Scale Mode: All values are allowed and displayed in a proper relation
    ''' </summary>
    Relative
    ''' <summary>
    ''' Manual Scale Mode: All values are allowed and displayed in relation to ScaleMaximum and ScaleMinimum values
    ''' </summary>
    Manual
End Enum

''' <summary>
''' Chart Refresh Mode Timer Control Mode
''' </summary>
Public Enum TimerMode
    ''' <summary>
    ''' Chart is refreshed when a value is added
    ''' </summary>
    Disabled
    ''' <summary>
    ''' Chart is refreshed every <c>TimerInterval</c> milliseconds, adding all values
    ''' in the queue to the chart. If there are no values in the queue, a 0 (zero) is added
    ''' </summary>
    Simple
    ''' <summary>
    ''' Chart is refreshed every <c>TimerInterval</c> milliseconds, adding an average of
    ''' all values in the queue to the chart. If there are no values in the queue,
    ''' 0 (zero) is added
    ''' </summary>
    SynchronizedAverage
    ''' <summary>
    ''' Chart is refreshed every <c>TimerInterval</c> milliseconds, adding the sum of
    ''' all values in the queue to the chart. If there are no values in the queue,
    ''' 0 (zero) is added
    ''' </summary>
    SynchronizedSum
End Enum

Partial Public Class PerfChart
    Inherits UserControl

#Region "*** Constants ***"

    ' Keep only a maximum MAX_VALUE_COUNT amount of values; This will allow
    Private Const MAX_VALUE_COUNT As Integer = 512
    ' Draw a background grid with a fixed line spacing
    Private Const GRID_SPACING As Integer = 16

#End Region

#Region "*** Member Variables ***"

    Private _visibleValues As Double = 0.0                       ' Amount of currently visible values (calculated from control width and value spacing)
    Private _valueSpacing As Double = 5.0                        ' Horizontal value space in Pixels
    Private _currentMaxValue As Double = 0                       ' The currently highest displayed value, required for Relative Scale Mode
    Private _gridScrollOffset As Integer = 0                     ' Offset value for the scrolling grid
    Private _averageValue As Double = 0                          ' The current average value
    Private _b3dstyle As Border3DStyle = Border3DStyle.Sunken    ' Border Style
    Private _scaleMode As ScaleMode = ScaleMode.Absolute         ' Scale mode for value aspect ratio
    Private _timerMode As TimerMode                              ' Timer Mode
    Private _drawValues As New List(Of Double)(MAX_VALUE_COUNT)  ' List of stored values
    Private _waitingValues As New Queue(Of Double)()             ' Value queue for Timer Modes
    Private _perfChartStyle As PerfChartStyle                    ' Style and Design
    Private _scaleMaximum As Double = 100                        ' Scale value Maximum
    Private _scaleMimimum As Double = 0                          ' Scale value Minimum

#End Region

#Region "*** Constructors ***"

    Public Sub New()
        InitializeComponent()

        ' Initialize Variables
        _perfChartStyle = New PerfChartStyle()

        ' Set Optimized Double Buffer to reduce flickering
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        ' Redraw when resized
        Me.SetStyle(ControlStyles.ResizeRedraw, True)

        Me.Font = SystemInformation.MenuFont
    End Sub

#End Region

#Region "*** Properties ***"

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Appearance and Style")> _
    Public Property PerfChartStyle() As PerfChartStyle
        Get
            Return _perfChartStyle
        End Get
        Set(ByVal value As PerfChartStyle)
            _perfChartStyle = value
        End Set
    End Property

    <DefaultValue(GetType(Border3DStyle), "Sunken"), Description("BorderStyle"), Category("Appearance")> _
    Public Shadows Property BorderStyle() As Border3DStyle
        Get
            Return _b3dstyle
        End Get
        Set(ByVal value As Border3DStyle)
            _b3dstyle = value
            Invalidate()
        End Set
    End Property

    Public Property ScaleMode() As ScaleMode
        Get
            Return _scaleMode
        End Get
        Set(ByVal value As ScaleMode)
            _scaleMode = value
        End Set
    End Property

    Public Property TimerMode() As TimerMode
        Get
            Return _timerMode
        End Get
        Set(ByVal value As TimerMode)
            If value = TimerMode.Disabled Then
                ' Stop and append only when changed
                If _timerMode <> TimerMode.Disabled Then
                    _timerMode = value

                    tmrRefresh.[Stop]()
                    ' If there are any values in the queue, append them
                    ChartAppendFromQueue()
                End If
            Else
                _timerMode = value
                tmrRefresh.Start()
            End If
        End Set
    End Property

    Public Property TimerInterval() As Integer
        Get
            Return tmrRefresh.Interval
        End Get
        Set(ByVal value As Integer)
            If value < 15 Then
                Throw New ArgumentOutOfRangeException("TimerInterval", value, "The Timer interval must be greater then 15")
            Else
                tmrRefresh.Interval = value
            End If
        End Set
    End Property

    Public Property ScaleMaximum() As Integer
        Get
            Return _scaleMaximum
        End Get
        Set(ByVal value As Integer)
            _scaleMaximum = value
        End Set
    End Property

    Public Property ScaleMinimum() As Integer
        Get
            Return _scaleMimimum
        End Get
        Set(ByVal value As Integer)
            _scaleMimimum = value
        End Set
    End Property

#End Region

#Region "*** Public Methods ***"

    ''' <summary>
    ''' Clears the whole chart
    ''' </summary>
    Public Sub Clear()
        _drawValues.Clear()
        Invalidate()
    End Sub

    ''' <summary>
    ''' Adds a value to the Chart Line
    ''' </summary>
    ''' <param name="value">progress value</param>
    Public Sub AddValue(ByVal value As Double)
        If _scaleMode = ScaleMode.Absolute AndAlso value > 100D Then
            Throw New Exception([String].Format("Values greater than 100 not allowed in ScaleMode: Absolute ({0})", value))
        End If

        Select Case _timerMode
            Case TimerMode.Disabled
                ChartAppend(value)
                Invalidate()
                Exit Select
            Case TimerMode.Simple, TimerMode.SynchronizedAverage, TimerMode.SynchronizedSum
                ' For all Timer Modes, the Values are stored in the Queue
                AddValueToQueue(value)
                Exit Select
            Case Else
                Throw New Exception([String].Format("Unsupported TimerMode: {0}", _timerMode))
        End Select
    End Sub

#End Region

#Region "*** Private Methods: Common ***"

    ''' <summary>
    ''' Add value to the queue for a timed refresh
    ''' </summary>
    ''' <param name="value"></param>
    Private Sub AddValueToQueue(ByVal value As Double)
        _waitingValues.Enqueue(value)
    End Sub

    ''' <summary>
    ''' Appends value <paramref name="value"/> to the chart (without redrawing)
    ''' </summary>
    ''' <param name="value">performance value</param>
    Private Sub ChartAppend(ByVal value As Double)
        ' Insert at first position; Negative values are flatten to 0 (zero)
        '_drawValues.Insert(0, Math.Max(value, 0))
        _drawValues.Insert(0, value)

        ' Remove last item if maximum value count is reached
        If _drawValues.Count > MAX_VALUE_COUNT Then
            _drawValues.RemoveAt(MAX_VALUE_COUNT)
        End If

        ' Calculate horizontal grid offset for "scrolling" effect
        _gridScrollOffset += _valueSpacing
        If _gridScrollOffset > GRID_SPACING Then
            _gridScrollOffset = _gridScrollOffset Mod GRID_SPACING
        End If
    End Sub

    ''' <summary>
    ''' Appends Values from queue
    ''' </summary>
    Private Sub ChartAppendFromQueue()
        ' Proceed only if there are values 
        If _waitingValues.Count > 0 Then
            If _timerMode = TimerMode.Simple Then
                While _waitingValues.Count > 0
                    ChartAppend(_waitingValues.Dequeue())
                End While
            ElseIf _timerMode = TimerMode.SynchronizedAverage OrElse _timerMode = TimerMode.SynchronizedSum Then
                ' appendValue variable is used for calculating the average or sum value
                Dim appendValue As Double = 0.0 '[Decimal].Zero
                Dim valueCount As Int32 = _waitingValues.Count

                While _waitingValues.Count > 0
                    appendValue += _waitingValues.Dequeue()
                End While

                ' Calculate Average value in SynchronizedAverage Mode
                If _timerMode = TimerMode.SynchronizedAverage Then
                    appendValue = appendValue / valueCount
                End If

                ' Finally append the value
                ChartAppend(appendValue)
            End If
        Else
            ' Always add 0 (Zero) if there are no values in the queue
            ChartAppend(0.0) '[Decimal].Zero)
        End If

        ' Refresh the Chart
        Invalidate()
    End Sub

    ''' <summary>
    ''' Calculates the vertical Position of a value in relation to the chart size,
    ''' Scale Mode and, if ScaleMode is Relative, to the current maximum value
    ''' </summary>
    ''' <param name="value">performance value</param>
    ''' <returns>vertical Point position in Pixels</returns>
    Private Function VerticalPosition(ByVal value As Double) As Double
        Dim Result As Double = 0.0

        If _scaleMode = ScaleMode.Absolute Then
            Result = value * Me.Height / 100
        ElseIf _scaleMode = ScaleMode.Relative Then
            Result = If((_currentMaxValue > 0), (value * Me.Height / _currentMaxValue), 0)
        ElseIf _scaleMode = ScaleMode.Manual Then
            Result = (value * Me.Height / _scaleMaximum)
            'Dim GraphHeight As Int32 = (Me.ScaleMaximum - Me.ScaleMinimum)
            'result = (value / GraphHeight) * Me.Height
        End If
        If value < 0 Then
            Debug.Print("")
        End If
        Result = (Me.Height / 2) - Result
        Return Result 'Convert.ToInt32(Math.Round(result))
    End Function

    ''' <summary>
    ''' Returns the currently highest (displayed) value, for Relative ScaleMode
    ''' </summary>
    ''' <returns></returns>
    Private Function GetHighestValueForRelativeMode() As Decimal
        Dim maxValue As Decimal = 0

        For i As Integer = 0 To _visibleValues - 1
            ' Set if higher then previous max value
            If _drawValues(i) > maxValue Then
                maxValue = _drawValues(i)
            End If
        Next

        Return maxValue
    End Function

#End Region

#Region "*** Private Methods: Drawing ***"

    ''' <summary>
    ''' Draws the chart (w/o background or grid, but with border) to the Graphics canvas
    ''' </summary>
    ''' <param name="g">Graphics</param>
    Private Sub DrawChart(ByVal g As Graphics)
        _visibleValues = Math.Min(CDbl(Me.Width / _valueSpacing), CDbl(_drawValues.Count))

        If _scaleMode = ScaleMode.Relative Then
            _currentMaxValue = GetHighestValueForRelativeMode()
        End If

        ' Dirty little "trick": initialize the first previous Point outside the bounds
        Dim previousPoint As New Point(Width + _valueSpacing, Height)
        Dim currentPoint As New Point()

        ' Only draw average line when possible (visibleValues) and needed (style setting)
        If _visibleValues > 0 AndAlso _perfChartStyle.ShowAverageLine Then
            _averageValue = 0
            DrawAverageLine(g)
        End If

        ' Connect all visible values with lines
        For i As Integer = 0 To _visibleValues - 1
            currentPoint.X = previousPoint.X - _valueSpacing
            currentPoint.Y = VerticalPosition(_drawValues(i))

            ' Actually draw the line
            g.DrawLine(_perfChartStyle.ChartLinePen.Pen, previousPoint, currentPoint)

            previousPoint = currentPoint
        Next

        ' Draw current relative maximum value string
        If _scaleMode = ScaleMode.Relative Then
            Dim sb As New SolidBrush(_perfChartStyle.ChartLinePen.Color)
            g.DrawString(_currentMaxValue.ToString(), Me.Font, sb, 4.0F, 2.0F)
        End If

        ' Draw Border on top
        ControlPaint.DrawBorder3D(g, 0, 0, Width, Height, _b3dstyle)
    End Sub

    Private Sub DrawAverageLine(ByVal g As Graphics)
        For i As Integer = 0 To _visibleValues - 1
            _averageValue += _drawValues(i)
        Next

        _averageValue = _averageValue / _visibleValues

        Dim YPosition As Double = VerticalPosition(_averageValue)
        g.DrawLine(_perfChartStyle.AvgLinePen.Pen, 0, CInt(YPosition), Width, CInt(YPosition))
    End Sub

    ''' <summary>
    ''' Draws the background gradient and the grid into Graphics <paramref name="g"/>
    ''' </summary>
    ''' <param name="g">Graphic</param>
    Private Sub DrawBackgroundAndGrid(ByVal g As Graphics)
        ' Draw the background Gradient rectangle
        Dim baseRectangle As New Rectangle(0, 0, Me.Width, Me.Height)
        Using gradientBrush As Brush = New LinearGradientBrush(baseRectangle, _perfChartStyle.BackgroundColorTop, _perfChartStyle.BackgroundColorBottom, LinearGradientMode.Vertical)
            g.FillRectangle(gradientBrush, baseRectangle)
        End Using

        ' Draw all visible, vertical gridlines (if wanted)
        If _perfChartStyle.ShowVerticalGridLines Then
            Dim i As Integer = Width - _gridScrollOffset
            While i >= 0
                g.DrawLine(_perfChartStyle.VerticalGridPen.Pen, i, 0, i, Height)
                i -= GRID_SPACING
            End While
        End If

        ' Draw all visible, horizontal gridlines (if wanted)
        If _perfChartStyle.ShowHorizontalGridLines Then
            Dim i As Integer = 0
            While i < Height
                g.DrawLine(_perfChartStyle.HorizontalGridPen.Pen, 0, i, Width, i)
                i += GRID_SPACING
            End While
        End If
    End Sub

#End Region

#Region "*** Overrides ***"

    ''' Override OnPaint method
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Enable AntiAliasing, if needed
        If _perfChartStyle.AntiAliasing Then
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
        End If

        DrawBackgroundAndGrid(e.Graphics)
        DrawChart(e.Graphics)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)

        Invalidate()
    End Sub

#End Region

#Region "*** Event Handlers ***"

    Private Sub colorSet_ColorSetChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Refresh Chart on Resize
        Invalidate()
    End Sub

    Private Sub tmrRefresh_Tick(ByVal sender As Object, ByVal e As EventArgs)
        ' Don't execute event if running in design time
        If Me.DesignMode Then
            Return
        End If

        ChartAppendFromQueue()
    End Sub

#End Region

End Class
