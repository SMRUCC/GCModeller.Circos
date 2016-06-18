Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations

    ''' <summary>
    ''' The circos distributed includes files.
    ''' (这个对象仅仅是为了引用Cricos系统内的预置的配置文件的设立的，故而<see cref="CircosDistributed.GenerateDocument">
    ''' </see>方法和<see cref="CircosDistributed.Save"></see>方法可以不会被实现)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CircosDistributed : Inherits CircosConfig

        Public ReadOnly Property Section As String

        ''' <summary>
        ''' 由于这些是系统的预置的数据，是不能够再修改了的，所以这里由于没有数据配置项，直接忽略掉了Circos配置数据
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Protected Sub New(path As String)
            Call MyBase.New(path, Circos:=Nothing)
        End Sub

        Protected Sub New(path As String, name As String)
            Me.New(path)
            Me.Section = name
        End Sub

        Protected Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return ""
        End Function

        ''' <summary>
        ''' Debugging, I/O an dother system parameters included from Circos distribution.
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property HouseKeeping As CircosDistributed =
            New CircosDistributed("etc/housekeeping.conf")

        Public Shared ReadOnly Property ColorBrain As CircosDistributed =
            New CircosDistributed("color.brain.conf", "colors")
        Public Shared ReadOnly Property Image As CircosDistributed =
            New CircosDistributed("etc/image.conf", "image")

        ''' <summary>
        ''' RGB/HSV color definitions, color lists, location of fonts, fill
        ''' patterns. Included from Circos distribution.
        '''
        ''' In older versions Of Circos, colors, fonts And patterns were
        ''' included individually. Now, this Is done from a central file. Make
        ''' sure that you're not importing these values twice by having
        '''
        ''' ```
        ''' *** Do Not Do THIS ***
        ''' &lt;colors>
        '''     &lt;&lt;include etc/colors.conf>>
        ''' &lt;colors>
        ''' **********************
        ''' ```
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ColorFontsPatterns As CircosDistributed =
            New CircosDistributed("etc/colors_fonts_patterns.conf")

        Public Overrides ReadOnly Property IsSystemConfig As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace
