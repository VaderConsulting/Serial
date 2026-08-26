' Copyright (C) 2007 A.J.Bauer
'
'  This software is provided as-is, without any express or implied
'  warranty.  In no event will the authors be held liable for any damages
'  arising from the use of this software.

'  Permission is granted to anyone to use this software for any purpose,
'  including commercial applications, and to alter it and redistribute it
'  freely, subject to the following restrictions:

'  1. The origin of this software must not be misrepresented; you must not
'     claim that you wrote the original software. if you use this software
'     in a product, an acknowledgment in the product documentation would be
'     appreciated but is not required.
'  2. Altered source versions must be plainly marked as such, and must not be
'     misrepresented as being the original software.
'  3. This notice may not be removed or altered from any source distribution.

'http://www.ucancode.net/CSharp_Tutorial_GDI+_Gauge_Source_Code.htm

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Diagnostics

    <ToolboxBitmapAttribute(GetType(Gauge), "Gauge.bmp"), DefaultEvent("ValueInRangeChanged"), Description("Displays a value on an analog gauge. Raises an event if the value enters one of the definable ranges.")> _
    Partial Public Class Gauge
        Inherits Control

#Region "enum, var, delegate, event"

        Public Enum NeedleColorEnum
            Gray = 0
            Red = 1
            Green = 2
            Blue = 3
            Yellow = 4
            Violet = 5
            Magenta = 6
        End Enum

        Private Const ZERO As [Byte] = 0
        Private Const NUMOFCAPS As [Byte] = 5
        Private Const NUMOFRANGES As [Byte] = 5

        Private fontBoundY1 As [Single]
        Private fontBoundY2 As [Single]
        Private gaugeBitmap As Bitmap
        Private drawGaugeBackground As [Boolean] = True

    Private _value As [Single]
    Private _valueIsInRange As [Boolean]() = {False, False, False, False, False}
    Private _CapIdx As [Byte] = 1
    Private _CapColor As Color() = {Color.Black, Color.Black, Color.Black, Color.Black, Color.Black}
    Private _CapText As [String]() = {"", "", "", "", ""}
    Private _CapPosition As Point() = {New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10), New Point(10, 10)}
    Private _Center As New Point(100, 100)
    Private _MinValue As [Single] = -100
    Private _MaxValue As [Single] = 400

    Private _BaseArcColor As Color = Color.Gray
    Private _BaseArcRadius As Int32 = 80
    Private _BaseArcStart As Int32 = 135
    Private _BaseArcSweep As Int32 = 270
    Private _BaseArcWidth As Int32 = 2

    Private _ScaleLinesInterColor As Color = Color.Black
    Private _ScaleLinesInterInnerRadius As Int32 = 73
    Private _ScaleLinesInterOuterRadius As Int32 = 80
    Private _ScaleLinesInterWidth As Int32 = 1

    Private _ScaleLinesMinorNumOf As Int32 = 9
    Private _ScaleLinesMinorColor As Color = Color.Gray
    Private _ScaleLinesMinorInnerRadius As Int32 = 75
    Private _ScaleLinesMinorOuterRadius As Int32 = 80
    Private _ScaleLinesMinorWidth As Int32 = 1

    Private _ScaleLinesMajorStepValue As [Single] = 50.0F
    Private _ScaleLinesMajorColor As Color = Color.Black
    Private _ScaleLinesMajorInnerRadius As Int32 = 70
    Private _ScaleLinesMajorOuterRadius As Int32 = 80
    Private _ScaleLinesMajorWidth As Int32 = 2

    Private _RangeIdx As [Byte]
    Private _RangeEnabled As [Boolean]() = {True, True, False, False, False}
    Private _RangeColor As Color() = {Color.LightGreen, Color.Red, Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control), Color.FromKnownColor(KnownColor.Control)}
    Private _RangeStartValue As [Single]() = {-100.0F, 300.0F, 0.0F, 0.0F, 0.0F}
    Private _RangeEndValue As [Single]() = {300.0F, 400.0F, 0.0F, 0.0F, 0.0F}
    Private _RangeInnerRadius As Int32() = {70, 70, 70, 70, 70}
    Private _RangeOuterRadius As Int32() = {80, 80, 80, 80, 80}

    Private _ScaleNumbersRadius As Int32 = 95
    Private _ScaleNumbersColor As Color = Color.Black
    Private _ScaleNumbersFormat As [String]
    Private _ScaleNumbersStartScaleLine As Int32
    Private _ScaleNumbersStepScaleLines As Int32 = 1
    Private _ScaleNumbersRotation As Int32 = 0

    Private _NeedleType As Int32 = 0
    Private _NeedleRadius As Int32 = 80
    Private _NeedleColor1 As NeedleColorEnum = NeedleColorEnum.Gray
    Private _NeedleColor2 As Color = Color.DimGray
    Private _NeedleWidth As Int32 = 2

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

