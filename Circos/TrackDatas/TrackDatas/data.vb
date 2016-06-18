Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization

Namespace TrackDatas

    ''' <summary>
    ''' Tracks data document generator.(使用这个对象生成data文件夹之中的数据文本文件)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class data(Of T As ITrackData) : Implements IEnumerable(Of T)

        Public Property FileName As String

        Protected __source As List(Of T)

        ''' <summary>
        ''' Gets the element type <typeparamref name="T"/>
        ''' </summary>
        ''' <returns></returns>
        Public Overloads Function [GetType]() As Type
            Return GetType(T)
        End Function

        Sub New(source As IEnumerable(Of T))
            __source = New List(Of T)(source)
        End Sub

        Protected Sub New()
        End Sub

        ''' <summary>
        ''' <see cref="GetJson"/>
        ''' </summary>
        ''' <returns></returns>
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

        Public Overridable Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            For Each x As T In __source.SafeQuery
                Yield x
            Next
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Class
End Namespace