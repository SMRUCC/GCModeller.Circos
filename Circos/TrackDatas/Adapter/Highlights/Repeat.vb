Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace TrackDatas.Highlights

    Public Class Repeat : Inherits Highlights

        Sub New(repeat As IEnumerable(Of Circos.Repeat), attrs As IEnumerable(Of Double))
            Dim clMaps As IdentityColors = New IdentityGradients(attrs.Min, attrs.Max, 512)
            Dim v As Double() = attrs.ToArray
            Me._highLights = repeat.ToArray(Function(x) __creates(x, maps:=clMaps, attrs:=v))
        End Sub

        Private Shared Function __creates(loci As Circos.Repeat, maps As IdentityColors, attrs As Double()) As HighLightsMeta
            Dim left As Integer = CInt(Val(loci.Minimum.Replace(",", "")))
            Dim Right As Integer = CInt(Val(loci.Maximum.Replace(",", "")))
            Dim r As Double() = attrs.Skip(left).Take(Right - left).ToArray
            Return New HighLightsMeta With {
                .left = left,
                .Right = Right,
                .Color = maps.GetColor(r.Average)
            }
        End Function

        Sub New(repeat As IEnumerable(Of Circos.Repeat), Optional Color As String = "Brown")
            Me._highLights = repeat.ToArray(
                Function(x) New HighLightsMeta With {
                    .Left = CInt(Val(x.Minimum)),
                    .Right = CInt(Val(x.Maximum)),
                    .Color = Color
                    })
        End Sub
    End Class
End Namespace