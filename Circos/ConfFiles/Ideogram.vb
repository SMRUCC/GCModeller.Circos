Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports System.Text
Namespace Documents.Configurations

    Public Class Ideogram : Inherits ConfigDoc
        Implements ICircosDocument

        Public Property Ideogram As Nodes.Ideogram = New Nodes.Ideogram

        Public Overrides ReadOnly Property IsSystemConfig As Boolean
            Get
                Return False
            End Get
        End Property

        Sub New(Circos As Circos)
            Call MyBase.New(IdeogramConf, Circos)
        End Sub

        Protected Friend Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Ideogram.GenerateDocument(IndentLevel)
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            FilePath = getPath(FilePath)
            If Encoding Is Nothing Then Encoding = System.Text.Encoding.ASCII
            Return GenerateDocument(0).SaveTo(FilePath, Encoding)
        End Function
    End Class
End Namespace