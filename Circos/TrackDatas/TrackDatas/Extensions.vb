Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Linq

Namespace TrackDatas

    Public Module Extensions

        Public Function FromColorMapping(cl As Circos.Colors.Mappings, idx As Integer, offset As Integer) As ValueTrackData
            Return New ValueTrackData With {
                .formatting = New Formatting With {
                    .fill_color = $"({cl.Color.R},{cl.Color.G},{cl.Color.B})"
                },
                .start = idx,
                .end = idx + 1 + offset,
                .value = cl.value
            }
        End Function

        Public Function Distinct(source As IEnumerable(Of ValueTrackData)) As ValueTrackData()
            Dim LQuery As ValueTrackData() = (From x As ValueTrackData
                                              In source
                                              Select x,
                                                  uid = $"{x.start}..{x.end}"
                                              Group By uid Into Group) _
                                                 .ToArray(Function(x) x.Group.First.x)
            Return LQuery
        End Function

        <Extension>
        Public Function Ranges(data As IEnumerable(Of ValueTrackData)) As DoubleRange
            Dim bufs As Double() = data.ToArray(Function(x) x.value)
            Return New DoubleRange(bufs)
        End Function
    End Module
End Namespace