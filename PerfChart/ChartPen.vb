Imports System.ComponentModel

<TypeConverterAttribute(GetType(ExpandableObjectConverter))> _
Public Class ChartPen

    Private _Pen As Pen

    Public Sub New()
        _Pen = New Pen(Color.Black)
    End Sub

    Public Property Color() As Color
        Get
            Return _Pen.Color
        End Get
        Set(ByVal value As Color)
            _Pen.Color = value
        End Set
    End Property

    Public Property DashStyle() As System.Drawing.Drawing2D.DashStyle
        Get
            Return _Pen.DashStyle
        End Get
        Set(ByVal value As System.Drawing.Drawing2D.DashStyle)
            _Pen.DashStyle = value
        End Set
    End Property

    Public Property Width() As Single
        Get
            Return _Pen.Width
        End Get
        Set(ByVal value As Single)
            _Pen.Width = value
        End Set
    End Property

    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Public ReadOnly Property Pen() As Pen
        Get
            Return _Pen
        End Get
    End Property

End Class