Imports System.Text
Imports LANS.SystemsBiology.ComponentModel

Namespace Documents.Karyotype.GeneObjects

    Public Class GeneMark : Inherits GenomeDescription

        Dim briefData As IGeneBrief()

        Sub New(briefData As IGeneBrief())
            Me.briefData = (From item In briefData Select item Order By item.Location.Left Ascending).ToArray
            Me.Size = (From obj In Me.briefData Select {obj.Location.Left, obj.Location.Right}).MatrixToList.Max
        End Sub

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)
            Dim Pre = briefData.First
            Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 1", Pre.Location.Left, Pre.Location.Right))

            For Each GeneObject In briefData.Skip(1)
                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 0", Pre.Location.Right, GeneObject.Location.Left))
                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} 1", GeneObject.Location.Left, GeneObject.Location.Right))
                Pre = GeneObject
            Next

            Return sBuilder.ToString
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property Size As Integer
    End Class
End Namespace