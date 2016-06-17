Imports System.Text

Namespace Documents.Karyotype.Data

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

        ''' <summary>
        ''' Using <see cref="ToString()"/> method for creates tracks data document.
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Overrides Function ToString() As String

    End Class

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
        Public Property formatting As Formatting

        Public Overrides Function ToString() As String
            Dim format As String = formatting.ToString
            Dim s As String = $"{chr} {start} {[end]} {value}"

            If Not String.IsNullOrEmpty(format) Then
                s &= " " & format
            End If

            Return s
        End Function
    End Class

    ''' <summary>
    ''' Annotated with formatting parameters that control how the point Is drawn. 
    ''' </summary>
    Public Structure Formatting

        ''' <summary>
        ''' example is ``10p``
        ''' </summary>
        Dim glyph_size As String
        ''' <summary>
        ''' example is ``circle``
        ''' </summary>
        Dim glyph As String
        Dim fill_color As String
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
    ''' The exception Is a stacked histogram, which associates a list Of values With a range.
    ''' 
    ''' ```
    ''' # stacked histograms take a list of values
    ''' chr12 1000 5000 0.25,0.35,0.60
    ''' ```
    ''' </summary>
    Public Class StackedTrackData : Inherits TrackData

        Public Property values As Double()

        Public Overrides Function ToString() As String
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

        Public Overrides Function ToString() As String
            Return $"{chr} {start} {[end]} {text}"
        End Function
    End Class
End Namespace