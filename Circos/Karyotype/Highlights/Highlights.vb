Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.TrackDatas

Namespace Documents.Karyotype.Highlights

    Public MustInherit Class Highlights : Inherits data(Of ValueTrackData)

        Public ReadOnly Property Highlights As HighLightsMeta()
            Get
                Return _highLights
            End Get
        End Property

        Sub New(source As IEnumerable(Of HighLightsMeta))
            _highLights = source.ToArray
        End Sub

        Protected Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)

            For Each Line As HighLightsMeta In _highLights
                Call sBuilder.AppendLine(String.Format("chr1 {0} {1} fill_color={2}", Line.Left, Line.Right, Line.Color))
            Next

            Return sBuilder.ToString
        End Function

        Const COG_NULL_EXCEPTION As String = "This error usually caused by the null COG data in the gene annotations. " &
            "Please check of the COG data in your genome annotation data make sure not all of the gene have no COG value" &
            "(at least should parts of the genes in the genome have COG assigned value)."

        Protected Sub __throwSourceNullEx(Of T)(source As IEnumerable(Of T))
            If source.IsNullOrEmpty Then
                Dim exMsg As String =
                    $"{Me.GetType.FullName}, {NameOf(_highLights)} data Is null!" &
                    vbCrLf &
                    vbCrLf &
                    COG_NULL_EXCEPTION
                Throw New DataException(exMsg)
            End If
        End Sub

        Public Overrides ReadOnly Property Max As Double
            Get
                If _highLights.IsNullOrEmpty Then
                    Return 1
                End If
                Return (From metaUnit As HighLightsMeta In _highLights Select Val(metaUnit.Value)).ToArray.Max
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                If _highLights.IsNullOrEmpty Then
                    Return 0
                End If
                Return (From metaUnit In _highLights Select Val(metaUnit.Value)).ToArray.Min
            End Get
        End Property
    End Class
End Namespace