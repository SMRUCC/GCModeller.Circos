Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Serialization

Namespace Documents.Configurations.Nodes

    ''' <summary>
    ''' Use ``<see cref="Circos.chromosomes_color"/>`` to change
    ''' the color Of the ideograms. This approach works well When the only
    ''' thing you want To Do Is change the color Of the segments. 
    '''
    ''' Another way To achieve this Is To actually redefine the colors which
    ''' are used To color the ideograms. The benefit Of doing this Is that
    ''' whenever you refer To the color (which you can use by Using the name
    ''' Of the chromosome), you Get the custom value.
    '''
    ''' If you Then look In the human karyotype file linked To above, you'll see
    ''' that Each chromosome's color is ``chrN`` where N is the number of the
    ''' chromosome. Thus, hs1 has color chr1, hs2 has color chr2, And so
    ''' On. For convenience, a color can be referenced Using 'chr' and 'hs'
    ''' prefixes (chr1 And hs1 are the same color).
    '''
    ''' Colors are redefined by overwriting color definitions, which are
    ''' found In the ``&lt;colors>`` block. This block Is included below from the
    ''' colors_fonts_patterns.conf file, which contains all the Default
    ''' definitions. To overwrite colors, use a "*" suffix And provide a New
    ''' value, which can be a lookup To another color.
    ''' </summary>
    Public Class OverwritesColors

        Public Property colors As Dictionary(Of NamedValue(Of String))

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Function GetConfigsValue() As String
            Dim sb As New StringBuilder

            Call sb.AppendLine("<colors>")

            For Each x As NamedValue(Of String) In colors.Values
                Call sb.AppendLine($"  {x.Name} = {x.x}")
            Next

            Call sb.AppendLine("</colors>")

            Return sb.ToString
        End Function
    End Class
End Namespace