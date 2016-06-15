Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports System.Text

Namespace Documents.Configurations

    Public Class Ticks : Inherits ConfigDoc
        Implements ICircosDocument

        <SimpleConfig> Public Property show_ticks As String = yes
        <SimpleConfig> Public Property show_tick_labels As String = yes

        <SimpleConfig> Public Property show_grid As String = no
        <SimpleConfig> Public Property grid_start As String = "dims(ideogram,radius_inner)-0.5r"
        <SimpleConfig> Public Property grid_end As String = "dims(ideogram,radius_inner)"

        Public Property Ticks As Nodes.Ticks

        Public Overrides ReadOnly Property IsSystemConfig As Boolean
            Get
                Return False
            End Get
        End Property

        Sub New(Circos As Circos)
            Call MyBase.New("ticks.conf", Circos)
            Ticks = Nodes.Ticks.DefaultConfiguration
        End Sub

        Protected Friend Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)
            For Each strLine As String In SimpleConfig.GenerateConfigurations(Me)
                Call sBuilder.AppendLine(strLine)
            Next

            Call sBuilder.AppendLine()
            Call sBuilder.AppendLine(Ticks.GenerateDocument(IndentLevel + 2))

            Return sBuilder.ToString
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            FilePath = getPath(FilePath)
            If Encoding Is Nothing Then Encoding = System.Text.Encoding.ASCII
            Return GenerateDocument(0).SaveTo(FilePath, Encoding)
        End Function
    End Class
End Namespace