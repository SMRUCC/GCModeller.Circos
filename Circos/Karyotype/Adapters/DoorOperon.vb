Imports System.Text
Imports LANS.SystemsBiology.Assembly.DOOR
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Linq

Namespace Karyotype

    ''' <summary>
    ''' 最外层的Ideogram，
    ''' </summary>
    Public Class DOOROperon : Inherits SkeletonInfo

        Sub New(DoorFile As String)
            Dim DOOR As DOOR = DOOR_API.Load(DoorFile)
            Me.__bands = New List(Of Band)(GenerateDocument(DOOR))
            Me.Size = __bands.Select(Function(x) {x.start, x.end}).MatrixAsIterator.Max
            Call __karyotype()
        End Sub

        Private Overloads Iterator Function GenerateDocument(DOOR As DOOR) As IEnumerable(Of Band)
            For Each Operon As Operon In DOOR.DOOROperonView
                Dim loci As Integer() = New Integer() {
                    CInt(Operon.InitialX.Location.Left),
                    CInt(Operon.LastGene.Location.Right)
                }
                Yield New Band With {
                    .chrName = "chr1",
                    .bandX = Operon.Key,
                    .bandY = Operon.InitialX.Synonym,
                    .start = loci.Min,
                    .end = loci.Max,
                    .color = "blue"
                }
            Next
        End Function

        Public Overrides ReadOnly Property Size As Integer
    End Class
End Namespace