Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic

Namespace Documents.Karyotype

    ''' <summary>
    ''' 必须要在SubNew之中设置参数，然后重写<see cref="TrackDataDocument.Generatedocument">文件数据生成</see>方法
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class TrackDataDocument(Of T As TrackData) : Inherits ITextFile

        Protected __meta As List(Of T)

        ''' <summary>
        ''' 这个文档所描绘的图形是否接受系统的自动位置的计算
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property AutoLayout As Boolean

        Protected MustOverride Function GenerateDocument() As String

        Public MustOverride ReadOnly Property Max As Double
        Public MustOverride ReadOnly Property Min As Double

        Public Shadows Property FilePath As String
            Get
                Return MyBase.FilePath
            End Get
            Set(value As String)
                MyBase.FilePath = value
            End Set
        End Property

        Public NotOverridable Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            Return Me.GenerateDocument.SaveTo(path:=getPath(FilePath), encoding:=Encoding)
        End Function

        Public Overridable Function LegendsDrawing(ref As Drawing.Point, ByRef Device As GDIPlusDeviceHandle) As Drawing.Point
            Return ref
        End Function
    End Class

    Public MustInherit Class GenomeDescription : Inherits TrackDataDocument(Of ValueTrackData)

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

        Protected MustOverride Overrides Function GenerateDocument() As String
    End Class
End Namespace