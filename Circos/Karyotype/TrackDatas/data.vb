Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Serialization

Namespace Documents.Karyotype.TrackDatas

    ''' <summary>
    ''' Tracks data document generator
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class data(Of T As TrackData) : Inherits List(Of T)

        Sub New()
            Call MyBase.New(0)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Function GetDocumentText() As String
            Dim sb As New StringBuilder

            For Each x As T In Me
                If Not String.IsNullOrEmpty(x.comment) Then
                    Call sb.AppendLine("# " & x.comment)
                End If
                Call sb.AppendLine(x.ToString)
            Next

            Return sb.ToString
        End Function
    End Class
End Namespace