#Region "hidden, overridden inherited properties"

    Public Shadows Property AllowDrop() As [Boolean]
        Get
            Return False
        End Get

        Set(ByVal value As [Boolean])
        End Set
    End Property

    Public Shadows Property AutoSize() As [Boolean]
        Get
            Return False
        End Get

        Set(ByVal value As [Boolean])
        End Set
    End Property

    Public Shadows Property ForeColor() As [Boolean]
        Get
            Return False
        End Get
        Set(ByVal value As [Boolean])
        End Set
    End Property

    Public Shadows Property ImeMode() As [Boolean]
        Get
            Return False
        End Get
        Set(ByVal value As [Boolean])
        End Set
    End Property

    Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            MyBase.BackColor = value
            drawGaugeBackground = True
            Refresh()
        End Set
    End Property

    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            MyBase.Font = value
            drawGaugeBackground = True
            Refresh()
        End Set
    End Property

    Public Overrides Property BackgroundImageLayout() As System.Windows.Forms.ImageLayout
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
        'InitializeComponent()

        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

#Region "properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The value.")> _
    Public Property Value() As [Single]
        Get
            Return _value
        End Get
        Set(ByVal value As [Single])
            If _value <> value Then
                _value = Math.Min(Math.Max(value, _MinValue), _MaxValue)

                If Me.DesignMode Then
                    drawGaugeBackground = True
                End If

                For counter As Int32 = 0 To NUMOFRANGES - 2
                    If (_RangeStartValue(counter) <= _value) AndAlso (_value <= _RangeEndValue(counter)) AndAlso (_RangeEnabled(counter)) Then
                        If Not _valueIsInRange(counter) Then
                            RaiseEvent ValueInRangeChanged(Me, New ValueInRangeChangedEventArgs(counter))
                        End If
                    Else
                        _valueIsInRange(counter) = False
                    End If
                Next
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.RefreshProperties(RefreshProperties.All), System.ComponentModel.Description("The caption index. set this to a value of 0 up to 4 to change the corresponding caption's properties.")> _
    Public Property Cap_Idx() As [Byte]
        Get
            Return _CapIdx
        End Get
        Set(ByVal value As [Byte])
            If (_CapIdx <> value) AndAlso (0 <= value) AndAlso (value < 5) Then
                _CapIdx = value
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the caption text.")> _
    Private Property CapColor() As Color
        Get
            Return _CapColor(_CapIdx)
        End Get
        Set(ByVal value As Color)
            If _CapColor(_CapIdx) <> value Then
                _CapColor(_CapIdx) = value
                CapColors = _CapColor
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property CapColors() As Color()
        Get
            Return _CapColor
        End Get
        Set(ByVal value As Color())
            _CapColor = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The text of the caption.")> _
    Public Property CapText() As [String]
        Get
            Return _CapText(_CapIdx)
        End Get
        Set(ByVal value As [String])
            If _CapText(_CapIdx) <> value Then
                _CapText(_CapIdx) = value
                CapsText = _CapText
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property CapsText() As [String]()
        Get
            Return _CapText
        End Get
        Set(ByVal value As [String]())
            For counter As Int32 = 0 To 4
                _CapText(counter) = value(counter)
            Next
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The position of the caption.")> _
    Public Property CapPosition() As Point
        Get
            Return _CapPosition(_CapIdx)
        End Get
        Set(ByVal value As Point)
            If _CapPosition(_CapIdx) <> value Then
                _CapPosition(_CapIdx) = value
                CapsPosition = _CapPosition
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property CapsPosition() As Point()
        Get
            Return _CapPosition
        End Get
        Set(ByVal value As Point())
            _CapPosition = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The center of the gauge (in the control's client area).")> _
    Public Property Center() As Point
        Get
            Return _Center
        End Get
        Set(ByVal value As Point)
            If _Center <> value Then
                _Center = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The minimum value to show on the scale.")> _
    Public Property MinValue() As [Single]
        Get
            Return _MinValue
        End Get
        Set(ByVal value As [Single])
            If (_MinValue <> value) AndAlso (value < _MaxValue) Then
                _MinValue = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The maximum value to show on the scale.")> _
    Public Property MaxValue() As [Single]
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As [Single])
            If (_MaxValue <> value) AndAlso (value > _MinValue) Then
                _MaxValue = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the base arc.")> _
    Public Property BaseArcColor() As Color
        Get
            Return _BaseArcColor
        End Get
        Set(ByVal value As Color)
            If _BaseArcColor <> value Then
                _BaseArcColor = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The radius of the base arc.")> _
    Public Property BaseArcRadius() As Int32
        Get
            Return _BaseArcRadius
        End Get
        Set(ByVal value As Int32)
            If _BaseArcRadius <> value Then
                _BaseArcRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The start angle of the base arc.")> _
    Public Property BaseArcStart() As Int32
        Get
            Return _BaseArcStart
        End Get
        Set(ByVal value As Int32)
            If _BaseArcStart <> value Then
                _BaseArcStart = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The sweep angle of the base arc.")> _
    Public Property BaseArcSweep() As Int32
        Get
            Return _BaseArcSweep
        End Get
        Set(ByVal value As Int32)
            If _BaseArcSweep <> value Then
                _BaseArcSweep = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The width of the base arc.")> _
    Public Property BaseArcWidth() As Int32
        Get
            Return _BaseArcWidth
        End Get
        Set(ByVal value As Int32)
            If _BaseArcWidth <> value Then
                _BaseArcWidth = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
    Public Property ScaleLinesInterColor() As Color
        Get
            Return _ScaleLinesInterColor
        End Get
        Set(ByVal value As Color)
            If _ScaleLinesInterColor <> value Then
                _ScaleLinesInterColor = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The inner radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
    Public Property ScaleLinesInterInnerRadius() As Int32
        Get
            Return _ScaleLinesInterInnerRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesInterInnerRadius <> value Then
                _ScaleLinesInterInnerRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The outer radius of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
    Public Property ScaleLinesInterOuterRadius() As Int32
        Get
            Return _ScaleLinesInterOuterRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesInterOuterRadius <> value Then
                _ScaleLinesInterOuterRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The width of the inter scale lines which are the middle scale lines for an uneven number of minor scale lines.")> _
    Public Property ScaleLinesInterWidth() As Int32
        Get
            Return _ScaleLinesInterWidth
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesInterWidth <> value Then
                _ScaleLinesInterWidth = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The number of minor scale lines.")> _
    Public Property ScaleLinesMinorNumOf() As Int32
        Get
            Return _ScaleLinesMinorNumOf
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMinorNumOf <> value Then
                _ScaleLinesMinorNumOf = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the minor scale lines.")> _
    Public Property ScaleLinesMinorColor() As Color
        Get
            Return _ScaleLinesMinorColor
        End Get
        Set(ByVal value As Color)
            If _ScaleLinesMinorColor <> value Then
                _ScaleLinesMinorColor = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The inner radius of the minor scale lines.")> _
    Public Property ScaleLinesMinorInnerRadius() As Int32
        Get
            Return _ScaleLinesMinorInnerRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMinorInnerRadius <> value Then
                _ScaleLinesMinorInnerRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The outer radius of the minor scale lines.")> _
    Public Property ScaleLinesMinorOuterRadius() As Int32
        Get
            Return _ScaleLinesMinorOuterRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMinorOuterRadius <> value Then
                _ScaleLinesMinorOuterRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The width of the minor scale lines.")> _
    Public Property ScaleLinesMinorWidth() As Int32
        Get
            Return _ScaleLinesMinorWidth
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMinorWidth <> value Then
                _ScaleLinesMinorWidth = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The step value of the major scale lines.")> _
    Public Property ScaleLinesMajorStepValue() As [Single]
        Get
            Return _ScaleLinesMajorStepValue
        End Get
        Set(ByVal value As [Single])
            If (_ScaleLinesMajorStepValue <> value) AndAlso (value > 0) Then
                _ScaleLinesMajorStepValue = Math.Max(Math.Min(value, _MaxValue), _MinValue)
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the major scale lines.")> _
    Public Property ScaleLinesMajorColor() As Color
        Get
            Return _ScaleLinesMajorColor
        End Get
        Set(ByVal value As Color)
            If _ScaleLinesMajorColor <> value Then
                _ScaleLinesMajorColor = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The inner radius of the major scale lines.")> _
    Public Property ScaleLinesMajorInnerRadius() As Int32
        Get
            Return _ScaleLinesMajorInnerRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMajorInnerRadius <> value Then
                _ScaleLinesMajorInnerRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The outer radius of the major scale lines.")> _
    Public Property ScaleLinesMajorOuterRadius() As Int32
        Get
            Return _ScaleLinesMajorOuterRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMajorOuterRadius <> value Then
                _ScaleLinesMajorOuterRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The width of the major scale lines.")> _
    Public Property ScaleLinesMajorWidth() As Int32
        Get
            Return _ScaleLinesMajorWidth
        End Get
        Set(ByVal value As Int32)
            If _ScaleLinesMajorWidth <> value Then
                _ScaleLinesMajorWidth = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.RefreshProperties(RefreshProperties.All), System.ComponentModel.Description("The range index. set this to a value of 0 up to 4 to change the corresponding range's properties.")> _
    Public Property Range_Idx() As [Byte]
        Get
            Return _RangeIdx
        End Get
        Set(ByVal value As [Byte])
            If (_RangeIdx <> value) AndAlso (0 <= value) AndAlso (value < NUMOFRANGES) Then
                _RangeIdx = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("Enables or disables the range selected by Range_Idx.")> _
    Public Property RangeEnabled() As [Boolean]
        Get
            Return _RangeEnabled(_RangeIdx)
        End Get
        Set(ByVal value As [Boolean])
            If _RangeEnabled(_RangeIdx) <> value Then
                _RangeEnabled(_RangeIdx) = value
                RangesEnabled = _RangeEnabled
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesEnabled() As [Boolean]()
        Get
            Return _RangeEnabled
        End Get
        Set(ByVal value As [Boolean]())
            _RangeEnabled = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the range.")> _
    Public Property RangeColor() As Color
        Get
            Return _RangeColor(_RangeIdx)
        End Get
        Set(ByVal value As Color)
            If _RangeColor(_RangeIdx) <> value Then
                _RangeColor(_RangeIdx) = value
                RangesColor = _RangeColor
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesColor() As Color()
        Get
            Return _RangeColor
        End Get
        Set(ByVal value As Color())
            _RangeColor = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The start value of the range, must be less than RangeEndValue.")> _
    Public Property RangeStartValue() As [Single]
        Get
            Return _RangeStartValue(_RangeIdx)
        End Get
        Set(ByVal value As [Single])
            If (_RangeStartValue(_RangeIdx) <> value) AndAlso (value < _RangeEndValue(_RangeIdx)) Then
                _RangeStartValue(_RangeIdx) = value
                RangesStartValue = _RangeStartValue
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesStartValue() As [Single]()
        Get
            Return _RangeStartValue
        End Get
        Set(ByVal value As [Single]())
            _RangeStartValue = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The end value of the range. Must be greater than RangeStartValue.")> _
    Public Property RangeEndValue() As [Single]
        Get
            Return _RangeEndValue(_RangeIdx)
        End Get
        Set(ByVal value As [Single])
            If (_RangeEndValue(_RangeIdx) <> value) AndAlso (_RangeStartValue(_RangeIdx) < value) Then
                _RangeEndValue(_RangeIdx) = value
                RangesEndValue = _RangeEndValue
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesEndValue() As [Single]()
        Get
            Return _RangeEndValue
        End Get
        Set(ByVal value As [Single]())
            _RangeEndValue = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The inner radius of the range.")> _
    Public Property RangeInnerRadius() As Int32
        Get
            Return _RangeInnerRadius(_RangeIdx)
        End Get
        Set(ByVal value As Int32)
            If _RangeInnerRadius(_RangeIdx) <> value Then
                _RangeInnerRadius(_RangeIdx) = value
                RangesInnerRadius = _RangeInnerRadius
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesInnerRadius() As Int32()
        Get
            Return _RangeInnerRadius
        End Get
        Set(ByVal value As Int32())
            _RangeInnerRadius = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The inner radius of the range.")> _
    Public Property RangeOuterRadius() As Int32
        Get
            Return _RangeOuterRadius(_RangeIdx)
        End Get
        Set(ByVal value As Int32)
            If _RangeOuterRadius(_RangeIdx) <> value Then
                _RangeOuterRadius(_RangeIdx) = value
                RangesOuterRadius = _RangeOuterRadius
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Property RangesOuterRadius() As Int32()
        Get
            Return _RangeOuterRadius
        End Get
        Set(ByVal value As Int32())
            _RangeOuterRadius = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The radius of the scale numbers.")> _
    Public Property ScaleNumbersRadius() As Int32
        Get
            Return _ScaleNumbersRadius
        End Get
        Set(ByVal value As Int32)
            If _ScaleNumbersRadius <> value Then
                _ScaleNumbersRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The color of the scale numbers.")> _
    Public Property ScaleNumbersColor() As Color
        Get
            Return _ScaleNumbersColor
        End Get
        Set(ByVal value As Color)
            If _ScaleNumbersColor <> value Then
                _ScaleNumbersColor = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The format of the scale numbers.")> _
    Public Property ScaleNumbersFormat() As [String]
        Get
            Return _ScaleNumbersFormat
        End Get
        Set(ByVal value As [String])
            If _ScaleNumbersFormat <> value Then
                _ScaleNumbersFormat = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The number of the scale line to start writing numbers next to.")> _
    Public Property ScaleNumbersStartScaleLine() As Int32
        Get
            Return _ScaleNumbersStartScaleLine
        End Get
        Set(ByVal value As Int32)
            If _ScaleNumbersStartScaleLine <> value Then
                _ScaleNumbersStartScaleLine = Math.Max(value, 1)
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The number of scale line steps for writing numbers.")> _
    Public Property ScaleNumbersStepScaleLines() As Int32
        Get
            Return _ScaleNumbersStepScaleLines
        End Get
        Set(ByVal value As Int32)
            If _ScaleNumbersStepScaleLines <> value Then
                _ScaleNumbersStepScaleLines = Math.Max(value, 1)
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The angle relative to the tangent of the base arc at a scale line that is used to rotate numbers. set to 0 for no rotation or e.g. set to 90.")> _
    Public Property ScaleNumbersRotation() As Int32
        Get
            Return _ScaleNumbersRotation
        End Get
        Set(ByVal value As Int32)
            If _ScaleNumbersRotation <> value Then
                _ScaleNumbersRotation = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The type of the needle, currently only type 0 and 1 are supported. Type 0 looks nicers but if you experience performance problems you might consider using type 1.")> _
    Public Property NeedleType() As Int32
        Get
            Return _NeedleType
        End Get
        Set(ByVal value As Int32)
            If _NeedleType <> value Then
                _NeedleType = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The radius of the needle.")> _
    Public Property NeedleRadius() As Int32
        Get
            Return _NeedleRadius
        End Get
        Set(ByVal value As Int32)
            If _NeedleRadius <> value Then
                _NeedleRadius = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The first color of the needle.")> _
    Public Property NeedleColor1() As NeedleColorEnum
        Get
            Return _NeedleColor1
        End Get
        Set(ByVal value As NeedleColorEnum)
            If _NeedleColor1 <> value Then
                _NeedleColor1 = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The second color of the needle.")> _
    Public Property NeedleColor2() As Color
        Get
            Return _NeedleColor2
        End Get
        Set(ByVal value As Color)
            If _NeedleColor2 <> value Then
                _NeedleColor2 = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Category("VBGauge"), System.ComponentModel.Description("The width of the needle.")> _
    Public Property NeedleWidth() As Int32
        Get
            Return _NeedleWidth
        End Get
        Set(ByVal value As Int32)
            If _NeedleWidth <> value Then
                _NeedleWidth = value
                drawGaugeBackground = True
                Refresh()
            End If
        End Set
    End Property

