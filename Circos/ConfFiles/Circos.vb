Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic
Imports System.Text
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Configurations.Nodes.Plots
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights
Imports Microsoft.VisualBasic.Language

Namespace Documents.Configurations

    Public Class CircosAttribute : Inherits SimpleConfig
    End Class

    ''' <summary>
    ''' circos.conf
    '''                                     ____ _
    '''                                    / ___(_)_ __ ___ ___  ___
    '''                                   | |   | | '__/ __/ _ \/ __|
    '''                                   | |___| | | | (_| (_) \__ \
    '''                                   \____|_|_|  \___\___/|___/
    '''
    '''                                                round Is good
    '''
    ''' circos - generate circularly composited information graphics
    ''' 
    ''' (Circo基因组绘图程序的主配置文件)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Circos : Inherits ConfigDoc
        Implements ICircosDocument

        ''' <summary>
        ''' The basically genome structure plots.(基本的数据文件)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Circos> Public Property karyotype As String = "data/genes.txt"
        <Circos> Public Property genome As String = null
        <Circos> Public Property use_rules As String = yes

        <Circos> Public Property chromosomes_units As String = "5000"
        <Circos> Public Property chromosomes_display_default As String = yes
        <Circos> Public Property chromosomes As String = null
        <Circos> Public Property chromosomes_reverse As String = null
        <Circos> Public Property chromosomes_radius As String = null
        <Circos> Public Property chromosomes_scale As String = null
        <Circos> Public Property chromosomes_color As String = null
        <Circos> Public Property chromosomes_order As String = null
        <Circos> Public Property chromosomes_breaks As String = null

        <Circos> Public Property show_scatter As String = yes
        <Circos> Public Property show_line As String = yes
        <Circos> Public Property show_histogram As String = yes
        <Circos> Public Property show_heatmap As String = yes
        <Circos> Public Property show_tile As String = yes
        <Circos> Public Property show_highlight As String = yes
        <Circos> Public Property show_links As String = yes
        <Circos> Public Property show_highlights As String = yes
        <Circos> Public Property show_text As String = yes
        <Circos> Public Property show_heatmaps As String = yes

        <Circos> Public Property track_width As String = null
        <Circos> Public Property track_start As String = null
        <Circos> Public Property track_step As String = null

        ''' <summary>
        ''' 基因组的骨架信息
        ''' </summary>
        ''' <returns></returns>
        Public Property BasicKaryotypeData As Documents.Karyotype.GenomeDescription

        ''' <summary>
        ''' The genome size.(基因组的大小，当<see cref="BasicKaryotypeData"/>为空值的时候返回数值0)
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Size As Integer
            Get
                If BasicKaryotypeData Is Nothing Then
                    Return 0
                End If
                Return _BasicKaryotypeData.Size - BasicKaryotypeData.LoopHole
            End Get
        End Property

        ''' <summary>
        ''' 内部元素是有顺序的区别的
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Plots As Plot()
            Get
                Return _plots.ToArray
            End Get
        End Property

        Dim _plots As New List(Of Plot)

        Public Overrides ReadOnly Property IsSystemConfig As Boolean
            Get
                Return False
            End Get
        End Property

        Sub New()
            Call MyBase.New("circos.conf", Nothing)
            Me.main = Me
        End Sub

        Public Overrides Function Save(Optional outDIR As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            If String.IsNullOrEmpty(outDIR) Then
                outDIR = FileIO.FileSystem.GetParentPath(Me.FilePath)
            End If

            Dim dataDIR As String = $"{outDIR}/data/"
            Call FilePath.InvokeSet($"{outDIR}/{FileIO.FileSystem.GetFileInfo(FilePath).Name}")
            Call FileIO.FileSystem.CreateDirectory(dataDIR)

            App.CurrentDirectory = outDIR

            For i As Integer = 0 To _plots.Count - 1
                Dim x As Plot = _plots(i)
                Dim FileName As String = $"data/{x.Type}_data_{i}.txt"
                Call x.Save(FileName, System.Text.Encoding.ASCII)
            Next

            Call _BasicKaryotypeData.Save(karyotype, Encoding:=System.Text.Encoding.ASCII)

            App.CurrentDirectory = outDIR

            Return GenerateDocument(0).SaveTo(FilePath, System.Text.Encoding.ASCII)
        End Function

        Public Overloads Shared Function CreateObject() As Circos
            Dim CircosConfig As Circos = New Circos With {
                .IncludeList = New List(Of ConfigDoc)
            }

            Call CircosConfig.IncludeList.Add(SystemPrefixConfigDoc.ColorFontsPatterns)
            Call CircosConfig.IncludeList.Add(SystemPrefixConfigDoc.HouseKeeping)

            Return CircosConfig
        End Function

#Region "默认的图形属性"

        Dim stroke_thickness As String = "0"
        Dim stroke_color As String = ""
#End Region

        ''' <summary>
        ''' 函数会根据元素的个数的情况自动的调整在圈内的位置
        ''' </summary>
        ''' <param name="plotElement"></param>
        ''' <remarks></remarks>
        Public Sub AddPlotElement(plotElement As Plot)
            Call Me._plots.Add(plotElement)

            If Not String.IsNullOrEmpty(stroke_thickness) Then
                plotElement.stroke_thickness = stroke_thickness
            End If
            If Not String.IsNullOrEmpty(stroke_color) Then
                plotElement.stroke_color = stroke_color
            End If

            Dim plotElements As Plot() =
                LinqAPI.Exec(Of Plot) <= From plotUnit As Plot
                                         In Me._plots
                                         Where plotUnit.KaryotypeCanBeAutoLayout
                                         Select plotUnit

            If plotElements.Length > 1 Then
                Call ForceAutoLayout(plotElements)
            End If
        End Sub

        ''' <summary>
        ''' 强制所有的元素都自动布局
        ''' </summary>
        Public Sub ForceAutoLayout()
            Call ForceAutoLayout(Me.Plots)
        End Sub

        ''' <summary>
        ''' 强制所指定的绘图元素自动布局
        ''' </summary>
        ''' <param name="elements"></param>
        Public Sub ForceAutoLayout(elements As Plot())
            Dim d = 0.8 / elements.Length / 2
            Dim p As Double = 0.95

            For Each item In elements
                item.r1 = p & "r"
                p -= d
                item.r0 = p & "r"
                p -= d / 5
            Next
        End Sub

        ''' <summary>
        ''' 不可以使用并行拓展，因为有顺序之分
        ''' 
        ''' {SpeciesName, Color}
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBlastAlignmentData() As KeyValuePair(Of String, String)()
            Dim LQuery = (From item In Me._plots
                          Where String.Equals(item.Type, "highlight", StringComparison.OrdinalIgnoreCase) AndAlso
                              TypeOf item.KaryotypeDocumentData Is BlastMaps
                          Let Alignment = DirectCast(item.KaryotypeDocumentData, BlastMaps)
                          Select New KeyValuePair(Of String, String)(Alignment.SubjectSpecies, Alignment.SpeciesColor)).ToArray
            Return LQuery
        End Function

        Protected Friend Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)
            Call sBuilder.AppendLine(Me.GenerateIncludes)
            Call sBuilder.AppendLine("<image>" & vbCrLf &
                                     "  <<include etc/image.conf>>" & vbCrLf &
                                     "</image>" & vbCrLf)

            For Each Line As String In SimpleConfig.GenerateConfigurations(Of Circos)(Me)
                Call sBuilder.AppendLine(Line)
            Next

            If Not _plots.IsNullOrEmpty Then
                Call sBuilder.AppendLine(vbCrLf & "<plots>")

                For Each plotRule In _plots
                    Call sBuilder.AppendLine()
                    Call sBuilder.AppendLine(plotRule.GenerateDocument(IndentLevel + 2))
                Next

                Call sBuilder.AppendLine()
                Call sBuilder.AppendLine("</plots>")
            End If

            Return sBuilder.ToString
        End Function
    End Class
End Namespace