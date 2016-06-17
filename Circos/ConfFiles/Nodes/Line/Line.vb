Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic

Namespace Documents.Configurations.Nodes.Plots.Lines

    Public Class Line : Inherits TracksPlot

        <SimpleConfig> Public Property color As String = "vdgrey"
        <SimpleConfig> Public Property max_gap As String = "1u"

        Public Property Backgrounds As List(Of Background)
        Public Property Axes As List(Of Axis)

        Public Sub New(Data As Karyotype.TrackDataDocument)
            Call MyBase.New(Data)

            Me.Axes = New List(Of Axis) From {New Axis}
            Me.Backgrounds = New List(Of Background) From {New Background}
        End Sub

        <SimpleConfig()> Public Overrides ReadOnly Property type As String
            Get
                Return "line"
            End Get
        End Property

        Protected Overrides Function GeneratePlotsElementListChunk() As Dictionary(Of String, List(Of CircosDocument))
            Dim Dict = MyBase.GeneratePlotsElementListChunk
            If Dict.IsNullOrEmpty Then
                Dict = New Dictionary(Of String, List(Of CircosDocument))
            End If

            If Not Me.Axes.IsNullOrEmpty Then Call Dict.Add("axes", (From item In Me.Axes Select DirectCast(item, CircosDocument)).ToList)
            If Not Me.Backgrounds.IsNullOrEmpty Then Call Dict.Add("backgrounds", (From item In Me.Backgrounds Select DirectCast(item, CircosDocument)).ToList)

            Return Dict
        End Function

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of Line)(Me)
        End Function
    End Class
End Namespace