Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Scripting

Namespace Documents.Configurations.Nodes.Plots

    Public MustInherit Class Plot
        Implements ICircosDocument

        <SimpleConfig()> Public MustOverride ReadOnly Property Type As String

        <SimpleConfig()> Public Property File As String
            Get
                Return Tools.TrimPath(_karyotypeDocData.FileName)
            End Get
            Set(value As String)
                _karyotypeDocData.FilePath = value
            End Set
        End Property

        Public ReadOnly Property KaryotypeCanBeAutoLayout As Boolean
            Get
                Return _karyotypeDocData.AutoLayout
            End Get
        End Property

        ''' <summary>
        ''' 圈外径(单位 r，请使用格式"&lt;double>r")
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig()> Public Property r1 As String = "0.75r"

        ''' <summary>
        ''' 圈内径(单位 r，请使用格式"&lt;double>r")
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig()> Public Property r0 As String = "0.6r"
        <SimpleConfig()> Public Property max As String = "1"
        <SimpleConfig()> Public Property min As String = "0"
        <SimpleConfig()> Public Property fill_color As String = "orange"
        ''' <summary>
        ''' 圈的朝向，是<see cref="ORIENTATION_IN"/>向内还是<see cref="ORIENTATION_OUT"/>向外
        ''' </summary>
        ''' <returns></returns>
        <SimpleConfig()> Public Property orientation As String = "in"
        <SimpleConfig()> Public Property thickness As String = "2"
        <SimpleConfig()> Public Property stroke_thickness As String = "0"
        <SimpleConfig()> Public Property stroke_color As String = "grey"

        Public Const ORIENTATION_OUT As String = "out"
        Public Const ORIENTATION_IN As String = "in"

        Public Property Rules As List(Of ConditionalRule)

        Protected _karyotypeDocData As Documents.Karyotype.KaryotypeDocument

        Public ReadOnly Property KaryotypeDocumentData As Documents.Karyotype.KaryotypeDocument
            Get
                Return _karyotypeDocData
            End Get
        End Property

        Public Sub New(Data As Documents.Karyotype.KaryotypeDocument)
            Me._karyotypeDocData = Data
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("({0}  --> {1})  {2}", Me.Type, Me._karyotypeDocData.GetType.Name, Me._karyotypeDocData.ToString)
        End Function

        Protected Overridable Function GetMaxValue() As String
            Return _karyotypeDocData.Max.ToString
        End Function

        Protected Overridable Function GetMinValue() As String
            Return _karyotypeDocData.Min.ToString
        End Function

        Public Overridable Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocument.GenerateDocument
            Dim IndentBlanks As String = New String(" "c, IndentLevel)
            Dim sBuilder As StringBuilder = New StringBuilder(IndentBlanks & "<plot>" & vbCrLf, 1024)

            Call sBuilder.AppendLine()
            Call sBuilder.AppendLine(String.Format("{0}#   --> ""{1}""", IndentBlanks, _karyotypeDocData.GetType.FullName))
            Call sBuilder.AppendLine()

            Me.max = GetMaxValue()
            Me.min = GetMinValue()

            For Each strLine As String In GetProperties()
                Call sBuilder.AppendLine(IndentBlanks & "  " & strLine)
            Next

            Dim PlotElements = GeneratePlotsElementListChunk()

            If Not PlotElements.IsNullOrEmpty Then

                For Each item In PlotElements
                    Call sBuilder.AppendLine(vbCrLf & IndentBlanks & String.Format("<{0}>", item.Key))

                    For Each Element In item.Value
                        Call sBuilder.AppendLine(Element.GenerateDocument(IndentLevel + 2))
                    Next

                    Call sBuilder.AppendLine(IndentBlanks & String.Format("</{0}>", item.Key))
                Next
            End If

            Call sBuilder.AppendLine(IndentBlanks & "</plot>")

            Return sBuilder.ToString
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

        Public Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean Implements ICircosDocument.Save
            Return _karyotypeDocData.Save(FilePath, Encoding)
        End Function
    End Class

    Public Class HeatMap : Inherits Plot

        <SimpleConfig> Public Property color As String = "spectral-7-div"
        <SimpleConfig> Public Property scale_log_base As String = "0.25"

        Public Sub New(Data As Documents.Karyotype.KaryotypeDocument)
            Call MyBase.New(Data)
        End Sub

        <SimpleConfig()> Public Overrides ReadOnly Property Type As String
            Get
                Return "heatmap"
            End Get
        End Property

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of HeatMap)(Me)
        End Function
    End Class

    Public Class Histogram : Inherits Plot

        <SimpleConfig()> Public Overrides ReadOnly Property Type As String
            Get
                Return "histogram"
            End Get
        End Property

        Public Sub New(Data As Documents.Karyotype.KaryotypeDocument)
            Call MyBase.New(Data)
        End Sub

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of Histogram)(Me)
        End Function
    End Class

    Public Class TextLabel : Inherits Plot

        <SimpleConfig> Public Property color As String = "black"
        <SimpleConfig> Public Property label_size As String = "16"
        <SimpleConfig> Public Property label_font As String = "light"
        <SimpleConfig> Public Property padding As String = "5p"
        <SimpleConfig> Public Property rpadding As String = "5p"
        <SimpleConfig> Public Property show_links As String = yes
        <SimpleConfig> Public Property link_dims As String = "5p,4p,8p,4p,0p"
        <SimpleConfig> Public Property link_thickness As String = "1p"
        <SimpleConfig> Public Property link_color As String = "dgrey"
        <SimpleConfig> Public Property label_snuggle As String = yes
        <SimpleConfig> Public Property max_snuggle_distance As String = "2.0r"
        <SimpleConfig> Public Property snuggle_sampling As String = "1"
        <SimpleConfig> Public Property snuggle_tolerance As String = "0.25r"
        <SimpleConfig> Public Property snuggle_link_overlap_test As String = yes
        <SimpleConfig> Public Property snuggle_link_overlap_tolerance As String = "2p"
        <SimpleConfig> Public Property snuggle_refine As String = yes

        Public ReadOnly Property Labels As Karyotype.Highlights.HighlightLabel
            Get
                Return Me.KaryotypeDocumentData.As(Of Karyotype.Highlights.HighlightLabel)
            End Get
        End Property

        Sub New(Data As Documents.Karyotype.Highlights.HighlightLabel)
            Call MyBase.New(Data)
        End Sub

        <SimpleConfig()> Public Overrides ReadOnly Property Type As String
            Get
                Return "text"
            End Get
        End Property

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of TextLabel)(Me)
        End Function
    End Class
End Namespace
