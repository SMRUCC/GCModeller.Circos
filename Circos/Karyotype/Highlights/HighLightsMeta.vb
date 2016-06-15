Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat.ComponentModels
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.ComponentModel.Loci
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace Documents.Karyotype.Highlights

    Public Class HighLightsMeta
        Public Property Left As Integer
        Public Property Right As Integer
        Public Property Value As String
        Public Property Color As String

        Public Overrides Function ToString() As String
            Return $"{Left} {Right} {Value} {Color}"
        End Function

        Public Shared Function FromColorMapping(cl As Circos.Colors.Mappings, idx As Integer, offset As Integer) As HighLightsMeta
            Return New HighLightsMeta With {
                .Color = $"({cl.Color.R},{cl.Color.G},{cl.Color.B})",
                .Left = idx,
                .Right = idx + 1 + offset,
                .Value = CStr(cl.value)
            }
        End Function

        Public Shared Function Distinct(source As IEnumerable(Of HighLightsMeta)) As HighLightsMeta()
            Dim LQuery = (From x As HighLightsMeta In source
                          Select x,
                              uid = $"{x.Left}..{x.Right}"
                          Group By uid Into Group) _
                             .ToArray(Function(x) x.Group.First.x)
            Return LQuery
        End Function
    End Class

    Namespace LocusLabels

        Public Class Name

            <Column("locus_tag")>
            Public Property Locus As String
            Public Property Name As String
            Public Property Minimum As Integer
            Public Property Maximum As Integer

            Public Function ToMeta() As HighLightsMeta
                Dim n = {Minimum, Maximum}
                Dim s As StringBuilder = New StringBuilder(Regex.Replace(Name, "\s+", "_"))

                Call s.Replace(",", "_")
                Call s.Replace(";", "_")
                Call s.Replace(".", "_")
                Call s.Replace("=", "_")

                Dim ss = s.ToString
                ss = Regex.Replace(ss, "[_]+", "_")

                Return New HighLightsMeta With {
                    .Left = n.Min,
                    .Right = n.Max,
                    .Value = ss
                }
            End Function

            Public Function Loci() As Location
                Return New Location(Minimum, Maximum)
            End Function

            Public Shared Function MatchLocus(source As IEnumerable(Of Name), PTT As PTT) As Name()
                Dim LQuery As Name() = (From x As Name In source.AsParallel
                                        Let matched As GeneBrief = __matches(x, PTT)
                                        Where Not matched Is Nothing
                                        Select x.InvokeSet(NameOf(x.Locus), matched.Synonym)).ToArray
                Return LQuery
            End Function

            Private Shared Function __matches(loci As Name, PTT As PTT) As GeneBrief
                Dim lcl As Location = loci.Loci
                Dim LQuery = (From x As GeneBrief In PTT.GeneObjects
                              Where lcl.Equals(x.Location, 10)
                              Select x).FirstOrDefault
                Return LQuery
            End Function
        End Class
    End Namespace
End Namespace