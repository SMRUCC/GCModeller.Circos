Imports System.Text
Imports LANS.SystemsBiology.Assembly.DOOR

Namespace Documents.Karyotype

    ''' <summary>
    ''' 最外层的Ideogram，
    ''' </summary>
    Public Class DoorOperon : Inherits TrackDataDocument

        Dim Door As DOOR

        Sub New(DoorFile As String)
            Door = DOOR_API.Load(DoorFile)
        End Sub

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)

            For Each Operon As Operon In Door.DOOROperonView
                Dim loci As Integer() = New Integer() {
                    CInt(Operon.InitialX.Location.Left),
                    CInt(Operon.LastGene.Location.Right)
                }
                Dim s As String = $"chr1 {loci.Min} {loci.Max} {1.0R}"
                Call sBuilder.AppendLine(s)
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
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace