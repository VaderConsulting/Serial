Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Diagnostics

Namespace GaugeApp
    <ToolboxBitmapAttribute(GetType(Gauge), "Gauge.bmp"), DefaultEvent("ValueInRangeChanged"), Description("Displays a value on an analog gauge. Raises an event if the value enters one of the definable ranges.")> _
    Partial Public Class Gauge
        Inherits Control

#Region " enum, var, delegate, event "

        Public Enum NeedleColorEnum
            Gray = 0
            Red = 1
            Green = 2
            Blue = 3
            Yellow = 4
            Violet = 5
            Magenta = 6
        End Enum

        Private Const ZERO As Byte = 0
        Private Const NUMOFCAPS As Byte = 5
        Private Const NUMOFRANGES As Byte = 5

        Private fontBoundY1 As Single
        Private fontBoundY2 As Single
        Private gaugeBitmap As Bitmap
        Private drawGaugeBackground As Boolean = True

        Private m_value As Single
        Private m_valueIsInRange As Boolean() = {False, False, False, False, False}
        Private m_CapIdx As Byte = 1
        Private m_CapColor As Color() = {Color.Black, Color.Black, Color.Black, Color.Black, Color.Black}
        Private m_CapText As String() = {"", "", "", "", ""}
        Private m_CapPosition As Point() = {New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10)}
        Private m_Center As New Point(100, 100)
        Private m_MinValue As Single = -100
        Private m_MaxValue As Single = 400

        Private m_BaseArcColor As Color = Color.Gray
        Private m_BaseArcRadius As Int32 = 80
        Private m_BaseArcStart As Int32 = 135
        Private m_BaseArcSweep As Int32 = 270
        Private m_BaseArcWidth As Int32 = 2

        Private m_ScaleLinesInterColor As Color = Color.Black
        Private m_ScaleLinesInterInnerRadius As Int32 = 73
        Private m_ScaleLinesInterOuterRadius As Int32 = 80
        Private m_ScaleLinesInterWidth As Int32 = 1

        Private m_ScaleLinesMinorNumOf As Int32 = 9
        Private m_ScaleLinesMinorColor As Color = Color.Gray
        Private m_ScaleLinesMinorInnerRadius As Int32 = 75
        Private m_ScaleLinesMinorOuterRadius As Int32 = 80
        Private m_ScaleLinesMinorWidth As Int32 = 1

        Private m_ScaleLinesMajorStepValue As Single = 50.0F
        Private m_ScaleLinesMajorColor As Color = Color.Black
        Private m_ScaleLinesMajorInnerRadius As Int32 = 70
        Private m_ScaleLinesMajorOuterRadius As Int32 = 80
        Private m_ScaleLinesMajorWidth As Int32 = 2

        Private m_RangeIdx As Byte
        Private m_RangeEnabled As Boolean() = {True, True, False, False, False}
        Private m_RangeColor As Color() = {Color.LightGreen, Color.Red, Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control)}
        Private m_RangeStartValue As Single() = {-100.0F, 300.0F, 0.0F, 0.0F, 0.0F}
        Private m_RangeEndValue As Single() = {300.0F, 400.0F, 0.0F, 0.0F, 0.0F}
        Private m_RangeInnerRadius As Int32() = {70, 70, 70, 70, 70}
        Private m_RangeOuterRadius As Int32() = {80, 80, 80, 80, 80}

        Private m_ScaleNumbersRadius As Int32 = 95
        Private m_ScaleNumbersColor As Color = Color.Black
        Private m_ScaleNumbersFormat As String
        Private m_ScaleNumbersStartScaleLine As Int32
        Private m_ScaleNumbersStepScaleLines As Int32 = 1
        Private m_ScaleNumbersRotation As Int32 = 0

        Private m_NeedleType As Int32 = 0
        Private m_NeedleRadius As Int32 = 80
        Private m_NeedleColor1 As NeedleColorEnum = NeedleColorEnum.Gray
        Private m_NeedleColor2 As Color = Color.DimGray
        Private m_NeedleWidth As Int32 = 2

        Public Class ValueInRangeChangedEventArgs
            Inherits EventArgs
            Public valueInRange As Int32

            Public Sub New(ByVal valueInRange As Int32)
                Me.valueInRange = valueInRange
            End Sub
        End Class

        Public Delegate Sub ValueInRangeChangedDelegate(ByVal sender As [Object], ByVal e As ValueInRangeChangedEventArgs)
        <Description("This event is raised if the value falls into a defined range.")> _
        Public Event ValueInRangeChanged As ValueInRangeChangedDelegate
#End Region

