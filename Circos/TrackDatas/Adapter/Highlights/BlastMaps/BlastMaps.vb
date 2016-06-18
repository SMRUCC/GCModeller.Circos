Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult

Namespace Documents.Karyotype.Highlights

    ''' <summary>
    ''' 必须是同一个物种的
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BlastMaps : Inherits Highlights

        Dim hits As HitRecord()
        Dim Color As String
        Dim identityMode As IdentityColors

        Sub New(hits As HitRecord(), Color As String, Optional identityMode As IdentityColors = Nothing)
            Me.hits = hits
            Me.Color = Color
            Me.identityMode = identityMode
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

        Private Function __identitiesColor() As String
            Dim DocBuilder As StringBuilder = New StringBuilder(4096)

            For Each hit As HitRecord In hits
                Dim d As Double = hit.Identity
                Dim cl As String = identityMode.GetColor(d)
                Dim s As String =
                    $"chr1 {hit.QueryStart} {hit.QueryEnd} fill_color={cl}"
                Call DocBuilder.AppendLine(s)
            Next

            Return DocBuilder.ToString
        End Function

        Protected Overrides Function GenerateDocument() As String
            If Not identityMode Is Nothing Then
                Return __identitiesColor()
            End If

            Dim doc As StringBuilder = New StringBuilder(4096)

            For Each hit As HitRecord In hits
                Call doc.AppendLine($"chr1 {hit.QueryStart} {hit.QueryEnd} fill_color={Color}")
            Next

            Return doc.ToString
        End Function

        Public Overrides Function ToString() As String
            Dim ssID As String = SubjectSpecies
            If String.IsNullOrEmpty(ssID) Then
                Return MyBase.ToString
            Else
                Return ssID
            End If
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Call __throwSourceNullEx(hits)
                Return (From hit As HitRecord In hits Select hit.Identity).ToArray.Max
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Call __throwSourceNullEx(hits)
                Return (From hit As HitRecord In hits Select hit.Identity).ToArray.Min
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace