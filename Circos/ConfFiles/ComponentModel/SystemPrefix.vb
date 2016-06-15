Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations

    ''' <summary>
    ''' 这个对象仅仅是为了引用Cricos系统内的预置的配置文件的设立的，故而<see cref="SystemPrefixConfigDoc.GenerateDocument"></see>方法和<see cref="SystemPrefixConfigDoc.Save"></see>方法可以不会被实现
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SystemPrefixConfigDoc : Inherits ConfigDoc

        ''' <summary>
        ''' 由于这些是系统的预置的数据，是不能够再修改了的，所以这里由于没有数据配置项，直接忽略掉了Circos配置数据
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <remarks></remarks>
        Protected Sub New(Path As String)
            Call MyBase.New(Path, Circos:=Nothing)
        End Sub

        Public Shared ReadOnly Property HouseKeeping As SystemPrefixConfigDoc =
            New SystemPrefixConfigDoc("etc/housekeeping.conf")
        Public Shared ReadOnly Property ColorFontsPatterns As SystemPrefixConfigDoc =
            New SystemPrefixConfigDoc("etc/colors_fonts_patterns.conf")

        Public Overrides ReadOnly Property IsSystemConfig As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Friend Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return ""
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            Return True
        End Function
    End Class
End Namespace
