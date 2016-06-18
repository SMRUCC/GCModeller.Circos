Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Colors
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace Documents.Karyotype.Highlights

    Public MustInherit Class IdentityColors

        ''' <summary>
        ''' 默认的颜色
        ''' </summary>
        ''' <returns></returns>
        Public Property [Default] As String

        Public MustOverride Function GetColor(identity As Double) As String

    End Class

    Public Class IdentityLevels : Inherits IdentityColors

        Sub New([default] As String)
            Me.Default = [default]
        End Sub

        Public Overrides Function GetColor(identity As Double) As String
            Select Case identity
                Case 0 To 0.5 : Return CircosColor.FromColor(Drawing.Color.Blue)
                Case 0.5 To 0.7 : Return CircosColor.FromColor(Drawing.Color.Green)
                Case 0.7 To 0.8 : Return CircosColor.FromColor(Drawing.Color.Violet)
                Case Else
                    Return [Default]
            End Select
        End Function
    End Class

    Public Class IdentityGradients : Inherits IdentityColors

        Dim min, max As Double
        Dim d As Double()
        Dim depth As Integer
        Dim dCl As String()

        Sub New(min As Double, max As Double, Optional depth As Integer = 10, Optional [default] As String = "Brown", Optional mapName As String = "Jet")
            Me.min = min
            Me.max = max
            Me.depth = depth
            Me.d = New Double(depth) {}
            Me.Default = [default]

            Dim dd = (max - min) / depth
            Dim n As Double = min

            For i As Integer = 0 To depth
                d(i) = n
                n += dd
            Next

            Dim maps = GradientMaps.GradientMappings(d, mapName:=mapName, mapLevel:=depth, replaceBase:=True)
            dCl = maps.ToArray(Function(x) x.CircosColor)
        End Sub

        Public Overrides Function GetColor(identity As Double) As String
            For i As Integer = 0 To depth - 1
                If identity.RangesAt(d(i), d(i + 1)) Then
                    Return dCl(i)
                End If
            Next

            Return Me.Default
        End Function
    End Class
End Namespace