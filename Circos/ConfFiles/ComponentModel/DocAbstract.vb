Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic

Namespace Documents.Configurations

    ''' <summary>
    ''' Abstract of the circos config files.
    ''' </summary>
    Public MustInherit Class CircosConfig : Inherits ITextFile
        Implements ICircosDocument

        ''' <summary>
        ''' 文档的对其他的配置文件的引用列表
        ''' </summary>
        ''' <returns></returns>
        Public Property Includes As List(Of CircosConfig)

        ''' <summary>
        ''' 主配置文件Circos.conf
        ''' </summary>
        ''' <returns></returns>
        Public Property main As Circos

        ''' <summary>
        ''' 是否为系统自带的绘图文件，这个属性是和生成引用路径相关的
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride ReadOnly Property IsSystemConfig As Boolean

        Sub New(FileName As String, Circos As Circos)
            MyBase.FilePath = FileName
            Me.main = Circos
        End Sub

        Protected Function GenerateIncludes() As String
            If Includes.IsNullOrEmpty Then
                Return ""
            End If

            Dim sb As New StringBuilder(1024)

            For Each includeFile As CircosConfig In Includes
                Dim refPath As String = Tools.TrimPath(includeFile)
                Call sb.AppendLine($"<<include {refPath}>>")
                Call includeFile.Save(Encoding:=Encoding.ASCII)
            Next

            Return sb.ToString
        End Function

        ''' <summary>
        ''' 配置文件的引用的相对路径
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property RefPath As String
            Get
                Return Tools.TrimPath(Me)
            End Get
        End Property

        Public Const TicksConf As String = "ticks.conf"
        Public Const IdeogramConf As String = "ideogram.conf"

        Protected Friend MustOverride Function GenerateDocument(IndentLevel As Integer) As String Implements ICircosDocument.GenerateDocument
        Public MustOverride Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean Implements ICircosDocument.Save
    End Class
End Namespace