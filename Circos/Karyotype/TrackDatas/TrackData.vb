Imports System.Text
Imports Microsoft.VisualBasic.Linq

Namespace Documents.Karyotype.TrackDatas

    ''' <summary>
    ''' + Finally, links are a special track type which associates two ranges together. The format For links Is analogous To other data types, 
    ''' except now two coordinates are specified.
    ''' 
    ''' ```
    ''' chr12 1000 5000 chr15 5000 7000
    ''' ```
    ''' </summary>
    Public Structure link
        Dim a As TrackData
        Dim b As TrackData

        Public Overrides Function ToString() As String
            Return a.ToString & " " & b.ToString
        End Function
    End Structure

    ''' <summary>
    ''' Data for tracks is loaded from a plain-text file. Each data point is stored on a 
    ''' separate line, except for links which use two lines per link.
    ''' 
    ''' The definition Of a data point within a track Is based On the genomic range, 
    ''' which Is a combination Of chromosome And start/End position.
    ''' </summary>
    ''' <remarks>
    ''' Data for tracks is loaded from a plain-text file. Each data point is stored on a separate line, except for links which use two lines per link.
    ''' The definition Of a data point within a track Is based On the genomic range, which Is a combination Of chromosome And start/End position. 
    ''' For example,
    ''' 
    ''' ```
    ''' # the basis for a data point Is a range
    ''' chr12 1000 5000
    ''' ```
    ''' 
    ''' All data values, regardless Of track type, will be positioned Using a range rather than a Single position. To explicitly specify a Single position, 
    ''' use a range With equal start And End positions.
    ''' 
    ''' + Tracks such As scatter plot, line plot, histogram Or heat map, associate a value With Each range. The input To this kind Of track would be
    ''' 
    ''' ```
    ''' # scatter, line, histogram And heat maps require a value
    ''' chr12 1000 5000 0.25
    ''' ```
    ''' 
    ''' + The exception Is a stacked histogram, which associates a list Of values With a range.
    ''' 
    ''' ```
    ''' # stacked histograms take a list of values
    ''' chr12 1000 5000 0.25,0.35,0.60
    ''' ```
    ''' 
    ''' + The value For a text track Is interpreted As a text label (other tracks require that this field be a floating point number).
    ''' 
    ''' ```
    ''' # value for text tracks Is interpreted as text
    ''' chr12 1000 5000 geneA
    ''' ```
    ''' 
    ''' + The tile track does Not take a value—only a range.
    ''' 
    ''' ```
    ''' chr12 1000 5000
    ''' ```
    ''' 
    ''' + Finally, links are a special track type which associates two ranges together. The format For links Is analogous To other data types, 
    ''' except now two coordinates are specified.
    ''' 
    ''' ```
    ''' chr12 1000 5000 chr15 5000 7000
    ''' ```
    ''' 
    ''' + In addition to the chromosome, range And (if applicable) value, each data point can be annotated with formatting parameters that control how the point Is drawn. 
    ''' The parameters need to be compatible with the track type for which the file Is destined. Thus, a scatter plot data point might have
    ''' 
    ''' ```
    ''' chr12 1000 5000 0.25 glyph_size=10p,glyph=circle
    ''' ```
    ''' 
    ''' whereas a histogram data point might include the Option To fill the data value's bin
    ''' 
    ''' ```
    ''' chr12 1000 5000 0.25 fill_color=orange
    ''' ```
    ''' 
    ''' + Other features, such As URLs, can be associated With any data point. For URLs the parameter can contain parsable fields (e.g. [start]) which 
    ''' are populated automatically With the data point's associated property.
    ''' 
    ''' ```
    ''' # the URL for this point would be
    ''' # http://domain.com/script?start=1000&amp;end=5000&amp;chr=chr12
    ''' chr12 1000 5000 0.25 url=http//domain.com/script?start=[start]&amp;end=[end]&amp;chr=[chr]
    ''' ```
    ''' </remarks>
    Public MustInherit Class TrackData

        ''' <summary>
        ''' Chromosomes name
        ''' </summary>
        Public Property chr As String
        Public Property start As Integer
        Public Property [end] As Integer
        Public Property formatting As Formatting
        Public Property comment As String

        ''' <summary>
        ''' Using <see cref="ToString()"/> method for creates tracks data document.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Dim s As String = __trackData()
            Dim format As String = formatting.ToString

            If Not String.IsNullOrEmpty(format) Then
                s &= " " & format
            End If

            Return s
        End Function

        Protected MustOverride Function __trackData() As String

    End Class

    ''' <summary>
    ''' Annotated with formatting parameters that control how the point Is drawn. 
    ''' </summary>
    Public Structure Formatting

        ''' <summary>
        ''' Only works in scatter, example is ``10p``
        ''' </summary>
        Dim glyph_size As String
        ''' <summary>
        ''' Only works in scatter, example is ``circle``
        ''' </summary>
        Dim glyph As String
        ''' <summary>
        ''' Works on histogram
        ''' </summary>
        Dim fill_color As String
        ''' <summary>
        ''' Works on any <see cref="Trackdata"/> data type.
        ''' </summary>
        Dim URL As String

        Public Overrides Function ToString() As String
            Dim s As New StringBuilder

            Call __attach(s, NameOf(glyph), glyph)
            Call __attach(s, NameOf(glyph_size), glyph_size)
            Call __attach(s, NameOf(fill_color), fill_color)
            Call __attach(s, "url", URL)

            Return s.ToString
        End Function

        Private Shared Sub __attach(ByRef s As StringBuilder, name As String, value As String)
            If s.Length = 0 Then
                Call s.Append($"{name}={value}")
            Else
                Call s.Append($",{name}={value}")
            End If
        End Sub
    End Structure

    ''' <summary>
    ''' Tracks such As scatter plot, line plot, histogram Or heat map, associate a value With Each range. The input To this kind Of track would be
    ''' 
    ''' ```
    ''' # scatter, line, histogram And heat maps require a value
    ''' chr12 1000 5000 0.25
    ''' ```
    ''' </summary>
    ''' <remarks>
    ''' In addition to the chromosome, range And (if applicable) value, each data point can be annotated with formatting parameters that control how the point Is drawn. 
    ''' The parameters need to be compatible with the track type for which the file Is destined. Thus, a scatter plot data point might have
    ''' 
    ''' ```
    ''' chr12 1000 5000 0.25 glyph_size=10p,glyph=circle
    ''' ```
    ''' </remarks>
    Public Class ValueTrackData : Inherits TrackData

        Public Property value As Double

        Protected Overrides Function __trackData() As String
            Return $"{chr} {start} {[end]} {value}"
        End Function

        Public Shared Function FromColorMapping(cl As Circos.Colors.Mappings, idx As Integer, offset As Integer) As ValueTrackData
            Return New ValueTrackData With {
                .formatting = New Formatting With {
                    .fill_color = $"({cl.Color.R},{cl.Color.G},{cl.Color.B})"
                },
                .start = idx,
                .end = idx + 1 + offset,
                .value = cl.value
            }
        End Function

        Public Shared Function Distinct(source As IEnumerable(Of ValueTrackData)) As ValueTrackData()
            Dim LQuery = (From x As ValueTrackData
                          In source
                          Select x,
                              uid = $"{x.start}..{x.end}"
                          Group By uid Into Group) _
                             .ToArray(Function(x) x.Group.First.x)
            Return LQuery
        End Function
    End Class

    ''' <summary>
    ''' The exception Is a stacked histogram, which associates a list Of values With a range.
    ''' 
    ''' ```
    ''' # stacked histograms take a list of values
    ''' chr12 1000 5000 0.25,0.35,0.60
    ''' ```
    ''' </summary>
    Public Class StackedTrackData : Inherits TrackData

        Public Property values As Double()

        Protected Overrides Function __trackData() As String
            Dim values As String = Me.values.Select(Function(d) d.ToString).JoinBy(",")
            Return $"{chr} {start} {[end]} {values}"
        End Function
    End Class

    ''' <summary>
    ''' The value For a text track Is interpreted As a text label (other tracks require that this field be a floating point number).
    ''' 
    ''' ```
    ''' # value for text tracks Is interpreted as text
    ''' chr12 1000 5000 geneA
    ''' ```
    ''' </summary>
    Public Class TextTrackData : Inherits TrackData

        Public Property text As String

        Protected Overrides Function __trackData() As String
            Return $"{chr} {start} {[end]} {text}"
        End Function
    End Class
End Namespace