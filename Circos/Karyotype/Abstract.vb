Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Configurations
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Linq

Namespace Karyotype

    ''' <summary>
    ''' The annotated genome skeleton information.
    ''' </summary>
    Public MustInherit Class SkeletonInfo
        Implements ICircosDocument

        ''' <summary>
        ''' 基因组的大小
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride ReadOnly Property Size As Integer
        ''' <summary>
        ''' 缺口的大小
        ''' </summary>
        ''' <returns></returns>
        Public Property LoopHole As Integer

        Protected __karyotypes As List(Of Karyotype)
        Protected __bands As List(Of Band)

        Public Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocNode.GenerateDocument
            Dim sb As New StringBuilder

            For Each x As IKaryotype In __karyotypes
                Call sb.AppendLine(x.GetData)
            Next
            For Each x As IKaryotype In __bands.SafeQuery
                Call sb.AppendLine(x.GetData)
            Next

            Return sb.ToString
        End Function

        Public Function Save(Optional Path As String = "", Optional encoding As Encoding = Nothing) As Boolean Implements ISaveHandle.Save
            Return GenerateDocument(Scan0).SaveTo(Path, encoding)
        End Function

        Public Function Save(Optional Path As String = "", Optional encoding As Encodings = Encodings.UTF8) As Boolean Implements ISaveHandle.Save
            Return Save(Path, encoding.GetEncodings)
        End Function
    End Class
End Namespace