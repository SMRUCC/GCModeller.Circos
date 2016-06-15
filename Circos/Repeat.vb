Imports LANS.SystemsBiology.ComponentModel.Loci

Public Class Repeat
    Public Property Name As String
    Public Property Minimum As String
    Public Property Maximum As String
    Public Property Length As Integer
    Public Property Direction As String

    Public ReadOnly Property Strands As Strands
        Get
            Return GetStrand(Direction)
        End Get
    End Property
End Class