#Region " hidden , overridden inherited properties "

        Public Shadows Property AllowDrop() As Boolean
            Get
                Return False
            End Get

            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Shadows Property AutoSize() As Boolean
            Get
                Return False
            End Get

            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Shadows Property ForeColor() As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Shadows Property ImeMode() As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
            End Set
        End Property

        Public Overloads Overrides Property BackColor() As System.Drawing.Color
            Get
                Return MyBase.BackColor
            End Get
            Set(ByVal value As System.Drawing.Color)
                MyBase.BackColor = value
                drawGaugeBackground = True
                Refresh()
            End Set
        End Property

        Public Overloads Overrides Property Font() As System.Drawing.Font
            Get
                Return MyBase.Font
            End Get
            Set(ByVal value As System.Drawing.Font)
                MyBase.Font = value
                drawGaugeBackground = True
                Refresh()
            End Set
        End Property

        Public Overloads Overrides Property BackgroundImageLayout() As System.Windows.Forms.ImageLayout
            Get
                Return MyBase.BackgroundImageLayout
            End Get
            Set(ByVal value As System.Windows.Forms.ImageLayout)
                MyBase.BackgroundImageLayout = value
                drawGaugeBackground = True
                Refresh()
            End Set
        End Property

#End Region

        ' Public Sub New()
        Public Sub New()
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        End Sub

