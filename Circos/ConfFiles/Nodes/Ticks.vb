Imports Microsoft.VisualBasic.ComponentModel.Settings
Imports Microsoft.VisualBasic
Imports System.Text

Namespace Documents.Configurations.Nodes

    Public Class Ticks : Inherits CircosDocument
        Implements ICircosDocNode

        <SimpleConfig> Public Property skip_first_label As String = no
        <SimpleConfig> Public Property skip_last_label As String = no
        <SimpleConfig> Public Property radius As String = "dims(ideogram,radius_outer)"
        <SimpleConfig> Public Property tick_separation As String = "2p"
        <SimpleConfig> Public Property min_label_distance_to_edge As String = "0p"
        <SimpleConfig> Public Property label_separation As String = "5p"
        <SimpleConfig> Public Property label_offset As String = "5p"
        <SimpleConfig> Public Property label_size As String = "36p"
        <SimpleConfig> Public Property color As String = "black"
        <SimpleConfig> Public Property multiplier As String = "0.001"
        <SimpleConfig> Public Property thickness As String = "3p"
        <SimpleConfig> Public Property size As String = "20p"

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

        <SimpleConfig> Public Property spacing As String = "500u"
        <SimpleConfig> Public Property color As String = "black"
        <SimpleConfig> Public Property show_label As String = yes
        <SimpleConfig> Public Property suffix As String = """ kb"""
        <SimpleConfig> Public Property label_size As String = "36p"
        <SimpleConfig> Public Property format As String = "%s"
        <SimpleConfig> Public Property grid As String = yes
        <SimpleConfig> Public Property grid_color As String = "black"
        <SimpleConfig> Public Property grid_thickness As String = "4p"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("tick", IndentLevel, Nothing)
        End Function
    End Class
End Namespace