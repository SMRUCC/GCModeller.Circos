Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult

Namespace TrackDatas.Highlights

    ''' <summary>
    ''' 必须是同一个物种的
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BlastMaps : Inherits Highlights

        Dim hits As HitRecord()
        Dim Color As String
        Dim identityMode As IdentityColors
        Dim chr As String

        ''' <summary>
        ''' 使用直方图来显示比对成功的区域
        ''' </summary>
        ''' <param name="hits"></param>
        ''' <param name="Color"></param>
        ''' <param name="identityMode"></param>
        ''' <param name="chr"></param>
        Sub New(hits As HitRecord(), Color As String, Optional identityMode As IdentityColors = Nothing, Optional chr As String = "chr1")
            Me.hits = hits
            Me.Color = Color
            Me.identityMode = identityMode
            Me.chr = chr
        End Sub

        Public ReadOnly Property SpeciesColor As String
            Get
                Return Color
            End Get
        End Property

        Public ReadOnly Property SubjectSpecies As String
            Get
                If hits.IsNullOrEmpty Then
                    Return ""
                Else
                    Return hits.First.SubjectIDs
                End If
            End Get
        End Property

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of ValueTrackData)
            Dim color As Func(Of Double, String)

            If Not identityMode Is Nothing Then
                color = AddressOf identityMode.GetColor
            Else
                color = Function(d) Me.Color
            End If

            For Each hit As HitRecord In hits
                Dim d As Double = hit.Identity
                Dim cl As String = color(d)

                Yield New ValueTrackData With {
                    .chr = chr,
                    .start = hit.QueryStart,
                    .end = hit.QueryEnd,
                    .formatting = New Formatting With {
                        .fill_color = cl
                    },
                    .value = 1
                }
            Next
        End Function

        Public Overrides Function ToString() As String
            Dim ssID As String = SubjectSpecies
            If String.IsNullOrEmpty(ssID) Then
                Return MyBase.ToString
            Else
                Return ssID
            End If
        End Function
    End Class
End Namespace