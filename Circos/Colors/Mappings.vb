Imports System.Drawing

Namespace Colors

    Public Class Mappings
        Public Property value As Double
        Public Property Level As Integer
        Public Property Color As Color

        Public ReadOnly Property CircosColor As String
            Get
                Return $"({Color.R},{Color.G},{Color.B})"
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return $"{value} ==> {Level}   @{Color.ToString}"
        End Function
    End Class
End Namespace