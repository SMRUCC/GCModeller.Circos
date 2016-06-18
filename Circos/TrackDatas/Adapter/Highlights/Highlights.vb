Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel

Namespace TrackDatas.Highlights

    Public MustInherit Class Highlights : Inherits data(Of ValueTrackData)

        Sub New(source As IEnumerable(Of ValueTrackData))
            Call MyBase.New(source)
        End Sub

        Protected Sub New()
            Call MyBase.New(Nothing)
        End Sub

        Const COG_NULL_EXCEPTION As String = "This error usually caused by the null COG data in the gene annotations. " &
            "Please check of the COG data in your genome annotation data make sure not all of the gene have no COG value" &
            "(at least should parts of the genes in the genome have COG assigned value)."

        Protected Sub __throwSourceNullEx(Of T)(source As IEnumerable(Of T))
            If source.IsNullOrEmpty Then
                Dim exMsg As String =
                    $"{Me.GetType.FullName}, data Is null!" &
                    vbCrLf &
                    vbCrLf &
                    COG_NULL_EXCEPTION
                Throw New DataException(exMsg)
            End If
        End Sub
    End Class
End Namespace