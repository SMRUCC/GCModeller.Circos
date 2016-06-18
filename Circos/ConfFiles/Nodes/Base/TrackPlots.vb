Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Scripting
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas
Imports Microsoft.VisualBasic.ComponentModel

Namespace Documents.Configurations.Nodes.Plots

    Public Interface ITrackPlot
        <Circos> ReadOnly Property type As String

        ''' <summary>
        ''' 输入的路径会根据配置情况转换为相对路径或者绝对路径
        ''' </summary>
        ''' <returns></returns>
        <Circos> Property file As String
        Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
    End Interface

    Public MustInherit Class TracksPlot(Of T As ITrackData)
        Implements ICircosDocument
        Implements ITrackPlot

        <Circos> Public MustOverride ReadOnly Property type As String Implements ITrackPlot.type

        ''' <summary>
        ''' 输入的路径会根据配置情况转换为相对路径或者绝对路径
        ''' </summary>
        ''' <returns></returns>
        <Circos> Public Property file As String Implements ITrackPlot.file
            Get
                Return Tools.TrimPath(TracksData.FileName)
            End Get
            Set(value As String)
                TracksData.FileName = value
            End Set
        End Property

        ''' <summary>
        ''' 圈外径(单位 r，请使用格式"&lt;double>r")
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Circos> Public Property r1 As String = "0.75r"

        ''' <summary>
        ''' 圈内径(单位 r，请使用格式"&lt;double>r")
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' The track is confined within r0/r1 radius limits. When using the
        ''' relative "r" suffix, the values are relative To the position Of the
        ''' ideogram.
        ''' </remarks>
        <Circos> Public Property r0 As String = "0.6r"
        <Circos> Public Property max As String = "1"
        <Circos> Public Property min As String = "0"
        <Circos> Public Overridable Property fill_color As String = "orange"
        ''' <summary>
        ''' 圈的朝向，是<see cref="ORIENTATION_IN"/>向内还是<see cref="ORIENTATION_OUT"/>向外
        ''' </summary>
        ''' <returns></returns>
        <Circos> Public Property orientation As String = "in"
        ''' <summary>
        ''' To turn off default outline, set the outline thickness to zero. 
        ''' If you want To permanently disable this Default, edit
        ''' ``etc/tracks/histogram.conf`` In the Circos distribution.
        ''' </summary>
        ''' <returns></returns>
        <Circos> Public Property thickness As String = "2p"
        <Circos> Public Property stroke_thickness As String = "0"
        <Circos> Public Property stroke_color As String = "grey"

        Public Const ORIENTATION_OUT As String = "out"
        Public Const ORIENTATION_IN As String = "in"

        Public Property Rules As List(Of ConditionalRule)

        ''' <summary>
        ''' data文件夹之中的绘图数据
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TracksData As data(Of T)

        Public Sub New(data As IEnumerable(Of T))
            Me.TracksData = New data(Of T)(data)
        End Sub

        Public Overrides Function ToString() As String
            Return $"({type}  --> {Me.TracksData.GetType.Name})  {Me.TracksData.ToString}"
        End Function

        Protected Overridable Function GetMaxValue() As String
            Return _karyotypeDocData.Max.ToString
        End Function

        Protected Overridable Function GetMinValue() As String
            Return _karyotypeDocData.Min.ToString
        End Function

        Public Overridable Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocument.GenerateDocument
            Dim IndentBlanks As String = New String(" "c, IndentLevel)
            Dim sb As StringBuilder = New StringBuilder(IndentBlanks & "<plot>" & vbCrLf, 1024)

            Call sb.AppendLine()
            Call sb.AppendLine(String.Format("{0}#   --> ""{1}""", IndentBlanks, TracksData.GetType.FullName))
            Call sb.AppendLine()

            Me.max = GetMaxValue()
            Me.min = GetMinValue()

            For Each strLine As String In GetProperties()
                Call sb.AppendLine(IndentBlanks & "  " & strLine)
            Next

            Dim PlotElements = GeneratePlotsElementListChunk()

            If Not PlotElements.IsNullOrEmpty Then

                For Each item In PlotElements
                    Call sb.AppendLine(vbCrLf & IndentBlanks & String.Format("<{0}>", item.Key))

                    For Each o As CircosDocument In item.Value
                        Call sb.AppendLine(o.GenerateDocument(IndentLevel + 2))
                    Next

                    Call sb.AppendLine(IndentBlanks & String.Format("</{0}>", item.Key))
                Next
            End If

            Call sb.AppendLine(IndentBlanks & "</plot>")

            Return sb.ToString
        End Function

        ''' <summary>
        ''' SimpleConfig.GenerateConfigurations(Of &lt;PlotType>)(Me)，需要手工复写以得到正确的类型
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected MustOverride Function GetProperties() As String()

        Protected Overridable Function GeneratePlotsElementListChunk() As Dictionary(Of String, List(Of CircosDocument))
            If Not Rules.IsNullOrEmpty Then
                Return New Dictionary(Of String, List(Of CircosDocument)) From {{"rules", (From item In Rules Select DirectCast(item, CircosDocument)).ToList}}
            Else
                Return Nothing
            End If
        End Function

        Public Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean Implements ICircosDocument.Save, ITrackPlot.Save
            Return TracksData.GetDocumentText.SaveTo(FilePath, Encoding)
        End Function

        Public Function Save(Optional Path As String = "", Optional encoding As Encodings = Encodings.UTF8) As Boolean Implements ISaveHandle.Save
            Return Save(Path, encoding.GetEncodings)
        End Function
    End Class
End Namespace
