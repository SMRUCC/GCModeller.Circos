Imports LANS.SystemsBiology.SequenceModel.ISequenceModel
Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel

Namespace Documents.Karyotype.NtProps

    ''' <summary>
    ''' G+C/G-C偏移量
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GCSkew : Inherits TrackDataDocument

        Dim _Steps As Integer
        Dim ChunkBuffer As Double()

        Sub New(SequenceModel As I_PolymerSequenceModel, SlideWindowSize As Integer, Steps As Integer, Circular As Boolean)
            _Steps = Steps
            ChunkBuffer = NucleotideModels.GCSkew(
                SequenceModel,
                SlideWindowSize,
                Steps,
                Circular)
        End Sub

        Sub New(data As IEnumerable(Of Double), [step] As Integer)
            _Steps = [step]
            ChunkBuffer = data.ToArray

            Dim Avg = data.Average
            For i As Integer = 0 To ChunkBuffer.Length - 1
                ChunkBuffer(i) = ChunkBuffer(i) - Avg
            Next
        End Sub

        Protected Sub New()
        End Sub

        Public Overrides Function ToString() As String
            Return String.Join(", ", ChunkBuffer)
        End Function

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(10240)
            Dim p As Integer

            For i As Integer = 0 To ChunkBuffer.Count - 1
                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} {2}", p, p + _Steps, ChunkBuffer(i)))
                p += _Steps
            Next

            Return sBuilder.ToString
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Return ChunkBuffer.Max
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return ChunkBuffer.Min
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return True
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Length"></param>
        ''' <param name="Width">Width设置为0的时候为1个像素的宽度</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CreateLineData(Length As Integer, Width As Integer) As GCSkew
            Return New GCSkew With {
                .ChunkBuffer = New Double() {CDbl(Width)},
                ._Steps = Length
            }
        End Function
    End Class
End Namespace