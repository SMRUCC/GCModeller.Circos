Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace TrackDatas.Highlights

    Public Class Repeat : Inherits Highlights

        Sub New(repeat As IEnumerable(Of NtProps.Repeat), attrs As IEnumerable(Of Double))
            Dim clMaps As IdentityColors = New IdentityGradients(attrs.Min, attrs.Max, 512)
            Dim v As Double() = attrs.ToArray
            Me.__source = New List(Of ValueTrackData)(repeat.ToArray(Function(x) __creates(x, maps:=clMaps, attrs:=v)))
        End Sub

        Private Shared Function __creates(loci As NtProps.Repeat, maps As IdentityColors, attrs As Double()) As ValueTrackData
            Dim left As Integer = CInt(Val(loci.Minimum.Replace(",", "")))
            Dim Right As Integer = CInt(Val(loci.Maximum.Replace(",", "")))
            Dim r As Double() = attrs.Skip(left).Take(Right - left).ToArray

            Return New ValueTrackData With {
                .start = left,
                .end = Right,
                .formatting = New Formatting With {
                    .fill_color = maps.GetColor(r.Average)
                }
            }
        End Function

        Sub New(repeat As IEnumerable(Of NtProps.Repeat), Optional Color As String = "Brown")
            Me.__source =
                LinqAPI.MakeList(Of ValueTrackData) <= From x As NtProps.Repeat
                                                       In repeat
                                                       Select New ValueTrackData With {
                                                           .start = CInt(Val(x.Minimum)),
                                                           .end = CInt(Val(x.Maximum)),
                                                           .formatting = New Formatting With {
                                                               .fill_color = Color
                                                           }
                                                       }
        End Sub
    End Class
End Namespace