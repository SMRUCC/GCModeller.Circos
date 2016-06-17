Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Serialization

Namespace Documents.Karyotype.TrackDatas

    ''' <summary>
    ''' Tracks data document generator.(使用这个对象生成data文件夹之中的数据文本文件)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class data(Of T As ITrackData) : Inherits List(Of T)

        Public Property FileName As String

        Sub New(source As IEnumerable(Of T))
            Call MyBase.New(source)
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
                Call sb.AppendLine(x.GetLineData)
            Next

            Return sb.ToString
        End Function
    End Class
End Namespace