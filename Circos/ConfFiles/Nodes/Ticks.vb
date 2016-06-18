Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic
Imports System.Text

Namespace Configurations.Nodes

    Public Class Ticks : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property skip_first_label As String = no
        <Circos> Public Property skip_last_label As String = no
        <Circos> Public Property radius As String = "dims(ideogram,radius_outer)"
        <Circos> Public Property tick_separation As String = "2p"
        <Circos> Public Property min_label_distance_to_edge As String = "0p"
        <Circos> Public Property label_separation As String = "5p"
        <Circos> Public Property label_offset As String = "5p"
        <Circos> Public Property label_size As String = "36p"
        <Circos> Public Property color As String = "black"
        ''' <summary>
        ''' the tick label is derived by multiplying the tick position
        ''' by ``<see cref="multiplier"/>`` and casting it in ``<see cref="format"/>``:
        '''
        ''' ```
        ''' sprintf(format,position*multiplier)
        ''' ```
        ''' </summary>
        ''' <returns></returns>
        <Circos> Public Property multiplier As String = "0.001"
        <Circos> Public Property thickness As String = "3p"
        <Circos> Public Property size As String = "20p"

        Public Property Ticks As List(Of Tick) = New List(Of Tick)

        Public Shared Function DefaultConfiguration() As Ticks
            Dim Ticks As New List(Of Tick)
            Call Ticks.Add(New Tick With {.spacing = "1u", .show_label = no, .grid_thickness = "1p"})
            'Call Ticks.Add(New Tick With {.spacing = "0.5u", .show_label = YES, .label_size = "20p", .format = "%d"})
            Call Ticks.Add(New Tick With {.spacing = "5u", .show_label = yes, .label_size = "28p", .format = "%d"})

            Return New Ticks With {.Ticks = Ticks}
        End Function

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("ticks", IndentLevel, InsertElements:=Ticks)
        End Function
    End Class

    ''' <summary>
    ''' Rule unit and displaying
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Tick : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property size As String
        <Circos> Public Property spacing As String = "500u"
        <Circos> Public Property color As String = "black"
        <Circos> Public Property show_label As String = yes
        <Circos> Public Property suffix As String = """ kb"""
        <Circos> Public Property label_size As String = "36p"
        ''' <summary>
        ''' Example as: ``label_offset = 10p``
        ''' </summary>
        ''' <returns></returns>
        Public Property label_offset As String
        ''' <summary>
        ''' |format control|types                  |
        ''' |--------------|-----------------------|
        ''' |%d            |integer                |
        ''' |%f            |float                  |
        ''' |%.1f          |float With one Decimal |
        ''' |%.2f          |float With two decimals|
        '''
        ''' For other formats, see http://perldoc.perl.org/functions/sprintf.html
        ''' </summary>
        ''' <returns></returns>
        <Circos> Public Property format As String = "%s"
        <Circos> Public Property grid As String = yes
        <Circos> Public Property grid_color As String = "black"
        <Circos> Public Property grid_thickness As String = "4p"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("tick", IndentLevel, Nothing)
        End Function
    End Class
End Namespace