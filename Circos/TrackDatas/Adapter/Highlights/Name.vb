Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat.ComponentModels
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.ComponentModel.Loci
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace TrackDatas.Highlights

    Public Class Name

        <Column("locus_tag")>
        Public Property Locus As String
        Public Property Name As String
        Public Property Minimum As Integer
        Public Property Maximum As Integer

        Public Function ToMeta() As ValueTrackData
            Dim n As Integer() = {Minimum, Maximum}
            Dim s As New StringBuilder(Regex.Replace(Name, "\s+", "_"))

            Call s.Replace(",", "_")
            Call s.Replace(";", "_")
            Call s.Replace(".", "_")
            Call s.Replace("=", "_")

            Dim ss As String = s.ToString
            ss = Regex.Replace(ss, "[_]+", "_")

            Return New ValueTrackData With {
                .start = n.Min,
                .end = n.Max,
                .value = ss.ParseDouble
            }
        End Function

        Public Function Loci() As Location
            Return New Location(Minimum, Maximum)
        End Function

        Public Shared Iterator Function MatchLocus(source As IEnumerable(Of Name), PTT As PTT) As IEnumerable(Of Name)
            For Each x As Name In source.AsParallel
                Dim matched As GeneBrief = __matches(x, PTT)

                If matched Is Nothing Then
                    Continue For
                End If

                x.Locus = matched.Synonym

                Yield x
            Next
        End Function

        Private Shared Function __matches(loci As Name, PTT As PTT) As GeneBrief
            Dim lcl As Location = loci.Loci
            Dim LQuery As GeneBrief =
                LinqAPI.DefaultFirst(Of GeneBrief) <= From x As GeneBrief
                                                      In PTT.GeneObjects
                                                      Where lcl.Equals(x.Location, 10)
                                                      Select x
            Return LQuery
        End Function
    End Class
End Namespace