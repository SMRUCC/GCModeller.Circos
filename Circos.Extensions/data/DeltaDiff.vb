Imports System.Text
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.NCBIBlastResult
Imports LANS.SystemsBiology.SequenceModel
Imports LANS.SystemsBiology.AnalysisTools.ComparativeGenomics
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel.DataStructures

Namespace Documents.Karyotype.NtProps

    Public Class DeltaDiff : Inherits KaryotypeDocument

        Dim _Steps As Integer
        Dim ChunkBuffer As Double()

        Sub New(SequenceModel As I_PolymerSequenceModel, SlideWindowSize As Integer, Steps As Integer)
            Me._Steps = Steps
            Dim NT = New NucleotideModels.NucleicAcid(SequenceModel)
            Dim SW = NT.ToArray.CreateSlideWindows(SlideWindowSize, Steps)
            Dim NT_Cache = New NucleicAcid(NT.ToArray)
            Dim ChunkBuffer = (From n In SW.AsParallel
                               Select n.Left,
                                   d = Sigma(NT_Cache, New NucleotideModels.NucleicAcid(n.Elements))
                               Order By Left Ascending).ToArray

            Dim LastSegment = SW.Last.Elements.ToList
            Dim TempChunk As List(Of NucleotideModels.DNA)
            Dim p As Long = SW.Last.Left
            Dim NT_Array = NT.ToArray
            Dim List = New List(Of Double)

            For i As Integer = 0 To LastSegment.Count - 1 Step Steps
                TempChunk = LastSegment.Skip(i).ToList
                Call TempChunk.AddRange(NT_Array.Take(i).ToArray)
                Call List.Add(Sigma(NT_Cache, New NucleotideModels.NucleicAcid(TempChunk.ToArray)))
            Next

            Dim MergeList = (From item In ChunkBuffer Select item.d).ToList
            Call MergeList.AddRange(List)
            Me.ChunkBuffer = MergeList.ToArray
        End Sub

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return True
            End Get
        End Property

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
    End Class
End Namespace