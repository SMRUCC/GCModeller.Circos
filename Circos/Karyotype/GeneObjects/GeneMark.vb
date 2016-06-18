'Imports System.Text
'Imports LANS.SystemsBiology.ComponentModel
'Imports Microsoft.VisualBasic.Linq

'Namespace Karyotype.GeneObjects

'    Public Class GeneMark : Inherits SkeletonInfo

'        Dim briefData As IGeneBrief()

'        Sub New(briefData As IGeneBrief())
'            Me.briefData = (From item In briefData Select item Order By item.Location.Left Ascending).ToArray
'            Me.Size = (From g As IGeneBrief In Me.briefData Select {g.Location.Left, g.Location.Right}).MatrixAsIterator.Max
'        End Sub

'        Protected Overrides Function GenerateDocument() As String
'            Dim sBuilder As StringBuilder = New StringBuilder(1024)
'            Dim Pre = briefData.First
'            Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 1", Pre.Location.Left, Pre.Location.Right))

'            For Each GeneObject In briefData.Skip(1)
'                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 0", Pre.Location.Right, GeneObject.Location.Left))
'                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 1", GeneObject.Location.Left, GeneObject.Location.Right))
'                Pre = GeneObject
'            Next

'            Return sBuilder.ToString
'        End Function

'        Public Overrides ReadOnly Property Size As Integer
'    End Class
'End Namespace