#End Region

#Region "helper"

    Private Sub FindFontBounds()
        'find upper and lower bounds for numeric characters
        Dim c1 As Int32
        Dim c2 As Int32
        Dim boundfound As [Boolean]
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
                c2 += 1
            End While
            c1 += 1
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
                c2 += 1
            End While
            c1 -= 1
        End While
    End Sub

#End Region

#Region "base member overrides"

    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
        If (Width < 10) OrElse (Height < 10) Then
            Return
        End If

        If drawGaugeBackground Then
            drawGaugeBackground = False

            FindFontBounds()

            gaugeBitmap = New Bitmap(Width, Height, pe.Graphics)
            Dim ggr As Graphics = Graphics.FromImage(gaugeBitmap)
            ggr.FillRectangle(New SolidBrush(BackColor), ClientRectangle)

            If BackgroundImage IsNot Nothing Then
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
            Dim rangeStartAngle As [Single]
            Dim rangeSweepAngle As [Single]
            For counter As Int32 = 0 To NUMOFRANGES - 1
                If _RangeEndValue(counter) > _RangeStartValue(counter) AndAlso _RangeEnabled(counter) Then
                    rangeStartAngle = _BaseArcStart + (_RangeStartValue(counter) - _MinValue) * _BaseArcSweep / (_MaxValue - _MinValue)
                    rangeSweepAngle = (_RangeEndValue(counter) - _RangeStartValue(counter)) * _BaseArcSweep / (_MaxValue - _MinValue)
                    gp.Reset()
                    gp.AddPie(New Rectangle(_Center.X - _RangeOuterRadius(counter), _Center.Y - _RangeOuterRadius(counter), 2 * _RangeOuterRadius(counter), 2 * _RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                    gp.Reverse()
                    gp.AddPie(New Rectangle(_Center.X - _RangeInnerRadius(counter), _Center.Y - _RangeInnerRadius(counter), 2 * _RangeInnerRadius(counter), 2 * _RangeInnerRadius(counter)), rangeStartAngle, rangeSweepAngle)
                    gp.Reverse()
                    ggr.SetClip(gp)
                    ggr.FillPie(New SolidBrush(_RangeColor(counter)), New Rectangle(_Center.X - _RangeOuterRadius(counter), _Center.Y - _RangeOuterRadius(counter), 2 * _RangeOuterRadius(counter), 2 * _RangeOuterRadius(counter)), rangeStartAngle, rangeSweepAngle)
                End If
            Next

            ggr.SetClip(ClientRectangle)
            If _BaseArcRadius > 0 Then
                ggr.DrawArc(New Pen(_BaseArcColor, _BaseArcWidth), New Rectangle(_Center.X - _BaseArcRadius, _Center.Y - _BaseArcRadius, 2 * _BaseArcRadius, 2 * _BaseArcRadius), _BaseArcStart, _BaseArcSweep)
            End If

            Dim valueText As [String] = ""
            Dim boundingBox As SizeF
            Dim countValue As [Single] = 0
            Dim counter1 As Int32 = 0
            While countValue <= (_MaxValue - _MinValue)
                valueText = (_MinValue + countValue).ToString(_ScaleNumbersFormat)
                ggr.ResetTransform()
                boundingBox = ggr.MeasureString(valueText, Font, -1, StringFormat.GenericTypographic)

                gp.Reset()
                gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMajorOuterRadius, _Center.Y - _ScaleLinesMajorOuterRadius, 2 * _ScaleLinesMajorOuterRadius, 2 * _ScaleLinesMajorOuterRadius))
                gp.Reverse()
                gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMajorInnerRadius, _Center.Y - _ScaleLinesMajorInnerRadius, 2 * _ScaleLinesMajorInnerRadius, 2 * _ScaleLinesMajorInnerRadius))
                gp.Reverse()
                ggr.SetClip(gp)

                ggr.DrawLine(New Pen(_ScaleLinesMajorColor, _ScaleLinesMajorWidth), CSng(Center.X), CSng(Center.Y), CSng(Center.X + 2 * _ScaleLinesMajorOuterRadius * Math.Cos((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue)) * Math.PI / 180.0)), CSng(Center.Y + 2 * _ScaleLinesMajorOuterRadius * Math.Sin((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue)) * Math.PI / 180.0)))

                gp.Reset()
                gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMinorOuterRadius, _Center.Y - _ScaleLinesMinorOuterRadius, 2 * _ScaleLinesMinorOuterRadius, 2 * _ScaleLinesMinorOuterRadius))
                gp.Reverse()
                gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMinorInnerRadius, _Center.Y - _ScaleLinesMinorInnerRadius, 2 * _ScaleLinesMinorInnerRadius, 2 * _ScaleLinesMinorInnerRadius))
                gp.Reverse()
                ggr.SetClip(gp)

                If countValue < (_MaxValue - _MinValue) Then
                    For counter2 As Int32 = 1 To _ScaleLinesMinorNumOf
                        If ((_ScaleLinesMinorNumOf Mod 2) = 1) AndAlso (CInt(_ScaleLinesMinorNumOf / 2) + 1 = counter2) Then
                            gp.Reset()
                            gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesInterOuterRadius, _Center.Y - _ScaleLinesInterOuterRadius, 2 * _ScaleLinesInterOuterRadius, 2 * _ScaleLinesInterOuterRadius))
                            gp.Reverse()
                            gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesInterInnerRadius, _Center.Y - _ScaleLinesInterInnerRadius, 2 * _ScaleLinesInterInnerRadius, 2 * _ScaleLinesInterInnerRadius))
                            gp.Reverse()
                            ggr.SetClip(gp)

                            ggr.DrawLine(New Pen(_ScaleLinesInterColor, _ScaleLinesInterWidth), _
                                         CSng(Center.X), CSng(Center.Y), _
                                         CSng(Center.X + 2 * _ScaleLinesInterOuterRadius * Math.Cos((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue) + counter2 * _BaseArcSweep / (CSng((_MaxValue - _MinValue) / _ScaleLinesMajorStepValue) * (_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)), _
                                         CSng(Center.Y + 2 * _ScaleLinesInterOuterRadius * Math.Sin((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue) + counter2 * _BaseArcSweep / (CSng((_MaxValue - _MinValue) / _ScaleLinesMajorStepValue) * (_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)))

                            gp.Reset()
                            gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMinorOuterRadius, _Center.Y - _ScaleLinesMinorOuterRadius, 2 * _ScaleLinesMinorOuterRadius, 2 * _ScaleLinesMinorOuterRadius))
                            gp.Reverse()
                            gp.AddEllipse(New Rectangle(_Center.X - _ScaleLinesMinorInnerRadius, _Center.Y - _ScaleLinesMinorInnerRadius, 2 * _ScaleLinesMinorInnerRadius, 2 * _ScaleLinesMinorInnerRadius))
                            gp.Reverse()
                            ggr.SetClip(gp)
                        Else
                            ggr.DrawLine(New Pen(_ScaleLinesMinorColor, _ScaleLinesMinorWidth), _
                                         CSng(Center.X), CSng(Center.Y), _
                                         CSng(Center.X + 2 * _ScaleLinesMinorOuterRadius * Math.Cos((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue) + counter2 * _BaseArcSweep / (CSng((_MaxValue - _MinValue) / _ScaleLinesMajorStepValue) * (_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)), _
                                         CSng(Center.Y + 2 * _ScaleLinesMinorOuterRadius * Math.Sin((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue) + counter2 * _BaseArcSweep / (CSng((_MaxValue - _MinValue) / _ScaleLinesMajorStepValue) * (_ScaleLinesMinorNumOf + 1))) * Math.PI / 180.0)))
                        End If
                    Next
                End If

                ggr.SetClip(ClientRectangle)

                If _ScaleNumbersRotation <> 0 Then
                    ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
                    ggr.RotateTransform(90.0F + _BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue))
                End If

                ggr.TranslateTransform(CSng(Center.X + _ScaleNumbersRadius * Math.Cos((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue)) * Math.PI / 180.0F)), CSng(Center.Y + _ScaleNumbersRadius * Math.Sin((_BaseArcStart + countValue * _BaseArcSweep / (_MaxValue - _MinValue)) * Math.PI / 180.0F)), System.Drawing.Drawing2D.MatrixOrder.Append)


                If counter1 >= ScaleNumbersStartScaleLine - 1 Then
                    ggr.DrawString(valueText, Font, New SolidBrush(_ScaleNumbersColor), -boundingBox.Width / 2, -fontBoundY1 - (fontBoundY2 - fontBoundY1 + 1) / 2, StringFormat.GenericTypographic)
                End If

                countValue += _ScaleLinesMajorStepValue
                counter1 += 1
            End While

            ggr.ResetTransform()
            ggr.SetClip(ClientRectangle)

            If _ScaleNumbersRotation <> 0 Then
                ggr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
            End If

            For counter As Int32 = 0 To NUMOFCAPS - 1
                If _CapText(counter) <> "" Then
                    ggr.DrawString(_CapText(counter), Font, New SolidBrush(_CapColor(counter)), _CapPosition(counter).X, _CapPosition(counter).Y, StringFormat.GenericTypographic)
                End If
            Next
        End If

        pe.Graphics.DrawImageUnscaled(gaugeBitmap, 0, 0)
        pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

        Dim brushAngle As [Single] = CInt(_BaseArcStart + (_value - _MinValue) * _BaseArcSweep / (_MaxValue - _MinValue)) Mod 360
        Dim needleAngle As [Double] = brushAngle * Math.PI / 180

        Select Case _NeedleType
            Case 0
                Dim points As PointF() = New PointF(2) {}
                Dim brush1 As Brush = Brushes.White
                Dim brush2 As Brush = Brushes.White
                Dim brush3 As Brush = Brushes.White
                Dim brush4 As Brush = Brushes.White

                Dim brushBucket As Brush = Brushes.White
                Dim subcol As Int32 = CInt(((brushAngle + 225) Mod 180) * 100 / 180)
                Dim subcol2 As Int32 = CInt(((brushAngle + 135) Mod 180) * 100 / 180)

                pe.Graphics.FillEllipse(New SolidBrush(_NeedleColor2), Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                Select Case _NeedleColor1
                    Case NeedleColorEnum.Gray
                        brush1 = New SolidBrush(Color.FromArgb(80 + subcol, 80 + subcol, 80 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(180 - subcol, 180 - subcol, 180 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(80 + subcol2, 80 + subcol2, 80 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(180 - subcol2, 180 - subcol2, 180 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Gray, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Red
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Red, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Green
                        brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Green, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Blue
                        brush1 = New SolidBrush(Color.FromArgb(subcol, subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 100 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 100 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Blue, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Magenta
                        brush1 = New SolidBrush(Color.FromArgb(subcol, 145 + subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(100 - subcol, 245 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(subcol2, 145 + subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(100 - subcol2, 245 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Magenta, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Violet
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, subcol, 145 + subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 100 - subcol, 245 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, subcol2, 145 + subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 100 - subcol2, 245 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Violet, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                    Case NeedleColorEnum.Yellow
                        brush1 = New SolidBrush(Color.FromArgb(145 + subcol, 145 + subcol, subcol))
                        brush2 = New SolidBrush(Color.FromArgb(245 - subcol, 245 - subcol, 100 - subcol))
                        brush3 = New SolidBrush(Color.FromArgb(145 + subcol2, 145 + subcol2, subcol2))
                        brush4 = New SolidBrush(Color.FromArgb(245 - subcol2, 245 - subcol2, 100 - subcol2))
                        pe.Graphics.DrawEllipse(Pens.Violet, Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)
                        Exit Select
                End Select

                If Math.Floor(CSng(((brushAngle + 225) Mod 360) / 180.0)) = 0 Then
                    brushBucket = brush1
                    brush1 = brush2
                    brush2 = brushBucket
                End If

                If Math.Floor(CSng(((brushAngle + 135) Mod 360) / 180.0)) = 0 Then
                    brush4 = brush3
                End If

                points(0).X = CSng(Center.X + _NeedleRadius * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y + _NeedleRadius * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X - _NeedleRadius / 20 * Math.Cos(needleAngle))
                points(1).Y = CSng(Center.Y - _NeedleRadius / 20 * Math.Sin(needleAngle))
                points(2).X = CSng(Center.X - _NeedleRadius / 5 * Math.Cos(needleAngle) + _NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                points(2).Y = CSng(Center.Y - _NeedleRadius / 5 * Math.Sin(needleAngle) + _NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                pe.Graphics.FillPolygon(brush1, points)

                points(2).X = CSng(Center.X - _NeedleRadius / 5 * Math.Cos(needleAngle) + _NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                points(2).Y = CSng(Center.Y - _NeedleRadius / 5 * Math.Sin(needleAngle) + _NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                pe.Graphics.FillPolygon(brush2, points)

                points(0).X = CSng(Center.X - (_NeedleRadius / 20 - 1) * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y - (_NeedleRadius / 20 - 1) * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X - _NeedleRadius / 5 * Math.Cos(needleAngle) + _NeedleWidth * 2 * Math.Cos(needleAngle + Math.PI / 2))
                points(1).Y = CSng(Center.Y - _NeedleRadius / 5 * Math.Sin(needleAngle) + _NeedleWidth * 2 * Math.Sin(needleAngle + Math.PI / 2))
                points(2).X = CSng(Center.X - _NeedleRadius / 5 * Math.Cos(needleAngle) + _NeedleWidth * 2 * Math.Cos(needleAngle - Math.PI / 2))
                points(2).Y = CSng(Center.Y - _NeedleRadius / 5 * Math.Sin(needleAngle) + _NeedleWidth * 2 * Math.Sin(needleAngle - Math.PI / 2))
                pe.Graphics.FillPolygon(brush4, points)

                points(0).X = CSng(Center.X - _NeedleRadius / 20 * Math.Cos(needleAngle))
                points(0).Y = CSng(Center.Y - _NeedleRadius / 20 * Math.Sin(needleAngle))
                points(1).X = CSng(Center.X + _NeedleRadius * Math.Cos(needleAngle))
                points(1).Y = CSng(Center.Y + _NeedleRadius * Math.Sin(needleAngle))

                pe.Graphics.DrawLine(New Pen(_NeedleColor2), Center.X, Center.Y, points(0).X, points(0).Y)
                pe.Graphics.DrawLine(New Pen(_NeedleColor2), Center.X, Center.Y, points(1).X, points(1).Y)
                Exit Select
            Case 1
                Dim startPoint As New Point(CInt(Center.X - _NeedleRadius / 8 * Math.Cos(needleAngle)), CInt(Center.Y - _NeedleRadius / 8 * Math.Sin(needleAngle)))
                Dim endPoint As New Point(CInt(Center.X + _NeedleRadius * Math.Cos(needleAngle)), CInt(Center.Y + _NeedleRadius * Math.Sin(needleAngle)))

                pe.Graphics.FillEllipse(New SolidBrush(_NeedleColor2), Center.X - _NeedleWidth * 3, Center.Y - _NeedleWidth * 3, _NeedleWidth * 6, _NeedleWidth * 6)

                Select Case _NeedleColor1
                    Case NeedleColorEnum.Gray
                        pe.Graphics.DrawLine(New Pen(Color.DarkGray, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.DarkGray, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Red
                        pe.Graphics.DrawLine(New Pen(Color.Red, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Red, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Green
                        pe.Graphics.DrawLine(New Pen(Color.Green, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Green, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Blue
                        pe.Graphics.DrawLine(New Pen(Color.Blue, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Blue, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Magenta
                        pe.Graphics.DrawLine(New Pen(Color.Magenta, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Magenta, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Violet
                        pe.Graphics.DrawLine(New Pen(Color.Violet, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Violet, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                    Case NeedleColorEnum.Yellow
                        pe.Graphics.DrawLine(New Pen(Color.Yellow, _NeedleWidth), Center.X, Center.Y, endPoint.X, endPoint.Y)
                        pe.Graphics.DrawLine(New Pen(Color.Yellow, _NeedleWidth), Center.X, Center.Y, startPoint.X, startPoint.Y)
                        Exit Select
                End Select
                Exit Select
        End Select
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        drawGaugeBackground = True
        Refresh()
    End Sub

#End Region

    End Class