Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Documents.Configurations.Nodes

    Public Class Spacing : Inherits CircosDocument
        Implements ICircosDocNode

        <SimpleConfig> Public Property [default] As String = "1u"
        <SimpleConfig> Public Property break As String = "0u"

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("spacing", IndentLevel, Nothing)
        End Function
    End Class

    Public Class Ideogram : Inherits CircosDocument
        Implements ICircosDocNode

        ''' <summary>
        ''' # thickness (px) of chromosome ideogram
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property thickness As String = "30p"
        <SimpleConfig> Public Property stroke_thickness As String = "0"

        ''' <summary>
        ''' # ideogram border color
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property stroke_color As String = "black"
        <SimpleConfig> Public Property fill As String = yes

        ''' <summary>
        ''' # the default chromosome color is set here and any value
        ''' # defined in the karyotype file overrides it
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property fill_color As String = "black"

        ''' <summary>
        ''' # fractional radius position of chromosome ideogram within image
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property radius As String = "0.85r"
        <SimpleConfig> Public Property show_label As String = no
        <SimpleConfig> Public Property label_font As String = "default"
        <SimpleConfig> Public Property label_radius As String = "dims(ideogram,radius) + 0.05r"
        <SimpleConfig> Public Property label_size As String = "36"
        <SimpleConfig> Public Property label_parallel As String = yes
        <SimpleConfig> Public Property label_case As String = "upper"

        ''' <summary>
        ''' # cytogenetic bands
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property band_stroke_thickness As String = "0"

        ''' <summary>
        ''' # show_bands determines whether the outline of cytogenetic bands
        ''' # will be seen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property show_bands As String = yes

        ''' <summary>
        ''' # in order to fill the bands with the color defined in the karyotype
        ''' # file you must set fill_bands
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SimpleConfig> Public Property fill_bands As String = yes

        Public Property Spacing As Spacing = New Spacing

        Public Overrides Function GenerateDocument(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("ideogram", IndentLevel + 2, {Spacing})
        End Function
    End Class
End Namespace