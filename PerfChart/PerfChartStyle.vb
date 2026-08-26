Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Drawing

<TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class PerfChartStyle

    Private _verticalGridPen As ChartPen
    Private _horizontalGridPen As ChartPen
    Private _avgLinePen As ChartPen
    Private _chartLinePen As ChartPen

    Private _backgroundColorTop As Color = Color.DarkGreen
    Private _backgroundColorBottom As Color = Color.DarkGreen

    Private _showVerticalGridLines As Boolean = True
    Private _showHorizontalGridLines As Boolean = True
    Private _showAverageLine As Boolean = True
    Private _antiAliasing As Boolean = True

    Public Sub New()
        _verticalGridPen = New ChartPen()
        _horizontalGridPen = New ChartPen()
        _avgLinePen = New ChartPen()
        _chartLinePen = New ChartPen()
    End Sub

    Public Property ShowVerticalGridLines() As Boolean
        Get
            Return _showVerticalGridLines
        End Get
        Set(ByVal value As Boolean)
            _showVerticalGridLines = value
        End Set
    End Property

    Public Property ShowHorizontalGridLines() As Boolean
        Get
            Return _showHorizontalGridLines
        End Get
        Set(ByVal value As Boolean)
            _showHorizontalGridLines = value
        End Set
    End Property

    Public Property ShowAverageLine() As Boolean
        Get
            Return _showAverageLine
        End Get
        Set(ByVal value As Boolean)
            _showAverageLine = value
        End Set
    End Property

    Public Property VerticalGridPen() As ChartPen
        Get
            Return _verticalGridPen
        End Get
        Set(ByVal value As ChartPen)
            _verticalGridPen = value
        End Set
    End Property

    Public Property HorizontalGridPen() As ChartPen
        Get
            Return _horizontalGridPen
        End Get
        Set(ByVal value As ChartPen)
            _horizontalGridPen = value
        End Set
    End Property

    Public Property AvgLinePen() As ChartPen
        Get
            Return _avgLinePen
        End Get
        Set(ByVal value As ChartPen)
            _avgLinePen = value
        End Set
    End Property

    Public Property ChartLinePen() As ChartPen
        Get
            Return _chartLinePen
        End Get
        Set(ByVal value As ChartPen)
            _chartLinePen = value
        End Set
    End Property

    Public Property AntiAliasing() As Boolean
        Get
            Return _antiAliasing
        End Get
        Set(ByVal value As Boolean)
            _antiAliasing = value
        End Set
    End Property

    Public Property BackgroundColorTop() As Color
        Get
            Return _backgroundColorTop
        End Get
        Set(ByVal value As Color)
            _backgroundColorTop = value
        End Set
    End Property

    Public Property BackgroundColorBottom() As Color
        Get
            Return _backgroundColorBottom
        End Get
        Set(ByVal value As Color)
            _backgroundColorBottom = value
        End Set
    End Property

End Class