#Region " properties "

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The value.")> _
        Public Property Value() As Single
            Get
                Return m_value
            End Get

            Set(ByVal value As Single)
                If m_value <> value Then
                    m_value = Math.Min(Math.Max(value, m_MinValue), m_MaxValue)

                    If Me.DesignMode Then
                        drawGaugeBackground = True
                    End If

                    Dim counter As Int32 = 0
                    While counter < NUMOFRANGES - 1
                        If (m_RangeStartValue(counter) <= m_value) AndAlso (m_value <= m_RangeEndValue(counter)) AndAlso (m_RangeEnabled(counter)) Then
                            If Not m_valueIsInRange(counter) Then
                                'If ValueInRangeChanged <> Nothing Then
                                '    RaiseEvent ValueInRangeChanged(Me, New ValueInRangeChangedEventArgs(counter))
                                'End If
                            End If
                        Else
                            m_valueIsInRange(counter) = False
                        End If
                        System.Math.Max(System.Threading.Interlocked.Increment(counter), counter - 1)
                    End While
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.RefreshProperties(RefreshProperties.All), System.ComponentModel.Description("The caption index. set this to a value of 0 up to 4 to change the corresponding caption's properties.")> _
        Public Property Cap_Idx() As Byte
            Get
                Return m_CapIdx
            End Get
            Set(ByVal value As Byte)
                If (m_CapIdx <> value) AndAlso (0 <= value) AndAlso (value < 5) Then
                    m_CapIdx = value
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the caption text.")> _
        Private Property CapColor() As Color
            Get
                Return m_CapColor(m_CapIdx)
            End Get
            Set(ByVal value As Color)
                If m_CapColor(m_CapIdx) <> value Then
                    m_CapColor(m_CapIdx) = value
                    CapColors = m_CapColor
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property CapColors() As Color()
            Get
                Return m_CapColor
            End Get
            Set(ByVal value As Color())
                m_CapColor = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The text of the caption.")> _
        Public Property CapText() As String
            Get
                Return m_CapText(m_CapIdx)
            End Get
            Set(ByVal value As String)
                If m_CapText(m_CapIdx) <> value Then
                    m_CapText(m_CapIdx) = value
                    CapsText = m_CapText
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property CapsText() As String()
            Get
                Return m_CapText
            End Get
            Set(ByVal value As String())
                Dim counter As Int32 = 0
                While counter < 5
                    m_CapText(counter) = value(counter)
                    System.Math.Max(System.Threading.Interlocked.Increment(counter), counter - 1)
                End While
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The position of the caption.")> _
        Public Property CapPosition() As Point
            Get
                Return m_CapPosition(m_CapIdx)
            End Get
            Set(ByVal value As Point)
                If m_CapPosition(m_CapIdx) <> value Then
                    m_CapPosition(m_CapIdx) = value
                    CapsPosition = m_CapPosition
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property CapsPosition() As Point()
            Get
                Return m_CapPosition
            End Get
            Set(ByVal value As Point())
                m_CapPosition = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The center of the gauge (in the control's client area).")> _
        Public Property Center() As Point
            Get
                Return m_Center
            End Get
            Set(ByVal value As Point)
                If m_Center <> value Then
                    m_Center = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The minimum value to show on the scale.")> _
        Public Property MinValue() As Single
            Get
                Return m_MinValue
            End Get
            Set(ByVal value As Single)
                If (m_MinValue <> value) AndAlso (value < m_MaxValue) Then
                    m_MinValue = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The maximum value to show on the scale.")> _
        Public Property MaxValue() As Single
            Get
                Return m_MaxValue
            End Get
            Set(ByVal value As Single)
                If (m_MaxValue <> value) AndAlso (value > m_MinValue) Then
                    m_MaxValue = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the base arc.")> _
        Public Property BaseArcColor() As Color
            Get
                Return m_BaseArcColor
            End Get
            Set(ByVal value As Color)
                If m_BaseArcColor <> value Then
                    m_BaseArcColor = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The radius of the base arc.")> _
        Public Property BaseArcRadius() As Int32
            Get
                Return m_BaseArcRadius
            End Get
            Set(ByVal value As Int32)
                If m_BaseArcRadius <> value Then
                    m_BaseArcRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The start angle of the base arc.")> _
        Public Property BaseArcStart() As Int32
            Get
                Return m_BaseArcStart
            End Get
            Set(ByVal value As Int32)
                If m_BaseArcStart <> value Then
                    m_BaseArcStart = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The sweep angle of the base arc.")> _
        Public Property BaseArcSweep() As Int32
            Get
                Return m_BaseArcSweep
            End Get
            Set(ByVal value As Int32)
                If m_BaseArcSweep <> value Then
                    m_BaseArcSweep = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The width of the base arc.")> _
        Public Property BaseArcWidth() As Int32
            Get
                Return m_BaseArcWidth
            End Get
            Set(ByVal value As Int32)
                If m_BaseArcWidth <> value Then
                    m_BaseArcWidth = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
        Public Property ScaleLinesInterColor() As Color
            Get
                Return m_ScaleLinesInterColor
            End Get
            Set(ByVal value As Color)
                If m_ScaleLinesInterColor <> value Then
                    m_ScaleLinesInterColor = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The inner radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
        Public Property ScaleLinesInterInnerRadius() As Int32
            Get
                Return m_ScaleLinesInterInnerRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesInterInnerRadius <> value Then
                    m_ScaleLinesInterInnerRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The outer radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
        Public Property ScaleLinesInterOuterRadius() As Int32
            Get
                Return m_ScaleLinesInterOuterRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesInterOuterRadius <> value Then
                    m_ScaleLinesInterOuterRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The width of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
        Public Property ScaleLinesInterWidth() As Int32
            Get
                Return m_ScaleLinesInterWidth
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesInterWidth <> value Then
                    m_ScaleLinesInterWidth = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The number of minor scale lines.")> _
        Public Property ScaleLinesMinorNumOf() As Int32
            Get
                Return m_ScaleLinesMinorNumOf
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMinorNumOf <> value Then
                    m_ScaleLinesMinorNumOf = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the minor scale lines.")> _
        Public Property ScaleLinesMinorColor() As Color
            Get
                Return m_ScaleLinesMinorColor
            End Get
            Set(ByVal value As Color)
                If m_ScaleLinesMinorColor <> value Then
                    m_ScaleLinesMinorColor = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The inner radius of the minor scale lines.")> _
        Public Property ScaleLinesMinorInnerRadius() As Int32
            Get
                Return m_ScaleLinesMinorInnerRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMinorInnerRadius <> value Then
                    m_ScaleLinesMinorInnerRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The outer radius of the minor scale lines.")> _
        Public Property ScaleLinesMinorOuterRadius() As Int32
            Get
                Return m_ScaleLinesMinorOuterRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMinorOuterRadius <> value Then
                    m_ScaleLinesMinorOuterRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The width of the minor scale lines.")> _
        Public Property ScaleLinesMinorWidth() As Int32
            Get
                Return m_ScaleLinesMinorWidth
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMinorWidth <> value Then
                    m_ScaleLinesMinorWidth = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The step value of the major scale lines.")> _
        Public Property ScaleLinesMajorStepValue() As Single
            Get
                Return m_ScaleLinesMajorStepValue
            End Get
            Set(ByVal value As Single)
                If (m_ScaleLinesMajorStepValue <> value) AndAlso (value > 0) Then
                    m_ScaleLinesMajorStepValue = Math.Max(Math.Min(value, m_MaxValue), m_MinValue)
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the major scale lines.")> _
        Public Property ScaleLinesMajorColor() As Color
            Get
                Return m_ScaleLinesMajorColor
            End Get
            Set(ByVal value As Color)
                If m_ScaleLinesMajorColor <> value Then
                    m_ScaleLinesMajorColor = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The inner radius of the major scale lines.")> _
        Public Property ScaleLinesMajorInnerRadius() As Int32
            Get
                Return m_ScaleLinesMajorInnerRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMajorInnerRadius <> value Then
                    m_ScaleLinesMajorInnerRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The outer radius of the major scale lines.")> _
        Public Property ScaleLinesMajorOuterRadius() As Int32
            Get
                Return m_ScaleLinesMajorOuterRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMajorOuterRadius <> value Then
                    m_ScaleLinesMajorOuterRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The width of the major scale lines.")> _
        Public Property ScaleLinesMajorWidth() As Int32
            Get
                Return m_ScaleLinesMajorWidth
            End Get
            Set(ByVal value As Int32)
                If m_ScaleLinesMajorWidth <> value Then
                    m_ScaleLinesMajorWidth = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.RefreshProperties(RefreshProperties.All), System.ComponentModel.Description("The range index. set this to a value of 0 up to 4 to change the corresponding range's properties.")> _
        Public Property Range_Idx() As Byte
            Get
                Return m_RangeIdx
            End Get
            Set(ByVal value As Byte)
                If (m_RangeIdx <> value) AndAlso (0 <= value) AndAlso (value < NUMOFRANGES) Then
                    m_RangeIdx = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("Enables or disables the range selected by Range_Idx.")> _
        Public Property RangeEnabled() As Boolean
            Get
                Return m_RangeEnabled(m_RangeIdx)
            End Get
            Set(ByVal value As Boolean)
                If m_RangeEnabled(m_RangeIdx) <> value Then
                    m_RangeEnabled(m_RangeIdx) = value
                    RangesEnabled = m_RangeEnabled
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesEnabled() As Boolean()
            Get
                Return m_RangeEnabled
            End Get
            Set(ByVal value As Boolean())
                m_RangeEnabled = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the range.")> _
        Public Property RangeColor() As Color
            Get
                Return m_RangeColor(m_RangeIdx)
            End Get
            Set(ByVal value As Color)
                If m_RangeColor(m_RangeIdx) <> value Then
                    m_RangeColor(m_RangeIdx) = value
                    RangesColor = m_RangeColor
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesColor() As Color()
            Get
                Return m_RangeColor
            End Get
            Set(ByVal value As Color())
                m_RangeColor = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The start value of the range, must be less than RangeEndValue.")> _
        Public Property RangeStartValue() As Single
            Get
                Return m_RangeStartValue(m_RangeIdx)
            End Get
            Set(ByVal value As Single)
                If (m_RangeStartValue(m_RangeIdx) <> value) AndAlso (value < m_RangeEndValue(m_RangeIdx)) Then
                    m_RangeStartValue(m_RangeIdx) = value
                    RangesStartValue = m_RangeStartValue
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesStartValue() As Single()
            Get
                Return m_RangeStartValue
            End Get
            Set(ByVal value As Single())
                m_RangeStartValue = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The end value of the range. Must be greater than RangeStartValue.")> _
        Public Property RangeEndValue() As Single
            Get
                Return m_RangeEndValue(m_RangeIdx)
            End Get
            Set(ByVal value As Single)
                If (m_RangeEndValue(m_RangeIdx) <> value) AndAlso (m_RangeStartValue(m_RangeIdx) < value) Then
                    m_RangeEndValue(m_RangeIdx) = value
                    RangesEndValue = m_RangeEndValue
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesEndValue() As Single()
            Get
                Return m_RangeEndValue
            End Get
            Set(ByVal value As Single())
                m_RangeEndValue = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The inner radius of the range.")> _
        Public Property RangeInnerRadius() As Int32
            Get
                Return m_RangeInnerRadius(m_RangeIdx)
            End Get
            Set(ByVal value As Int32)
                If m_RangeInnerRadius(m_RangeIdx) <> value Then
                    m_RangeInnerRadius(m_RangeIdx) = value
                    RangesInnerRadius = m_RangeInnerRadius
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesInnerRadius() As Int32()
            Get
                Return m_RangeInnerRadius
            End Get
            Set(ByVal value As Int32())
                m_RangeInnerRadius = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The inner radius of the range.")> _
        Public Property RangeOuterRadius() As Int32
            Get
                Return m_RangeOuterRadius(m_RangeIdx)
            End Get
            Set(ByVal value As Int32)
                If m_RangeOuterRadius(m_RangeIdx) <> value Then
                    m_RangeOuterRadius(m_RangeIdx) = value
                    RangesOuterRadius = m_RangeOuterRadius
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(False)> _
        Public Property RangesOuterRadius() As Int32()
            Get
                Return m_RangeOuterRadius
            End Get
            Set(ByVal value As Int32())
                m_RangeOuterRadius = value
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The radius of the scale numbers.")> _
        Public Property ScaleNumbersRadius() As Int32
            Get
                Return m_ScaleNumbersRadius
            End Get
            Set(ByVal value As Int32)
                If m_ScaleNumbersRadius <> value Then
                    m_ScaleNumbersRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The color of the scale numbers.")> _
        Public Property ScaleNumbersColor() As Color
            Get
                Return m_ScaleNumbersColor
            End Get
            Set(ByVal value As Color)
                If m_ScaleNumbersColor <> value Then
                    m_ScaleNumbersColor = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The format of the scale numbers.")> _
        Public Property ScaleNumbersFormat() As String
            Get
                Return m_ScaleNumbersFormat
            End Get
            Set(ByVal value As String)
                If m_ScaleNumbersFormat <> value Then
                    m_ScaleNumbersFormat = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The number of the scale line to start writing numbers next to.")> _
        Public Property ScaleNumbersStartScaleLine() As Int32
            Get
                Return m_ScaleNumbersStartScaleLine
            End Get
            Set(ByVal value As Int32)
                If m_ScaleNumbersStartScaleLine <> value Then
                    m_ScaleNumbersStartScaleLine = Math.Max(value, 1)
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The number of scale line steps for writing numbers.")> _
        Public Property ScaleNumbersStepScaleLines() As Int32
            Get
                Return m_ScaleNumbersStepScaleLines
            End Get
            Set(ByVal value As Int32)
                If m_ScaleNumbersStepScaleLines <> value Then
                    m_ScaleNumbersStepScaleLines = Math.Max(value, 1)
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The angle relative to the tangent of the base arc at a scale line that is used to rotate numbers. set to 0 for no rotation or e.g. set to 90.")> _
        Public Property ScaleNumbersRotation() As Int32
            Get
                Return m_ScaleNumbersRotation
            End Get
            Set(ByVal value As Int32)
                If m_ScaleNumbersRotation <> value Then
                    m_ScaleNumbersRotation = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The type of the needle, currently only type 0 and 1 are supported. Type 0 looks nicers but if you experience performance problems you might consider using type 1.")> _
        Public Property NeedleType() As Int32
            Get
                Return m_NeedleType
            End Get
            Set(ByVal value As Int32)
                If m_NeedleType <> value Then
                    m_NeedleType = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The radius of the needle.")> _
        Public Property NeedleRadius() As Int32
            Get
                Return m_NeedleRadius
            End Get
            Set(ByVal value As Int32)
                If m_NeedleRadius <> value Then
                    m_NeedleRadius = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The first color of the needle.")> _
        Public Property NeedleColor1() As NeedleColorEnum
            Get
                Return m_NeedleColor1
            End Get
            Set(ByVal value As NeedleColorEnum)
                If m_NeedleColor1 <> value Then
                    m_NeedleColor1 = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The second color of the needle.")> _
        Public Property NeedleColor2() As Color
            Get
                Return m_NeedleColor2
            End Get
            Set(ByVal value As Color)
                If m_NeedleColor2 <> value Then
                    m_NeedleColor2 = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

        <System.ComponentModel.Browsable(True), System.ComponentModel.Category("AGauge"), System.ComponentModel.Description("The width of the needle.")> _
        Public Property NeedleWidth() As Int32
            Get
                Return m_NeedleWidth
            End Get
            Set(ByVal value As Int32)
                If m_NeedleWidth <> value Then
                    m_NeedleWidth = value
                    drawGaugeBackground = True
                    Refresh()
                End If
            End Set
        End Property

#End Region

#Region " helper "

        Private Sub FindFontBounds()
            'find upper and lower bounds for numeric characters
            Dim c1 As Int32
            Dim c2 As Int32
            Dim boundfound As Boolean
            Dim b As Bitmap
            Dim g As Graphics
            Dim backBrush As New SolidBrush(Color.White)
            Dim foreBrush As New SolidBrush(Color.Black)
            Dim boundingBox As SizeF

            b = New Bitmap(5, 5)
            g = Graphics.FromImage(b)
            boundingBox = g.MeasureString("0123456789", Font, -1, StringFormat.GenericTypographic)
            b = New Bitmap(CInt(boundingBox.Width), CInt(boundingBox.Height))
            g = Graphics.FromImage(b)
            g.FillRectangle(backBrush, 0.0F, 0.0F, boundingBox.Width, boundingBox.Height)
            g.DrawString("0123456789", Font, foreBrush, 0.0F, 0.0F, StringFormat.GenericTypographic)

            fontBoundY1 = 0
            fontBoundY2 = 0
            c1 = 0
            boundfound = False
            While (c1 < b.Height) AndAlso (Not boundfound)
                c2 = 0
                While (c2 < b.Width) AndAlso (Not boundfound)
                    If b.GetPixel(c2, c1) <> backBrush.Color Then
                        fontBoundY1 = c1
                        boundfound = True
                    End If
                    System.Math.Max(System.Threading.Interlocked.Increment(c2), c2 - 1)
                End While
                System.Math.Max(System.Threading.Interlocked.Increment(c1), c1 - 1)
            End While

            c1 = b.Height - 1
            boundfound = False
            While (0 < c1) AndAlso (Not boundfound)
                c2 = 0
                While (c2 < b.Width) AndAlso (Not boundfound)
                    If b.GetPixel(c2, c1) <> backBrush.Color Then
                        fontBoundY2 = c1
                        boundfound = True
                    End If
                    System.Math.Max(System.Threading.Interlocked.Increment(c2), c2 - 1)
                End While
                System.Math.Max(System.Threading.Interlocked.Decrement(c1), c1 + 1)
            End While
        End Sub

#End Region

#Region " base member overrides "

        Protected Overloads Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
        End Sub

        Protected Overloads Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
            If (Width < 10) OrElse (Height < 10) Then
                Return
            End If

            If drawGaugeBackground Then
                drawGaugeBackground = False

                FindFontBounds()

                gaugeBitmap = New Bitmap(Width, Height, pe.Graphics)
                Dim ggr As Graphics = Graphics.FromImage(gaugeBitmap)
                ggr.FillRectangle(New SolidBrush(BackColor), ClientRectangle)

                If Not BackgroundImage Is Nothing Then
                    Select Case BackgroundImageLayout
                        Case ImageLayout.Center
                            ggr.DrawImageUnscaled(BackgroundImage, Width / 2 - BackgroundImage.Width / 2, Height / 2 - BackgroundImage.Height / 2)
                            Exit Select
                        Case ImageLayout.None
                            ggr.DrawImageUnscaled(BackgroundImage, 0, 0)
                            Exit Select
                        Case ImageLayout.Stretch
                            ggr.DrawImage(BackgroundImage, 0, 0, Width, Height)
                            Exit Select
                        Case ImageLayout.Tile
                            Dim pixelOffsetX As Int32 = 0
                            Dim pixelOffsetY As Int32 = 0
                            While pixelOffsetX < Width
                                pixelOffsetY = 0
                                While pixelOffsetY < Height
                                    ggr.DrawImageUnscaled(BackgroundImage, pixelOffsetX, pixelOffsetY)
                                    pixelOffsetY += BackgroundImage.Height
                                End While
                                pixelOffsetX += BackgroundImage.Width
                            End While
                            Exit Select
                        Case ImageLayout.Zoom
                            If CSng(BackgroundImage.Width / Width) < CSng(BackgroundImage.Height / Height) Then
                                ggr.DrawImage(BackgroundImage, 0, 0, Height, Height)
                            Else
                                ggr.DrawImage(BackgroundImage, 0, 0, Width, Width)
                            End If
                            Exit Select
                    End Select
                End If

                ggr.SmoothingMode = SmoothingMode.HighQuality
                ggr.PixelOffsetMode = PixelOffsetMode.HighQuality

                Dim gp As New GraphicsPath()
                Dim rangeStartAngle As Single
                Dim rangeSweepAngle As Single
                Dim counter As Int32 = 0
                While counter < NUMOFRANGES
                    If m_RangeEndValue(counter) > m_RangeStartValue(counter) AndAlso m_RangeEnabled(counter) Then
                        rangeStartAngle = m_BaseArcStart + (m_RangeStartValue(counter) - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue)
                        rangeSweepAngle = (m_RangeEndValue(counter) - m_RangeStartValue(counter)) * m_BaseArcSweep / (m_MaxValue - m_MinValue)
                        gp.Reset()
                        gp.AddPie(New Rectangle(m_Center.X - m_RangeOuterRadius(counter), m_Center.Y - m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                        gp.Reverse()
                        gp.AddPie(New Rectangle(m_Center.X - m_RangeInnerRadius(counter), m_Center.Y - m_RangeInnerRadius(counter), 2 * m_RangeInnerRadius(counter), 2 * m_RangeInnerRadius(counter)), rangeStartAngle, rangeSweepAngle)
                        gp.Reverse()
                        ggr.SetClip(gp)
                        ggr.FillPie(New SolidBrush(m_RangeColor(counter)), New Rectangle(m_Center.X - m_RangeOuterRadius(counter), m_Center.Y - m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter), 2 * m_RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                    End If
                    System.Math.Max(System.Threading.Interlocked.Increment(counter), counter - 1)
                End While

                ggr.SetClip(ClientRectangle)
                If m_BaseArcRadius > 0 Then
                    ggr.DrawArc(New Pen(m_BaseArcColor, m_BaseArcWidth), New Rectangle(m_Center.X - m_BaseArcRadius, m_Center.Y - m_BaseArcRadius, 2 * m_BaseArcRadius, 2 * m_BaseArcRadius), m_BaseArcStart, m_BaseArcSweep)
                End If

                Dim valueText As String = ""
                Dim boundingBox As SizeF
                Dim countValue As Single = 0
                Dim counter1 As Int32 = 0
                While countValue <= (m_MaxValue - m_MinValue)
                    valueText = (m_MinValue + countValue).ToString(m_ScaleNumbersFormat)
                    ggr.ResetTransform()
                    boundingBox = ggr.MeasureString(valueText, Font, -1, StringFormat.GenericTypographic)

                    gp.Reset()
                    gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMajorOuterRadius, m_Center.Y - m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius, 2 * m_ScaleLinesMajorOuterRadius))
                    gp.Reverse()
                    gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMajorInnerRadius, m_Center.Y - m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius, 2 * m_ScaleLinesMajorInnerRadius))
                    gp.Reverse()
                    ggr.SetClip(gp)

                    ggr.DrawLine(New Pen(m_ScaleLinesMajorColor, m_ScaleLinesMajorWidth), CSng(Center.X), CSng(Center.Y), CSng(Center.X + 2 * m_ScaleLinesMajorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180)), CSng(Center.Y + 2 * m_ScaleLinesMajorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180)))

                    gp.Reset()
                    gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius))
                    gp.Reverse()
                    gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius))
                    gp.Reverse()
                    ggr.SetClip(gp)

                    If countValue < (m_MaxValue - m_MinValue) Then
                        Dim counter2 As Int32 = 1
                        While counter2 <= m_ScaleLinesMinorNumOf
                            If ((m_ScaleLinesMinorNumOf Mod 2) = 1) AndAlso (CInt(m_ScaleLinesMinorNumOf / 2) + 1 = counter2) Then
                                gp.Reset()
                                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesInterOuterRadius, m_Center.Y - m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius, 2 * m_ScaleLinesInterOuterRadius))
                                gp.Reverse()
                                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesInterInnerRadius, m_Center.Y - m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius, 2 * m_ScaleLinesInterInnerRadius))
                                gp.Reverse()
                                ggr.SetClip(gp)

                                ggr.DrawLine(New Pen(m_ScaleLinesInterColor, m_ScaleLinesInterWidth), CSng(Center.X), CSng(Center.Y), CSng(Center.X + 2 * m_ScaleLinesInterOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((CSng((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180)), CSng(Center.Y + 2 * m_ScaleLinesInterOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((CSng((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180)))

                                gp.Reset()
                                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorOuterRadius, m_Center.Y - m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius, 2 * m_ScaleLinesMinorOuterRadius))
                                gp.Reverse()
                                gp.AddEllipse(New Rectangle(m_Center.X - m_ScaleLinesMinorInnerRadius, m_Center.Y - m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius, 2 * m_ScaleLinesMinorInnerRadius))
                                gp.Reverse()
                                ggr.SetClip(gp)
                            Else
                                ggr.DrawLine(New Pen(m_ScaleLinesMinorColor, m_ScaleLinesMinorWidth), CSng(Center.X), CSng(Center.Y), CSng(Center.X + 2 * m_ScaleLinesMinorOuterRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((CSng((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180)), CSng(Center.Y + 2 * m_ScaleLinesMinorOuterRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue) + counter2 * m_BaseArcSweep / ((CSng((m_MaxValue - m_MinValue) / m_ScaleLinesMajorStepValue)) * (m_ScaleLinesMinorNumOf + 1))) * Math.PI / 180)))
                            End If
                            System.Math.Max(System.Threading.Interlocked.Increment(counter2), counter2 - 1)
                        End While
                    End If

                    ggr.SetClip(ClientRectangle)

                    If m_ScaleNumbersRotation <> 0 Then
                        ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
                        ggr.RotateTransform(90.0F + m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue))
                    End If

                    ggr.TranslateTransform(CSng(Center.X + m_ScaleNumbersRadius * Math.Cos((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0F)), CSng(Center.Y + m_ScaleNumbersRadius * Math.Sin((m_BaseArcStart + countValue * m_BaseArcSweep / (m_MaxValue - m_MinValue)) * Math.PI / 180.0F)), System.Drawing.Drawing2D.MatrixOrder.Append)


                    If counter1 >= ScaleNumbersStartScaleLine - 1 Then
                        ggr.DrawString(valueText, Font, New SolidBrush(m_ScaleNumbersColor), -boundingBox.Width / 2, -fontBoundY1 - (fontBoundY2 - fontBoundY1 + 1) / 2, StringFormat.GenericTypographic)
                    End If

                    countValue += m_ScaleLinesMajorStepValue
                    System.Math.Max(System.Threading.Interlocked.Increment(counter1), counter1 - 1)
                End While

                ggr.ResetTransform()
                ggr.SetClip(ClientRectangle)

                If m_ScaleNumbersRotation <> 0 Then
                    ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
                End If

                'Dim counter As Int32 = 0
                While counter < NUMOFCAPS
                    If m_CapText(counter) <> "" Then
                        ggr.DrawString(m_CapText(counter), Font, New SolidBrush(m_CapColor(counter)), m_CapPosition(counter).X, m_CapPosition(counter).Y, StringFormat.GenericTypographic)
                    End If
                    System.Math.Max(System.Threading.Interlocked.Increment(counter), counter - 1)
                End While
            End If

            pe.Graphics.DrawImageUnscaled(gaugeBitmap, 0, 0)
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

            Dim brushAngle As Single = CInt(m_BaseArcStart + (m_value - m_MinValue) * m_BaseArcSweep / (m_MaxValue - m_MinValue) Mod 360)
            Dim needleAngle As [Double] = brushAngle * Math.PI / 180

            Select Case m_NeedleType
                Case 0
                    Dim points As PointF() = New PointF(3) {}
                    Dim brush1 As Brush = Brushes.White
                    Dim brush2 As Brush = Brushes.White
                    Dim brush3 As Brush = Brushes.White
                    Dim brush4 As Brush = Brushes.White

                    Dim brushBucket As Brush = Brushes.White
                    Dim subcol As Int32 = CInt(((brushAngle + 225) Mod 180) * 100 / 180)
                    Dim subcol2 As Int32 = CInt(((brushAngle + 135) Mod 180) * 100 / 180)

                    pe.Graphics.FillEllipse(New SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                    Select Case m_NeedleColor1
                        Case NeedleColorEnum.Gray
                            brush1 = New SolidBrush(Color.FromArgb(80 + subcol, 80 + subcol, 80 + subcol))
                            brush2 = New SolidBrush(Color.FromArgb(180 - subcol, 180 - subcol, 180 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(80 + subcol2, 80 + subcol2, 80 + subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(180 - subcol2, 180 - subcol2, 180 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Gray, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Red
                            brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, subcol))
                            brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 100 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 100 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Red, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Green
                            brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, subcol))
                            brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 100 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 100 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Green, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Blue
                            brush1 = New SolidBrush(Color.FromArgb(subcol, subcol, 145 + subcol))
                            brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 100 - subcol, 245 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(subcol2, subcol2, 145 + subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 100 - subcol2, 245 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Blue, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Magenta
                            brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, 145 + subcol))
                            brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 245 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, 145 + subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 245 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Magenta, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Violet
                            brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, 145 + subcol))
                            brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 245 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, 145 + subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 245 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                        Case NeedleColorEnum.Yellow
                            brush1 = New SolidBrush(Color.FromArgb(145 + subcol, 145 + subcol, subcol))
                            brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 245 - subcol, 100 - subcol))
                            brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, 145 + subcol2, subcol2))
                            brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 245 - subcol2, 100 - subcol2))
                            pe.Graphics.DrawEllipse(Pens.Violet, Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)
                            Exit Select
                    End Select

                    If Math.Floor(DirectCast((((brushAngle + 225) Mod 360) / 180), Single)) = 0 Then
                        brushBucket = brush1
                        brush1 = brush2
                        brush2 = brushBucket
                    End If

                    If Math.Floor(DirectCast((((brushAngle + 135) Mod 360) / 180), Single)) = 0 Then
                        brush4 = brush3
                    End If

                    points(0).X = CSng(Center.X + m_NeedleRadius * Math.Cos(needleAngle))
                    points(0).Y = CSng(Center.Y + m_NeedleRadius * Math.Sin(needleAngle))
                    points(1).X = CSng(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle))
                    points(1).Y = CSng(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle))
                    points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                    points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                    pe.Graphics.FillPolygon(brush1, points)

                    points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                    points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                    pe.Graphics.FillPolygon(brush2, points)

                    points(0).X = CSng(Center.X - (m_NeedleRadius / 20 - 1) * Math.Cos(needleAngle))
                    points(0).Y = CSng(Center.Y - (m_NeedleRadius / 20 - 1) * Math.Sin(needleAngle))
                    points(1).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                    points(1).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                    points(2).X = CSng(Center.X - m_NeedleRadius / 5 * Math.Cos(needleAngle) + m_NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                    points(2).Y = CSng(Center.Y - m_NeedleRadius / 5 * Math.Sin(needleAngle) + m_NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                    pe.Graphics.FillPolygon(brush4, points)

                    points(0).X = CSng(Center.X - m_NeedleRadius / 20 * Math.Cos(needleAngle))
                    points(0).Y = CSng(Center.Y - m_NeedleRadius / 20 * Math.Sin(needleAngle))
                    points(1).X = CSng(Center.X + m_NeedleRadius * Math.Cos(needleAngle))
                    points(1).Y = CSng(Center.Y + m_NeedleRadius * Math.Sin(needleAngle))

                    pe.Graphics.DrawLine(New Pen(m_NeedleColor2), Center.X, Center.Y, points(0).X, points(0).Y)
                    pe.Graphics.DrawLine(New Pen(m_NeedleColor2), Center.X, Center.Y, points(1).X, points(1).Y)
                    Exit Select
                Case 1
                    Dim startPoint As New Point(CInt(Center.X - m_NeedleRadius / 8 * Math.Cos(needleAngle)), CInt(Center.Y - m_NeedleRadius / 8 * Math.Sin(needleAngle)))
                    Dim endPoint As New Point(CInt(Center.X + m_NeedleRadius * Math.Cos(needleAngle)), CInt(Center.Y + m_NeedleRadius * Math.Sin(needleAngle)))

                    pe.Graphics.FillEllipse(New SolidBrush(m_NeedleColor2), Center.X - m_NeedleWidth * 3, Center.Y - m_NeedleWidth * 3, m_NeedleWidth * 6, m_NeedleWidth * 6)

                    Select Case m_NeedleColor1
                        Case NeedleColorEnum.Gray
                            pe.Graphics.DrawLine(New Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.DarkGray, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Red
                            pe.Graphics.DrawLine(New Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Red, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Green
                            pe.Graphics.DrawLine(New Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Green, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Blue
                            pe.Graphics.DrawLine(New Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Blue, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Magenta
                            pe.Graphics.DrawLine(New Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Magenta, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Violet
                            pe.Graphics.DrawLine(New Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Violet, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                        Case NeedleColorEnum.Yellow
                            pe.Graphics.DrawLine(New Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                            pe.Graphics.DrawLine(New Pen(Color.Yellow, m_NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                            Exit Select
                    End Select
                    Exit Select
            End Select
        End Sub

        Protected Overloads Overrides Sub OnResize(ByVal e As EventArgs)
            drawGaugeBackground = True
            Refresh()
        End Sub

#End Region

    End Class
End Namespace
