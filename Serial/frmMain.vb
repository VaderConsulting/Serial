Public Class frmMain

    Private Delegate Sub UpdateControlDelegate(ByVal Value As Double)
    Private _FormClosing As Int32 = 0 ' Normally I would use a Boolean, but Interlocked doesn't work on them

    Private _XOffset As Int32 = My.Settings.XOffset
    Private _YOffset As Int32 = My.Settings.YOffset
    Private _ZOffset As Int32 = My.Settings.ZOffset

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If SerialPort1.IsOpen Then
            ' Thread safe
            _FormClosing = Threading.Interlocked.Increment(_FormClosing)
            RemoveHandler SerialPort1.DataReceived, AddressOf SerialPort1_DataReceived
            ' -----------
            Try
                Application.Exit()
            Catch
                Application.Exit()
            End Try
        End If
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        If _FormClosing Then Exit Sub

        SyncLock SerialPort1
            If SerialPort1.IsOpen Then
                Try
                    If Threading.Interlocked.Read(CLng(_FormClosing)) = 1 Then Exit Sub

                    Dim Data As String = SerialPort1.ReadLine
                    Dim Values() As String = Split(Data, " ")

                    For i As Int16 = 0 To Values.Length - 1
                        Dim Elements() As String = Values(i).Split(":")
                        If Elements.Length = 2 Then
                            Dim Command As String = Elements(0)
                            Dim Value As String = Elements(1)

                            Select Case Command.ToLower
                                Case "li0"
                                    'If Threading.Interlocked.Read(CLng(_FormClosing)) = 0 Then SetLight(CInt(Value))
                                Case "acx"
                                    If Threading.Interlocked.Read(CLng(_FormClosing)) = 0 Then SetValueX(CDbl((Value / 10) - _XOffset))
                                Case "acy"
                                    If Threading.Interlocked.Read(CLng(_FormClosing)) = 0 Then SetValueY(CDbl((Value / 10) - _YOffset))
                                Case "acz"
                                    If Threading.Interlocked.Read(CLng(_FormClosing)) = 0 Then SetValueZ(CDbl((Value / 10) - _ZOffset))
                                Case "tc0"
                                    'If Threading.Interlocked.Read(CLng(_FormClosing)) = 0 Then SetTemp1(CInt(Value))
                                Case "tc1"
                                Case "tc2"
                            End Select
                        End If
                    Next

                    'Trace.Write(Data)
                Catch ex As Exception
                End Try

            End If
        End SyncLock
    End Sub

    <STAThread()> _
    Private Sub SetValueX(ByVal Value As Double)
        If Threading.Interlocked.Read(CLng(_FormClosing)) = 1 Then Exit Sub

        If GaugeX.InvokeRequired Then
            Dim oDelegate As New UpdateControlDelegate(AddressOf SetValueX)
            Me.Invoke(oDelegate, New Object() {Value})
        Else
            Dim ChartValue As Double = Value
            PerfChartX.AddValue(ChartValue)
            GaugeX.Value = Math.Round(Value, 2)
            lblX.Text = Math.Round(Value, 2)
        End If

    End Sub

    <STAThread()> _
    Private Sub SetValueY(ByVal Value As Double)
        If Threading.Interlocked.Read(CLng(_FormClosing)) = 1 Then Exit Sub

        If GaugeY.InvokeRequired Then
            Dim oDelegate As New UpdateControlDelegate(AddressOf SetValueY)
            Me.Invoke(oDelegate, New Object() {Value})
        Else
            Dim ChartValue As Double = Value
            PerfChartY.AddValue(ChartValue)
            GaugeY.Value = Math.Round(Value, 2)
            lblY.Text = Math.Round(Value, 2)
        End If

    End Sub

    <STAThread()> _
    Private Sub SetValueZ(ByVal Value As Double)
        If Threading.Interlocked.Read(CLng(_FormClosing)) = 1 Then Exit Sub

        If GaugeZ.InvokeRequired Then
            Dim oDelegate As New UpdateControlDelegate(AddressOf SetValueZ)
            Me.Invoke(oDelegate, New Object() {Value})
        Else
            Dim ChartValue As Double = Value
            PerfChartZ.AddValue(ChartValue)
            GaugeZ.Value = Math.Round(Value, 2)
            lblZ.Text = Math.Round(Value, 2)
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SerialPort1.PortName = My.Settings.COMPort
        If Not SerialPort1.IsOpen Then
            SerialPort1.DtrEnable = True
            SerialPort1.ReadBufferSize = 256
            SerialPort1.Open()
        End If
    End Sub

End Class
